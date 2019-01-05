using System;
using System.Collections.Generic;

namespace ISEKAI_Model
{
    public class Choice : Command
    {
        public override int commandNumber {get {return 15;}}
        public int choiceNumber {get; private set;}
        public List<ChoiceEffect> choiceList {get; private set;}

        public Choice(int choiceNumber, List<ChoiceEffect> choiceList)
        {
            this.choiceNumber = choiceNumber;
            this.choiceList = choiceList;
        }
    }
}