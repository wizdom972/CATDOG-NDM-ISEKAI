using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using ISEKAI_Model;
using System.Linq;

public enum Location
{
    Outskirts, Town
}

public class UITownManager : MonoBehaviour
{
    

    public TutorialManager tutorialManager;

    public Transform test;

    public GameObject moveBtnLocation;
    public GameObject ButtonGoOutskirt;
    public GameObject background;

    public Text textPleasant;
    public Text textFood;
    public Text textTurn;
    public Text textAP;
    public Text textLocation;

    public Sprite townSprite;
    public Sprite outskirtsSprite;

    private Button _moveBtnLocation;
    private Button _buttonGoOutskirt;

    private Text _moveTxtlocation;
    private Text _buttonGoOutskirtText;

    private SpriteRenderer _background;
    private Location _location;
    private GameObject _eventList;

    public Transform town, outskirts;
    

    // Start is called before the first frame update
    void Start()
    {
        _location = Location.Outskirts;
        _background = background.GetComponent<SpriteRenderer>();
        _moveBtnLocation = moveBtnLocation.GetComponent<Button>();
        _buttonGoOutskirt = ButtonGoOutskirt.GetComponent<Button>();

        _moveTxtlocation = moveBtnLocation.GetComponentInChildren<Text>();
        _buttonGoOutskirtText = ButtonGoOutskirt.GetComponentInChildren<Text>();
        
        _moveBtnLocation.onClick.AddListener(OnMoveBtnClick);
        _buttonGoOutskirt.onClick.AddListener(OnButtonGoOutskirtClicked);

        UpdatePanel();
        if (!GameManager.instance.isTutorialPlayed)
        {
            //tutorialManager.InitTexts();
            //tutorialManager.ProceedTutorial();
        }
        SetParentsOfEvents();
    }

    //If button clicked, change location, and replace ui depend on location
    public void OnMoveBtnClick()
    {
        GameManager gm = GameManager.instance;
        Debug.Log(gm.game.turn.turnNumber);
        //tutorialManager.ProceedTutorial();
        switch (_location)
        {
            case Location.Outskirts:
                _background.sprite = townSprite;
                _location = Location.Town;
                outskirts.gameObject.SetActive(false);
                town.gameObject.SetActive(true);
                textLocation.text = "마을";

                moveBtnLocation.SetActive(false);
                ButtonGoOutskirt.SetActive(true);


                break;

            default:
                throw new InvalidOperationException("Location should be town or outskirts");
        }
        GameManager.instance.TryUpdateEventSDs();
    }

    //If button clicked, change location, and replace ui depend on location
    public void OnButtonGoOutskirtClicked()
    {
        GameManager gm = GameManager.instance;
        Debug.Log(gm.game.turn.turnNumber);
        //tutorialManager.ProceedTutorial();
        switch (_location)
        {

            case Location.Town:
                _background.sprite = outskirtsSprite;
                _location = Location.Outskirts;
                outskirts.gameObject.SetActive(true);
                town.gameObject.SetActive(false);
                textLocation.text = "마을 외곽";

                moveBtnLocation.SetActive(true);
                ButtonGoOutskirt.SetActive(false);

                break;

            default:
                throw new InvalidOperationException("Location should be town or outskirts");
        }
        GameManager.instance.TryUpdateEventSDs();
    }

    /*
    public void OnClickNextTurnButton()
    {
        Game _game = GameManager.instance.game;
        _game.Proceed();
        GameManager.instance.forcedEventEnumerator = GameManager.instance.game.forcedVisibleEventList.GetEnumerator();
        GameManager.instance.TryOccurForcedEvent();
        UpdatePanel();
        Debug.Log(GameManager.instance.game.turn.turnNumber);
        foreach (EventCore e in GameManager.instance.game.visibleEventsList)
            Debug.Log(e.eventName);
    }
    */
    public void UpdatePanel()
    {
        Game _game = GameManager.instance.game;
        textFood.text = _game.town.remainFoodAmount.ToString();
        textPleasant.text = _game.town.totalPleasantAmount + "/" + 200;
        textTurn.text = _game.turn.ToString();
        GameManager.instance.TryInstantiateEventSDs();
        GameManager.instance.TryUpdateEventSDs();
        SetParentsOfEvents();
    }
    
    public void SetParentsOfEvents()
    {
        GameManager gm = GameManager.instance;
        foreach(Transform t in gm.eventSDList)
        {
            if (t == null)
                continue;
            else
            {
                if (gm.SmallLocationToBigLocation(gm.GetEventCoreFromEventSd(t).location) == Location.Town)
                    t.SetParent(town);
                else
                    t.SetParent(outskirts);
            }
        }
    }
}
