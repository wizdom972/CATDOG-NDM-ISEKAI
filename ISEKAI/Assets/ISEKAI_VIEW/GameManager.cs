using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ISEKAI_Model;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<EventCore>.Enumerator forcedEventEnumerator;
    public static GameManager instance;
    public EventCore currentEvent;
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
        var a = game.allEventsList.Find(e => e.eventName.Equals(sd.name));
        Debug.Log(a.eventName);
        return a;
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
}
