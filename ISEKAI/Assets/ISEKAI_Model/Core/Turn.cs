using System;
using UnityEngine;

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
            totalMonthNumber = 4;
            state = State.PreTurn;
        }
        public Season season { get
            {
                switch (monthNumber)
                {
                    case 2:
                    case 3:
                    case 4:
                        return Season.Spring;
                    case 5:
                    case 6:
                    case 7:
                        return Season.Summer;
                    case 8:
                    case 9:
                    case 10:
                        return Season.Autumn;
                    case 11:
                    case 0:
                    case 1:
                        return Season.Winter;
                    default:
                        throw new InvalidOperationException("EERAR");
                }
            }
        }
        public State state {get; private set;}
        public int year => 1994 + ((turnNumber - 1) / 2);
        public int turnNumber { get; private set; }
        public int totalMonthNumber;
        public int monthNumber => totalMonthNumber % 12;

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
            return (year + "년 " + (monthNumber + 1) + "월, " + s);
        }
        public bool IsFormerSeason() // if the current season is winter or summer, it returns true.
        {
            return (season == Season.Winter || season == Season.Summer);
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