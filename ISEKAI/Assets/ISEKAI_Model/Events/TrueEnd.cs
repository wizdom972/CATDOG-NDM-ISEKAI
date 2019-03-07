using System;
using System.Collections.Generic;
using System.Linq;

namespace ISEKAI_Model
{
    class TrueEnd : EventCore
    {
        public override int forcedEventPriority { get { return 902; } }
        public override string eventName { get { return "TRUE 엔딩 이벤트"; } }
        public override EventLocation location { get { return EventLocation.None; } }
        public override int givenMaxTurn { get { return 10; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/TrueEnd.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.endingGameOverStatus == 0 && game.additionalEndingOption == 1;
        }

        public TrueEnd(Game game) : base(game)
        {
        }
    }
}