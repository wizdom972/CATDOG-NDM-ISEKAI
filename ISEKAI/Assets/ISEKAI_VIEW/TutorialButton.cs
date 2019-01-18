using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public TutorialManager tutorialManager;
    void OnMouseDown()
    {
        tutorialManager.ProceedTutorial();
    }

}
