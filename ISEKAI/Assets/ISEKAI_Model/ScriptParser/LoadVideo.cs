using System;

namespace ISEKAI_Model
{
    public class LoadVideo : Command
    {
        public override int commandNumber {get {return 14;}}
        public string filePath {get; private set;}

        public LoadVideo(string filePath)
        {
            this.filePath = filePath;
        }
    }
}