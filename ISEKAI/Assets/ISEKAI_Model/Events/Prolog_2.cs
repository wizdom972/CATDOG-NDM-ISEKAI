using System.Collections;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class Prolog_2 : EventCore
    {
        public override int forcedEventPriority { get { return 999; } }
        public override string eventName { get { return "프롤로그 이벤트-2"; } }
        public override EventLocation location { get { return EventLocation.None; } }
        public override int givenMaxTurn { get { return 10; } }
        public override int turnsLeft { get; protected set; }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Prolog_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return true;
        }

        public Prolog_2(Game game) : base(game)
        {
            turnsLeft = 0;
        }
    }
}
