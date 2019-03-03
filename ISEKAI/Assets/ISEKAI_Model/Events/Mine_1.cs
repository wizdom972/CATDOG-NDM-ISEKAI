using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Mine_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "광산 이벤트 1"; } }
        public override EventLocation location { get { return EventLocation.TaskLeaderHouse; } }
        public override int givenMaxTurn { get { return 2; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Mine_1.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool turnCondition = game.turn.turnNumber >= 2;
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            if (_isFirstOccur && turnCondition)
            {
                _isFirstOccur = false;
                return turnCondition;
            }
            else
            {
                if (_isFirstOccur)
                    return false;
                return turnCondition && chanceCondition;
            }
        }

        private bool _isFirstOccur = true;

        public Mine_1(Game game): base(game)
        {
            characterName = "작업반장";
        }
    }
}