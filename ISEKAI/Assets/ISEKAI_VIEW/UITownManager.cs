using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UITownManager : MonoBehaviour
{
    private enum Location
    {
        Outskirts, Town
    }

    public GameObject btnLocation;
    public GameObject background;

    private Button _btnLocation;
    private Text _txtlocation;
    private SpriteRenderer _background;
    private Location _location;

    // Start is called before the first frame update
    void Start()
    {
        _location = Location.Outskirts;
        _background = background.GetComponent<SpriteRenderer>();
        _btnLocation = btnLocation.GetComponent<Button>();
        _txtlocation = btnLocation.GetComponentInChildren<Text>();

        _btnLocation.onClick.AddListener(OnMoveBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMoveBtnClick()
    {
        switch (_location)
        {
            case Location.Outskirts:
               _background.sprite = Resources.Load<Sprite>("bg_town");
                _location = Location.Town;
                _txtlocation.text = "마을 외곽으로";
                break;

            case Location.Town:
                _background.sprite = Resources.Load<Sprite>("bg_outskirts");
                _location = Location.Outskirts;
                _txtlocation.text = "마을로";
                break;

            default:
                throw new InvalidOperationException("Location should be town or outskirts");
        }
    }
}
