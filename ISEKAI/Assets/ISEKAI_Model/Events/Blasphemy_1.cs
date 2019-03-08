using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Blasphemy_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "개종 이벤트 1"; } }
        public override EventLocation location { get { return EventLocation.TownWitchHouse; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Blasphemy_1.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool turnCondition = game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 3")).status == EventStatus.Completed;
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

        public Blasphemy_1(Game game): base(game)
        {
            characterName = "임페리우스";
        }

        public override void Complete()
        {
            game.town.totalPleasantAmount += 5;
            base.Complete();
        }
    }
}