using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISEKAI_Model;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIEventManager : MonoBehaviour
{
    public GameObject containerFullScript;
    public GameObject containerChoice;
    public GameObject containerConversation;

    public GameObject spritePeopleLeft;
    public GameObject spritePeopleCenter;
    public GameObject sprtiePeopleRight;

    public Button buttonSkip;
    public Button buttonAuto;
    public Button buttonNext;

    private EventManager _eventManager;

    void OnEnable()
    {
        SceneManager.sceneLoaded += _InitEventManager;
    }

    private void _InitEventManager(Scene s, LoadSceneMode m)
    {
        Game game = GameManager.instance.game;
        var eventList = game.allEventsList;
        _eventManager = new EventManager(eventList.Find(e => e.eventName.Equals(GameManager.instance.currentEventName)));

        containerChoice.SetActive(false);
        containerConversation.SetActive(false);
        containerFullScript.SetActive(false);

        spritePeopleLeft.SetActive(false);
        spritePeopleCenter.SetActive(false);
        sprtiePeopleRight.SetActive(false);
    }

    public void OnClickSkipButton()
    {

    }
    
    public void OnClickAutoButton()
    {

    }

    public void OnClickNextButton()
    {
        _eventManager.ExecuteOneScript();
    }
}
