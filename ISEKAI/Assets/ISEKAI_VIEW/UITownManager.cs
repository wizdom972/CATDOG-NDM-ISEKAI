﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using ISEKAI_Model;
using UnityEngine.SceneManagement;

public enum Location
{
    Outskirts, Town
}

public class UITownManager : MonoBehaviour
{
    

    public TutorialManager tutorialManager;

    public Transform test;

    public GameObject moveBtnLocation;
    public GameObject background;

    public Text textPleasant;
    public Text textFood;
    public Text textTurn;
    public Text textAP;
    public Text textLocation;

    public Sprite townSprite;
    public Sprite outskirtsSprite;

    public GameObject nextTurn;

    private Button _moveBtnLocation;
    private Text _moveTxtlocation;
    private SpriteRenderer _background;
    private Location _location;
    private GameObject _eventList;

    public Transform town, outskirts;
    

    // Start is called before the first frame update
    void Start()
    {
        _location = Location.Outskirts;
        _background = background.GetComponent<SpriteRenderer>();
        _moveBtnLocation = moveBtnLocation.GetComponent<Button>();
        _moveTxtlocation = moveBtnLocation.GetComponentInChildren<Text>();
        _moveBtnLocation.onClick.AddListener(OnMoveBtnClick);
        UpdatePanel();
        if (!GameManager.instance.isTutorialPlayed)
        {
            tutorialManager.InitTexts();
            tutorialManager.ProceedTutorial();
        }
    }

    //If button clicked, change location, and replace ui depend on location
    public void OnMoveBtnClick()
    {
        tutorialManager.ProceedTutorial();
        switch (_location)
        {
            case Location.Outskirts:
                _background.sprite = townSprite;
                _location = Location.Town;
                outskirts.gameObject.SetActive(false);
                town.gameObject.SetActive(true);
                textLocation.text = "마을";
                _moveTxtlocation.text = "마을 외곽으로";
                break;

            case Location.Town:
                _background.sprite = outskirtsSprite;
                _location = Location.Outskirts;
                outskirts.gameObject.SetActive(true);
                town.gameObject.SetActive(false);
                textLocation.text = "마을 외곽";
                _moveTxtlocation.text = "마을로";
                break;

            default:
                throw new InvalidOperationException("Location should be town or outskirts");
        }
        UpdatePanel();
    }

    public void OnClickNextTurnButton()
    {
        Game _game = GameManager.instance.game;
        _game.Proceed();
        GameManager.instance.TryOccurForcedEvent();
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        Game _game = GameManager.instance.game;
        textFood.text = _game.town.remainFoodAmount.ToString();
        textPleasant.text = _game.town.totalPleasantAmount + "/" + 200;
        textTurn.text = _game.turn.ToString();
        textAP.text ="AP: " + _game.remainAP + "/" + 4;
        if (_game.turn.IsFormerSeason())
            nextTurn.SetActive(false);
        else
            nextTurn.SetActive(true);
        GameManager.instance.TryInstantiateEventSDs();
        GameManager.instance.TryUpdateEventSDs();
    }

    public List<Transform> eventSDList = new List<Transform>();
    /*
    public void TryInstantiateEventSDs() // find an events which is newly set to visible and make an SD of them.
    {
        Game game = GameManager.instance.game;
        foreach (EventCore e in game.visibleEventsList)
        {
            Debug.Log(e.eventName + " " + e.status + " " + e.givenMaxTurn);
            if (e.forcedEventPriority > 0) continue; // if event is forced event, there is no need to make SD.
            if (e.isNew) // if 
            {
                var sd = Instantiate(eventPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //TODO : set event sprite to the sprite of this event.
                if (_SmallLocationToBigLocation(e.location) == Location.Town)
                    sd.SetParent(town);
                else
                    sd.SetParent(outskirts);
                sd.name = e.eventName;
                if (e.givenMaxTurn < 0)
                    sd.GetChild(2).gameObject.SetActive(false);
                else
                    sd.GetChild(2).GetComponent<SpriteRenderer>().sprite = turnsLeftSprites[e.givenMaxTurn - 1]; // sprite array index is 0-based, but starts with sprite of 1, so -1 is needed.
                sd.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = numberSprites[e.cost];
                if (e.availableSeason == Season.None)
                    sd.GetChild(4).gameObject.SetActive(false);
                else
                    sd.GetChild(4).GetComponent<SpriteRenderer>().sprite = seasonSprites[(int)e.availableSeason - 1];
                eventSDList.Add(sd);
            }
        }
    }

    public void TryUpdateEventSDs()
    {
        List<Transform> toDestroyList = new List<Transform>();
        foreach(Transform sd in eventSDList)
        {
            
            EventCore e = GameManager.instance.GetEventCoreFromEventSd(sd);
            if (e.seasonCheck())
                sd.gameObject.SetActive(true);
            else
                sd.gameObject.SetActive(false);

            if (e.givenMaxTurn < 0)
                return;
            
            sd.GetChild(2).GetComponent<SpriteRenderer>().sprite = turnsLeftSprites[e.turnsLeft - 1]; // sprite array index is 0-based, but starts with sprite of 1, so -1 is needed.
            sd.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = numberSprites[e.cost];
            if (e.availableSeason == Season.None)
                sd.GetChild(4).gameObject.SetActive(false);
            else
                sd.GetChild(4).GetComponent<SpriteRenderer>().sprite = seasonSprites[(int)e.availableSeason - 1];
            if (e.turnsLeft != e.givenMaxTurn)
                sd.GetChild(3).gameObject.SetActive(false);
            if (e.turnsLeft <= 0)
            {
                toDestroyList.Add(sd);
                continue;
            }
        }
        foreach(Transform sd in toDestroyList)
        {
            Destroy(sd.gameObject);
            eventSDList.Remove(sd);
        }
    }*/
}
