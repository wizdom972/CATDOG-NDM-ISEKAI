using ISEKAI_Model;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public GameObject UI;
    public GameObject containerFullScript;
    public GameObject containerChoice;
    public GameObject containerConversation;

    public GameObject[] spritePeople;   // use same location index from SpriteLocation

    public GameObject spriteBackground;
    public GameObject spriteCG;

    public GameObject prefabChoiceButton;

    public GameObject cameraMainCamera;
    public float shakeAmount = 0.7f;

    public AudioSource audioBGM;

    public Animator animatorCamera;

    public Text textCharacterInfo;
    public Text textScript;
    public Text textFullScript;

    private String _fullScript = "";
    private int _scriptLength = 4;

    void Start()
    {
        SetUpEventManager();
        ExecuteOneScript();
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
        if (!scriptEnumerator.MoveNext())
        {
            Debug.Log(GameManager.instance.game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 2")).status + " " + GameManager.instance.game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 2")).IsFirstVisible());
            eventCore.Complete();
            Debug.Log(GameManager.instance.game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 2")).status + " " + GameManager.instance.game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 2")).IsFirstVisible());
            SceneManager.LoadScene("TownScene", LoadSceneMode.Single);
            GameManager.instance.TryOccurForcedEvent();
            Debug.Log(GameManager.instance.game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 2")).status + " " + GameManager.instance.game.allEventsList.Find(e => e.eventName.Equals("농사 이벤트 2")).IsFirstVisible());
            GameManager.instance.TryInstantiateEventSDs();
            GameManager.instance.TryUpdateEventSDs();
        }
        else
        {
            _ExecuteCommand(scriptEnumerator.Current);
        }
    }

    private void _ExecuteCommand(Command c)
    {
        if(c.commandNumber != 0) //if not explanation
        {
            _fullScript = "";
        }

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

    private void _Explanation(Explanation explanation)
    {
        Debug.Log("explanation");

        containerChoice.SetActive(false);
        containerConversation.SetActive(false);
        containerFullScript.SetActive(true);
        textFullScript.text = _fullScriptHandler(explanation.contents);
    }

    private String _fullScriptHandler(String s)
    {
        if(_scriptLength > 0)
        {
            if (_fullScript.Equals(""))
                _fullScript = s;
            else
                _fullScript = _fullScript + "\n" + s;

            _scriptLength--;
        }
        else
        {
            _fullScript = "";
            _fullScript += s;
            _scriptLength = 4;
        }
        
        return _fullScript + "<<";
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
            _setBright(conversation.brightCharacter);
    }

    private void _LoadCharacter(LoadCharacter loadCharacter)
    {
        Debug.Log("LoadCharacter");
        Sprite character;

        if(loadCharacter.location != SpriteLocation.None)
        {
            character = Resources.Load<Sprite>(loadCharacter.filePath);
            spritePeople[(int)loadCharacter.location].GetComponent<SpriteRenderer>().sprite = character;

            spritePeople[(int)loadCharacter.location].SetActive(true);
        }

        ExecuteOneScript();
    }

    private void _UnloadCharacter(UnloadCharacter unloadCharacter)
    {
        Debug.Log("UnloadCharacter");

        if (unloadCharacter.location != SpriteLocation.None)
        {
            spritePeople[(int)unloadCharacter.location].SetActive(false);
        }

        ExecuteOneScript();
    }

    private void _LoadBackground(LoadBackground loadBackground)
    {
        Debug.Log("LoadBackground");

        Sprite background;

        background = Resources.Load<Sprite>(loadBackground.filePath);
        spriteBackground.GetComponent<SpriteRenderer>().sprite = background;

        spriteBackground.SetActive(true);

        ExecuteOneScript();
    }

    private void _PlayMusic(PlayMusic playMusic)
    {
        Debug.Log("PlayMusic");

        AudioClip bgm;
        bgm = Resources.Load<AudioClip>(playMusic.filePath);
        audioBGM.clip = bgm;

        audioBGM.Play();

        ExecuteOneScript();
    }

    private void _StopMusic(StopMusic stopMusic)
    {
        Debug.Log("StopMusic");

        audioBGM.Stop();

        ExecuteOneScript();
    }

    private void _LoadCG(LoadCG loadCG)
    {
        Debug.Log("LoadCG");

        Sprite cg;
        cg = Resources.Load<Sprite>(loadCG.filePath);
        spriteCG.GetComponent<SpriteRenderer>().sprite = cg;        

        StartCoroutine("showCG");
    }

    IEnumerator showCG()
    {
        spriteCG.SetActive(true);
        UI.SetActive(false);

        yield return new WaitForSeconds(1f);

        //Debug.Log("hi");
        spriteCG.GetComponent<SpriteRenderer>().sortingLayerName = "background"; 
        UI.SetActive(true);

        ExecuteOneScript();
    }

    private void _UnloadCG(UnloadCG unloadCG)
    {
        Debug.Log("UnloadCG");

        spriteCG.SetActive(false);

        ExecuteOneScript();
    }

    private void _VFXCamerashake(VFXCameraShake vfxCameraShake)
    {
        Debug.Log("VFXCamerashake");

        StartCoroutine("cameraShake");
    }

    IEnumerator cameraShake()
    {
        animatorCamera.Play("shake");

        float aniLength = animatorCamera.GetCurrentAnimatorStateInfo(0).length * 2;

        yield return new WaitForSeconds(aniLength);

        ExecuteOneScript();
    }

    private void _VFXLoadSprite(VFXLoadSprite vfxLoadSprite)
    {
        Debug.Log("VFXLoadSprite");

        Sprite vfxSprite;
        vfxSprite = Resources.Load<Sprite>(vfxLoadSprite.filePath);
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

        //choice fuction
        _choiceHandler(choice.choiceList);
    }

    private void _choiceHandler(List<ChoiceEffect> l)
    {
        int listNum;
        listNum = l.Count;
        Debug.Log(listNum);

        int yPos = 0;

        for (int i = 0; i < listNum; i++)
        {
            if(listNum % 2 == 0)        //if list length is even
            {
                //Debug.Log("in even case");
                yPos = (-25) * (int)Math.Pow(-1, i + 1) * ((-2 * (i + 1)) + (int)Math.Pow(-1, i + 1) + 1);
            }
            else                        //if list length is odd
            {
                //Debug.Log("in odd case");
                yPos = (-25) * (int)Math.Pow(-1, i + 1) * ((2 * (i + 1)) + (int)Math.Pow(-1, i + 1) - 1);
                //Debug.Log(yPos);
            }


            GameObject choice = Instantiate(prefabChoiceButton,
                                        new Vector3(0, 0, 0), Quaternion.identity,
                                        containerChoice.GetComponent<Transform>());
            choice.GetComponent<Transform>().localPosition = new Vector3(0, yPos, 0);
            choice.GetComponentInChildren<Text>().text = l[i].choiceName;

            //TODO: add onClick function
        }
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
                temp.color = new Color(1f, 1f, 1f, 1f);
                continue;
            }

            temp.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }
    }
}
