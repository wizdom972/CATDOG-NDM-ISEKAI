using System.Collections;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class Ending : EventCore
    {
        public override int forcedEventPriority { get { return 1000; } }
        public override string eventName { get { return "엔딩 이벤트"; } }
        public override EventLocation location { get { return EventLocation.None; } }
        public override int givenMaxTurn { get { return 10; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Ending.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool turnCondition = game.turn.turnNumber == 13;
            bool eventCondition;
            int prevEventChoiceNum = game.TryGetChoiceHistory("북한 척후병 이벤트", 0);
            if (prevEventChoiceNum == -1 || prevEventChoiceNum == 0)
                eventCondition = false;
            else
                eventCondition = true;
            return turnCondition || eventCondition;
        }

        public Ending(Game game) : base(game)
        {
        }
    }
}
