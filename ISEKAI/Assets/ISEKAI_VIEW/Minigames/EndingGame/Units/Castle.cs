using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Castle : EndingGameUnit
{
    public override int unitNumber => 12;
    public override string unitName { get { return "성벽"; } }
    public override int attackSpeed { get {
            switch(endingGameManager.archerUnit)
            {
                case "궁병": return 1;
                case "석궁병": return 3;
                case "석궁병(철)": return 4;
                default: return 99999;

            }
        } }
    public override int attackRange { get {
            switch (endingGameManager.archerUnit)
            {
                case "궁병": return 15;
                case "석궁병": return 5;
                case "석궁병(철)": return 15;
                default: return 0;

            }
        } }
    public override bool isAllyUnit => true;
    public override void Update()
    {
        if (endingGame.isInWave)
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
        }
    }
}
