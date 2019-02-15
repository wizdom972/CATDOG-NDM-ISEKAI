using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Mine_3 : EventCore, IMinigamePlayable
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "광산 이벤트 3"; } }
        public override EventLocation location { get { return EventLocation.FrontMount; } }
        public override int givenMaxTurn { get { return -1; } }
        public override int cost { get { return 3; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Mine_3.txt"); } } // command list.

        public int playerScore { get; set; }

        public void DoMinigameBehavior()
        {
            game.town.totalIronAmount += playerScore;
        }

        protected override bool exclusiveCondition()
        {
            return game.allEventsList.Find(e => e.eventName.Equals("광산 이벤트 2")).status == EventStatus.Completed;
        }

        private bool _isFirstOccur = true;

        public Mine_3(Game game): base(game)
        {

        }
        public override void Complete()
        {
            game.isIronActivated = true;
            game.isMineUnlocked = true;
            DoMinigameBehavior();

            base.Complete();
        }
    }
}