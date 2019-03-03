using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Blasphemy_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "개종 이벤트 2"; } }
        public override EventLocation location { get { return EventLocation.TownWitchHouse; } }
        public override int givenMaxTurn { get { return 2; } }
        public override int cost { get { return 3; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Blasphemy_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "개종 이벤트 1").status == EventStatus.Completed;
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

        public Blasphemy_2(Game game): base(game)
        {
            characterName = "임페리우스";
        }
        public override void Complete()
        {
            game.town.totalPleasantAmount += 10;
            game.town.pleasantChangeAddition += 20;
            base.Complete();
        }
    }
}