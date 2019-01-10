using System;

namespace ISEKAI_Model
{
    class VFXPause : Command
    {
        public override int commandNumber { get { return 18; } }

        public int second { get; private set; }

        public VFXPause(int second)
        {
            this.second = second;
        }
    }
}
