using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class CastleBuilding_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "성 쌓기 이벤트 2"; } }
        public override EventLocation location { get { return EventLocation.TechGuideStaffHouse; } }
        public override int givenMaxTurn { get { return 2; } }
        public override int cost { get { return 6; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/CastleBuilding_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 0;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "성 쌓기 이벤트 1").status == EventStatus.Completed;
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

        public CastleBuilding_2(Game game): base(game)
        {
            characterName = "작업반장";
        }

        public override void Complete()
        {           
            base.Complete();

            game.town.totalPleasantAmount -= 50;
            game.town.ConsumeFood(300);
            game.town.totalFoodProduction -= 30;
        }
    }
}