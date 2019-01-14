using System;
using System.Collections.Generic;
using System.Linq;

namespace ISEKAI_Model
{
    public class Game
    {
        public Game() // initiallize actual game to play. An instance of Game class is one game.
        {
            turn = new Turn();
            town = new Town();
            _InitEvents();
            Proceed();
        }
        public const int maxAP = 4; // max AP of the game.
        public int remainAP {get; set;} // remaining AP of the game.
        public Town town {get; private set;} // main town of the game. see Town class.
        public Turn turn {get; private set;} // indicating season, turn number, etc. see Turn class. 

        public List<EventCore> allEventsList = new List<EventCore>();

        public List<EventCore> visibleEventsList => allEventsList.FindAll(e => e.status == EventStatus.Visible);

        public void Proceed() // if you want to move on (next season, or next turn), just call it.
        {
            switch (turn.state)
            {
                case State.PreTurn:
                    _DoPreTurnBehavior();
                    turn.MoveToNextState();
                    break;

                case State.InTurn:
                    if (turn.IsFormerSeason())
                    {
                        turn.MoveToNextSeason();
                        _OccurEvents();
                    }
                    else
                    {
                        turn.MoveToNextState();
                        Proceed();
                    }
                    break;

                case State.PostTurn:
                    _DoPostTurnBehavior();
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
            _SetAllEventActivable();
            _OccurEvents();
        }
        private void _DoPostTurnBehavior()
        {
            //Console.WriteLine ("This is PostTurn");
            town.ConsumeFood();
            town.ApplyPleasantChange();
            turn.MoveToNextState();
            turn.MoveToNextSeason();
            turn.IncreaseTurnNumber();
            _ReduceEveryEventsTurnsLeft();
        }

        public void ApplyChoiceEffect(ChoiceEffect choiceEffect)
        {
            foreach((ChoiceEffectKind, ChoiceEffectType, float) effect in choiceEffect.effectList)
            {
                ChoiceEffectType type = effect.Item2;
                float f = effect.Item3;
                switch (effect.Item1)
                {
                    case ChoiceEffectKind.Food:
                        town.remainFoodAmount = ApplyChoiceEffect(town.remainFoodAmount, type, f);
                        break;
                    case ChoiceEffectKind.FoodP:
                        town.totalFoodProduction = ApplyChoiceEffect(town.totalFoodProduction, type, f);
                        break;
                    case ChoiceEffectKind.Horse:
                        town.totalHorseAmount = ApplyChoiceEffect(town.totalHorseAmount, type, f);
                        break;
                    case ChoiceEffectKind.HorseP:
                        town.totalHorseProduction = ApplyChoiceEffect(town.totalHorseProduction, type, f);
                        break;
                    case ChoiceEffectKind.Morale:
                        town.totalPleasantAmount = ApplyChoiceEffect(town.totalPleasantAmount, type, f);
                        break;
                    case ChoiceEffectKind.Steel:
                        town.totalIronAmount = ApplyChoiceEffect(town.totalIronAmount, type, f);
                        break;
                    case ChoiceEffectKind.SteelP:
                        town.totalIronProduction = ApplyChoiceEffect(town.totalIronProduction, type, f);
                        break;
                    default:
                        break;
                }
            }
        }
        private float ApplyChoiceEffect(float toChange, ChoiceEffectType type, float f)
        {
            float result;
            switch (type)
            {
                case ChoiceEffectType.Add:
                    result = toChange + f;
                    return result;
                case ChoiceEffectType.Divide:
                    result = toChange / f;
                    return result;
                case ChoiceEffectType.Multiply:
                    result = toChange * f;
                    return result;
                case ChoiceEffectType.Subtract:
                    result = toChange - f;
                    return result;
                default:
                    throw new InvalidOperationException("Error on ApplyChoiceEffect()");
            }
        }

        private void _ReduceEveryEventsTurnsLeft() // Not recommended to call manually. Only called by Proceed().
        {
            foreach (EventCore e in allEventsList)
            {
                if(e.isActivatedAlready)
                    e.ReduceTurnsLeft();

                if (e.turnsLeft <= 0 && e.isActivatedAlready)
                {
                    e.status = EventStatus.Ready;
                    e.isActivatedAlready = false;
                    e.isRemovedLastTurn = true;
                }
            }
        }

        private void _SetAllEventActivable()
        {
            foreach (EventCore e in allEventsList)
                e.isRemovedLastTurn = false;
        }

        private void _OccurEvents() // Not recommended to call manually. Only called by Proceed().
        {
            foreach (EventCore e in allEventsList)
            {
                if (e.IsFirstVisible() && 
                    e.status == EventStatus.Ready && 
                    !e.isActivatedAlready)
                {
                    e.ActivateEvent();
                }
                else if (e.isActivatedAlready)
                {
                    if (e.seasonCheck() && e.turnsLeft > 0)
                        e.status = EventStatus.Visible;
                    else
                        e.status = EventStatus.Ready;
                }
            }
        }

        private void _InitEvents() // should add EVERY events when new event plan comes.
        {
            allEventsList.Add(new ExampleEvent1(this));
        }
    }
}