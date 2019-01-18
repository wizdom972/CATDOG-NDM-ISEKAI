using System;
using System.Collections.Generic;
using System.Linq;

namespace ISEKAI_Model
{
    class ExampleEvent1 : EventCore
    {
        public override string eventName {get {return "예시 이벤트";}}
        public override int givenMaxTurn {get {return 4;}}
        public override int turnsLeft {get; protected set;}
        public override int cost {get {return 2;}}
        public override Season availableSeason {get {return Season.Summer;}}
        public override int forcedEventPriority {get {return 0;}}
        public override EventLocation location { get { return EventLocation.Field; } }
        public override List<Command> script {get {return Parser.ParseScript("Assets/ISEKAI_Model/Scripts/ExampleEvent1.txt");}} // command list.
        protected override bool exclusiveCondition()
        {
            bool chanceCheck;
            Random r = new Random();
            int cond = r.Next(0, 10);
            if (cond >= 0 && cond < 3)
                chanceCheck = true;
            else
                chanceCheck = false;
            bool foodCheck;
            if (game.town.totalPleasantAmount >= 0)
                foodCheck = true;
            else
                foodCheck = false;
            return chanceCheck && foodCheck;
        }
        public ExampleEvent1(Game game) : base(game)
        {
            turnsLeft = 0;
        }
        
    }
}