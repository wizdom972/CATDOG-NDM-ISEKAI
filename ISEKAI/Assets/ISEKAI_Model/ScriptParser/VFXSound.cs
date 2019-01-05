using System;

namespace ISEKAI_Model
{
    public class VFXSound : Command
    {
        public override int commandNumber {get {return 12;}}
        public string filePath {get; private set;}
        public VFXSound(string filePath)
        {
            this.filePath = filePath;
        }
    }
}