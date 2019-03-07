using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Hunting_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "사냥 이벤트 1"; } }
        public override EventLocation location { get { return EventLocation.BackMount; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Hunting_1.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool turnCondition = game.turn.totalMonthNumber >= 13 && game.turn.turnNumber <= 9;
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 1;
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

        public Hunting_1(Game game): base(game)
        {
            characterName = "마을 사람";
        }

        public override void Complete()
        {
            game.town.totalFoodProduction += 5;
            game.town.totalPleasantAmount += 5;
            game.town.remainFoodAmount += 20;
            base.Complete();
        }
    }
}