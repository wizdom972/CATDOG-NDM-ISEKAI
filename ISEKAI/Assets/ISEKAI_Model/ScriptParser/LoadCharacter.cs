using System;

namespace ISEKAI_Model
{
    
    public class LoadCharacter : Command
    {
        public override int commandNumber {get {return 2;}}
        public string filePath {get; private set;}
        public SpriteLocation location {get; private set;}

        public LoadCharacter(string filePath, SpriteLocation location)
        {
            this.filePath = filePath;
            this.location = location;
        }

    }
}