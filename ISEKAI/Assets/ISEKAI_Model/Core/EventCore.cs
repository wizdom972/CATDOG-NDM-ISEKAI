using System;
using System.Collections.Generic;
using System.Linq;

namespace ISEKAI_Model
{
    public enum EventStatus
    {
        Completed, Ready, Visible
    }
    public abstract class EventCore // Every future event must inherit this.
    {
        private bool _isActivatedAlready;
        public List<(int, int)> choiceHistory = new List<(int, int)>(); // <item1>th choice, selected <item2>th branch. (0-based)
        public abstract int forcedEventPriority {get;} // 0 if the event is not forced event.
        public abstract string eventName {get;}
        public EventStatus status {get; set;}
        public abstract int givenMaxTurn {get;}
        public abstract int turnsLeft {get; protected set;} // how many turns left for this event to be gone.
        public abstract int cost {get;} // how many AP this event takes.
        public abstract Season availableSeason {get;} // when this event is available.

        protected abstract bool exclusiveCondition(Game game); // exclusive emergence condition of each event.
        public static void InitEvents() // should add EVERY events when new event plan comes.
        {
            _activatedEvents.Add(new ExampleEvent1());
        }
        public static void OccurEvents(Game game) // Not recommended to call manually. Only called by Proceed().
        {
            foreach (EventCore e in _activatedEvents)
            {
                if (e.IsFirstVisible(game) && e.status == EventStatus.Ready && !e._isActivatedAlready)
                {
                    e.turnsLeft = e.givenMaxTurn;
                    e.status = EventStatus.Visible;
                    e._isActivatedAlready = true;
                }
                else if (e._isActivatedAlready)
                {
                    if (e.seasonCheck(game) && e.turnsLeft > 0)
                        e.status = EventStatus.Visible;
                    else
                        e.status = EventStatus.Ready;
                }
            }
        }
        protected EventCore()
        {
            status = EventStatus.Ready;
        }
        private static List<EventCore> _activatedEvents = new List<EventCore>(); // list of all activated events.
        public static List<EventCore> GetAllEvents() // MOST IMPORTANT FUNCTION!
        {
            return _activatedEvents;
        }

        public static void ReduceTurnsLeft() // Not recommended to call manually. Only called by Proceed().
        {
            foreach (EventCore e in _activatedEvents)
            {
                if(e._isActivatedAlready)
                    e.turnsLeft--;

                if (e.turnsLeft <= 0 && e._isActivatedAlready)
                {
                    e.status = EventStatus.Ready;
                    e._isActivatedAlready = false;
                }
            }
        }
        public bool IsFirstVisible(Game game) // if this returns true, event is set to Visible.
        {
            return exclusiveCondition(game) && seasonCheck(game);
        }
        
        public bool seasonCheck(Game game)
        {
            bool seasonCheck;
            if (availableSeason == Season.None)
                seasonCheck = true;
            else
                seasonCheck = availableSeason == game.turn.season;
            return seasonCheck;
        }
        public abstract List<Command> script {get;}
    }
}
