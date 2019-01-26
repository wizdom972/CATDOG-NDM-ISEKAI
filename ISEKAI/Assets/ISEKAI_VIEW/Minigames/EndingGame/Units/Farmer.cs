using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Farmer : EndingGameUnit
{
    public override int unitNumber => 0;
    public override string unitName { get { return "농민"; } }
    public override int attackSpeed { get { return 2; } }
    public override int attackRange { get { return 1; } }
    public override bool isAllyUnit => true;
}
