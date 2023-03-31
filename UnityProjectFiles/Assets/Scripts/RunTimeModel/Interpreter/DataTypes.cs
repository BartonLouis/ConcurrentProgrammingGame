using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Interpreter
{
    public class Value
    {
        // Interpret Values as each data type
        public virtual ClassValue GetAsClass() { return null; }
        public virtual PlayerValue GetAsPlayer() { return null; }
        public virtual SideValue GetAsSide() { return null; }
        public virtual MessageValue GetAsMessage() { return null; }
        public virtual IntValue GetAsInteger() { return null; }
        public virtual BoolValue GetAsBoolean() { return null; }
        public virtual StringValue GetAsString() { return null; }
    }
    public class PlayerValue : Value
    {
        
        
        // Likely going to change
        public Character PlayerRef { get; }

        public PlayerValue(Character playerRef)
        {
            PlayerRef = playerRef;
        }

        public override PlayerValue GetAsPlayer()
        {
            return this;
        }

        public static BoolValue EQ(PlayerValue p1, PlayerValue p2)
        {
            return new BoolValue(p1.PlayerRef == p2.PlayerRef);
        }

        public static BoolValue NEQ(PlayerValue p1, PlayerValue p2)
        {
            return new BoolValue(p1.PlayerRef != p2.PlayerRef);
        }

        public override string ToString()
        {
            return $"Player Object: {PlayerRef}";
        }
    }

    public class ClassValue : Value
    {
        public enum ClassType
        {
            Damage,
            Support,
            Tank,
            Any
        }

        public ClassType Value { get; }

        public ClassValue(ClassType type)
        {
            Value = type;
        }

        public override StringValue GetAsString()
        {
            return new StringValue(Value.ToString());
        }

        public override ClassValue GetAsClass()
        {
            return this;
        }

        public static BoolValue EQ(ClassValue c1, ClassValue c2)
        {
            if (c1.Value == ClassType.Any || c2.Value == ClassType.Any)
            {
                return new BoolValue(true);
            }
            else
            {
                return new BoolValue(c1.Value == c2.Value);
            }
        }

        public static BoolValue NEQ(ClassValue c1, ClassValue c2)
        {
            if (c1.Value == ClassType.Any || c2.Value == ClassType.Any)
            {
                return new BoolValue(false);
            }
            else
            {
                return new BoolValue(c1.Value != c2.Value);
            }
        }

        public override string ToString()
        {
            return $"Class Object: {Value.ToString()}";
        }
    }

    public class SideValue : Value
    {
        public enum Side
        {
            Left,
            Right
        }

        public Side Value { get; }

        public SideValue(Side value)
        {
            Value = value;
        }

        public override StringValue GetAsString()
        {
            return new StringValue(Value.ToString());
        }

        public static BoolValue EQ(SideValue s1, SideValue s2)
        {
            return new BoolValue(s1.Value == s2.Value);
        }

        public static BoolValue NEQ(SideValue s1, SideValue s2)
        {
            return new BoolValue(s1.Value != s2.Value);
        }

        public override string ToString()
        {
            return $"Side Object: {Value.ToString()}";
        }

        public override SideValue GetAsSide()
        {
            return this;
        }
    }

    public class MessageValue : Value
    {
        public PlayerValue PlayerComponent { get; }
        public StringValue StringComponent { get; }

        public MessageValue(PlayerValue playerComponent, StringValue stringComponent)
        {
            PlayerComponent = playerComponent;
            StringComponent = stringComponent;
        }

        public override MessageValue GetAsMessage()
        {
            return this;
        }

        public override string ToString()
        {
            return $"Message Object: ({PlayerComponent.ToString()}, {StringComponent.ToString()})";
        }
    }

    public class IntValue : Value
    {
        public int Value { get; }

        public IntValue(int value)
        {
            Value = value;
        }

        public override IntValue GetAsInteger()
        {
            return this;
        }

        public override BoolValue GetAsBoolean()
        {
            return new BoolValue(Value != 0);
        }

        public static IntValue Add(IntValue num1, IntValue num2)
        {
            return new IntValue(num1.Value + num2.Value);
        }

        public static IntValue Sub(IntValue num1, IntValue num2)
        {
            return new IntValue(num1.Value - num2.Value);
        }

        public static IntValue Mul(IntValue num1, IntValue num2)
        {
            return new IntValue(num1.Value * num2.Value);
        }

        public static IntValue Div(IntValue num1, IntValue num2)
        {
            return new IntValue(num1.Value / num2.Value);
        }

        public static IntValue Pow(IntValue num1, IntValue num2)
        {
            return new IntValue((int)Math.Floor(Math.Pow((double)num1.Value, (double)num2.Value)));
        }

        public static IntValue Mod(IntValue num1, IntValue num2)
        {
            return new IntValue(num1.Value % num2.Value);
        }

        public static BoolValue EQ(IntValue num1, IntValue num2)
        {
            return new BoolValue(num1.Value == num2.Value);
        }

        public static BoolValue NEQ(IntValue num1, IntValue num2)
        {
            return new BoolValue(num1.Value != num2.Value);
        }

        public static BoolValue GT(IntValue num1, IntValue num2)
        {
            return new BoolValue(num1.Value > num2.Value);
        }

        public static BoolValue LT(IntValue num1, IntValue num2)
        {
            return new BoolValue(num1.Value < num2.Value);
        }

        public static BoolValue GTEQ(IntValue num1, IntValue num2)
        {
            return new BoolValue(num1.Value <= num2.Value);
        }

        public static BoolValue LTEQ(IntValue num1, IntValue num2)
        {
            return new BoolValue(num1.Value <= num2.Value);
        }

        public override string ToString()
        {
            return $"Int Object: {Value.ToString()}";
        }

    }

    public class StringValue : Value
    {
        public string Value { get; }

        public StringValue(string value)
        {
            Value = value;
        }

        public override StringValue GetAsString()
        {
            return this;
        }

        public override BoolValue GetAsBoolean()
        {
            return new BoolValue(Value != "");
        }

        public static BoolValue EQ(StringValue s1, StringValue s2)
        {
            return new BoolValue(s1.Value == s2.Value);
        }

        public static BoolValue NEQ(StringValue s1, StringValue s2)
        {
            return new BoolValue(s1.Value != s2.Value);
        }

        public override string ToString()
        {
            return $"String Object: \"{Value}\"";
        }
    }

    public class BoolValue : Value
    {
        public bool Value { get; }

        public BoolValue(bool value)
        {
            this.Value = value;
        }

        public override BoolValue GetAsBoolean()
        {
            return this;
        }

        public static BoolValue EQ(BoolValue b1, BoolValue b2)
        {
            return new BoolValue(b1.Value == b2.Value);
        }

        public static BoolValue NEQ(BoolValue b1, BoolValue b2)
        {
            return new BoolValue(b1.Value != b2.Value);
        }

        public static BoolValue And(BoolValue b1, BoolValue b2)
        {
            return new BoolValue(b1.Value && b2.Value);
        }

        public static BoolValue Or(BoolValue b1, BoolValue b2)
        {
            return new BoolValue(b1.Value || b2.Value);
        }

        public static BoolValue Not(BoolValue b1)
        {
            return new BoolValue(!b1.Value);
        }

        public override string ToString()
        {
            return $"Bool Object: {Value}";
        }

        public override IntValue GetAsInteger()
        {
            if (Value == false)
            {
                return new IntValue(0);
            }
            else
            {
                return new IntValue(1);
            }
        }

        public override StringValue GetAsString()
        {
            return new StringValue(Value.ToString());
        }
    }

}
