using System;
using System.Collections.Generic;
using System.Linq;

namespace ISEKAI_Model
{
    class DeadEnd : EventCore
    {
        public override int forcedEventPriority { get { return 1500; } }
        public override string eventName { get { return "DEAD END"; } }
        public override EventLocation location { get { return EventLocation.None; } }
        public override int givenMaxTurn { get { return 10; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/DeadEnd.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.town.totalPleasantAmount <= 0;
        }

        public DeadEnd(Game game) : base(game)
        {
        }
    }
}