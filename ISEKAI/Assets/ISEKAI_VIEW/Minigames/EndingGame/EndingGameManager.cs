using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISEKAI_Model;

public class EndingGameManager : MonoBehaviour
{
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        game = GameManager.instance.game;
        _InitWaves();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject unitPrefab;

    public static EndingGameManager instance;
    public Game game;
    public Queue<string> productionQueue = new Queue<string>();
    public List<GameObject> deployedAllyUnits = new List<GameObject>();
    public List<GameObject> deployedEnemyUnits = new List<GameObject>();
    public List<Queue<string>> waves = new List<Queue<string>>();

    private void _InitWaves()
    {
        var wave0 = new Queue<string>();
        for (int i = 0; i < 5; ++i)
            wave0.Enqueue("북한 군인");
        var wave1 = new Queue<string>();
        wave1.Enqueue("북한 군인");
        wave1.Enqueue("북한 군인");
        wave1.Enqueue("북한 장교");
        wave1.Enqueue("북한 군인");
        wave1.Enqueue("북한 군인");
        var wave2 = new Queue<string>();
        wave2.Enqueue("북한 군인");
        wave2.Enqueue("북한 군인");
        wave2.Enqueue("북한 장교");
        wave2.Enqueue("북한 군인");
        wave2.Enqueue("북한 군인");
        wave2.Enqueue("T-34 탱크");
        waves.Add(wave0);
        waves.Add(wave1);
        waves.Add(wave2);
    }

    public void MakeUnit(string unitName)
    {
        var unitObject = Instantiate(unitPrefab);
        var unitInfo = unitObject.GetComponent<EndingGameUnit>();
        switch(unitName)
        {
            /*case "농민":
                unitInfo.attackPower = 5;
                unitInfo.attackRange = 1;
                unitInfo.attackSpeed = 2;
                unitInfo.hp = 15;*/

        }
    }
}
