using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class Hunting_3 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "사냥 이벤트 3"; } }
        public override EventLocation location { get { return EventLocation.BackMount; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Hunting_3.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool turnCondition = game.turn.turnNumber >= 3 && game.turn.turnNumber <= 12;
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 1;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "사냥 이벤트 2").status == EventStatus.Completed;
            if (_isFirstOccur && turnCondition && prevCondition)
            {
                _isFirstOccur = false;
                return turnCondition && prevCondition;
            }
            else
            {
                if (_isFirstOccur)
                    return false;
                return prevCondition && turnCondition && chanceCondition;
            }
        }

        private bool _isFirstOccur = true;

        public Hunting_3(Game game): base(game)
        {
            characterName = "선녀";
        }

        public override void Complete()
        {
            game.town.totalFoodProduction += 20;
            game.town.totalPleasantAmount += 5;
            game.town.remainFoodAmount += 5;
            base.Complete();
        }
    }
}