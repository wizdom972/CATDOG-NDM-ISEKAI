using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ISEKAI_Model;
using UnityEngine.UI;

public class UIEventManager : MonoBehaviour
{
    public Button buttonSkip;
    public Button buttonAuto;
    public Button buttonNext;

    private EventManager _eventManager;

    // Start is called before the first frame update
    void Start()
    {
        _eventManager = GameManager.instance.currentEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickSkipButton()
    {

    }
    
    public void OnClickAutoButton()
    {

    }

    public void OnClickNextButton()
    {
        _eventManager.ExecuteOneScript();
    }
}
