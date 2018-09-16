using System.Collections;
using System.Collections.Generic;

namespace uDocuGen
{
    public struct Variable
    {
        public string Description;

        public string Name;

        public string Type;

        public string Scope;

        public string UniqueID;
    }

    public struct Function
    {
        public string Description;

        public string Name;

        public string Type;

        public string Scope;

        public string Arguments;

        public string UniqueID;

    }

    public class Obj : Document
    {

        public string FilePath;

        public string Description;

        public string Name;

        public string Inheritance;

        public string Scope;

        public string UniqueId;

        public IList<Variable> Variables = new List<Variable>();

        public IList<Function> Functions = new List<Function>();

        public Obj(string path)
        {
            FilePath = path;
            Description = "";
            Name = "";
            Inheritance = "";
            Scope = "";
            UniqueId = "";
        }

    }
}