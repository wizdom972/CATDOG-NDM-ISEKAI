using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ISEKAI_Model;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EventManager currentEvent;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    public Game game = new Game(); // represents one game.
}
