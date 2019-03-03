using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class NKAgent_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "봄이 루트 이벤트 2"; } }
        public override EventLocation location { get { return EventLocation.TaskLeaderHouse; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 2; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/NKAgent_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool choiceCondition = game.TryGetChoiceHistory("봄이 루트 이벤트 1", 0) == 0;
            bool prevCondition = game.allEventsList.Find(e => e.eventName.Equals("봄이 루트 이벤트 1")).status == EventStatus.Completed;

            if (_isOccured)
                return false;
            else
            {
                _isOccured = true;
                return choiceCondition && prevCondition;
            }

            
        }

        private bool _isOccured = false;

        public override void Complete()
        {
            base.Complete();
            game.town.remainFoodAmount -= 20;
        }

        public NKAgent_2(Game game): base(game)
        {
            characterName = "봄이짱";
        }
    }
}