using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class HorseRaising_1 : EventCore
    {
        public override int forcedEventPriority { get { return 0; } }
        public override string eventName { get { return "말 기르기 이벤트 1"; } }
        public override EventLocation location { get { return EventLocation.TaskLeaderHouse; } }
        public override int givenMaxTurn { get { return 3; } }
        public override int cost { get { return 1; } }
        public override Season availableSeason { get { return Season.None; } }
        public override List<Command> script { get { return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/HorseRaising_1.txt"); } } // command list.
        
        protected override bool exclusiveCondition()
        {
            bool prevCondition = game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 3")).status == EventStatus.Completed;
            int chance = (new Random()).Next() / 10;
            bool chanceCondition = chance <= 2;
            return prevCondition && chanceCondition;
        }

        public HorseRaising_1(Game game): base(game)
        {
            characterName = "작업반장";
        }
    }
}