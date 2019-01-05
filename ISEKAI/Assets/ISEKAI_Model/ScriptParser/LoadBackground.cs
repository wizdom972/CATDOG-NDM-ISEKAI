using System;

namespace ISEKAI_Model
{
    public class LoadBackground : Command
    {
        public override int commandNumber {get {return 4;}}
        public string filePath {get; private set;}

        public LoadBackground(string filePath)
        {
            this.filePath = filePath;
        }
    }
}