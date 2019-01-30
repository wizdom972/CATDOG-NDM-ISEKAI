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
        public Turn() // initiallize turn instance.
        {
            turnNumber = 1;
            season = Season.Summer;
            state = State.PreTurn;
            year = 1994;
        }
        public Season season {get; private set;}
        public State state {get; private set;}
        public int year {get; private set;}
        public int turnNumber { get; private set; }

        public override string ToString()
        {
            /*if (state == State.PreTurn || state == State.PostTurn)
                return state + " of Turn " + year;
            else
                return season + " of Turn " + year;*/
            string s;
            switch(season)
            {
                case Season.Autumn:
                    s = "가을";
                    break;
                case Season.Spring:
                    s = "봄";
                    break;
                case Season.Summer:
                    s = "여름";
                    break;
                case Season.Winter:
                    s = "겨울";
                    break;
                default:
                    throw new InvalidOperationException("season of turn cannot be None.");
            }
            return (year + "년 " + s);
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
                    year++;
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