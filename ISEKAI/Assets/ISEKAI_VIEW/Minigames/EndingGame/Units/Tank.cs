using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tank : EndingGameUnit
{
    public override int unitNumber => 11;
    public override string unitName { get { return "T-34 탱크"; } }
    public override int attackSpeed { get { return 5; } }
    public override int attackRange { get { return 20; } }
    public override bool isAllyUnit => false;
    public override float unitSize => 6;


    /*
    public override void Update()
    {
        if (attackTarget != null && !isInBattleState)
        {
            InvokeRepeating("Attack", 0.5f, attackSpeed);
            isInBattleState = true;
        }

        if (attackTarget == null)
        {
            isInBattleState = false;
            CancelInvoke("Attack");
        }

        int moveSpeed;

        if (isAllyUnit)
            moveSpeed = speed;
        else
            moveSpeed = -speed;

        if (!isInBattleState && !isTooCloseFrontUnit)
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
    */
}
