using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IronCrossbowman : EndingGameUnit
{
    public override int unitNumber => 5;
    public override string unitName { get { return "석궁병(철)"; } }
    public override int attackSpeed { get { return 4; } }
    public override int attackRange { get { return 15; } }
    public override bool isAllyUnit => true;

}
