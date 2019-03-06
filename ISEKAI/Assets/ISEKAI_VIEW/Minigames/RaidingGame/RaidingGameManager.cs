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
    public Text skillDiscription;
    public Text boarBehaviourDiscription;

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
}