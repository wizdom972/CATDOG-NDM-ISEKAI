using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ISEKAI_Model;

public class EventLoader : MonoBehaviour // This script is attatched to event SD for them to load EventScene when clicked.
{
    public Sprite[] thisSprites;


    private void OnMouseEnter()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = thisSprites[1];
    }

    private void OnMouseExit()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = thisSprites[0];
    }

    void OnMouseUpAsButton()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = thisSprites[2];
        GameManager.instance.LoadEventScene(gameObject.transform);
    }
}
