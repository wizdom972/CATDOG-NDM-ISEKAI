using System;

namespace ISEKAI_Model
{
    public class LoadCG : Command
    {
        public override int commandNumber {get {return 7;}}
        public string filePath {get; private set;}
        public LoadCG(string filePath)
        {
            this.filePath = filePath;
        }
    }
}