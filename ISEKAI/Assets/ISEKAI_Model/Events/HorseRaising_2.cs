using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class HorseRaising_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "말 기르기 이벤트 2"; } }
        public override EventLocation location { get { return EventLocation.BackMount; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/HorseRaising_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool turnCondition = game.turn.turnNumber >= 3 && game.turn.turnNumber <= 10;
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "말 기르기 이벤트 1").status == EventStatus.Completed;
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

        public HorseRaising_2(Game game): base(game)
        {
            characterName = "통계원";
        }
    }
}