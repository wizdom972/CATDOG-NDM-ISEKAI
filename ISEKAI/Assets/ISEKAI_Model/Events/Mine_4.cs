﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class Mine_4 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "광산 이벤트 4"; } }
        public override EventLocation location { get { return EventLocation.FrontMount; } }
        public override int givenMaxTurn { get { return -1; } }
        public override int cost { get { return 3; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/Mine_4.txt"); } } // command list.
        

        protected override bool exclusiveCondition()
        {
            return game.allEventsList.Find(e => e.eventName.Equals("광산 이벤트 3")).status == EventStatus.Completed;
        }

        public Mine_4(Game game): base(game)
        {
            characterName = "선녀";
        }
        public override void Complete()
        {
            game.isRifleActivated = true;
            game.rifleAmount += 5;

            base.Complete();
        }
    }
}