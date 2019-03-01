using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Farming_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "농사 이벤트 1"; } }
        public override EventLocation location { get { return EventLocation.Field; } }
        public override int givenMaxTurn { get { return 1; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.Spring; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Farming_1.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return true;
        }

        public Farming_1(Game game): base(game)
        {

        }
    }
}