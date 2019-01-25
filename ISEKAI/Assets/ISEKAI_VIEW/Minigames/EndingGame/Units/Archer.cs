using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Archer : EndingGameUnit
{
    public override int unitNumber => 3;
    public override string unitName { get { return "궁병"; } }
    public override int attackPower { get { return 6; } }
    public override int attackSpeed { get { return 1; } }
    public override int attackRange { get { return 20; } }
    public override bool isAllyUnit => true;

}
