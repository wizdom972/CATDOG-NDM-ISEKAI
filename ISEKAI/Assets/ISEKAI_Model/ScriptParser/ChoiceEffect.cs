using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public enum ChoiceEffectKind
    {
        None, FoodP, Food, Morale, Steel, SteelP, Horse, HorseP
    }
    public enum ChoiceEffectType
    {
        None, Add, Subtract, Multiply, Divide
    }
    public class ChoiceEffect : Command
    {
        public override int commandNumber {get {return 16;}}
        public int choiceBranchNumber {get; private set;}
        public string choiceName {get; private set;}

        public List<(ChoiceEffectKind, ChoiceEffectType, float)> effectList {get; private set;}
        public ChoiceEffect(int choiceBranchNumber, string choiceName, List<(ChoiceEffectKind, ChoiceEffectType, float)> effectList)
        {
            this.choiceBranchNumber = choiceBranchNumber;
            this.choiceName = choiceName;
            this.effectList = effectList;
        }
    }
}