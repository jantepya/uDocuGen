using System.Collections;
using System.Collections.Generic;

namespace uDocuGen
{
    public struct Variable
    {
        public string description;

        public string name;

        public string type;

        public string state;
    }

    public struct Function
    {
        public string description;

        public string name;

        public string type;

        public string state;

        public string arguments;
    }

    public class Obj : Document
    {

        public string filePath;

        public string description;

        public string name;

        public string inheritance;

        public string state;

        public IList<Variable> variables = new List<Variable>();

        public IList<Function> functions = new List<Function>();

        public Obj(string path)
        {
            filePath = path;
            description = "";
            name = "";
            inheritance = "";
            state = "";
        }

    }
}