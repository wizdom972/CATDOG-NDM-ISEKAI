using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class NKAgent_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "봄이 루트 이벤트 1"; } }
        public override EventLocation location { get { return EventLocation.FrontMount; } }
        public override int givenMaxTurn { get { return 2; } }
        public override int cost { get { return 0; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/NKAgent_1.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            bool seasonCondition;
            seasonCondition = game.turn.season == Season.Winter || game.turn.season == Season.Spring;
            bool prevCondition = game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 3")).status == EventStatus.Completed;
            if (_isOccured)
                return false;
            else
            {
                _isOccured = true;
                return seasonCondition && prevCondition;
            }
        }

        private bool _isOccured = false;

        public NKAgent_1(Game game): base(game)
        {
            characterName = "선녀";
        }
    }
}