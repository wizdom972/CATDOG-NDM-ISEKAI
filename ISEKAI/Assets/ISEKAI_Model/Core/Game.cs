using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ISEKAI_Model
{
    public class Game
    {
        public Game() // initiallize actual game to play. An instance of Game class is one game.
        {
            turn = new Turn();
            town = new Town();
            _InitEvents();
            OccurEvents();
            Proceed(turn.season);
        }
        public Town town {get; private set;} // main town of the game. see Town class.
        public Turn turn {get; private set; } // indicating season, turn number, etc. see Turn class.

        public bool isMineUnlocked = false;
        public bool isFarmUnlocked = false;

        public bool isIronActivated = false;
        public bool isHorseActivated = false;
        public bool isArrowWeaponActivated = false;
        public bool isBowActivated = false;
        public bool isRifleActivated = false;
        public bool isKnightActivated = false;

        public bool bomRifleManHPModifier = false;
        public bool bomRifleManAtkModifier = false;
        public bool horseRaisingKnightModifier = false;
        public bool expansion1Modifier = false;
        public bool expansion2Modifier = false;


        public int additionalEndingOption = -1; // -1 if nothing. 0 if NKAgent_3 cleared. 1 if NKAgent_5 cleared.
        public int endingGameOverStatus = -1; // -1 if not yet played, 0 if won, 1 if lost.

        public int rifleAmount = 0;
        public int castleHP = 0;

        public Dictionary<string, List<(int, int)>> choiceHistories = new Dictionary<string, List<(int, int)>>(); // <item1>th choice, selected <item2>th branch. (0-based)
        public List<EventCore> allEventsList = new List<EventCore>();
        public List<EventCore> forcedVisibleEventList { get
            {
                var lst = allEventsList.FindAll(e => e.status == EventStatus.ForcedVisible);
                _SortForcedEventList(lst);
                return lst;
            } }
        public List<EventCore> visibleEventsList => allEventsList.FindAll(e => e.status == EventStatus.Visible);

        public int TryGetChoiceHistory(string eventName, int choiceNumber) // returns -1 if it couldn't find that event.
        {
            if (!choiceHistories.ContainsKey(eventName))
                return -1;
            else
            {
                foreach ((int, int) his in choiceHistories[eventName])
                {
                    if (his.Item1 == choiceNumber)
                        return his.Item2;
                }
                throw new InvalidOperationException("The event has no choice whose number is " + choiceNumber);
            }
        }

        private void _InitEvents() // should add EVERY events when new event plan comes.
        {
            allEventsList.Add(new Prolog_1(this));
            allEventsList.Add(new Prolog_2(this));

            allEventsList.Add(new ReturnWarning(this));

            allEventsList.Add(new DeadEnd(this));
            allEventsList.Add(new BadEnd(this));
            allEventsList.Add(new GoodEnd(this));
            allEventsList.Add(new TruckEnd(this));
            allEventsList.Add(new TrueEnd(this));
            allEventsList.Add(new MysteryEnd(this));

            allEventsList.Add(new Ending(this));

            allEventsList.Add(new Farming_1(this));
            allEventsList.Add(new Farming_2(this));
            allEventsList.Add(new Farming_3(this));

            allEventsList.Add(new NKScout(this));

            allEventsList.Add(new NKAgent_1(this));
            allEventsList.Add(new NKAgent_2(this));
            allEventsList.Add(new NKAgent_3(this));
            allEventsList.Add(new NKAgent_4(this));
            allEventsList.Add(new NKAgentInterlude_1(this));
            allEventsList.Add(new NKAgentInterlude_2(this));

            allEventsList.Add(new HorseRaising_1(this));
            allEventsList.Add(new HorseRaising_2(this));
            allEventsList.Add(new HorseRaising_3(this));
            allEventsList.Add(new HorseRaising_4(this));

            allEventsList.Add(new Hunting_1(this));
            allEventsList.Add(new Hunting_2(this));
            allEventsList.Add(new Hunting_3(this));

            allEventsList.Add(new Mine_1(this));
            allEventsList.Add(new Mine_2(this));
            allEventsList.Add(new Mine_3(this));
            allEventsList.Add(new Mine_4(this));
            allEventsList.Add(new Mine_Repeat(this));

            allEventsList.Add(new Blasphemy_1(this));
            allEventsList.Add(new Blasphemy_2(this));

            allEventsList.Add(new CastleBuilding_1(this));
            allEventsList.Add(new CastleBuilding_2(this));

            allEventsList.Add(new RoadRestoration(this));
            allEventsList.Add(new Expansion_1(this));
            allEventsList.Add(new Expansion_2(this));
            allEventsList.Add(new Expansion_3_1(this));
            allEventsList.Add(new Expansion_3_2(this));
        }

        private int _HowManySeasonsHavePassed(Season before, Season after)
        {
            if (after >= before)
                return after - before;
            else
                return after - before + 4;
        }

        public void Proceed(Season startSeason) // from startSeason to current season, it Proceeds.
        {
            Season _startSeason = startSeason;

            for (int i = 0; i < _HowManySeasonsHavePassed(_startSeason, turn.season); i++)
            {
                switch (turn.state)
                {
                    case State.PreTurn:
                        _DoPreTurnBehavior();
                        turn.MoveToNextState();
                        break;
                    case State.InTurn:
                        if (_startSeason == Season.Winter || _startSeason == Season.Summer)
                        {
                            _startSeason = _MoveToNextSeason(_startSeason);
                            break;
                        }
                        else
                        {
                            turn.MoveToNextState();
                            --i;
                            break;
                        }
                    case State.PostTurn:
                        _DoPostTurnBehavior();
                        _startSeason = _MoveToNextSeason(_startSeason);
                        --i;
                        break;
                }
            }



            /*
            Debug.Log(turn.state + " " + turn.season);
            switch (turn.state)
            {
                case State.PreTurn:
                    _DoPreTurnBehavior();
                    turn.MoveToNextState();
                    break;

                case State.InTurn:
                    if (turn.IsFormerSeason())
                    {
                        //turn.MoveToNextSeason();
                        //OccurEvents();
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
            */
        }

        private Season _MoveToNextSeason(Season season)
        {
            switch (season)
            {
                case Season.Winter:
                    return Season.Spring;

                case Season.Spring:
                    return Season.Summer;

                case Season.Summer:
                    return Season.Autumn;

                case Season.Autumn:
                    return Season.Winter;
                default:
                    throw new InvalidOperationException("asdfadf");
            }
        }

        private void _DoPreTurnBehavior()
        {
            town.ApplyPreTurnChange();
            _SetAllEventActivable();
        }
        private void _DoPostTurnBehavior()
        {
            town.ApplyPostTurnChange();
            turn.IncreaseTurnNumber();
            //_ReduceEveryEventsTurnsLeft();
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
                    throw new InvalidOperationException(toChange.ToString() + " " + type.ToString() +  " " + f.ToString());
            }
        }

        /*private void _ReduceEveryEventsTurnsLeft() // Not recommended to call manually. Only called by Proceed().
        {
            foreach (EventCore e in allEventsList)
            {
                if (e.givenMaxTurn < 0)
                    continue;
                if (e.status == EventStatus.Completed)
                    continue;
                if(e.isActivatedAlready)
                    e.ReduceTurnsLeft();

                if (e.turnsLeft <= 0 && e.isActivatedAlready)
                {
                    e.status = EventStatus.Ready;
                    e.isActivatedAlready = false;
                    e.isRemovedLastTurn = true;
                }
            }
        }*/

        public void _ReduceEveryEventsMonthsLeft(int monthsPassed) // Not recommended to call manually. Only called by Proceed().
        {
            foreach (EventCore e in allEventsList)
            {
                if (e.givenMaxMonth < 0)
                    continue;
                if (e.status == EventStatus.Completed)
                    continue;
                if (e.isActivatedAlready)
                    e.ReduceMonthsLeft(monthsPassed);

                if (e.monthsLeft <= 0 && e.isActivatedAlready)
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

        public void OccurEvents()
        {
            foreach (EventCore e in allEventsList)
            {
                if (e.status == EventStatus.Completed)
                    continue;
                if (e.isForcedEvent && e.IsFirstVisible())
                {
                    e.status = EventStatus.ForcedVisible;
                    continue;
                }
                if (e.IsFirstVisible() && 
                    e.status == EventStatus.Ready && 
                    !e.isActivatedAlready)
                {
                    e.ActivateEvent();
                }
                else if (e.isActivatedAlready)
                {
                    if (e.SeasonCheck() && e.monthsLeft > 0)
                        e.status = EventStatus.Visible;
                    else
                        e.status = EventStatus.Ready;
                }
            }
        }

        private void _SortForcedEventList(List<EventCore> lst)
        {
            lst.Sort(delegate (EventCore e1, EventCore e2) { return e2.forcedEventPriority.CompareTo(e1.forcedEventPriority); });
        }
    }
}