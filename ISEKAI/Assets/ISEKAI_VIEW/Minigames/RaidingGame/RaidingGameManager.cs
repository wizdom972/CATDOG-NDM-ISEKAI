using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class RaidingGameManager : MonoBehaviour
{
    public Text playerHPText;
    public Text boarHPText;
    public Slider boarHPBar;
    public Text attackDiscription;
    public Text blockDiscription;
    public Text skillDiscription;
    public Text boarBehaviourDiscription;

    public Button atkBtn;
    public Button blkBtn;
    public Button sklBtn;
    public EventManager eventManager;

    public const int maxPlayerHP = 100;
    public int playerHP = 100;
    public const int maxBoarHP = 150;
    public int boarHP = 150;

    public int playerAtkPower = 20;
    public int boarAtkPower = 15;

    public int playerBlock = 0;
    public bool isReadyToEvade = false;
    public bool isBoarCharging = false;

    public int turnsLeft = 15;

    public int boarHealCount = 3;
    public int boarEmpowerCount = 1;
    public int boarChargeCount = 1;
        

    public BoarBehaviour whatBoarGonnaDo = BoarBehaviour.Attack;
    public PlayerSpecialSkill currentSpecialSkill = PlayerSpecialSkill.Empower;

    public enum BoarBehaviour
    {
        Attack, Charge, Heal, Empower, ChargeAttack
    }

    public enum PlayerSpecialSkill
    {
        Empower, Heal, ReadyToEvade
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _UpdatePanel();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
    }

    public void Proceed(int behaviourType) // 0 if attack, 1 if block, 2 if special
    {
        switch (behaviourType)
        {
            case 0:
                _PlayerAttack();
                break;
            case 1:
                _PlayerBlock();
                break;
            case 2:
                _PlayerSpecialAct();
                break;
            default:
                throw new InvalidOperationException("ERRORDSFASDFSFSDADF");
        }
        _BoarBehave();
        _SetPlayerSkill();
        _SetBoarBehaviour();
        _EndTurn();
        _UpdatePanel();
    }

    private void _UpdateBoarBehaviourDiscription()
    {
        string turnsLeftDiscription = turnsLeft + "턴 후에 와일드보어킹이 도망갑니다!\n\n";
        switch(whatBoarGonnaDo)
        {
            case BoarBehaviour.Attack:
                boarBehaviourDiscription.text = turnsLeftDiscription + "와일드보어킹이 당신을 " + boarAtkPower + "의 공격력으로 공격하려 합니다!";
                break;
            case BoarBehaviour.Charge:
                boarBehaviourDiscription.text = turnsLeftDiscription + "와일드보어킹이 다음 턴을 위한 강력한 돌진 공격을 충전하려 합니다!";
                break;
            case BoarBehaviour.Empower:
                boarBehaviourDiscription.text = turnsLeftDiscription + "와일드보어킹이 분노합니다! 와일드보어킹이 공격력이 10 증가합니다!";
                break;
            case BoarBehaviour.Heal:
                boarBehaviourDiscription.text = turnsLeftDiscription + "와일드보어킹이 휴식을 취하려 합니다! 와일드보어킹이 체력을 20 회복합니다!";
                break;
            case BoarBehaviour.ChargeAttack:
                boarBehaviourDiscription.text = turnsLeftDiscription + "와일드보어킹의 충전이 끝났습니다! 와일드보어킹이 당신을 " + boarAtkPower * 2 + "의 공격력으로 공격하려 합니다!";
                break;
        }
    }

    private void _UpdatePanel()
    {
        playerHPText.text = "your hp: " + playerHP;
        boarHPText.text = boarHP + "/" + maxBoarHP;
        boarHPBar.value = boarHP;

        _UpdateBoarBehaviourDiscription();
        _UpdatePlayerBehaviourDiscription();
    }

    private void _PlayerAttack()
    {
        boarHP -= playerAtkPower;
    }

    private void _PlayerBlock()
    {
        playerBlock += 10;
    }

    private void _PlayerSpecialAct()
    {
        switch (currentSpecialSkill)
        {
            case PlayerSpecialSkill.Empower:
                playerAtkPower += 10;
                break;
            case PlayerSpecialSkill.Heal:
                playerHP += 20;
                break;
            case PlayerSpecialSkill.ReadyToEvade:
                isReadyToEvade = true;
                break;
        }
    }

    private void _BoarBehave()
    {
        switch (whatBoarGonnaDo)
        {
            case BoarBehaviour.Attack:
                if (!isReadyToEvade)
                    playerHP = Math.Max(playerHP + playerBlock - boarAtkPower, 10);
                return;
            case BoarBehaviour.Charge:
                isBoarCharging = true;
                boarChargeCount--;
                return;
            case BoarBehaviour.Empower:
                boarEmpowerCount--;
                boarAtkPower += 10;
                return;
            case BoarBehaviour.Heal:
                boarHealCount--;
                boarHP = Math.Min(boarHP + 20, maxBoarHP);
                return;
            case BoarBehaviour.ChargeAttack:
                if (!isReadyToEvade)
                    playerHP = Math.Max(playerHP + playerBlock - boarAtkPower * 2, 10);
                isBoarCharging = false;
                return;
        }
    }

    private void _EndTurn()
    {
        _SetPlayerSkill();
        _SetBoarBehaviour();
        playerBlock = 0;
        turnsLeft--;
    }

    private void _SetPlayerSkill()
    {
        currentSpecialSkill = (PlayerSpecialSkill)((new System.Random()).Next() % 3);
    }

    private void _SetBoarBehaviour()
    {
        if (isBoarCharging)
        {
            whatBoarGonnaDo = BoarBehaviour.ChargeAttack;
        }
        else
        {
            whatBoarGonnaDo = (BoarBehaviour)((new System.Random()).Next() % 4);
            if ((boarHealCount <= 0 && whatBoarGonnaDo == BoarBehaviour.Heal) ||
                (boarEmpowerCount <= 0 && whatBoarGonnaDo == BoarBehaviour.Empower) ||
                (boarChargeCount <= 0 && whatBoarGonnaDo == BoarBehaviour.Charge))
            {
                _SetBoarBehaviour();
            }
        }
    }

    private void _UpdatePlayerBehaviourDiscription()
    {
        attackDiscription.text = playerAtkPower + "의 공격력으로 공격합니다.";
        blockDiscription.text = "이번 턴에 10의 피해를 덜 받습니다.";
        switch(currentSpecialSkill)
        {
            case PlayerSpecialSkill.Empower:
                skillDiscription.text = "공격력이 10 증가합니다.";
                break;
            case PlayerSpecialSkill.Heal:
                skillDiscription.text = "20의 체력을 회복합니다.";
                break;
            case PlayerSpecialSkill.ReadyToEvade:
                skillDiscription.text = "다음 턴에 와일드보어킹의 공격을 회피합니다.";
                break;
        }

    }

    public void OnPointerEnterBehaviourButtons(int type)
    {
        _UpdatePlayerBehaviourDiscription();
        TurnOffAllDiscription();
        switch(type)
        {
            case 0:
                attackDiscription.gameObject.SetActive(true);
                break;
            case 1:
                blockDiscription.gameObject.SetActive(true);
                break;
            case 2:
                skillDiscription.gameObject.SetActive(true);
                break;

        }
    }

    public void TurnOffAllDiscription()
    {
        skillDiscription.gameObject.SetActive(false);
        attackDiscription.gameObject.SetActive(false);
        blockDiscription.gameObject.SetActive(false);
    }

    private void _CheckForGameEnd()
    {
        if (boarHP <= 0)
        {
            _GameOver(0);
            return;
        }

        if (playerHP <= 10)
            _GameOver(1);

    }

    private void _GameOver(int type)
    {
        StartCoroutine(_StartGameEndingProcess(type));
    }

    private void _TurnOffAllButtons()
    {
        atkBtn.interactable = false;
        sklBtn.interactable = false;
        blkBtn.interactable = false;
    }

    private IEnumerator _StartGameEndingProcess(int type)
    {
        eventManager.ExecuteOneScript();
        if (type == 0)
        {
            boarBehaviourDiscription.text = "와일드보어킹이 피를 뿜으며 쓰러집니다! 우리가 와일드보어킹을 사냥했습니다!!";
            GameManager.instance.game.town.remainFoodAmount += 100;
            GameManager.instance.game.town.pleasantChangeAddition += 10;
        }
        else if (type == 0)
        {
            boarBehaviourDiscription.text = "당신이 큰 피해를 입고 집중력을 잃은 순간, 와일드보어킹이 도망치기 시작했습니다. 사냥에 성공하진 못했지만, 앞으로 마을에 내려오는 일은 없을 것입니다.";
            GameManager.instance.game.town.pleasantChangeAddition += 5;
        }

        yield return new WaitForSeconds(3f);
        eventManager.SetActiveEventSceneThings(true);
        SceneManager.SetActiveScene(eventManager.gameObject.scene);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}