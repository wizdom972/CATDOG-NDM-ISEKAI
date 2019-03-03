using System.Collections;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class ReturnWarning : EventCore
    {
        public override int forcedEventPriority { get { return 1000; } }
        public override string eventName { get { return "당의 귀환 경고 이벤트"; } }
        public override EventLocation location { get { return EventLocation.None; } }
        public override int givenMaxTurn { get { return 10; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/ReturnWarning.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.turn.year == 1997;
        }

        public ReturnWarning(Game game) : base(game)
        {
        }
    }
}
