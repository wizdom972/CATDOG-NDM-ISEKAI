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
    public abstract int attackPower { get; }
    public abstract int attackSpeed { get; }
    public abstract int attackRange { get; }
    public const int speed = 2;
    public bool isInBattleState = false;
    public abstract bool isAllyUnit { get; }

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

        if (!isInBattleState)
            transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    public void Attack()
    {
        if (attackTarget == null)
            return;

        attackTarget.hp -= attackPower;
        attackTarget.transform.GetChild(0).GetComponent<TextMesh>().text = "HP: " + attackTarget.hp;
        if (attackTarget.hp <= 0)
        {
            if (isAllyUnit)
                endingGame.deployedEnemyUnits.Dequeue();
            else
                endingGame.deployedAllyUnits.Dequeue();

            Destroy(attackTarget);
        }
    }

    /*public void TryEnterBattleState()
    {
        isInBattleState = true;
        if (attackTarget == null)
        {
            isInBattleState = false;
            return;
        }
        StartCoroutine(StartBattle());
    }

    public virtual IEnumerator StartBattle()
    {
        while(attackTarget != null)
        {
            attackTarget.hp -= attackPower;
            if (attackTarget.hp <= 0)
            {
                if (isAllyUnit)
                    endingGame.deployedEnemyUnits.Dequeue();
                else
                    endingGame.deployedAllyUnits.Dequeue();

                Destroy(attackTarget.gameObject);
                ExitBattleState();
                yield break;
            }
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    public void ExitBattleState()
    {
        isInBattleState = false;
    }*/
}
