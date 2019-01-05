using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ISEKAI_Model;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        textFood.text = game.town.remainFoodAmount.ToString();
        textPleasant.text = game.town.totalPleasantAmount + "/" + 200;
        textTurn.text = game.turn.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Text textPleasant;
    public Text textFood;
    public Text textTurn;

    public Game game = new Game(); // represents one game.

    public void OnClickNextTurn()
    {
        game.Proceed();
        textFood.text = game.town.remainFoodAmount.ToString();
        textPleasant.text = game.town.totalPleasantAmount + "/" + 200;
        textTurn.text = game.turn.ToString();
    }
  
}
