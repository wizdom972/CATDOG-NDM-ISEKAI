using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using ISEKAI_Model;

public class UITownManager : MonoBehaviour
{
    private enum Location
    {
        Outskirts, Town
    }

    public Transform eventPrefab;

    public GameObject moveBtnLocation;
    public GameObject background;

    public Text textPleasant;
    public Text textFood;
    public Text textTurn;

    private Button _moveBtnLocation;
    private Text _moveTxtlocation;
    private SpriteRenderer _background;
    private Location _location;
    private GameObject _eventList;

    public Sprite[] turnsLeftSprites;
    public Sprite[] seasonSprites;
    public Sprite[] numberSprites;

    // Start is called before the first frame update
    void Start()
    {
        _location = Location.Outskirts;
        _background = background.GetComponent<SpriteRenderer>();
        _moveBtnLocation = moveBtnLocation.GetComponent<Button>();
        _moveTxtlocation = moveBtnLocation.GetComponentInChildren<Text>();
        _moveBtnLocation.onClick.AddListener(OnMoveBtnClick);
    }

    //If button clicked, change location, and replace ui depend on location
    public void OnMoveBtnClick()
    {
        switch (_location)
        {
            case Location.Outskirts:
               _background.sprite = Resources.Load<Sprite>("bg_town");
                _location = Location.Town;
                _moveTxtlocation.text = "마을 외곽으로";
                break;

            case Location.Town:
                _background.sprite = Resources.Load<Sprite>("bg_outskirts");
                _location = Location.Outskirts;
                _moveTxtlocation.text = "마을로";
                break;

            default:
                throw new InvalidOperationException("Location should be town or outskirts");
        }
    }

    public void OnClickNextTurnButton()
    {
        Game _game = GameManager.instance.game;
        _game.Proceed();
        textFood.text = _game.town.remainFoodAmount.ToString();
        textPleasant.text = _game.town.totalPleasantAmount + "/" + 200;
        textTurn.text = _game.turn.ToString();
        TryInstantiateEventSDs();
        TryUpdateEventSDs();
    }

    public List<Transform> eventSDList = new List<Transform>();

    public void TryInstantiateEventSDs() // find an events which is newly set to visible and make an SD of them.
    {
        Game game = GameManager.instance.game;
        foreach (EventCore e in game.visibleEventsList)
        {
            if (e.forcedEventPriority > 0) continue; // if event is forced event, there is no need to make SD.
            if (e.isNew) // if 
            {
                var sd = Instantiate(eventPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                //TODO : set event sprite to the sprite of this event.
                sd.name = e.eventName;
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
        Game game = GameManager.instance.game;
        List<Transform> toDestroyList = new List<Transform>();
        foreach(Transform sd in eventSDList)
        {
            EventCore e = GetEventCoreFromEventSd(sd);
            if (e.turnsLeft <= 0)
            {
                toDestroyList.Add(sd);
                continue;
            }

            if (e.seasonCheck())
                sd.gameObject.SetActive(true);
            else
                sd.gameObject.SetActive(false);
            sd.GetChild(2).GetComponent<SpriteRenderer>().sprite = turnsLeftSprites[e.turnsLeft - 1]; // sprite array index is 0-based, but starts with sprite of 1, so -1 is needed.
            sd.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = numberSprites[e.cost];
            if (e.availableSeason == Season.None)
                sd.GetChild(4).gameObject.SetActive(false);
            else
                sd.GetChild(4).GetComponent<SpriteRenderer>().sprite = seasonSprites[(int)e.availableSeason - 1];
            if (e.turnsLeft != e.givenMaxTurn)
                sd.GetChild(3).gameObject.SetActive(false);
        }
        foreach(Transform sd in toDestroyList)
        {
            Destroy(sd.gameObject);
            eventSDList.Remove(sd);
        }
    }

    public EventCore GetEventCoreFromEventSd(Transform sd)
    {
        Game game = GameManager.instance.game;
        return game.allEventsList.Find(e => e.eventName.Equals(sd.name));
    }
}
