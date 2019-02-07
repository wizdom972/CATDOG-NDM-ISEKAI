using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{

    public int state;

    public bool isBodyOnIt = false;

    public void SetSpriteOfState()
    {
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        transform.GetChild(state).gameObject.SetActive(true);
    }
}
