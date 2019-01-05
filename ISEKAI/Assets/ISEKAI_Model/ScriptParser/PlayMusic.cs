using System;

namespace ISEKAI_Model
{
    public class PlayMusic : Command
    {
        public override int commandNumber { get {return 5;}}

        public bool isRepeating {get; private set;}
        public string filePath {get; private set;}
        public PlayMusic(bool isRepeating, string filePath)
        {
            this.isRepeating = isRepeating;
            this.filePath = filePath;
        }
    }
}