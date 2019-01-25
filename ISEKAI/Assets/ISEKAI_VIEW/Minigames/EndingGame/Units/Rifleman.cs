using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rifleman : EndingGameUnit
{
    public override int unitNumber => 6;
    public override string unitName { get { return "소총병"; } }
    public override int attackPower { get { return 20; } }
    public override int attackSpeed { get { return 3; } }
    public override int attackRange { get { return 20; } }
    public override bool isAllyUnit => true;
    
}
