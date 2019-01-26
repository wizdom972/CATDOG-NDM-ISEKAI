using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IronRifleman : EndingGameUnit
{
    public override int unitNumber => 7;
    public override string unitName { get { return "소총병(철)"; } }
    public override int attackSpeed { get { return 3; } }
    public override int attackRange { get { return 20; } }
    public override bool isAllyUnit => true;
    
}
