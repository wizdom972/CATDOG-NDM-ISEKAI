using System;

namespace ISEKAI_Model
{
    class ExampleEvent1 : Event
    {
        public override string eventName {get {return "ExampleEvent1";}}
        public override int givenMaxTurn {get {return 4;}}
        public override int turnsLeft {get; protected set;}
        public override int cost {get {return 2;}}
        public override Season availableSeason {get {return Season.Summer;}}
        public override bool isForcedEvent {get {return false;}}
        protected override bool exclusiveCondition(Game game)
        {
            bool chanceCheck;
                Random r = new Random();
                int cond = r.Next(0, 10);
                if (cond >= 0 && cond < 3)
                    chanceCheck = true;
                else
                    chanceCheck = false;
            bool foodCheck;
            if (game.town.remainFoodAmount >= 100)
                foodCheck = true;
            else
                foodCheck = false;
            return chanceCheck && foodCheck;
        }
        public ExampleEvent1()
        {
            turnsLeft = 0;
        }
        
    }
}