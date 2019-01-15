using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ISEKAI_Model;

public class EventLoader : MonoBehaviour // This script is attatched to event SD for them to load EventScene when clicked.
{
    public static EventLoader instance;

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

    void OnMouseUpAsButton()
    {
        LoadEventScene();
    }

    public void LoadEventScene()
    {
        EventCore eventCore = UITownManager.instance.GetEventCoreFromEventSd(gameObject.transform);
        SceneManager.LoadScene("EventScene", LoadSceneMode.Single);
        GameManager.instance.currentEvent = eventCore;
    }
    public void LoadEventScene(EventCore eventCore)
    {
        SceneManager.LoadScene("EventScene", LoadSceneMode.Single);
        GameManager.instance.currentEvent = eventCore;
    }
}
