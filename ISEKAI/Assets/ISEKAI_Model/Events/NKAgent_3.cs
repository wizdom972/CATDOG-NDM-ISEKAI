using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class NKAgent_3 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "봄이 루트 이벤트 3"; } }
        public override EventLocation location { get { return EventLocation.TownSquare; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/NKAgent_3.txt"); } } // command list.

        protected override bool exclusiveCondition()
        { 
            return game.allEventsList.Find(e => e.eventName.Equals("봄이 루트 이벤트 2")).status == EventStatus.Completed &&
                game.allEventsList.Find(e => e.eventName.Equals("길 복구 이벤트")).status == EventStatus.Completed;
        }

        public override void Complete()
        {
            base.Complete();
            game.town.remainFoodAmount -= 20;
            game.additionalEndingOption = 0;
        }

        public NKAgent_3(Game game): base(game)
        {
            characterName = "봄이짱";
        }
    }
}