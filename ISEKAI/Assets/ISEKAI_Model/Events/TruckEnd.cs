using System;
using System.Collections.Generic;
using System.Linq;

namespace ISEKAI_Model
{
    class TruckEnd : EventCore
    {
        public override int forcedEventPriority { get { return 901; } }
        public override string eventName { get { return "TRUCK 엔딩 이벤트"; } }
        public override EventLocation location { get { return EventLocation.None; } }
        public override int givenMaxTurn { get { return 10; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/TruckEnd.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.endingGameOverStatus == 1 && game.additionalEndingOption == 1;
        }

        public TruckEnd(Game game) : base(game)
        {
        }
    }
}