using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ISEKAI_Model
{
    public enum EventStatus
    {
        Completed, Ready, Visible, ForcedVisible
    }

    public enum EventLocation
    {
        None,
        BackMount,
        FrontMount,
        Field,
        WayToTown,
        Farm, // Conditional
        Mine, // Conditional
        TaskLeaderHouse,
        TechGuideStaffHouse,
        TownSquare,
        TownWell,
        CarpenterHouse,
        SecretaryHouse, // Conditional
        TownWitchHouse // Conditional
    }
    public abstract class EventCore // Every future event must inherit this.
    {
        public bool isNew => (_seasonMadeIn == game.turn.season) && (monthsLeft == givenMaxMonth);
        public bool isForcedEvent => forcedEventPriority > 0;
        public bool isActivatedAlready;
        public abstract int forcedEventPriority {get;} // 0 if the event is not forced event.
        public abstract string eventName {get;}
        public EventStatus status {get; set;}
        public abstract int givenMaxTurn {get;} // -1 if the event is permanent.
        public int givenMaxMonth { get { return (6 * givenMaxTurn); } }
        //public int turnsLeft {get; protected set;} // how many turns left for this event to be gone.
        public int monthsLeft { get; protected set; }
        public abstract int cost {get;} // how many AP this event takes.
        public abstract Season availableSeason {get;} // when this event is available.
        public abstract EventLocation location { get; }
        public abstract List<Command> script { get; }

        public string characterName = null; //null if forced event or no character.

        public Game game {get; private set;}
        private Season _seasonMadeIn = Season.None;
        protected abstract bool exclusiveCondition(); // exclusive emergence condition of each event.

        public bool isRemovedLastTurn = false;

        public void ActivateEvent()
        {
            status = EventStatus.Visible;
            monthsLeft = givenMaxMonth;
            isActivatedAlready = true;
        }

        protected EventCore(Game game)
        {
            status = EventStatus.Ready;
            this.game = game;
            monthsLeft = 0;
        }

        
        public bool IsFirstVisible() // if this returns true, event is set to Visible. (or ForcedVisible)
        {
            bool result;
            result =  exclusiveCondition() && SeasonCheck();
            if (result) _seasonMadeIn = game.turn.season;
            return result && !isRemovedLastTurn;
        }
        
        public bool SeasonCheck()
        {
            bool seasonCheck;
            if (availableSeason == Season.None)
                seasonCheck = true;
            else
                seasonCheck = availableSeason == game.turn.season;
            return seasonCheck;
        }


        /*public void ReduceTurnsLeft()
        {
            turnsLeft--;
        }*/


        public void ReduceMonthsLeft(int monthspassed)
        {
            monthsLeft -= monthspassed;
        }

        public virtual void Complete()
        {
            Season beforeSeason = game.turn.season;
            status = EventStatus.Completed;
            game.turn.totalMonthNumber += cost;

            game.Proceed(beforeSeason);
            game._ReduceEveryEventsMonthsLeft(cost);
            game.OccurEvents();
            /*
            if (beforeSeason != game.turn.season)
                game.Proceed();
                
            for (int i = 0; i < HowManySeasonsHavePassed(beforeSeason, game.turn.season); i++)
            {
                //game.Proceed();
                Debug.Log(game.turn.state + "asdf");
            }

            game.OccurEvents();
            */
        }

        
    }
}
