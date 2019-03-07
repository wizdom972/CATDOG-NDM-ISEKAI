using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    
    public void OnClickGameStartButton()
    {
        SceneManager.LoadScene("TownScene", LoadSceneMode.Single);
    }
}
