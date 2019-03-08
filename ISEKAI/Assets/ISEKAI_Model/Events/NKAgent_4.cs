using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class NKAgent_4 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "봄이 루트 이벤트 4"; } }
        public override EventLocation location { get { return EventLocation.TownSquare; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/NKAgent_4.txt"); } } // command list.

        protected override bool exclusiveCondition()
        { 
            return game.allEventsList.Find(e => e.eventName.Equals("봄이 루트 이벤트 3")).status == EventStatus.Completed &&
                game.allEventsList.Find(e => e.eventName.Equals("길 복구 이벤트")).status == EventStatus.Completed;
        }

        public override void Complete()
        {
            base.Complete();
            game.town.remainFoodAmount -= 20;
        }

        public NKAgent_4(Game game): base(game)
        {
            characterName = "봄이";
        }
    }
}