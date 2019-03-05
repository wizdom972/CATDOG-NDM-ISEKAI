using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISEKAI_Model;
using System;
using UnityEngine.UI;
using System.Linq;

public class EndingGameManager : MonoBehaviour
{

    public Slider progressBar;

    void Start()
    {
        game = GameManager.instance.game;
        /*
        game = new Game();
        game.town.remainFoodAmount += 1000;
        game.isIronActivated = true;
        game.isHorseActivated = true;
        game.town.totalIronAmount = 100;
        game.town.totalHorseAmount = 100;
        game.isArrowWeaponActivated = true;
        game.isBowActivated = false;
        game.isRifleActivated = true;
        game.castleHP = 300;
        */
        InitGameInfo();
        UpdatePanel();
        TurnOnAndOffNextWaveButton();
    }

    private Coroutine _production;
    private Coroutine _bar;

    public bool isInWave = false;

    public bool isCastleExists = false;
    public GameObject castle;
    public Transform castlePrefab;

    public Transform unitPrefab;
    public Game game;
    public Queue<string> productionQueue = new Queue<string>();
    public Queue<GameObject> deployedAllyUnits = new Queue<GameObject>();
    public Queue<GameObject> deployedEnemyUnits = new Queue<GameObject>();
    public List<Queue<string>> waves = new List<Queue<string>>();

    public Image[] productionQueueImage;
    public Button nextWave;
    public Text currentProductionText;

    public int riflemanCount = 0;
    public int currentWaveNumber = 0;

    public const int AllyStartPosition = -25;
    public const int EnemyStartPosition = 25;

    public Text food, iron, horse, wave;

    public Button meleeButton, archerButton, riflemanButton, knightButton;
    public string meleeUnit, archerUnit, rifleUnit, knightUnit = "기사";

    private EndingGameUnit _justMadeAllyUnit = null, _justMadeEnemyUnit = null;

    public void UpdatePanel()
    {
        food.text = "음식: " + game.town.remainFoodAmount;
        iron.text = "철: " + game.town.totalIronAmount;
        horse.text = "말: " + game.town.totalHorseAmount;
        _CheckProducibleUnits();
    }

    public void UpdateWaveNumber()
    {
        wave.text = (currentWaveNumber + 1) + "번째 공세";
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

        if (!game.isKnightActivated)
            knightButton.gameObject.SetActive(false);

        if (game.castleHP > 0)
        {
            castle = Instantiate(castlePrefab, new Vector2(-10, 0), Quaternion.identity).gameObject;
            castle.GetComponent<EndingGameUnit>().endingGame = this;
            castle.GetComponent<EndingGameUnit>().endingGameManager = this;
            Debug.Log(castle.name);
            castle.GetComponent<EndingGameUnit>().hp = game.castleHP;
            switch (archerUnit)
            {
                case "궁병": castle.GetComponent<EndingGameUnit>().attackPower = 6; break;
                case "석궁병": castle.GetComponent<EndingGameUnit>().attackPower = 10; break;
                case "석궁병(철)": castle.GetComponent<EndingGameUnit>().attackPower = 20; break;
                default: castle.GetComponent<EndingGameUnit>().attackPower = 0; break;
            }
            isCastleExists = true;
        }

        riflemanCount = game.rifleAmount;

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
        unitObject.GetComponent<EndingGameUnit>().endingGameManager = this;
        unitObject.GetComponent<EndingGameUnit>().frontUnit = _justMadeAllyUnit;
        unitObject.gameObject.SetActive(true);
        unitObject.GetChild(0).GetComponent<TextMesh>().text = "HP: " + unitObject.GetComponent<EndingGameUnit>().hp;
        deployedAllyUnits.Enqueue(unitObject.gameObject);
        _justMadeAllyUnit = unitObject.GetComponent<EndingGameUnit>();
    }

    public void MakeEnemyUnit(string unitName)
    {
        var unitObject = Instantiate(unitPrefab.GetChild(_GetUnitNumber(unitName)), new Vector3(EnemyStartPosition, -4f, 0), Quaternion.identity);
        unitObject.GetComponent<EndingGameUnit>().endingGame = this;
        unitObject.GetComponent<EndingGameUnit>().frontUnit = _justMadeEnemyUnit;
        unitObject.gameObject.SetActive(true);
        unitObject.GetChild(0).GetComponent<TextMesh>().text = "HP: " + unitObject.GetComponent<EndingGameUnit>().hp;
        deployedEnemyUnits.Enqueue(unitObject.gameObject);
        _justMadeEnemyUnit = unitObject.GetComponent<EndingGameUnit>();
    }

    public void ProceedWave()
    {
        isInWave = true;
        TurnOnAndOffNextWaveButton();
        StartCoroutine(_StartWave());
        _production = StartCoroutine(StartMakingUnits());
    }

