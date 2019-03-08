using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class HorseRaising_4 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "말 기르기 이벤트 4"; } }
        public override EventLocation location { get { return EventLocation.Farm; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 3; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/HorseRaising_4.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "말 기르기 이벤트 3").status == EventStatus.Completed;
            if (_isFirstOccur &&  prevCondition)
            {
                _isFirstOccur = false;
                return  prevCondition;
            }
            else
            {
                if (_isFirstOccur)
                    return false;
                return prevCondition &&  chanceCondition;
            }
        }

        private bool _isFirstOccur = true;

        public HorseRaising_4(Game game): base(game)
        {
            characterName = "통계원";
        }

        public override void Complete()
        {
            base.Complete();
            game.horseRaisingKnightModifier = true;
        }
    }
}