using System;

namespace ISEKAI_Model
{
    public interface IMinigamePlayable
    {
        int playerScore {get; set;}
        void DoMinigameBehavior(int score);
    }
}