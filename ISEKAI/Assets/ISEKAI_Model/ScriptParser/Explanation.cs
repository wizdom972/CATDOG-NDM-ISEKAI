
using System;

namespace ISEKAI_Model 
{
    public class Explanation : Command
    {
        public override int commandNumber {get {return 0;}}
        public string contents {get; private set;}
        public Explanation(string contents)
        {
            this.contents = contents;
        }
    }
}