    private IEnumerator _StartWave()
    {
        foreach (string unitName in waves[currentWaveNumber])
        {
            MakeEnemyUnit(unitName);
            yield return new WaitForSeconds(1f);
        }
        currentWaveNumber++;
    }

    private bool _IsUnitProducible(string unitName)
    {
        float mod;
        if (game.expansion2Modifier)
            mod = 0.7f;
        else
            mod = 1;
        switch (unitName)
        {
            case "농민":
                return game.town.remainFoodAmount >= 20 * mod;
            case "창병":
                return game.town.remainFoodAmount >= 20 * mod && game.town.totalIronAmount >= 1 * mod;
            case "파이크병":
                return game.town.remainFoodAmount >= 50 * mod && game.town.totalIronAmount >= 1 * mod;
            case "궁병":
                return game.town.remainFoodAmount >= 30 * mod;
            case "석궁병":
                return game.town.remainFoodAmount >= 20 * mod;
            case "석궁병(철)":
                return game.town.remainFoodAmount >= 20 * mod && game.town.totalIronAmount >= 2 * mod;
            case "소총병":
                return game.town.remainFoodAmount >= 35 * mod && riflemanCount > 0 * mod;
            case "소총병(철)":
                return game.town.remainFoodAmount >= 35 * mod && game.town.totalIronAmount >= 2 * mod && riflemanCount > 0;
            case "기사":
                return game.town.remainFoodAmount >= 40 * mod && game.town.totalIronAmount >= 10 * mod && game.town.totalHorseAmount >= 1;
            default:
                throw new InvalidOperationException("Unit " + unitName + " has not implemented.");
        }
    }
    private int _GetProductionTime(string unitName)
    {
        float mod;
        if (game.expansion2Modifier)
            mod = 0.7f;
        else
            mod = 1;
        switch (unitName)
        {
            case "농민":
                return (int)(1 * mod);
            case "창병":
                return (int)(2 * mod);
            case "파이크병":
                return (int)(3 * mod);
            case "궁병":
                return (int)(5 * mod);
            case "석궁병":
                return (int)(3 * mod);
            case "석궁병(철)":
                return (int)(4 * mod);
            case "소총병":
                return (int)(5 * mod);
            case "소총병(철)":
                return (int)(6 * mod);
            case "기사":
                return (int)(5 * mod);
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
        float mod;
        if (game.expansion2Modifier)
            mod = 0.7f;
        else
            mod = 1;
        switch (unitName)
        {
            case "농민":
                game.town.remainFoodAmount -= 20 * mod;
                break;
            case "창병":
                game.town.remainFoodAmount -= 20 * mod;
                game.town.totalIronAmount -= 1 * mod;
                break;
            case "파이크병":
                game.town.remainFoodAmount -= 50 * mod;
                game.town.totalIronAmount -= 1 * mod;
                break;
            case "궁병":
                game.town.remainFoodAmount -= 30 * mod;
                break;
            case "석궁병":
                game.town.remainFoodAmount -= 20 * mod;
                break;
            case "석궁병(철)":
                game.town.remainFoodAmount -= 20 * mod;
                game.town.totalIronAmount -= 2 * mod;
                break;
            case "소총병":
                game.town.remainFoodAmount -= 35 * mod;
                --riflemanCount;
                break;
            case "소총병(철)":
                game.town.remainFoodAmount -= 35 * mod;
                game.town.totalIronAmount -= 2 * mod;
                --riflemanCount;
                break;
            case "기사":
                game.town.remainFoodAmount -= 40 * mod;
                game.town.totalIronAmount -= 10 * mod;
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
                yield return (_bar = StartCoroutine(_ProgressBarHandler(unitName)));
                MakeAllyUnit(unitName);
            }
        }

    }

    private IEnumerator _ProgressBarHandler(string unitName)
    {
        currentProductionText.text = unitName + " 생산중";
        float unitTime = _GetProductionTime(unitName);
        float progress = 0;
        while(progressBar.maxValue > progress)
        {
            progress += 100 * Time.deltaTime / unitTime;
            progressBar.value = progress;
            yield return null;
        }
        progressBar.value = progressBar.minValue;
        currentProductionText.text = "";
    }

    public void TurnOnAndOffNextWaveButton()
    {
        nextWave.interactable = !isInWave;
        meleeButton.interactable = isInWave;
        archerButton.interactable = isInWave;
        riflemanButton.interactable = isInWave;
        knightButton.interactable = isInWave;
    }

    public void CleanUp()
    {
        foreach (GameObject e in deployedAllyUnits) Destroy(e);
        while (deployedAllyUnits.Count != 0) deployedAllyUnits.Dequeue();
        while (productionQueue.Count != 0) productionQueue.Dequeue();
        StopCoroutine(_production);
        StopCoroutine(_bar);
        progressBar.value = progressBar.minValue;
        currentProductionText.text = "";
        _UpdateProductionQueue();
        UpdateWaveNumber();
    }
}
