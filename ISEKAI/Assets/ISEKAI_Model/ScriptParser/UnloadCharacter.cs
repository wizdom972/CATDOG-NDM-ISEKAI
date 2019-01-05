using System;

namespace ISEKAI_Model
{
    public class UnloadCharacter : Command
    {
        public override int commandNumber {get {return 3;}}
        public SpriteLocation location {get; private set;}
        public UnloadCharacter(SpriteLocation location)
        {
            this.location = location;
        }
    }
}