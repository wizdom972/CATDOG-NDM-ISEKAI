﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Knight : EndingGameUnit
{
    public override int unitNumber => 8;
    public override string unitName { get { return "기사"; } }
    public override int attackSpeed { get { return 2; } }
    public override int attackRange { get { return 1; } }
    public override bool isAllyUnit => true;
    public override float unitSize => 6;

    public override void Start()
    {
        base.Start();
        if (GameManager.instance.game.horseRaisingKnightModifier)
        {
            hp = (int)(hp * 1.5f);
            attackPower = (int)(attackPower * 1.5f);
        }
    }
}
