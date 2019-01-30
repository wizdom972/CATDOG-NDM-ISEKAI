using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ISEKAI_Model;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public List<EventCore>.Enumerator forcedEventEnumerator;
    public Transform eventPrefab;
    public static GameManager instance;
    public EventCore currentEvent;

    public Sprite[] turnsLeftSprites;
    public Sprite[] seasonSprites;
    public Sprite[] numberSprites;

    public Transform town;
    public Transform outskirts;

    public bool isTutorialPlayed = false;
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
    public Game game = new Game(); // represents one game.

    void Start()
    {
        forcedEventEnumerator = game.forcedVisibleEventList.GetEnumerator();
        TryOccurForcedEvent();
    }


    public void LoadEventScene(EventCore eventCore)
    {
        SceneManager.LoadScene("EventScene", LoadSceneMode.Single);
        currentEvent = eventCore;
    }

    public EventCore GetEventCoreFromEventSd(Transform sd)
    {
        return game.allEventsList.Find(e => e.eventName.Equals(sd.name));
    }

    public void LoadEventScene(Transform t)
    {
        EventCore eventCore = GetEventCoreFromEventSd(t);
        currentEvent = eventCore;
        SceneManager.LoadScene("EventScene", LoadSceneMode.Single);
    }


    public void TryOccurForcedEvent()
    {
        if (!forcedEventEnumerator.MoveNext())
            return;
        LoadEventScene(forcedEventEnumerator.Current);
    }
    public List<Transform> eventSDList = new List<Transform>();

    public void TryInstantiateEventSDs() // find an events which is newly set to visible and make an SD of them.
    {
        foreach (EventCore e in game.visibleEventsList)
        {
            if (e.forcedEventPriority > 0) continue; // if event is forced event, there is no need to make SD.
            if (e.isNew) // if 
            {
                var sd = Instantiate(eventPrefab, new Vector3(0, 0, 0), Quaternion.identity);

                sd.position = GetEventSDVectorByLocation(e.location);
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
        eventSDList.RemoveAll(t => t == null);
        List<Transform> toDestroyList = new List<Transform>();
        foreach (Transform sd in eventSDList)
        {
            EventCore e = GetEventCoreFromEventSd(sd);
            if (e.SeasonCheck())
                sd.gameObject.SetActive(true);
            else
                sd.gameObject.SetActive(false);

            if (e.givenMaxTurn < 0)
                return;

            if (e.turnsLeft >= 1)
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
        foreach (Transform sd in toDestroyList)
        {
            Destroy(sd.gameObject);
            eventSDList.Remove(sd);
        }
    }
    public Location SmallLocationToBigLocation(EventLocation l)
    {
        if (l == 0)
            throw new InvalidOperationException("Forced Event can't be located anywhere.");

        if ((int)l <= 5 && l > 0)
            return Location.Outskirts;
        else
            return Location.Town;
    }

    public Vector3 GetEventSDVectorByLocation(EventLocation location)
    {
        return new Vector3((int)location - 6, (int)location - 6);
        /*switch((int)location)
        {
            case 1:
                return new Vector3(-1, 5);
            case 2:
                return new Vector3(1, -1);
            case 3:
                return new Vector3(8, 4);
            case 4:
                return new Vector3(0, 0);
            case 5:
                return new Vector3(0, 0);
            case 6:
                return new Vector3(3, 3);

        }*/
    }
}
