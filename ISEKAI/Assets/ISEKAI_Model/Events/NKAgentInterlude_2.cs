﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ISEKAI_Model
{
    public class NKAgentInterlude_2 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "봄이 루트 Interlude 2"; } }
        public override EventLocation location { get { return EventLocation.TownSquare; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 3; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/NKAgentInterlude_2.txt"); } } // command list.

        protected override bool exclusiveCondition()
        {
            return game.allEventsList.Find(e => e.eventName.Equals("봄이 루트 Interlude 1")).status == EventStatus.Completed;
        }

        public override void Complete()
        {
            base.Complete();
            game.bomRifleManHPModifier = true;
        }

        public NKAgentInterlude_2(Game game): base(game)
        {
            characterName = "봄이";
        }
    }
}