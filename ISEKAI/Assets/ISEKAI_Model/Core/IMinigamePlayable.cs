using System;

namespace ISEKAI_Model
{
    public interface IMinigamePlayable
    {
        int playerScore {get; set;} // result of playing minigame.
        void DoMinigameBehavior(); // do something with result score.
    }
}