using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NKSoldier : EndingGameUnit
{
    public override int unitNumber => 9;
    public override string unitName { get { return "북한 군인"; } }
    public override int attackSpeed { get { return 2; } }
    public override int attackRange { get { return 10; } }
    public override bool isAllyUnit => false;

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

        moveSpeed = -speed;
        if (endingGame.deployedAllyUnits.FirstOrDefault() != null)
        {
            if (-endingGame.deployedAllyUnits.First().transform.position.x + transform.position.x
                >= unitSize + endingGame.deployedAllyUnits.First().GetComponent<EndingGameUnit>().unitSize)
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        else
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);


        if (endingGame.deployedAllyUnits.Count > 0)
            if (endingGame.deployedAllyUnits.Peek()?.GetComponent<EndingGameUnit>().unitNumber == 8 
                && !endingGame.deployedEnemyUnits.Any(u => u.GetComponent<EndingGameUnit>().unitNumber == 10))
            {
                endingGame.deployedEnemyUnits.Dequeue();
                Destroy(gameObject);
                if (endingGame.deployedEnemyUnits.Count == 0)
                {
                    endingGame.isInWave = false;
                    endingGame.TurnOnAndOffNextWaveButton();
                    endingGame.CleanUp();
                }
            }
    }
}
