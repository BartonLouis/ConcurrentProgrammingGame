using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Interpreter
{

    public class ErrorListener : BaseErrorListener
    {

        List<string> errors;

        public ErrorListener()
        {
            errors = new List<string>();
        }

        public bool ErrorsOccured()
        {
            return (errors.Count > 0);
        }

        public List<string> GetErrors()
        {
            return errors;
        }

        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            errors.Add($"Error on line {line}: {msg}");
        }
    }
}
