using System;
using System.IO;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime;

namespace Interpreter
{
    // the int returned when visiting a node represents if that action was successful or not.
    public class RuntimeInstance : LanguageParserBaseVisitor<Value>
    {
        public enum RunTimeState
        {
            Loading,        // Loading the next instruction
            Waiting,        // Waiting for x amount of time steps to pass before executing
            Listening,      // Check the message queue for messages, Idle otherwise
            Locking,        // Check if the side you're waiting for is free, then execute
            Executing       // Perform action on completion of Listen or Wait
        }

        private static readonly Dictionary<string, Func<int>> WaitTimes = new Dictionary<string, Func<int>>()
        {
            {"BoolExpr",        () => {return 1;} },
            {"Assignment",      () => {return 1;} },
            {"Attack",          () => {return 2;} },
            {"HealSelf",        () => {return 3;} },
            {"DefendSelf",      () => {return 3;} },
            {"Heal",            () => {return 2;} },
            {"Boost",           () => {return 3;} },
            {"Defend",          () => {return 4;} },
            {"Block",           () => {return 5;} },
            {"Lock",            () => {return 3;} },
            {"ChargeUp",        () => {return 4;} },
            {"SendMessageTo",   () => {return 3;} },
            {"SendMessageToAll",() => {
                // Logic to get the number of players alive and multiple by cost of sending one message
                // Todo
                return 10;}
            },
            {"Yield",           () => {return 1;}}
        };

        private int WaitTime;                       // Number of steps required before action complete
        private RunTimeState State;                 // Hold the current state of execution
        private Func<Value> OnExecute;              // Function to execute once time wait complete
        private Queue<MessageValue> MessageQueue;   // Hold the current incoming messages

        private Stack<RuleContext> NextInstructionCallStack;        // Stores a stack of iteration nodes so that nested iteration is allowed
        private LanguageParserParser.ProgramContext Root;           // Stores the root node so that program can loop once complete
        private Environment Environment;                            // Stores variables for lookup

        private bool ValidProgram = false;
        private ErrorListener ErrorListener;

        public RuntimeInstance(LanguageParserParser.ProgramContext Root)
        {
            // Use this constructor if the root node has already been constructed outside of the class and error handling has already been done
            ValidProgram = true;

            WaitTime = 0;
            State = RunTimeState.Loading;
            OnExecute = null;
            MessageQueue = new Queue<MessageValue>();

            Environment = new Environment();
            this.Root = Root;
            NextInstructionCallStack = new Stack<RuleContext>();
            NextInstructionCallStack.Push(Root);
        }

        public RuntimeInstance(string source)
        {
            // Use this class to pass through raw source code. If the program is valid then it will allow the program to be run as normal, otherwise it will generate error messages

            AntlrInputStream inputStream = new AntlrInputStream(source);
            LanguageParserLexer lexer = new LanguageParserLexer(inputStream);

            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            LanguageParserParser parser = new LanguageParserParser(tokenStream);
            parser.RemoveErrorListeners();
            ErrorListener = new ErrorListener();
            parser.AddErrorListener(ErrorListener);
            LanguageParserParser.ProgramContext program = parser.program();

            if (!ErrorListener.ErrorsOccured())
            {
                ValidProgram = true;

                WaitTime = 0;
                State = RunTimeState.Loading;
                OnExecute = null;
                MessageQueue = new Queue<MessageValue>();

                Environment = new Environment();
                this.Root = program;
                NextInstructionCallStack = new Stack<RuleContext>();
                NextInstructionCallStack.Push(Root);
            }
        }

        public bool GetParseResult()
        {
            return ValidProgram;
        }

        public List<string> GetErrors()
        {
            return ErrorListener.GetErrors();
        }

        public void Step()
        {
            if (ValidProgram)
            {
                switch (State)
                {
                    case RunTimeState.Loading:
                        // Load the next instruction by visiting the top node on the next instruction call stack
                        if (NextInstructionCallStack.Count > 0)
                        {
                            Visit(NextInstructionCallStack.Pop());
                        }
                        break;
                    case RunTimeState.Waiting:
                        // Move forward one time step
                        WaitTime--;
                        if (WaitTime == 0)
                        {
                            State = RunTimeState.Executing;
                        }
                        break;
                    case RunTimeState.Listening:
                        // Check for messages, do nothing if no messages
                        if (MessageQueue.Count > 0)
                        {
                            State = RunTimeState.Executing;
                        }
                        break;
                    case RunTimeState.Locking:
                        // Waits a minimum time before trying to lock resource
                        // Once time is complete, check every update if cable is free, then lock it
                        bool free = false;
                        WaitTime--;
                        if (free && WaitTime <= 0)
                        {
                            OnExecute?.Invoke();
                            State = RunTimeState.Loading;
                        }
                        break;
                    case RunTimeState.Executing:
                        // Complete the instruction
                        OnExecute?.Invoke();
                        WaitTime = 0;
                        OnExecute = null;
                        State = RunTimeState.Loading;
                        break;
                }
            }
            Console.WriteLine("");
        }


