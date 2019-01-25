using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crossbowman : EndingGameUnit
{
    public override int unitNumber => 4;
    public override string unitName { get { return "석궁병"; } }
    public override int attackPower { get { return 10; } }
    public override int attackSpeed { get { return 3; } }
    public override int attackRange { get { return 5; } }
    public override bool isAllyUnit => true;

}
