using ISEKAI_Model;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public GameObject containerFullScript;
    public GameObject containerChoice;
    public GameObject containerConversation;

    public GameObject spritePeopleLeft;
    public GameObject spritePeopleCenter;
    public GameObject sprtiePeopleRight;

    public Image test;

    public GameObject[] spritePeople;   // use same location index from SpriteLocation

    public Text textCharacterInfo;
    public Text textScript;
    public Text textFullScript;

    
    void Start()
    {
        SetUpEventManager();
    }

    public void SetUpEventManager() // when playing new event, this instance should be made.
    {
        eventCore = GameManager.instance.currentEvent;
        scriptEnumerator = GameManager.instance.currentEvent.script.GetEnumerator();
    }

    public EventCore eventCore { get; private set; } // when event SD is clicked and scene changed, it should be set to that event.

    public List<Command>.Enumerator scriptEnumerator;

    public void ExecuteOneScript()
    {
        // TODO:
        // when MoveNext() returns false, it must go back to TownScene.
        scriptEnumerator.MoveNext();
        _ExecuteCommand(scriptEnumerator.Current);
    }

    private void _ExecuteCommand(Command c)
    {
        switch (c.commandNumber)
        {
            case 0:
                _Explanation(c as Explanation);
                break;
            case 1:
                _Conversation(c as Conversation);
                break;
            case 2:
                _LoadCharacter(c as LoadCharacter);
                break;
            case 3:
                _UnloadCharacter(c as UnloadCharacter);
                break;
            case 4:
                _LoadBackground(c as LoadBackground);
                break;
            case 5:
                _PlayMusic(c as PlayMusic);
                break;
            case 6:
                _StopMusic(c as StopMusic);
                break;
            case 7:
                _LoadCG(c as LoadCG);
                break;
            case 8:
                _UnloadCG(c as UnloadCG);
                break;
            case 9:
                _VFXCamerashake(c as VFXCameraShake);
                break;
            case 10:
                _VFXLoadSprite(c as VFXLoadSprite);
                break;
            case 11:
                _VFXUnloadSprite(c as VFXUnloadSprite);
                break;
            case 12:
                _VFXSound(c as VFXSound);
                break;
            case 13:
                _LoadMinigame(c as LoadMinigame);
                break;
            case 14:
                _LoadVideo(c as LoadVideo);
                break;
            case 15:
                _Choice(c as Choice);
                break;
            case 16:
                break;
            case 17:
                _VFXTransition(c as VFXTransition);
                break;
            case 18:
                _VFXPause(c as VFXPause);
                break;
            default:
                throw new NotImplementedException("The command which holds that number is not implemented.");
        }
    }

    private void _enableContainerConversation(bool i)
    {
        GameObject contianerTest = GameObject.Find("Canvas/ContainerConversation/ContainerCharacterInfo/BackgroundCharacterInfo");
        Image testImage = contianerTest.GetComponent<Image>();

        if (i == true)
        {
            testImage.enabled = false;
        }
    }

    private void _Explanation(Explanation explanation)
    {
        Debug.Log("explanation");

        //if(GameObject.Find("Canvas/ContainerConversation") == null)
        //{
        //    Debug.Log("안찾아짐");
        //}

        //_enableContainerConversation(true);

        //containerChoice.SetActive(false);
        //containerConversation.SetActive(false);
        //containerFullScript.SetActive(true);
    }

    private void _Conversation(Conversation conversation)
    {
        Debug.Log("conversation");

        containerChoice.SetActive(false);
        containerConversation.SetActive(true);
        containerFullScript.SetActive(false);

        textCharacterInfo.text = conversation.characterName;
        textScript.text = conversation.contents;
        if (conversation.brightCharacter != SpriteLocation.None)
            spritePeople[(int) conversation.brightCharacter].SetActive(true);
        _setBright(conversation.brightCharacter);
    }

    private void _LoadCharacter(LoadCharacter loadCharacter)
    {
        Debug.Log("LoadCharacter");
    }

    private void _UnloadCharacter(UnloadCharacter unloadCharacter)
    {
        Debug.Log("UnloadCharacter");
    }

    private void _LoadBackground(LoadBackground loadBackground)
    {
        Debug.Log("LoadBackground");
    }

    private void _PlayMusic(PlayMusic playMusic)
    {
        Debug.Log("PlayMusic");
    }

    private void _StopMusic(StopMusic stopMusic)
    {
        Debug.Log("StopMusic");
    }

    private void _LoadCG(LoadCG loadCG)
    {
        Debug.Log("LoadCG");
    }

    private void _UnloadCG(UnloadCG unloadCG)
    {
        Debug.Log("UnloadCG");
    }

    private void _VFXCamerashake(VFXCameraShake vfxCameraShake)
    {
        Debug.Log("VFXCamerashake");
    }

    private void _VFXLoadSprite(VFXLoadSprite vfxLoadSprite)
    {
        Debug.Log("VFXLoadSprite");
    }

    private void _VFXUnloadSprite(VFXUnloadSprite vfxUnloadSprite)
    {
        Debug.Log("VFXUnloadSprite");
    }

    private void _VFXSound(VFXSound vfxSound)
    {
        Debug.Log("VFXSoun");
    }

    private void _LoadMinigame(LoadMinigame loadMinigame)
    {
        Debug.Log("LoadMinigame");
    }

    private void _LoadVideo(LoadVideo loadVideo)
    {
        Debug.Log("LoadVideo");
    }

    private void _Choice(Choice choice)
    {
        Debug.Log("Choice");

        containerChoice.SetActive(true);
        containerConversation.SetActive(false);
        containerFullScript.SetActive(false);
    }

    private void _VFXTransition(VFXTransition vfxTransition)
    {
        Debug.Log("VFXTransition");
    }

    private void _VFXPause(VFXPause vfxPause)
    {
        Debug.Log("VFXPause");
    }

    private void _setBright(SpriteLocation location)
    {
        SpriteRenderer temp;
        int bright = (int)location;

        for (int i = 1; i < 4; i++)
        {
            temp = spritePeople[i].GetComponent<SpriteRenderer>();
       
            if(i == bright)
            {
                temp.color = new Color(255f, 255f, 255f, 255f);
                continue;
            }

            temp.color = new Color(97f, 97f, 97f, 255f);
        }
    }
}
