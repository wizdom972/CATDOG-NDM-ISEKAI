using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ISEKAI_Model;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EventCore currentEvent;
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

}
