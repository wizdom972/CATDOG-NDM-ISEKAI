﻿using System;

namespace ISEKAI_Model
{
    public class Game
    {
        public Game() // initiallize actual game to play. An instance of Game class is one game.
        {
            turn = new Turn();
            town = new Town();
            EventCore.InitEvents();
            Proceed();
        }
        public const int maxAP = 4; // max AP of the game.
        public int remainAP {get; private set;} // remaining AP of the game.
        public Town town {get; private set;} // main town of the game. see Town class.
        public Turn turn {get; private set;} // indicating season, turn number, etc. see Turn class. 



        public void Proceed() // if you want to move on (next season, or next turn), just call it.
        {
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
                        EventCore.OccurEvents(this);
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
                    EventCore.ReduceTurnsLeft();
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
            EventCore.OccurEvents(this);
        }
        private void _DoPostTurnBehavior()
        {
            //Console.WriteLine ("This is PostTurn");
            town.ConsumeFood();
            town.ApplyPleasantChange();
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
    }
}