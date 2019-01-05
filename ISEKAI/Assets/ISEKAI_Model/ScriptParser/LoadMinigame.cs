using System;

namespace ISEKAI_Model
{
    public class LoadMinigame : Command
    {
        public override int commandNumber {get {return 13;}}
        public string minigameName {get; private set;}
        public LoadMinigame(string minigameName)
        {
            this.minigameName = minigameName;
        }
    }
}