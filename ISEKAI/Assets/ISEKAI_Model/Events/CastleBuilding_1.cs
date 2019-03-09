using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class CastleBuilding_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "성 쌓기 이벤트 1"; } }
        public override EventLocation location { get { return EventLocation.TaskLeaderHouse; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 5; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/CastleBuilding_1.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 0;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "농사 이벤트 3").status == EventStatus.Completed &&
                game.allEventsList.Find(e => e.eventName.Equals("당의 귀환 경고")).status == EventStatus.Completed;
            if (_isFirstOccur && prevCondition)
            {
                _isFirstOccur = false;
                return prevCondition;
            }
            else
            {
                if (_isFirstOccur)
                    return false;
                return prevCondition && chanceCondition;
            }
        }

        private bool _isFirstOccur = true;

        public CastleBuilding_1(Game game): base(game)
        {
            characterName = "작업반장";
        }

        public override void Complete()
        {           
            base.Complete();

            game.town.totalPleasantAmount -= 40;
            game.town.ConsumeFood(200);
            game.town.totalFoodProduction -= 30;
        }
    }
}