        public override Value VisitMultipleExpr([NotNull] LanguageParserParser.MultipleExprContext context)
        {
            NextInstructionCallStack.Push(context.prog);
            return Visit(context.expr);
        }

        public override Value VisitSingleExpr([NotNull] LanguageParserParser.SingleExprContext context)
        {
            if (NextInstructionCallStack.Count == 0)
            {
                NextInstructionCallStack.Push(Root);
            }
            return Visit(context.expr);
        }

        public override Value VisitSingleIf([NotNull] LanguageParserParser.SingleIfContext context)
        {
            // Wait to evaluate expression
            WaitTime = WaitTimes["BoolExpr"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                // If the expression evaluates to true, visit the internal node
                if (Visit(context.expr).GetAsBoolean().Value)
                {
                    NextInstructionCallStack.Push(context.prog);
                }
                return null;
            };
            return null;
        }

        public override Value VisitExtendedIf([NotNull] LanguageParserParser.ExtendedIfContext context)
        {
            // Wait to evaluate expression
            WaitTime = WaitTimes["BoolExpr"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                // If the expression evaluates to true, visit the internal node
                if (Visit(context.expr).GetAsBoolean().Value)
                {
                    NextInstructionCallStack.Push(context.prog);
                }
                else
                {
                    NextInstructionCallStack.Push(context.@else);
                }
                return null;
            };
            return null;
        }

        public override Value VisitElse([NotNull] LanguageParserParser.ElseContext context)
        {
            return Visit(context.prog);
        }

        public override Value VisitElseIf([NotNull] LanguageParserParser.ElseIfContext context)
        {
            // Wait to evaluate expression
            return Visit(context.select);
        }

        public override Value VisitWhile([NotNull] LanguageParserParser.WhileContext context)
        {
            WaitTime = WaitTimes["BoolExpr"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                if (Visit(context.expr).GetAsBoolean().Value)
                {
                    NextInstructionCallStack.Push(context);
                    NextInstructionCallStack.Push(context.prog);
                }
                return null;
            };

            return null;
        }

        public override Value VisitAtomAssignment([NotNull] LanguageParserParser.AtomAssignmentContext context)
        {
            // This instruction will take one time step to execute
            WaitTime = WaitTimes["Assignment"]();
            State = RunTimeState.Waiting;
            // Once the wait time is over, assign a value to the variable by evaluating the right expression
            OnExecute = () =>
            {
                Environment.SetVariable(context.v.ID().GetText(), Visit(context.a));
                return null;
            };
            return null;
        }

        public override Value VisitMathExprAssignment([NotNull] LanguageParserParser.MathExprAssignmentContext context)
        {
            // This instruction will take one time step to execute
            WaitTime = WaitTimes["Assignment"]();
            State = RunTimeState.Waiting;
            // Once the wait time is over, assign a value to the variable by evaluating the right expression
            OnExecute = () =>
            {
                Environment.SetVariable(context.v.ID().GetText(), Visit(context.expr));
                return null;
            };
            return null;
        }

        public override Value VisitBoolExprAssignment([NotNull] LanguageParserParser.BoolExprAssignmentContext context)
        {
            // This instruction will take one time step to execute
            WaitTime = WaitTimes["Assignment"]();
            State = RunTimeState.Waiting;
            // Once the wait time is over, assign a value to the variable by evaluating the right expression
            OnExecute = () =>
            {
                Environment.SetVariable(context.v.ID().GetText(), Visit(context.expr));
                return null;
            };
            return null;
        }

        public override Value VisitFunctionAssignment([NotNull] LanguageParserParser.FunctionAssignmentContext context)
        {
            WaitTime = WaitTimes["Assignment"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                Environment.SetVariable(context.v.ID().GetText(), Visit(context.f));
                return null;
            };
            return base.VisitFunctionAssignment(context);
        }

        public override Value VisitListenAssignment([NotNull] LanguageParserParser.ListenAssignmentContext context)
        {
            WaitTime = WaitTimes["Listen"]();
            State = RunTimeState.Listening;
            OnExecute = () =>
            {
                Environment.SetVariable(context.v.ID().GetText(), MessageQueue.Dequeue());
                return null;
            };
            return base.VisitListenAssignment(context);
        }

