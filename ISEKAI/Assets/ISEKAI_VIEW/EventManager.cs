using ISEKAI_Model;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public GameObject UI;
    public GameObject UI_ruby;
    public GameObject EventItems;
    public GameObject EventSceneCamera;
    public GameObject containerFullScript;
    public GameObject containerChoice;
    public GameObject containerConversation;

    public GameObject[] spritePeople;   // use same location index from SpriteLocation
    public GameObject[] spritePeopleTmp;

    public GameObject spriteBackground;
    public GameObject spriteBackgroundTemp;
    public GameObject spriteCG;
    public GameObject spriteVFX;
    public GameObject spriteFade;

    public GameObject prefabChoiceButton;

    public GameObject cameraMainCamera;

    public AudioSource audioBGM;
    public AudioSource audioVFX;

    public VideoPlayer videoVFX;

    public Animator animatorCamera;

    public Text textCharacterInfo;
    public Text textScript;
    public Text textFullScript;
    public Text textFullScriptTest;

    public GameObject fullScriptText;
    public GameObject scriptText;

    public Button NextButton;

    public GameObject screenTouchSensor;

    public UIEventManager uiEventManager;

    [HideInInspector]
    public bool inputInteractable;

    /// <summary>
    /// 모든 next 버튼을 잠굽니다.
    /// </summary>
    public bool isNextButtonActive
    { set
        {
            NextButton.interactable = value;
            screenTouchSensor.SetActive(value);
            inputInteractable = value;
        }
    }
    public SpriteLocation characterBrightNum = SpriteLocation.None;

    public GameObject UIButton;
    public GameObject UIScript;

    private String _fullScript = "";
    private int _scriptLength = 6;

    private int _loadingCharacterCount = 0;

    private bool isVideoPlaying;

    void Start()
    {
        SetUpEventManager();
        ExecuteOneScript();
    }

    void Update()
    {
        //비디오
        if (videoVFX.isPlaying && (Input.anyKeyDown || Input.touchCount>0))
        {
            Debug.Log("stop");
            StopCoroutine("loadVideoandPlay");
            videoVFX.Stop();
            isVideoPlaying = false;
            ExecuteOneScript();

            //스크립트 실행하고 살리기
            UIButton.SetActive(true);
            UIScript.SetActive(true);
            isNextButtonActive = true;
        }

        //인풋 들어오는지.
        //다른거로 바꿀 준비.
        //if (Input.GetAxis("Submit") != 0) //이거로 하면 한번에 2개씩 나감.
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (inputInteractable)
            {
                isNextButtonActive = false;
                ExecuteOneScript();
            }
        }
        //NextButton.interactable = isNextButtonActive;
    }

    public void SetUpEventManager() // when playing new event, this instance should be made.
    {
        eventCore = GameManager.instance.currentEvent;
        characterBrightNum = SpriteLocation.None;
        scriptEnumerator = GameManager.instance.currentEvent.script.GetEnumerator();
    }

    public EventCore eventCore { get; private set; } // when event SD is clicked and scene changed, it should be set to that event.

    public List<Command>.Enumerator scriptEnumerator;
    /* 왜인지 모르겠는데 이거 하면 터짐 ㅂㄷ..
    {
        get
        {
            return _scriptEnumerator;
        }

        set
        {
            Debug.Log(value);
            _scriptEnumerator = value;
        }
    }
    public List<Command>.Enumerator _scriptEnumerator;
    */
    public void ExecuteOneScript()
    {
        if (!scriptEnumerator.MoveNext())
        {
            eventCore.Complete();
            SceneManager.LoadScene("TownScene", LoadSceneMode.Single);
            Debug.Log("HONTONI: " + GameManager.instance.game.turn.totalMonthNumber.ToString());
            //GameManager.instance.TryOccurForcedEvent();
            //GameManager.instance.TryInstantiateEventSDs();
            //GameManager.instance.TryUpdateEventSDs();
        }
        else
        {
            Command c = scriptEnumerator.Current;
            _ExecuteCommand(c);
        }
    }

    private void _ExecuteCommand(Command c)
    {
        isNextButtonActive = true;
        

        int choiceDependencyNum;
        choiceDependencyNum = c.choiceDependency.Item1;

        int choiceBranchDependencyNum;
        choiceBranchDependencyNum = c.choiceDependency.Item2;


        if (choiceDependencyNum == -1)
        {
            if (choiceBranchDependencyNum == -1)         //(-1, -1)
            {
                //do nothing
            }
            else        // choice branch num is 0-9     //(-1, n)
            {
                string currentEvent;
                currentEvent = GameManager.instance.currentEvent.eventName;

                List<(int, int)> choiceHistory = new List<(int, int)>();
                GameManager.instance.game.choiceHistories.TryGetValue(currentEvent, out choiceHistory);
                Debug.Log(choiceHistory.Count);
                int choiceBranch;
                choiceBranch = choiceHistory[choiceHistory.Count - 1].Item2;

                if (c.choiceDependency.Item2 != choiceBranch)
                {
                    ExecuteOneScript();
                    return;
                }
            }
        }
        else        //choice num is 0-9                 //(n, m)
        {
            string currentEvent;
            currentEvent = GameManager.instance.currentEvent.eventName;

            List<(int, int)> choiceHistory = new List<(int, int)>();
            GameManager.instance.game.choiceHistories.TryGetValue(currentEvent, out choiceHistory);

            for (int i = 0; i < choiceHistory.Count; i++)
            {
                if (choiceHistory[i] != (choiceDependencyNum, choiceBranchDependencyNum))       //if dependency not match, return
                {
                    ExecuteOneScript();
                    return;
                }
                else
                {
                    // do nothing
                }
            }
        }

        //for full script
        if (c.commandNumber != 0) //if not explanation
        {
            _fullScript = "";
            _scriptLength = 4;
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
            case 19:
                _GameEnd(c as GameEnd);
                break;
            default:
                throw new NotImplementedException("The command which holds that number is not implemented.");
        }
    }

    IEnumerator WaitUntilCharacterLoadedExplanation(Explanation explanation)
    {
        if (_loadingCharacterCount != 0)
        {
            Debug.Log("Wait Until Character Loaded");
        }
        yield return new WaitUntil(() => _loadingCharacterCount == 0);
        containerChoice.SetActive(false);
        containerConversation.SetActive(false);
        containerFullScript.SetActive(true);

        textFullScript.text = _fullScriptHandler(explanation.contents);

        foreach (Transform child in fullScriptText.transform)
        {
            GameObject.Destroy(child.gameObject);
            Debug.Log("웨않되");
        }

        fullScriptText.GetComponent<RubyText>().isCalled = false;
    }

    IEnumerator WaitUntilCharacterLoadedConversation(Conversation conversation)
    {
        if (_loadingCharacterCount != 0)
        {
            Debug.Log("Wait Until Character Loaded");
        }
        yield return new WaitUntil(() => _loadingCharacterCount == 0);

        containerChoice.SetActive(false);
        containerConversation.SetActive(true);
        containerFullScript.SetActive(false);

        textCharacterInfo.text = conversation.characterName;
        textScript.text = conversation.contents;

        if (conversation.brightCharacter != SpriteLocation.None)
        {
            _setBright(conversation.brightCharacter);
        }
        else
        {
            _setBright(SpriteLocation.None);
        }


        foreach (Transform child in scriptText.transform)
        {
            GameObject.Destroy(child.gameObject);
            Debug.Log("웨않되2");
        }

        scriptText.GetComponent<RubyText>().isCalled = false;
    }

    private void _Explanation(Explanation explanation)
    {
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        Debug.Log("explanation");
        UIScript.SetActive(true);

        StartCoroutine(WaitUntilCharacterLoadedExplanation(explanation));
    }

    private String _fullScriptHandler(String s)
    {
        if (_scriptLength > 0)
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
        UIScript.SetActive(true);
        
        characterBrightNum = conversation.brightCharacter;
        StartCoroutine(WaitUntilCharacterLoadedConversation(conversation));
    }



    private void _LoadCharacter(LoadCharacter loadCharacter)
    {

        containerChoice.SetActive(false);
        containerConversation.SetActive(false);
        containerFullScript.SetActive(false);

        Debug.Log("LoadCharacter");

        if (loadCharacter.location != SpriteLocation.None)
        {
            isNextButtonActive = false;
            StartCoroutine(fadeCharacter(loadCharacter));
            //ExecuteOneScript();
        }


    }

    IEnumerator fadeCharacter(LoadCharacter loadCharacter)
    {
        _loadingCharacterCount++;
        
        ExecuteOneScript();

        bool isOrigin = false;

        // 원래 캐릭터 위치.
        var charGameObject1 = spritePeople[(int)loadCharacter.location];
        var spriteRenderer1 = charGameObject1.GetComponent<SpriteRenderer>();


        //바꿀 캐릭터 위치
        var charGameObject2 = spritePeopleTmp[(int)loadCharacter.location];
        var spriteRenderer2 = charGameObject2.GetComponent<SpriteRenderer>();


        //캐릭터가 있을때.
        if (spriteRenderer1.sprite != null)
        {
            isOrigin = true;
        }

        //새로운 캐릭터 켜기.
        charGameObject2.SetActive(true);

        //새로운 캐릭터 가져오기
        Sprite newCharacter = Resources.Load<Sprite>(loadCharacter.filePath);
        spriteRenderer2.sprite = newCharacter;

        //색 처음색 가져오기.
        Color color1 = spriteRenderer1.color;
        Color color2 = spriteRenderer2.color;

        color1.a = 1.0f;
        color2.a = 0.0f;

        //페이드 인 아웃 동시에
        while (color2.a < 1.0f)
        {
            //페이드아웃
            if(isOrigin)
            {
                color1.a -= 0.1f;
                spriteRenderer1.color = color1;
            }

            //페이드 인
            color2.a += 0.1f;
            spriteRenderer2.color = color2;

            yield return new WaitForSeconds(0.04f);
        }

        //마지막색 설정.
        color1.a = 0.2f;
        spriteRenderer1.color = color1;

        //원래거 끄기.
        charGameObject1.SetActive(false);

        //바꿔두기
        spritePeople[(int)loadCharacter.location] = charGameObject2;
        spritePeopleTmp[(int)loadCharacter.location] = charGameObject1;


        if (characterBrightNum != loadCharacter.location)
        { 
            characterBrightNum = SpriteLocation.None;
            _setBright(SpriteLocation.None);
        }
        //ExecuteOneScript();
        _loadingCharacterCount--;
        isNextButtonActive = true;
    }

    private void _UnloadCharacter(UnloadCharacter unloadCharacter)
    {
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        Debug.Log("UnloadCharacter");

        if (unloadCharacter.location != SpriteLocation.None)
        {
            spritePeople[(int)unloadCharacter.location].GetComponent<SpriteRenderer>().sprite = null;
            spritePeople[(int)unloadCharacter.location].SetActive(false);
        }
        
        ExecuteOneScript();
    }

    private void _LoadBackground(LoadBackground loadBackground)
    {
        Debug.Log("LoadBackground");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        Sprite background = Resources.Load<Sprite>(loadBackground.filePath);

        if (background == null)
        {
            Debug.Log(loadBackground.filePath + " doesn't exist");
            return;
        }

        var mainBackgroundRenderer = spriteBackground.GetComponent<SpriteRenderer>();
        if (mainBackgroundRenderer == null) return;

        var tmpBackgroundRenderer = spriteBackgroundTemp.GetComponent<SpriteRenderer>();
        if (tmpBackgroundRenderer == null) return;

        if(mainBackgroundRenderer.sprite == null)
        {
            // 그냥 페이드 인
            spriteBackgroundTemp.SetActive(false);
            isNextButtonActive = false;
            StartCoroutine(FadeBackground(loadBackground, background, mainBackgroundRenderer));
        }
        else
        {
            // Dissolve
            //spriteBackground.transform.localScale = new Vector3(1, 1, 1);

            var width = mainBackgroundRenderer.sprite.bounds.size.x;
            var height = mainBackgroundRenderer.sprite.bounds.size.y;

            var worldScreenHeight = Camera.main.orthographicSize * 2.0;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            //spriteBackground.transform.localScale = new Vector3((float)worldScreenWidth / width, (float)worldScreenHeight / height, 1);
            spriteBackgroundTemp.transform.localScale = new Vector3((float)worldScreenWidth / width, (float)worldScreenHeight / height, 1);

            spriteBackground.SetActive(true);
            spriteBackgroundTemp.SetActive(true);

            isNextButtonActive = false;
            StartCoroutine(DissolveBackground(loadBackground, background, mainBackgroundRenderer, tmpBackgroundRenderer));
        }

    }


    IEnumerator FadeBackground(LoadBackground loadBackground, Sprite background, SpriteRenderer spriteRenderer)
    {

        Color color = spriteRenderer.color;
        color.a = 0.0f;
        spriteRenderer.color = color;
        spriteRenderer.sprite = background;

        spriteBackground.transform.localScale = new Vector3(1, 1, 1);

        var width = spriteRenderer.sprite.bounds.size.x;
        var height = spriteRenderer.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        spriteBackground.transform.localScale = new Vector3((float)worldScreenWidth / width, (float)worldScreenHeight / height, 1);

        spriteBackground.SetActive(true);

        while (color.a < 1.0f)
        {
            color.a += 0.1f;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(0.05f);
        }

        isNextButtonActive = true;
       
        ExecuteOneScript();
    }


    IEnumerator DissolveBackground(
        LoadBackground loadBackground,
        Sprite background,
        SpriteRenderer mainBackgroundRenderer,
        SpriteRenderer tmpBackgroundRenderer
        )
    {

        Color mainBackgroundColor = mainBackgroundRenderer.color;
        mainBackgroundColor.a = 1.0f;

        Color tmpBackgroundColor = tmpBackgroundRenderer.color;
        tmpBackgroundColor.a = 0.0f;

        tmpBackgroundRenderer.sprite = background;

        spriteBackgroundTemp.transform.localScale = new Vector3(1, 1, 1);

        var tmpWidth = tmpBackgroundRenderer.sprite.bounds.size.x;
        var tmpHeight = tmpBackgroundRenderer.sprite.bounds.size.y;

        var tmpWorldScreenHeight = Camera.main.orthographicSize * 2.0;
        var tmpWorldScreenWidth = tmpWorldScreenHeight / Screen.height * Screen.width;

        spriteBackgroundTemp.transform.localScale = new Vector3((float)tmpWorldScreenWidth / tmpWidth, (float)tmpWorldScreenHeight / tmpHeight, 1);

        while (mainBackgroundColor.a > 0.0f)
        {
            mainBackgroundColor.a -= 0.1f;
            tmpBackgroundColor.a += 0.1f;

            mainBackgroundRenderer.color = mainBackgroundColor;
            tmpBackgroundRenderer.color = tmpBackgroundColor;

            yield return new WaitForSeconds(0.05f);
        }

        mainBackgroundColor.a = 1.0f;

        mainBackgroundRenderer.sprite = background;
        mainBackgroundRenderer.color = mainBackgroundColor;
        spriteBackground.transform.localScale = new Vector3((float)tmpWorldScreenWidth / tmpWidth, (float)tmpWorldScreenHeight / tmpHeight, 1);
        isNextButtonActive = true;
       
        ExecuteOneScript();
    }


    private void _PlayMusic(PlayMusic playMusic)
    {
        Debug.Log("PlayMusic");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        AudioClip bgm;
        bgm = Resources.Load<AudioClip>(playMusic.filePath);
        audioBGM.clip = bgm;

        audioBGM.Play();
        
        ExecuteOneScript();
    }

    private void _StopMusic(StopMusic stopMusic)
    {
        Debug.Log("StopMusic");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        audioBGM.Stop();
        
        ExecuteOneScript();
    }

    private void _LoadCG(LoadCG loadCG)
    {
        Debug.Log("LoadCG");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        Sprite cg;
        cg = Resources.Load<Sprite>(loadCG.filePath);
        spriteCG.GetComponent<SpriteRenderer>().sprite = cg;

        isNextButtonActive = false;
        UIButton.SetActive(false);
        UIScript.SetActive(false);
        SpriteRenderer cgRenderer = spriteCG.GetComponent<SpriteRenderer>();

        StartCoroutine(showCG(cgRenderer));
    }

    IEnumerator showCG(SpriteRenderer cgRenderer)
    {

        Color cgColor = cgRenderer.color;
        cgColor.a = 0.0f;
        spriteCG.SetActive(true);
        while (cgColor.a < 1.0f)
        {
            cgColor.a += 0.05f;

            cgRenderer.color = cgColor;

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);

        //Debug.Log("hi");
        spriteCG.GetComponent<SpriteRenderer>().sortingLayerName = "background";
        spriteCG.GetComponent<SpriteRenderer>().sortingOrder =
            spriteBackground.GetComponent<SpriteRenderer>().sortingOrder + 1;

        isNextButtonActive = true;
        UIButton.SetActive(true);
        UIScript.SetActive(true);
        
        ExecuteOneScript();
    }

    private void _UnloadCG(UnloadCG unloadCG)
    {
        Debug.Log("UnloadCG");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        SpriteRenderer cgRenderer = spriteCG.GetComponent<SpriteRenderer>();
        spriteCG.SetActive(false);
        ExecuteOneScript();

        StartCoroutine(dissolveCG(cgRenderer));
    }

    IEnumerator dissolveCG(SpriteRenderer cgRenderer)
    {

        Color cgColor = cgRenderer.color;
        while (cgColor.a > 0.0f)
        {
            cgColor.a -= 0.05f;

            cgRenderer.color = cgColor;

            yield return new WaitForSeconds(0.05f);
        }
        spriteCG.SetActive(false);
        cgColor.a = 1f;
        cgRenderer.color = cgColor;
    }

    private void _VFXCamerashake(VFXCameraShake vfxCameraShake)
    {
        Debug.Log("VFXCamerashake");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        isNextButtonActive = false;
        StartCoroutine(cameraShake());
    }

    IEnumerator cameraShake()
    {
        animatorCamera.Play("shake");

        float aniLength = animatorCamera.GetCurrentAnimatorStateInfo(0).length * 2;

        yield return new WaitForSeconds(aniLength);

        isNextButtonActive = true;
        ExecuteOneScript();
    }

    //private bool _IsAnimatorPlaying(Animator animator)
    //{
    //    return animator.GetCurrentAnimatorStateInfo(0).length >
    //           animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    //}

    private float _GetAnimLength(Animator animator)
    {
        float length = 0.0f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            length += clip.length;
        }

        return length;
    }


    private void _VFXLoadSprite(VFXLoadSprite vfxLoadSprite)
    {
        Debug.Log("VFXLoadSprite");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        Sprite vfxSprite;
        vfxSprite = Resources.Load<Sprite>(vfxLoadSprite.filePath);

        spriteVFX.GetComponent<SpriteRenderer>().sprite = vfxSprite;
        spriteVFX.transform.position = new Vector3(vfxLoadSprite.width, vfxLoadSprite.height, 0);
        
        bool tmp = vfxLoadSprite.isGIF;
        Debug.Log("ISGIF: " + tmp.ToString());
        
        if (tmp)
        {
            
            Animator animator = spriteVFX.GetComponent<Animator>();
            animator.runtimeAnimatorController = Resources.Load(vfxLoadSprite.filePath + "AnimController") as RuntimeAnimatorController;
            Debug.Log(vfxLoadSprite.filePath + "AnimController");
            spriteVFX.SetActive(true);
            UIScript.SetActive(false);
            isNextButtonActive = false;
            StartCoroutine(WaitUnilAnimFinish(animator));
        }
        else
        {
            Animator animator = spriteVFX.GetComponent<Animator>();
            animator.runtimeAnimatorController = null;

            SpriteRenderer spriteRenderer = spriteVFX.GetComponent<SpriteRenderer>();

            spriteVFX.SetActive(true);
            UIScript.SetActive(false);
            isNextButtonActive = false;
            StartCoroutine(fadeinvfxsprite(spriteRenderer));
        }

    }


    IEnumerator WaitUnilAnimFinish(Animator animator)
    {
        yield return new WaitForSeconds(_GetAnimLength(animator));

        //UIButton.SetActive(true);
        UIScript.SetActive(true);
        isNextButtonActive = true;
        
        ExecuteOneScript();
    }

    IEnumerator fadeinvfxsprite(SpriteRenderer spriteRenderer)
    {
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0.0f;
        while (spriteColor.a < 1.0f)
        {
            spriteColor.a += 0.05f;

            spriteRenderer.color = spriteColor;

            yield return new WaitForSeconds(0.025f);
        }
        UIScript.SetActive(true);
        isNextButtonActive = true;
        
        ExecuteOneScript();
    }

    private void _VFXUnloadSprite(VFXUnloadSprite vfxUnloadSprite)
    {
        Debug.Log("VFXUnloadSprite");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        SpriteRenderer spriteRenderer = spriteVFX.GetComponent<SpriteRenderer>();
        StartCoroutine(dissolveSprite(spriteRenderer));
        
        ExecuteOneScript();
    }

    IEnumerator dissolveSprite(SpriteRenderer spriteRenderer)
    {
        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 1.0f;
        while (spriteColor.a > 0.0f)
        {
            spriteColor.a -= 0.1f;

            spriteRenderer.color = spriteColor;

            yield return new WaitForSeconds(0.025f);
        }
        spriteVFX.SetActive(false);
        spriteColor.a = 1.0f;
        spriteRenderer.color = spriteColor;
    }

    private void _VFXSound(VFXSound vfxSound)
    {
        Debug.Log("VFXSoun");
        AudioClip vfx;
        vfx = Resources.Load<AudioClip>(vfxSound.filePath);
        audioVFX.clip = vfx;
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        float soundLength;
        soundLength = (float)audioVFX.clip.length;

        audioVFX.Play();
        
        ExecuteOneScript();
        //StartCoroutine(soundVFXPlay(vfxSound));
    }

    /*IEnumerator soundVFXPlay(VFXSound vfxSound)
    {
        AudioClip vfx;
        vfx = Resources.Load<AudioClip>(vfxSound.filePath);
        audioVFX.clip = vfx;

        float soundLength;
        soundLength = (float)audioVFX.clip.length;

        audioVFX.Play();

        yield return new WaitForSeconds(soundLength);

        ExecuteOneScript();
    }*/

    private void _LoadMinigame(LoadMinigame loadMinigame)
    {
        Debug.Log("LoadMinigame");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        SetActiveEventSceneThings(false);
        SceneManager.LoadScene(loadMinigame.minigameName, LoadSceneMode.Additive);
        //ExecuteOneScript();
    }

    private void _LoadVideo(LoadVideo loadVideo)
    {
        Debug.Log("LoadVideo");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        VideoClip videoClip;
        videoClip = Resources.Load<VideoClip>(loadVideo.filePath);

        videoVFX.clip = videoClip;

        isNextButtonActive = false;
        UIButton.SetActive(false);
        UIScript.SetActive(false);

        StartCoroutine(loadVideoandPlay());
    }

    IEnumerator loadVideoandPlay()
    {
        float videoLength;
        videoLength = (float) videoVFX.clip.length;

        videoVFX.Play();

        yield return new WaitForSeconds(videoLength);

        UIButton.SetActive(true);
        UIScript.SetActive(true);

        if (isVideoPlaying)
        {
            ExecuteOneScript();
        }
    }

    private void _Choice(Choice choice)
    {
        Debug.Log("Choice");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        //버튼 죽이기
        isNextButtonActive = false;

        //모든 ui 제거
        containerChoice.SetActive(true);
        containerConversation.SetActive(false);
        containerFullScript.SetActive(false);
        UI.transform.Find("ContainerSetting").gameObject.SetActive(false);
        UI.transform.Find("ButtonNext").gameObject.SetActive(false);


        //choice fuction
        _choiceHandler(choice);
    }

    private void _choiceHandler(Choice mChoice)
    {
        List<ChoiceEffect> l = mChoice.choiceList;

        int listNum;
        listNum = l.Count;
        //Debug.Log(listNum);

        int choiceNum;
        choiceNum = mChoice.choiceNumber;

        Debug.Log("choiceNum:" + choiceNum);

        int yPos = 0;

        for (int i = 0; i < listNum; i++)
        {
            ChoiceEffect currentChoiceEffect = l[i];
                       
            if (listNum % 2 == 0)        //if list length is even
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
            choice.GetComponentInChildren<Text>().text = currentChoiceEffect.choiceName;

            //TODO: add onClick function
            choice.GetComponent<Button>().onClick.AddListener
                (() => _OnClickChoiceButton(currentChoiceEffect, choiceNum));
        }
    }

    private void _OnClickChoiceButton
        (ChoiceEffect mChoiceEffect, int mChoiceNum)
    {
        int mChoiceBranchNum;
        mChoiceBranchNum = mChoiceEffect.choiceBranchNumber;

        Debug.Log("choiceBranchNum: " + mChoiceBranchNum);

        //Add chosen choice effect to choice history
        Dictionary<string, List<(int, int)>> choiceHistory =
            GameManager.instance.game.choiceHistories;

        String currentEventName;
        currentEventName = GameManager.instance.currentEvent.eventName;

        List<(int, int)> choiceList = new List<(int, int)>();

        bool isEventExist;
        isEventExist = choiceHistory.ContainsKey(currentEventName);

        Debug.Log("isEventExist: " + isEventExist);

        bool isChoiceListExist;
        isChoiceListExist = choiceHistory.TryGetValue(currentEventName, out choiceList);

        Debug.Log("isChoiceListExist: " + isChoiceListExist);

        //for if there exist one more events
        if((!isEventExist) && (!isChoiceListExist))     //if key and value does not exist
        {
            Debug.Log("first case");

            choiceList = new List<(int, int)>();

            choiceList.Add((mChoiceNum, mChoiceBranchNum));
            choiceHistory.Add(currentEventName, choiceList);
        }
        else if(isEventExist)                           //if key exist
        {
            Debug.Log("second case");

            choiceHistory.TryGetValue(currentEventName, out choiceList);
            choiceList.Add((mChoiceNum, mChoiceBranchNum));
            choiceHistory[currentEventName] = choiceList;
        }
        
        Debug.Log(choiceHistory[currentEventName]);

        GameManager.instance.game.ApplyChoiceEffect(mChoiceEffect);
        GameManager.instance.game.choiceHistories = choiceHistory;

        Debug.Log((GameManager.instance.game.choiceHistories[currentEventName])[0]);
        
        //ExecuteOneScript();

        UI.transform.Find("ContainerSetting").gameObject.SetActive(true);
        UI.transform.Find("ButtonNext").gameObject.SetActive(true);

        //버튼 살리기
        isNextButtonActive = true;
        ExecuteOneScript();
    }

    private void _VFXTransition(VFXTransition vfxTransition)
    {
        Debug.Log("VFXTransition");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        StartCoroutine(transtionVFXPlay());
    }

    IEnumerator transtionVFXPlay()
    {
        UIScript.SetActive(false);
        isNextButtonActive = false;
        yield return StartCoroutine(fadeOut());

        //UIButton.SetActive(false);

        

        yield return new WaitForSeconds(0.25f);
        isNextButtonActive = true;
       
        ExecuteOneScript();
        yield return new WaitForSeconds(0.25f);

        yield return StartCoroutine(fadeIn());
    }

    IEnumerator fadeOut()
    {
        Color color = spriteFade.GetComponent<Image>().color;
        color.a = 0.0f;
        spriteFade.GetComponent<Image>().color = color;

        spriteFade.SetActive(true);

        while (color.a < 1.0f)
        {
            color.a += 0.1f;
            spriteFade.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator fadeIn()
    {
        Color color = spriteFade.GetComponent<Image>().color;
        color.a = 1.0f;
        spriteFade.GetComponent<Image>().color = color;

        spriteFade.SetActive(true);

        while (color.a > 0.0f)
        {
            color.a -= 0.1f;
            spriteFade.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.05f);
        }

        spriteFade.SetActive(false);
    }

    private void _VFXPause(VFXPause vfxPause)
    {
        Debug.Log("VFXPause");
        characterBrightNum = SpriteLocation.None;
        _setBright(SpriteLocation.None);
        isNextButtonActive = false;
        StartCoroutine(pauseVFXPlay(vfxPause.second / 1000));       //millisecond to second
    }

    IEnumerator pauseVFXPlay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        isNextButtonActive = true;
        
        ExecuteOneScript();
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

    public void SetActiveEventSceneThings(bool state)
    {
        UI.SetActive(state);
        UI_ruby.SetActive(state);
        EventItems.SetActive(state);
        EventSceneCamera.SetActive(state);
        spriteBackground.SetActive(state);
        spriteBackgroundTemp.SetActive(state);

    }

    private void _GameEnd(GameEnd gameEnd)
    {
        SceneManager.LoadScene("MainScene");
    }
}
