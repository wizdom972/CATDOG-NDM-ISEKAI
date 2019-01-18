using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ISEKAI_Model;

public class EventLoader : MonoBehaviour // This script is attatched to event SD for them to load EventScene when clicked.
{
    void OnMouseUpAsButton()
    {
        GameManager.instance.LoadEventScene(gameObject.transform);
    }
}
