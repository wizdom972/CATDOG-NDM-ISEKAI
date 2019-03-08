using System.Collections;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class Farming_3 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "농사 이벤트 3"; } }
        public override EventLocation location { get { return EventLocation.Field; } }
        public override int givenMaxTurn { get { return 1; } }
        public override int cost { get { return 3; } }
        public override Season availableSeason { get { return Season.Spring; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Farming_3.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 2")).status == EventStatus.Completed;
        }

        public Farming_3(Game game) : base(game)
        {
            characterName = "선녀";
        }
    }
}
