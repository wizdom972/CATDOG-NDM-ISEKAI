using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISEKAI_Model;
using System.Linq;

public abstract class EndingGameUnit : MonoBehaviour
{
    public abstract int unitNumber { get; }
    public abstract string unitName { get; }
    public int hp;
    public int attackPower;
    public abstract int attackSpeed { get; }
    public abstract int attackRange { get; }
    public const int speed = 2;
    public bool isInBattleState = false;
    public abstract bool isAllyUnit { get; }

    public virtual float unitSize => 2;

    public bool isTooCloseFrontUnit { get
        {
            if (frontUnit == null)
                return false;
            else
                return (Mathf.Abs(frontUnit.transform.position.x - transform.position.x) <= (unitSize + frontUnit.unitSize) / 2);
        }
    }

    public EndingGameUnit frontUnit;
    public EndingGameManager endingGame;
    public EndingGameUnit attackTarget { get
        {
            GameObject potentialTarget; //= endingGame.deployedEnemyUnits.FirstOrDefault();
            if (isAllyUnit) potentialTarget = endingGame.deployedEnemyUnits.FirstOrDefault();
            else potentialTarget = endingGame.deployedAllyUnits.FirstOrDefault();

            if (potentialTarget == null)
                return null;
            else if (Mathf.Abs(potentialTarget.transform.position.x - transform.position.x) <= attackRange)
                return potentialTarget.GetComponent<EndingGameUnit>();
            else
                return null;
        }
    }

    public virtual void Update()
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
        {
            moveSpeed = speed;
            if (endingGame.deployedEnemyUnits.FirstOrDefault() != null)
            {
                if (endingGame.deployedEnemyUnits.First().transform.position.x - transform.position.x 
                    >= (unitSize + endingGame.deployedEnemyUnits.First().GetComponent<EndingGameUnit>().unitSize) / 2)
                    transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            }
            else
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        }
        else
        {
            moveSpeed = -speed;
            if (endingGame.deployedAllyUnits.FirstOrDefault() != null)
            {
                if (-endingGame.deployedAllyUnits.First().transform.position.x + transform.position.x
                    >= unitSize + endingGame.deployedAllyUnits.First().GetComponent<EndingGameUnit>().unitSize)
                    transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            }
            else
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        
    }

    public void Attack()
    {
        if (attackTarget == null)
            return;

        attackTarget.hp -= attackPower;
        attackTarget.transform.GetChild(0).GetComponent<TextMesh>().text = "HP: " + attackTarget.hp;
        if (attackTarget.hp <= 0)
        {
            Destroy(attackTarget.gameObject);
            if (isAllyUnit)
            {
                endingGame.deployedEnemyUnits.Dequeue();
                if (EndingGameManager.instance.isInWave && endingGame.deployedEnemyUnits.Count == 0)
                {
                    EndingGameManager.instance.isInWave = false;
                    EndingGameManager.instance.TurnOnAndOffNextWaveButton();
                    EndingGameManager.instance.CleanUp();
                }
                    
            }
            else
                endingGame.deployedAllyUnits.Dequeue();
        }
    }

}
