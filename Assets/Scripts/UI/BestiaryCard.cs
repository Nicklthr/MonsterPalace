using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static BestiaryController;

public class BestiaryCard : MonoBehaviour
{

    public bool canDelete = false;
    public bool isEnabled = false;
    public int likeIndex;

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextTraduction _label;

    public SO_Monster _monster;
    public Activity _activity;
    public Placement _placement;
    public FoodTypeC _foodType;

    private BestiaryController _bestiary;

    public void Awake()
    {

    }

    private void OnEnable()
    {

    }

    public void InitializeCard(ButtonSelected type ) 
    {


        _bestiary = GameObject.FindGameObjectWithTag("Bestiary").GetComponent<BestiaryController>();


        if (!isEnabled)
        {
            _button.enabled = false;
        }

        if (!canDelete)
        {
            _closeButton.gameObject.SetActive(false);
        }

        switch (type)
        {
            case ButtonSelected.FOOD:
                if (_foodType.sprite != null)
                {
                    _image.sprite = _foodType.sprite;
                }
                _label.AssignID(_foodType.type.ToString() + "menu");
                break;
            case ButtonSelected.NEIGHBOURS:
                if (_monster.monsterSprite != null)
                {
                    _image.sprite = _monster.monsterSprite;
                }
                _label.AssignID(_monster.monsterType.ToString() + "menu");
                break;
            case ButtonSelected.PLACEMENT:
                if (_placement.sprite != null)
                {
                    _image.sprite = _placement.sprite;
                }
                _label.AssignID(_placement.type.ToString() + "menu");
                break;
            case ButtonSelected.ACTIVITY:
                if (_activity.sprite != null)
                {
                    _image.sprite = _activity.sprite;
                }
                _label.AssignID(_activity.type.ToString() + "menu");
                break;
            default:
                break;
        }

    }


    public void DeleteCard()
    {
        switch (_bestiary.currentButtonSelected)
        {
            case BestiaryController.ButtonSelected.FOOD:
                _bestiary.RemoveElement(likeIndex, null, null, null, _foodType);
                break;
            case BestiaryController.ButtonSelected.NEIGHBOURS:
                _bestiary.RemoveElement(likeIndex, _monster, null, null, null);
                break;
            case BestiaryController.ButtonSelected.PLACEMENT:
                _bestiary.RemoveElement(likeIndex, null, null, _placement, null);
                break;
            case BestiaryController.ButtonSelected.ACTIVITY:
                _bestiary.RemoveElement(likeIndex, null, _activity, null, null);
                break;
            default:
                break;
        }

        _bestiary.SelectButton();
    }

    public void AddCard()
    {
        switch (_bestiary.currentButtonSelected)
        {
            case BestiaryController.ButtonSelected.FOOD:
                _bestiary.AddElement(likeIndex, null, null, null, _foodType);
                break;
            case BestiaryController.ButtonSelected.NEIGHBOURS:
                _bestiary.AddElement(likeIndex, _monster, null, null, null);
                break;
            case BestiaryController.ButtonSelected.PLACEMENT:
                _bestiary.AddElement(likeIndex, null, null, _placement, null);
                break;
            case BestiaryController.ButtonSelected.ACTIVITY:
                _bestiary.AddElement(likeIndex, null, _activity, null, null);
                break;
            default:
                break;
        }

        _bestiary.SelectButton();
    }

    public BestiaryCard(bool _canDelete, bool _isEnabled, int _likeIndex, SO_Monster monster = null, Activity activity = null, Placement placement = null, FoodTypeC foodType = null)
    {
        canDelete = _canDelete;
        isEnabled = _isEnabled;
        likeIndex = _likeIndex;
        _monster = monster;
        _activity = activity;
        _placement = placement;
        _foodType = foodType;
    }
}
