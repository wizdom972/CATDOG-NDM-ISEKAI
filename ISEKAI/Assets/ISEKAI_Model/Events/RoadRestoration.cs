using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class RoadRestoration : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "길 복구 이벤트"; } }
        public override EventLocation location { get { return EventLocation.WayToTown; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/RoadRestoration.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool pleasantCondition = game.town.totalPleasantAmount >= 150;
            bool foodCondition = game.town.maxFoodConsumption <= game.town.totalFoodConsumption;
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            if (_isFirstOccur && pleasantCondition && foodCondition)
            {
                _isFirstOccur = false;
                return pleasantCondition && foodCondition;
            }
            else
            {
                if (_isFirstOccur)
                    return false;
                return pleasantCondition && foodCondition && chanceCondition;
            }
        }

        private bool _isFirstOccur = true;

        public RoadRestoration(Game game): base(game)
        {
            characterName = "통계원";
        }

        public override void Complete()
        {
            game.town.totalFoodProduction += 5;
            game.town.totalPleasantAmount -= 20;
            base.Complete();
        }
    }
}