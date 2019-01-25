using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISEKAI_Model;
using System;
using UnityEngine.UI;
using System.Linq;

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
    public Slider progressBar;

    void Start()
    {
        game = new Game();//GameManager.instance.game;
        
        game.town.remainFoodAmount += 1000;
        game.isIronActivated = true;
        game.isHorseActivated = true;
        game.town.totalIronAmount = 100;
        game.town.totalHorseAmount = 100;
        game.isArrowWeaponActivated = true;
        game.isBowActivated = false;
        game.isRifleActivated = true;
        
        InitGameInfo();
        UpdatePanel();
        StartCoroutine(StartMakingUnits());
    }

    public Transform unitPrefab;

    public static EndingGameManager instance;
    public Game game;
    public Queue<string> productionQueue = new Queue<string>();
    public Queue<GameObject> deployedAllyUnits = new Queue<GameObject>();
    public Queue<GameObject> deployedEnemyUnits = new Queue<GameObject>();
    public List<Queue<string>> waves = new List<Queue<string>>();

    public Image[] productionQueueImage;

    public int riflemanCount = 3;
    public int currentWaveNumber = 0;

    public const int AllyStartPosition = -25;
    public const int EnemyStartPosition = 25;

    public Text food, iron, horse, wave;

    public Button meleeButton, archerButton, riflemanButton, knightButton;
    public string meleeUnit, archerUnit, rifleUnit, knightUnit = "기사";
    public void UpdatePanel()
    {
        food.text = "음식: " + game.town.remainFoodAmount;
        iron.text = "철: " + game.town.totalIronAmount;
        horse.text = "말: " + game.town.totalHorseAmount;
        wave.text = (currentWaveNumber + 1) + "번째 공세";
        _CheckProducibleUnits();
    }

    private void _UpdateProductionQueue()
    {
        for (int i = 0; i < productionQueue.Count; i++)
            productionQueueImage[i].color = Color.red;
        for (int i = productionQueue.Count; i < 5; i++)
            productionQueueImage[i].color = Color.white;
    }

    public void InitGameInfo()
    {

        if (game.town.remainFoodAmount >= 500 && game.town.totalIronAmount >= 30)
            meleeUnit = "파이크병";
        else if (game.isIronActivated)
            meleeUnit = "창병";
        else
            meleeUnit = "농민";

        if (!game.isArrowWeaponActivated)
            archerButton.gameObject.SetActive(false);
        else if (game.isBowActivated)
            archerUnit = "궁병";
        else if (game.isIronActivated)
            archerUnit = "석궁병(철)";
        else
            archerUnit = "석궁병";

        if (!game.isRifleActivated)
            riflemanButton.gameObject.SetActive(false);
        else if (game.isIronActivated)
            rifleUnit = "소총병(철)";
        else
            rifleUnit = "소총병";

        if (!game.isIronActivated || !game.isHorseActivated)
            knightButton.gameObject.SetActive(false);

        _InitWaves();
    }

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
    public void MakeAllyUnit(string unitName)
    {
        var unitObject = Instantiate(unitPrefab.GetChild(_GetUnitNumber(unitName)), new Vector3(AllyStartPosition, -4f, 0), Quaternion.identity);
        unitObject.GetComponent<EndingGameUnit>().endingGame = this;
        unitObject.gameObject.SetActive(true);
        unitObject.GetChild(0).GetComponent<TextMesh>().text = "HP: " + unitObject.GetComponent<EndingGameUnit>().hp;
        deployedAllyUnits.Enqueue(unitObject.gameObject);
    }
    private bool _IsUnitProducible(string unitName)
    {
        switch (unitName)
        {
            case "농민":
                return game.town.remainFoodAmount >= 20;
            case "창병":
                return game.town.remainFoodAmount >= 20 && game.town.totalIronAmount >= 1;
            case "파이크병":
                return game.town.remainFoodAmount >= 50 && game.town.totalIronAmount >= 1;
            case "궁병":
                return game.town.remainFoodAmount >= 30;
            case "석궁병":
                return game.town.remainFoodAmount >= 20;
            case "석궁병(철)":
                return game.town.remainFoodAmount >= 20 && game.town.totalIronAmount >= 2;
            case "소총병":
                return game.town.remainFoodAmount >= 35 && riflemanCount > 0;
            case "소총병(철)":
                return game.town.remainFoodAmount >= 35 && game.town.totalIronAmount >= 2 && riflemanCount > 0;
            case "기사":
                return game.town.remainFoodAmount >= 40 && game.town.totalIronAmount >= 10 && game.town.totalHorseAmount >= 1;
            default:
                throw new InvalidOperationException("Unit " + unitName + " has not implemented.");
        }
    }
    private int _GetProductionTime(string unitName)
    {
        switch(unitName)
        {
            case "농민":
                return 1;
            case "창병":
                return 2;
            case "파이크병":
                return 3;
            case "궁병":
                return 5;
            case "석궁병":
                return 3;
            case "석궁병(철)":
                return 4;
            case "소총병":
                return 5;
            case "소총병(철)":
                return 6;
            case "기사":
                return 5;
            default:
                throw new InvalidOperationException("Unit " + unitName + " has not implemented.");
        }
    }

    private int _GetUnitNumber(string unitName)
    {
        switch (unitName)
        {
            case "농민":
                return 0;
            case "창병":
                return 1;
            case "파이크병":
                return 2;
            case "궁병":
                return 3;
            case "석궁병":
                return 4;
            case "석궁병(철)":
                return 5;
            case "소총병":
                return 6;
            case "소총병(철)":
                return 7;
            case "기사":
                return 8;
            case "북한 군인":
                return 9;
            case "북한 장교":
                return 10;
            case "T-34 탱크":
                return 11;
            default:
                throw new InvalidOperationException("Unit " + unitName + " has not implemented.");
        }
    }

    private void _ConsumeResource(string unitName)
    {
        switch (unitName)
        {
            case "농민":
                game.town.remainFoodAmount -= 20;
                break;
            case "창병":
                game.town.remainFoodAmount -= 20;
                game.town.totalIronAmount -= 1;
                break;
            case "파이크병":
                game.town.remainFoodAmount -= 50;
                game.town.totalIronAmount -= 1;
                break;
            case "궁병":
                game.town.remainFoodAmount -= 30;
                break;
            case "석궁병":
                game.town.remainFoodAmount -= 20;
                break;
            case "석궁병(철)":
                game.town.remainFoodAmount -= 20;
                game.town.totalIronAmount -= 2;
                break;
            case "소총병":
                game.town.remainFoodAmount -= 35;
                --riflemanCount;
                break;
            case "소총병(철)":
                game.town.remainFoodAmount -= 35;
                game.town.totalIronAmount -= 2;
                --riflemanCount;
                break;
            case "기사":
                game.town.remainFoodAmount -= 40;
                game.town.totalIronAmount -= 10;
                game.town.totalHorseAmount -= 1;
                break;
        }
    }

    private void _CheckProducibleUnits()
    {
        if (_IsUnitProducible(meleeUnit))
            meleeButton.interactable = true;
        else
            meleeButton.interactable = false;
        
        if(archerButton.gameObject.activeSelf)
        {
            if (_IsUnitProducible(archerUnit))
                archerButton.interactable = true;
            else
                archerButton.interactable = false;
        }
        if (riflemanButton.gameObject.activeSelf)
        {
            if (_IsUnitProducible(rifleUnit))
                riflemanButton.interactable = true;
            else
                riflemanButton.interactable = false;
        }
        if (knightButton.gameObject.activeSelf)
        {
            if (_IsUnitProducible(knightUnit))
                knightButton.interactable = true;
            else
                knightButton.interactable = false;
        }
    }

    private void _PutUnitIntoQueue(string unitName)
    {
        if (productionQueue.Count >= 5)
            return;
        _ConsumeResource(unitName);
        UpdatePanel();
        productionQueue.Enqueue(unitName);
        _UpdateProductionQueue();
    }

    public void OnClickMeleeButton()
    {
        _PutUnitIntoQueue(meleeUnit);
    }
    public void OnClickArcherButton()
    {
        _PutUnitIntoQueue(archerUnit);
    }
    public void OnClickRiflemanButton()
    {
        _PutUnitIntoQueue(rifleUnit);
    }
    public void OnClickKnightButton()
    {
        _PutUnitIntoQueue(knightUnit);
    }

    public IEnumerator StartMakingUnits()
    {
        while(true)
        {
            if (productionQueue.Count == 0)
            {
                yield return null;
            }
            else
            {
                string unitName = productionQueue.Dequeue();
                _UpdateProductionQueue();
                float productionTime = _GetProductionTime(unitName);
                yield return StartCoroutine(_ProgressBarHandler(productionTime));
                MakeAllyUnit(unitName);
            }
        }

    }

    private IEnumerator _ProgressBarHandler(float unitTime)
    {
        float progress = 0;
        while(progressBar.maxValue > progress)
        {
            progress += 100 * Time.deltaTime / unitTime;
            progressBar.value = progress;
            yield return null;
        }
        progressBar.value = progressBar.minValue;
    }
}
