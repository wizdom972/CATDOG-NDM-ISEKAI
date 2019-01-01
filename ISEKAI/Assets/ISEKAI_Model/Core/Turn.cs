using System;

namespace ISEKAI_Model
{
    public enum Season
    {  
        None, // Only for all-season-available events.
        Spring , Summer , Autumn , Winter
    }
    public enum State
    {
        PreTurn, InTurn, PostTurn
    }
    public class Turn
    {
        public Turn() // initiallize turn instance. It should be immediately proceeded to be {Spring, PreTurn, 1}, so should be look like this.
        {
            season = Season.Winter;
            state = State.PreTurn;
            turnNumber = 1;
        }
        public Season season {get; private set;}
        public State state {get; private set;}
        public int turnNumber {get; private set;}

        public override string ToString()
        {
            if (state == State.PreTurn || state == State.PostTurn)
                return state + " of Turn " + turnNumber;
            else
                return season + " of Turn " + turnNumber;
        }
        public bool IsFormerSeason() // if the current season is winter or summer, it returns true.
        {
            return (season == Season.Winter || season == Season.Summer);
        }
        public void MoveToNextSeason() // Not recommended to call manually. Only called by Proceed().
        {
            switch (season)
            {
                case Season.Winter:
                    season = Season.Spring;
                    break;

                case Season.Spring:
                    season = Season.Summer;
                    break;

                case Season.Summer:
                    season = Season.Autumn;
                    break;

                case Season.Autumn:
                    season = Season.Winter;
                    break;
            }
        }
        
        public void MoveToNextState() // Not recommended to call manually. Only called by Proceed().
        {
            switch (state)
            {
                case State.PreTurn:
                    state = State.InTurn;
                    break;
                case State.InTurn:
                    state = State.PostTurn;
                    break;
                case State.PostTurn:
                    state = State.PreTurn;
                    break;
            }
        }

        public void IncreaseTurnNumber() // Not recommended to call manually. Only called by Proceed().
        {
            turnNumber++;
        }
    }
}