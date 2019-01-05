using System;

namespace ISEKAI_Model
{
    public class Conversation : Command
    {
        public override int commandNumber {get {return 1;}}
        public string characterName {get; private set;}
        public string contents {get; private set;}
        public SpriteLocation brightCharacter {get; private set;}

        public Conversation(string characterName, string contents, SpriteLocation brightCharacter)
        {
            this.characterName = characterName;
            this.contents = contents;
            this.brightCharacter = brightCharacter;
        }
    }
}