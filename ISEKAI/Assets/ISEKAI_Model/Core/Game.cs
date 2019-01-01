using System;

namespace ISEKAI_Model
{
    public class Game
    {
        public Game() // initiallize actual game to play. An instance of Game class is one game.
        {
            turn = new Turn();
            town = new Town();
            Event.InitEvents();
            Proceed();
        }
        public const int maxAP = 4; // max AP of the game.
        public int remainAP {get; private set;} // remaining AP of the game.
        public Town town {get; private set;} // main town of the game. see Town class.
        public Turn turn {get; private set;} // indicating season, turn number, etc. see Turn class. 



        public void Proceed() // if you want to move on (next season, or next turn), just call it.
        {                          // this function returns EventType enum value, which indicates that whether bad ending happened or not, and if happend, which kind of it is.
                                   // so when using this function, you should wrap it in <if> block so that view can catch forced events.
            switch (turn.state)
            {
                case State.PreTurn:
                    _DoPreTurnBehavior();
                    turn.MoveToNextState();
                    if (town.totalPleasantAmount <= 0)
                    {
                        // TODO: Make bad ending event and set it to Visible.
                    }
                    else
                    {
                    }
                    break;

                case State.InTurn:
                    if (turn.IsFormerSeason())
                    {
                        turn.MoveToNextSeason();
                        Event.OccurEvents(this);
                    }
                    else
                    {
                        turn.MoveToNextState();
                        Proceed();
                    }
                    break;

                case State.PostTurn:
                    _DoPostTurnBehavior();
                    turn.MoveToNextState();
                    turn.MoveToNextSeason();
                    turn.IncreaseTurnNumber();
                    Event.ReduceTurnsLeft();
                    Proceed();
                    break;
            }
        }

        private void _DoPreTurnBehavior()
        {
            //Console.WriteLine ("This is PreTurn.");
            remainAP = maxAP;
            town.AddFoodProduction();
            town.ApplyPleasantChange();
            Event.OccurEvents(this);
        }
        private void _DoPostTurnBehavior()
        {
            //Console.WriteLine ("This is PostTurn");
            town.ConsumeFood();
            town.ApplyPleasantChange();
        }
    }
}