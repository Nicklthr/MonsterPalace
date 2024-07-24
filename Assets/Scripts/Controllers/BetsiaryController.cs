using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BetsiaryController : MonoBehaviour
{

    [SerializeField] private SO_Bestiary _bestiary;
    [SerializeField] private SO_MonsterList _monsterList;
    [SerializeField] private SO_ActivityList _activityList;
    [SerializeField] private SO_FoodTypeList _foodTypeList;
    [SerializeField] private SO_PlacementList _placementList;

    [SerializeField] private TextTraduction _speciesName;
    [SerializeField] private TextTraduction _speciesDescription;
    [SerializeField] private Image _speciesSprite;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;

    [SerializeField] private Button _foodButton;
    [SerializeField] private Button _neighboursButton;
    [SerializeField] private Button _activityButton;
    [SerializeField] private Button _placementsButton;

    [SerializeField] private GameObject _foodPanel;
    [SerializeField] private GameObject _neighboursPanel;
    [SerializeField] private GameObject _activityPanel;
    [SerializeField] private GameObject _placementsPanel;

    [SerializeField] private GameObject _selectItemPanel;

    private enum ButtonSelected
    {
        FOOD, NEIGHBOURS, PLACEMENT, ACTIVITY
    }

    private ButtonSelected currentButtonSelected = ButtonSelected.FOOD;

    private int currentIndex = 0;


    void OnEnable()
    {


    }


    // Start is called before the first frame update
    void Start()
    {
        ChargeMonsterDatas(currentIndex);
        CheckButton();
        _foodButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChargeMonsterDatas(int index)
    {
        _speciesSprite.sprite = _bestiary.monsterEntries[index].monsterDatas.monsterSprite;
        _speciesName.AssignID(_bestiary.monsterEntries[index].monsterDatas.monsterType.ToString());
        //_speciesDescription.AssignID(_bestiary.monsterEntries[index].monsterDatas.monsterType.ToString()+"description"); ;
    }

    
    public void CheckButton()
    {
        if (currentIndex == 0)
        {
            _previousButton.gameObject.SetActive(false);
        }
        else
        {
            _previousButton.gameObject.SetActive(true);
        }

        if (currentIndex == _bestiary.monsterEntries.Count - 1)
        {
            _nextButton.gameObject.SetActive(false);
        }
        else
        {
            _nextButton.gameObject.SetActive(true);
        }


    }
    
    public void NextPage()
    {
        currentIndex++;
        ChargeMonsterDatas(currentIndex);
        CheckButton();
        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                _foodButton.Select();
                break;
            case ButtonSelected.NEIGHBOURS:
                _neighboursButton.Select();
                break;
            case ButtonSelected.PLACEMENT:
                _placementsButton.Select();
                break;
            case ButtonSelected.ACTIVITY:
                _activityButton.Select();
                break;
            default:
                break;
        }
    }

    public void PrevPage()
    {
        currentIndex--;
        ChargeMonsterDatas(currentIndex);
        CheckButton();
        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                _foodButton.Select();
                break;
            case ButtonSelected.NEIGHBOURS:
                _neighboursButton.Select();
                break;
            case ButtonSelected.PLACEMENT:
                _placementsButton.Select();
                break;
            case ButtonSelected.ACTIVITY:
                _activityButton.Select();
                break;
            default:
                break;
        }
    }

    public void AddElement(int likeIndex)
    {
        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                break;
            case ButtonSelected.NEIGHBOURS:
                break;
            case ButtonSelected.PLACEMENT:
                break;
            case ButtonSelected.ACTIVITY:
                break;
            default:
                break;
        }
    }

    public void RemoveElement(int likeIndex)
    {
        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                break;
            case ButtonSelected.NEIGHBOURS:
                break;
            case ButtonSelected.PLACEMENT:
                break;
            case ButtonSelected.ACTIVITY:
                break;
            default:
                break;
        }
    }


    public void SelectPanel(int indexButtonSelected)
    {
        switch (indexButtonSelected)
        {
            case 1:
                currentButtonSelected = ButtonSelected.FOOD;
                break;
            case 2:
                currentButtonSelected = ButtonSelected.NEIGHBOURS;
                break;
            case 3:
                currentButtonSelected = ButtonSelected.PLACEMENT;
                break;
            case 4:
                currentButtonSelected = ButtonSelected.ACTIVITY;
                break;
            default:
                break;
        }

        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                _foodPanel.SetActive(true);
                _neighboursPanel.SetActive(false);
                _activityPanel.SetActive(false);
                _placementsPanel.SetActive(false);
                break;
            case ButtonSelected.NEIGHBOURS:
                _foodPanel.SetActive(false);
                _neighboursPanel.SetActive(true);
                _activityPanel.SetActive(false);
                _placementsPanel.SetActive(false);
                break;
            case ButtonSelected.PLACEMENT:
                _foodPanel.SetActive(false);
                _neighboursPanel.SetActive(false);
                _activityPanel.SetActive(false);
                _placementsPanel.SetActive(true);
                break;
            case ButtonSelected.ACTIVITY:
                _foodPanel.SetActive(false);
                _neighboursPanel.SetActive(false);
                _activityPanel.SetActive(true);
                _placementsPanel.SetActive(false);
                break;
            default:
                break;
        }
    }
}