        public override Value VisitAttack([NotNull] LanguageParserParser.AttackContext context)
        {
            WaitTime = WaitTimes["Attack"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitHealSelf([NotNull] LanguageParserParser.HealSelfContext context)
        {
            WaitTime = WaitTimes["HealSelf"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitDefendSelf([NotNull] LanguageParserParser.DefendSelfContext context)
        {
            WaitTime = WaitTimes["DefendSelf"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitHeal([NotNull] LanguageParserParser.HealContext context)
        {
            WaitTime = WaitTimes["Heal"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitBoost([NotNull] LanguageParserParser.BoostContext context)
        {
            WaitTime = WaitTimes["Boost"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitDefend([NotNull] LanguageParserParser.DefendContext context)
        {
            WaitTime = WaitTimes["Defend"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }


        public override Value VisitBlock([NotNull] LanguageParserParser.BlockContext context)
        {
            WaitTime = WaitTimes["Block"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitLock([NotNull] LanguageParserParser.LockContext context)
        {
            WaitTime = WaitTimes["Lock"]();
            State = RunTimeState.Locking;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitChargeUp([NotNull] LanguageParserParser.ChargeUpContext context)
        {
            WaitTime = WaitTimes["ChargeUp"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitSendMessageTo([NotNull] LanguageParserParser.SendMessageToContext context)
        {
            WaitTime = WaitTimes["SendMessageTo"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitSendMessageToAll([NotNull] LanguageParserParser.SendMessageToAllContext context)
        {
            WaitTime = WaitTimes["SendMessageToAll"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        public override Value VisitYield([NotNull] LanguageParserParser.YieldContext context)
        {
            WaitTime = WaitTimes["Yield"]();
            State = RunTimeState.Waiting;
            OnExecute = () =>
            {
                return null;
            };
            return null;
        }

        // Functions
        public override Value VisitGetEnemyOfType([NotNull] LanguageParserParser.GetEnemyOfTypeContext context)
        {
            // Logic to Get an enemy of type a
            return null;
        }

        public override Value VisitGetTeammateOfType([NotNull] LanguageParserParser.GetTeammateOfTypeContext context)
        {
            // Logic to get a teammate of type a
            return null;
        }

        public override Value VisitGetHealth([NotNull] LanguageParserParser.GetHealthContext context)
        {
            // Logic to get the current health of this character
            return null;
        }

        public override Value VisitGetMaxHealth([NotNull] LanguageParserParser.GetMaxHealthContext context)
        {
            // Logic to get the max health of this character
            return null;
        }

        public override Value VisitIsFullHealth([NotNull] LanguageParserParser.IsFullHealthContext context)
        {
            // Logic to check if the character is full health
            return null;
        }

        public override Value VisitIsCharged([NotNull] LanguageParserParser.IsChargedContext context)
        {
            // Logic to check if a character is charged up fo an attack
            return null;
        }

        public override Value VisitIsNone([NotNull] LanguageParserParser.IsNoneContext context)
        {
            // Logic to check if a given value is null
            return null;
        }

        public override Value VisitIsNotNone([NotNull] LanguageParserParser.IsNotNoneContext context)
        {
            // Logic to check if a given value is not null
            return null;
        }

        public override Value VisitGetPlayerComponent([NotNull] LanguageParserParser.GetPlayerComponentContext context)
        {
            // Logic to get the player component of a given message object
            return null;
        }

        public override Value VisitGetTextComponent([NotNull] LanguageParserParser.GetTextComponentContext context)
        {
            // Logic to get the text component of a given message object
            return null;
        }

        public override Value VisitGetClass([NotNull] LanguageParserParser.GetClassContext context)
        {
            // Logic to get the class of a given player object
            return null;
        }

        public override Value VisitGetTimeLeft([NotNull] LanguageParserParser.GetTimeLeftContext context)
        {
            // Logic to check how much time is left on the current turn
            return null;
        }


        // Boolean expresions

        public override Value VisitBoolParen([NotNull] LanguageParserParser.BoolParenContext context)
        {
            return Visit(context.expr);
        }

        public override Value VisitBoolAnd([NotNull] LanguageParserParser.BoolAndContext context)
        {
            return Operations.And(Visit(context.e1), Visit(context.e2));
        }

        public override Value VisitBoolOr([NotNull] LanguageParserParser.BoolOrContext context)
        {
            return Operations.Or(Visit(context.e1), Visit(context.e2));
        }

        public override Value VisitBoolNot([NotNull] LanguageParserParser.BoolNotContext context)
        {
            return Operations.Not(Visit(context.expr));
        }

        public override Value VisitBoolAtom([NotNull] LanguageParserParser.BoolAtomContext context)
        {
            return Visit(context.a);
        }

        public override Value VisitBoolEq([NotNull] LanguageParserParser.BoolEqContext context)
        {
            switch (context.op.Type)
            {
                case LanguageParserLexer.EQ:
                    return Operations.EQ(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.NOTEQ:
                    return Operations.NEQ(Visit(context.e1), Visit(context.e2));
            }
            return null;
        }

        public override Value VisitMathEq([NotNull] LanguageParserParser.MathEqContext context)
        {
            switch (context.op.Type)
            {
                case LanguageParserLexer.GT:
                    return Operations.GT(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.LT:
                    return Operations.LT(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.GTE:
                    return Operations.GTEQ(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.LTE:
                    return Operations.LTEQ(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.EQ:
                    return Operations.EQ(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.NOTEQ:
                    return Operations.NEQ(Visit(context.e1), Visit(context.e2));
            }
            return null;
        }
        // Math Expressions
        public override Value VisitMathParen([NotNull] LanguageParserParser.MathParenContext context)
        {
            return Visit(context.expr);
        }

        public override Value VisitMathAddSub([NotNull] LanguageParserParser.MathAddSubContext context)
        {
            switch (context.op.Type)
            {
                case LanguageParserLexer.ADD:
                    return Operations.Add(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.SUB:
                    return Operations.Sub(Visit(context.e1), Visit(context.e2));
                default:
                    return null;
            }
        }

        public override Value VisitMathMulDiv([NotNull] LanguageParserParser.MathMulDivContext context)
        {
            switch (context.op.Type)
            {
                case LanguageParserLexer.MUL:
                    return Operations.Mul(Visit(context.e1), Visit(context.e2));
                case LanguageParserLexer.DIV:
                    return Operations.Div(Visit(context.e1), Visit(context.e2));
                default:
                    return null;
            }
        }

        public override Value VisitMathPow([NotNull] LanguageParserParser.MathPowContext context)
        {
            switch (context.op.Type)
            {
                case LanguageParserLexer.POW:
                    return Operations.Pow(Visit(context.e1), Visit(context.e2));
                default:
                    return null;
            }
        }

        public override Value VisitMathMod([NotNull] LanguageParserParser.MathModContext context)
        {
            switch (context.op.Type)
            {
                case LanguageParserLexer.MOD:
                    return Operations.Mod(Visit(context.e1), Visit(context.e2));
                default:
                    return null;
            }
        }

        public override Value VisitMathAtom([NotNull] LanguageParserParser.MathAtomContext context)
        {
            return Visit(context.a);
        }

        // Variables

        public override Value VisitVar([NotNull] LanguageParserParser.VarContext context)
        {
            // Lookup a variables value in the environment table
            return Environment.Lookup(context.ID().GetText());
        }

        // Literals
        public override Value VisitLiteral([NotNull] LanguageParserParser.LiteralContext context)
        {
            if (context.INT() != null)
            {
                return new IntValue(Convert.ToInt32(context.INT().GetText()));
            }
            else if (context.STRING() != null)
            {
                return new StringValue(context.STRING().GetText().Replace("\"", ""));
            }
            else if (context.message() != null)
            {
                return Visit(context.message());
            }
            else if (context.side() != null)
            {
                return Visit(context.side());
            }
            else if (context.boolean() != null)
            {
                return Visit(context.boolean());
            }
            else
            {
                return Visit(context.@class());
            }
        }

        public override Value VisitMessage([NotNull] LanguageParserParser.MessageContext context)
        {
            return new MessageValue(
                Visit(context.a1).GetAsPlayer(),
                Visit(context.a2).GetAsString()
            );
        }

        public override Value VisitSide([NotNull] LanguageParserParser.SideContext context)
        {
            if (context.LEFT() != null)
            {
                return new SideValue(SideValue.Side.Left);
            }
            else
            {
                return new SideValue(SideValue.Side.Right);
            }
        }

        public override Value VisitBoolean([NotNull] LanguageParserParser.BooleanContext context)
        {
            if (context.FALSE() != null)
            {
                return new BoolValue(false);
            }
            else
            {
                return new BoolValue(true);
            }
        }

        public override Value VisitClass([NotNull] LanguageParserParser.ClassContext context)
        {
            if (context.DAMAGE() != null)
            {
                return new ClassValue(ClassValue.ClassType.Damage);
            }
            else if (context.SUPPORT() != null)
            {
                return new ClassValue(ClassValue.ClassType.Support);
            }
            else if (context.TANK() != null)
            {
                return new ClassValue(ClassValue.ClassType.Tank);
            }
            else if (context.ANY() != null)
            {
                return new ClassValue(ClassValue.ClassType.Any);
            }
            return null;
        }
    }
}
