using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class Expansion_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "확장 이벤트 2"; } }
        public override EventLocation location { get { return EventLocation.WayToTown; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 3; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Expansion_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "확장 이벤트 1").status == EventStatus.Completed;
            if (_isFirstOccur &&  prevCondition)
            {
                _isFirstOccur = false;
                return  prevCondition;
            }
            else
            {
                if (_isFirstOccur)
                    return false;
                return prevCondition && chanceCondition;
            }
        }

        private bool _isFirstOccur = true;

        public Expansion_2(Game game): base(game)
        {
            characterName = "작업반장";
        }
    }
}