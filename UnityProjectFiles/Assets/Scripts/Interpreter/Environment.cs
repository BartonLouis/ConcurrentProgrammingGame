using System.Collections.Generic;
using System.IO;

namespace Interpreter
{
    class Environment
    {

        Dictionary<string, Value> Variables;

        public Environment()
        {
            Variables = new Dictionary<string, Value>();
        }

        public void SetVariable(string name, Value value)
        {
            if (Variables.ContainsKey(name))
            {
                Variables[name] = value;
            }
            else
            {
                Variables.Add(name, value);
            }
        }

        public Value Lookup(string name)
        {
            if (Variables.ContainsKey(name))
            {
                return Variables[name];
            }
            else
            {
                return null;
            }
        }

        public void Remove(string name)
        {
            if (Variables.ContainsKey(name))
            {
                Variables.Remove(name);
            }
        }

        public void Expose(StreamWriter outputStream)
        {
            outputStream.AutoFlush = true;
            outputStream.WriteLine("Variables: ");
            foreach (string var in Variables.Keys)
            {
                if (Variables[var] != null)
                {
                    outputStream.WriteLine($"\t{var} : {Variables[var].ToString()}");
                }
                else
                {
                    outputStream.WriteLine($"\t{var} : None");
                }
            }
        }

    }
}
