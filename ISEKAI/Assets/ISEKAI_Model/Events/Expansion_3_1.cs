using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class Expansion_3_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "확장 이벤트 3-1"; } }
        public override EventLocation location { get { return EventLocation.WayToTown; } }
        public override int givenMaxTurn { get { return 2; } }
        public override int cost { get { return 5; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Expansion_3_1.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            bool prevCondition = game.allEventsList.Find(e => e.eventName == "확장 이벤트 2").status == EventStatus.Completed;
            bool choiceCondition = game.TryGetChoiceHistory("확장 이벤트 2", 0) == 0;
            if (_isFirstOccur &&  prevCondition && choiceCondition)
            {
                _isFirstOccur = false;
                return  prevCondition;
            }
            else
            {
                if (_isFirstOccur)
                    return false;
                return prevCondition && chanceCondition && choiceCondition;
            }
        }

        private bool _isFirstOccur = true;

        public Expansion_3_1(Game game): base(game)
        {
            characterName = "마을 사람";
        }

        public override void Complete()
        {
            base.Complete();
            game.expansion1Modifier = true;
        }
    }
}