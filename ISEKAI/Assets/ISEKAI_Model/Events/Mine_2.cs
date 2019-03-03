using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Mine_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "광산 이벤트 2"; } }
        public override EventLocation location { get { return EventLocation.TechGuideStaffHouse; } }
        public override int givenMaxTurn { get { return -1; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Mine_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.allEventsList.Find(e => e.eventName.Equals("광산 이벤트 1")).status == EventStatus.Completed;
        }

        private bool _isFirstOccur = true;

        public Mine_2(Game game): base(game)
        {
            characterName = "작업반장";
        }
    }
}