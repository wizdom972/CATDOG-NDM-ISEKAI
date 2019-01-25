using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spearman : EndingGameUnit
{
    public override int unitNumber => 1;
    public override string unitName { get { return "창병"; } }
    public override int attackPower { get { return 10; } }
    public override int attackSpeed { get { return 2; } }
    public override int attackRange { get { return 1; } }
    public override bool isAllyUnit => true;

}
