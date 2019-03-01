using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Mine_Repeat : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "철광석 캐기 이벤트"; } }
        public override EventLocation location { get { return EventLocation.Mine; } }
        public override int givenMaxTurn { get { return -1; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Mine_Repeat.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.allEventsList.Find(e => e.eventName.Equals("광산 이벤트 3")).status == EventStatus.Completed;
        }

        public Mine_Repeat(Game game): base(game)
        {

        }

        public override void Complete()
        {
            base.Complete();
            status = EventStatus.Visible;
        }
    }
}