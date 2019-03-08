using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISEKAI_Model;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIEventManager : MonoBehaviour
{
    public GameObject containerFullScript;
    public GameObject containerChoice;
    public GameObject containerConversation;
    public GameObject fade;

    public GameObject spritePeopleLeft;
    public GameObject spritePeopleCenter;
    public GameObject sprtiePeopleRight;

    public Button buttonSkip;
    public Button buttonAuto;
    public Button buttonNext;

    public EventManager eventManager;

    public bool isAuto;
    public bool isSkip;

    private bool _isSkipRunning;
    private bool _isAutoRunning;

    void Awake()
    {
        containerChoice.SetActive(false);
        containerConversation.SetActive(false);
        containerFullScript.SetActive(false);
        fade.SetActive(false);

        spritePeopleLeft.SetActive(false);
        spritePeopleCenter.SetActive(false);
        sprtiePeopleRight.SetActive(false);

        isAuto = false;
        isSkip = false;

        _isAutoRunning = false;
        _isSkipRunning = false;
    }

    /*void Update()
    {
        //for auto on, automatically after choice
        if(eventManager.scriptEnumerator.Current.commandNumber != 15
            && isAuto == true
            && _isAutoRunning == false)
        {
            StartCoroutine(autoPlay());
        }

        //for skip on, automatically after choice
        if(eventManager.scriptEnumerator.Current.commandNumber != 15
            && isSkip == true
            && _isSkipRunning == false)
        {
            _skipPlay();
        }
    }*/



    public void OnClickSkipButton()
    {
        isSkip = !isSkip;
        Debug.Log("isSkip: " + isSkip);

        if (isSkip == true)
        {
            buttonSkip.gameObject.
                GetComponentInChildren<Text>().text = "Skip\nOn";

            
        }
        else if (isSkip == false)
        {
            buttonSkip.gameObject.
                GetComponentInChildren<Text>().text = "Skip\nOff";
        }

        _skipPlay();
    }

    private void _skipPlay()
    {   
        _isSkipRunning = true;

        try
        {
            while (true)
            {
                if(eventManager.scriptEnumerator.Current == null) // 더이상 스크립트 없을떄 (인거같음) 그냥 꺼버리게 설정.
                {
                    _isAutoRunning = false;
                    break;
                }

                if (eventManager.scriptEnumerator.Current.commandNumber == 15)       //if choice, stop excute and restart auto by update
                {
                    _isAutoRunning = false;
                    break;
                }

                if (isSkip == false)
                {
                    break;
                }

                eventManager.ExecuteOneScript();
            }
        }
        catch(Exception e)
        {
            Debug.Log(eventManager);
            Debug.Log(e.Data);
        }
    }
    
    public void OnClickAutoButton()
    {
        isAuto = !isAuto;
        Debug.Log("isAuto: " + isAuto);

        if(isAuto == true)
        {
            buttonAuto.gameObject.
                GetComponentInChildren<Text>().text = "Auto\nOn";

            buttonNext.interactable = false;

            StartCoroutine(autoPlay());
        }
        else if(isAuto == false)
        {
            buttonAuto.gameObject.
                GetComponentInChildren<Text>().text = "Auto\nOff";

            buttonNext.interactable = true;
            
            StopCoroutine(autoPlay());
        }
    }

    public IEnumerator autoPlay()
    {
        _isAutoRunning = true;

        while(true)
        {
            if(eventManager.scriptEnumerator.Current.commandNumber == 15)       //if choice stop corutine and restart auto by update
            {
                _isAutoRunning = false;
                break;
            }
            
            if(isAuto == false)
            {
                break;
            }

            eventManager.ExecuteOneScript();

            yield return new WaitForSeconds(1f);
        }
    }

    public void OnClickNextButton()
    {
        eventManager.ExecuteOneScript();
    }

    public void OnTouchScreen()
    {
        eventManager.ExecuteOneScript();
    }
}
