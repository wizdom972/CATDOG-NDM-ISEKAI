using System;
using System.Collections.Generic;
using System.Linq;

namespace ISEKAI_Model
{
    public enum EventStatus
    {
        Completed, Ready, Visible
    }

    public enum EventLocation
    {
        None,
        BackMount,
        Field,
        WayToTown,
        Farm, // Conditional
        Mine, // Conditional
        TaskLeaderHouse,
        TechGuideStaffHouse,
        TownSquare,
        TownWell,
        SecretaryHouse, // Conditional
        TownWitchHouse // Conditional
    }
    public abstract class EventCore // Every future event must inherit this.
    {
        public bool isNew => (_seasonMadeIn == game.turn.season) && (turnsLeft == givenMaxTurn);

        public bool isActivatedAlready;
        public List<(int, int)> choiceHistory = new List<(int, int)>(); // <item1>th choice, selected <item2>th branch. (0-based)
        public abstract int forcedEventPriority {get;} // 0 if the event is not forced event.
        public abstract string eventName {get;}
        public EventStatus status {get; set;}
        public abstract int givenMaxTurn {get;}
        public abstract int turnsLeft {get; protected set;} // how many turns left for this event to be gone.
        public abstract int cost {get;} // how many AP this event takes.
        public abstract Season availableSeason {get;} // when this event is available.
        public abstract EventLocation location { get; }
        public abstract List<Command> script { get; }

        public Game game {get; private set;}
        private Season _seasonMadeIn = Season.None;
        protected abstract bool exclusiveCondition(); // exclusive emergence condition of each event.

        public bool isRemovedLastTurn = false;

        public void ActivateEvent()
        {
            status = EventStatus.Visible;
            turnsLeft = givenMaxTurn;
            isActivatedAlready = true;
        }

        protected EventCore(Game game)
        {
            status = EventStatus.Ready;
            this.game = game;
        }

        
        public bool IsFirstVisible() // if this returns true, event is set to Visible.
        {
            bool result;
            result =  exclusiveCondition() && seasonCheck();
            if (result) _seasonMadeIn = game.turn.season;
            return result && !isRemovedLastTurn;
        }
        
        public bool seasonCheck()
        {
            bool seasonCheck;
            if (availableSeason == Season.None)
                seasonCheck = true;
            else
                seasonCheck = availableSeason == game.turn.season;
            return seasonCheck;
        }

        public void CompleteEvent(Game game) // should be called when the event is completed.
        {
            game.remainAP -= cost;
            status = EventStatus.Completed;
        }

        public void ReduceTurnsLeft()
        {
            turnsLeft--;
        }
    }
}
