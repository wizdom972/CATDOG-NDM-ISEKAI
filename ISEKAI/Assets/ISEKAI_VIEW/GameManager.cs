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
    public Sprite[] seasonSprites;

    public Sprite[] LeeSprites;
    public Sprite[] LeeFSprites;
    public Sprite[] MokSprites;
    public Sprite[] MooSprites;
    public Sprite[] NorSprites;
    public Sprite[] RohSprites;
    public Sprite[] SolSprites;
    public Sprite[] TongSprites;

    public Sprite[] DefaultSprites;

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

    //public GameObject EventSDList;
    public List<Transform> eventSDList = new List<Transform>();

    public void TryInstantiateEventSDs() // find an events which is newly set to visible and make an SD of them.
    {
        eventSDList.RemoveAll(t => t == null);
        foreach (EventCore e in game.visibleEventsList)
        {
            if (e.forcedEventPriority > 0) continue; // if event is forced event, there is no need to make SD.
            //if (e.isNew) // if 
            else {
                var sd = Instantiate(eventPrefab, new Vector3(0, 0, 0), Quaternion.identity);

                sd.position = GetEventSDVectorByLocation(e.location);
                sd.name = e.eventName;
                if (e.givenMaxMonth < 0)
                    sd.GetChild(2).gameObject.SetActive(false);
                //else
                    //sd.GetChild(2).GetComponent<SpriteRenderer>().sprite = turnsLeftSprites[e.givenMaxTurn - 1]; // sprite array index is 0-based, but starts with sprite of 1, so -1 is needed.
                else
                {
                    
                    int totalMonthLeft = e.monthsLeft;

                    int year = totalMonthLeft / 12;
                    int month = totalMonthLeft % 12 + 1;

                    string apText;
                    if (year == 0)
                        apText = "   " + month + "개월 남음";
                    else
                        apText = year + "년 " + month + "개월 남음";

                    sd.GetChild(2).GetComponent<TextMesh>().text = apText;
                }
                //sd.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = numberSprites[e.cost];
                sd.GetChild(1).GetComponent<TextMesh>().text = e.cost + "달";



                if (e.availableSeason == Season.None)
                    sd.GetChild(4).gameObject.SetActive(false);
                else
                    sd.GetChild(4).GetComponent<SpriteRenderer>().sprite = seasonSprites[(int)e.availableSeason - 1];

                var sdSprite = sd.GetChild(0).GetComponent<SpriteRenderer>();

                switch(e.characterName) //선녀/작업반장/기술지도원/통계원/목수/임페리우스/봄이/군인
                {
                    case "선녀":
                        sd.GetComponent<EventLoader>().thisSprites = LeeSprites;
                        break;
                    case "작업반장":
                        sd.GetComponent<EventLoader>().thisSprites = LeeFSprites;
                        break;
                    case "기술지도원":
                        sd.GetComponent<EventLoader>().thisSprites = RohSprites;
                        break;
                    case "통계원":
                        sd.GetComponent<EventLoader>().thisSprites = TongSprites;
                        break;
                    case "목수":
                        sd.GetComponent<EventLoader>().thisSprites = MokSprites;
                        break;
                    case "임페리우스":
                        sd.GetComponent<EventLoader>().thisSprites = MooSprites;
                        break;
                    case "봄이":
                        sd.GetComponent<EventLoader>().thisSprites = NorSprites;
                        break;
                    case "군인":
                        sd.GetComponent<EventLoader>().thisSprites = SolSprites;
                        break;
                    default:
                        sd.GetComponent<EventLoader>().thisSprites = DefaultSprites;
                        break;
                }
                sd.GetChild(0).GetComponent<SpriteRenderer>().sprite = sd.GetComponent<EventLoader>().thisSprites[0];
                eventSDList.Add(sd);
            }
        }
    }

    public void TryUpdateEventSDs()
    {
        //eventSDList.RemoveAll(t => t == null);
        List<Transform> toDestroyList = new List<Transform>();
        foreach (Transform sd in eventSDList)
        {
            EventCore e = GetEventCoreFromEventSd(sd);
            if (e.SeasonCheck())
                sd.gameObject.SetActive(true);
            else
                sd.gameObject.SetActive(false);

            if (e.givenMaxMonth < 0)
                continue;

            if (e.monthsLeft >= 1)
            {
                //sd.GetChild(2).GetComponent<SpriteRenderer>().sprite = turnsLeftSprites[e.turnsLeft - 1]; // sprite array index is 0-based, but starts with sprite of 1, so -1 is needed.
                
                int totalMonthLeft = e.monthsLeft;

                int year = totalMonthLeft / 12;
                int month = totalMonthLeft % 12 + 1;

                string apText;
                if (year == 0)
                    apText = "   " + month + "개월 남음";
                else
                    apText = year + "년 " + month + "개월 남음";

                sd.GetChild(2).GetComponent<TextMesh>().text = apText;
            }

            //sd.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = numberSprites[e.cost];

            if (e.availableSeason == Season.None)
                sd.GetChild(4).gameObject.SetActive(false);
            else
                sd.GetChild(4).GetComponent<SpriteRenderer>().sprite = seasonSprites[(int)e.availableSeason - 1];
            if (e.monthsLeft != e.givenMaxMonth)
                sd.GetChild(3).gameObject.SetActive(false);
            if (e.monthsLeft <= 0)
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

        if ((int)l <= 6 && l > 0)
            return Location.Outskirts;
        else
            return Location.Town;
    }

    public Vector3 GetEventSDVectorByLocation(EventLocation location)
    {
        float x = 0;
        float y = 0;

        switch(location)
        {
            case EventLocation.BackMount:
                x = -13; y = -4;
                break;
            case EventLocation.CarpenterHouse:
                x = -5; y = -4.5f;
                break;
            case EventLocation.Farm:
                x = -8; y = 0;
                break;
            case EventLocation.Field:
                x = -4; y = -1;
                break;
            case EventLocation.FrontMount:
                x = -1; y = 5.5f;
                break;
            case EventLocation.Mine:
                x = 2; y = -6;
                break;
            case EventLocation.SecretaryHouse:
                x = -5; y = 5.25f;
                break;
            case EventLocation.TaskLeaderHouse:
                x = 6; y = 0.5f;
                break;
            case EventLocation.TechGuideStaffHouse:
                x = 5; y = -4.5f;
                break;
            case EventLocation.TownSquare:
                x = -2.5f; y = 0;
                break;
            case EventLocation.TownWell:
                x = 2; y = 1.5f;
                break;
            case EventLocation.TownWitchHouse:
                x = 6; y = 6.5f;
                break;
            case EventLocation.WayToTown:
                x = 7; y = 0;
                break;
        }

        return new Vector3(x, y);
    }
}
