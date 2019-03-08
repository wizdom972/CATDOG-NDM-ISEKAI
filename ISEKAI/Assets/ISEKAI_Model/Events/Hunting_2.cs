using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Hunting_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "사냥 이벤트 2"; } }
        public override EventLocation location { get { return EventLocation.CarpenterHouse; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 5; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Hunting_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool turnCondition = game.turn.totalMonthNumber <= 44;
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 1;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "사냥 이벤트 1").status == EventStatus.Completed;
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

        public Hunting_2(Game game): base(game)
        {
            characterName = "목수";
        }

        public override void Complete()
        {
            game.isArrowWeaponActivated = true;
            int bowOrCrossBow = game.TryGetChoiceHistory("사냥 이벤트 2", 0);
            if (bowOrCrossBow == 0)
                game.isBowActivated = true;
            else
                game.isBowActivated = false;
            
            base.Complete();
        }
    }
}