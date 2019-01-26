using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NKLieutenant : EndingGameUnit
{
    public override int unitNumber => 10;
    public override string unitName { get { return "북한 장교"; } }
    public override int attackSpeed { get { return 2; } }
    public override int attackRange { get { return 10; } }
    public override bool isAllyUnit => false;

    private void Start()
    {
        foreach(GameObject unit in endingGame.deployedEnemyUnits)
        {
            if (unit.GetComponent<EndingGameUnit>().unitNumber == 10)
                return;

            unit.GetComponent<EndingGameUnit>().attackPower *= 2;
        }
    }

}
