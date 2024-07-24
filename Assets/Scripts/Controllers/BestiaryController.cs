using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Michsky.UI.Dark.UIManagerText;

public class BestiaryController : MonoBehaviour
{

    [SerializeField] private SO_Bestiary _bestiary;
    [SerializeField] private SO_MonsterList _monsterList;
    [SerializeField] private SO_ActivityList _activityList;
    [SerializeField] private SO_FoodTypeList _foodTypeList;
    [SerializeField] private SO_PlacementList _placementList;

    [SerializeField] private GameObject _cardPrefab;

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

    [SerializeField] private GameObject _foodLikesList;
    [SerializeField] private GameObject _foodDislikesList;
    [SerializeField] private GameObject _neighbourLikesList;
    [SerializeField] private GameObject _neighbourDislikesList;
    [SerializeField] private GameObject _placementLikesList;
    [SerializeField] private GameObject _placementDislikesList;
    [SerializeField] private GameObject _activityLikesList;

    [SerializeField] private GameObject _selectItemPanel;
    [SerializeField] private GameObject _selectItemPanelList;

    public enum ButtonSelected
    {
        FOOD, NEIGHBOURS, PLACEMENT, ACTIVITY
    }

    public ButtonSelected currentButtonSelected = ButtonSelected.FOOD;

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
        // _speciesDescription.AssignID(_bestiary.monsterEntries[index].monsterDatas.monsterType.ToString()+"description");
        ReloadList(_foodLikesList, ButtonSelected.FOOD, 1);
        ReloadList(_foodDislikesList, ButtonSelected.FOOD, 2);
        ReloadList(_neighbourLikesList, ButtonSelected.NEIGHBOURS, 1);
        ReloadList(_neighbourDislikesList, ButtonSelected.NEIGHBOURS, 2);
        ReloadList(_placementLikesList, ButtonSelected.PLACEMENT, 1);
        ReloadList(_placementDislikesList, ButtonSelected.PLACEMENT, 2);
        ReloadList(_activityLikesList, ButtonSelected.ACTIVITY, 1);


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
        SelectButton();

    }

    public void SelectButton()
    {
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

    public void AddElement(int likeIndex, SO_Monster monster = null, Activity activity = null, Placement placement = null, FoodTypeC foodType = null)
    {
        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                if (likeIndex == 1)
                {
                    _bestiary.monsterEntries[currentIndex].foodTastes.foodLikes.Add(foodType);
                    ReloadList(_foodLikesList, currentButtonSelected, 1);
                }
                else if (likeIndex == 2)
                {
                    _bestiary.monsterEntries[currentIndex].foodTastes.foodDislikes.Add(foodType);
                    ReloadList(_foodDislikesList, currentButtonSelected, 2);
                } 
                break;
            case ButtonSelected.NEIGHBOURS:
                if (likeIndex == 1)
                {
                    _bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourLikes.Add(monster);
                    ReloadList(_neighbourLikesList, currentButtonSelected, 1);
                }
                else if (likeIndex == 2)
                {
                    _bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourDislikes.Add(monster);
                    ReloadList(_neighbourDislikesList, currentButtonSelected, 2);
                }
                break;
            case ButtonSelected.PLACEMENT:
                if (likeIndex == 1)
                {
                    _bestiary.monsterEntries[currentIndex].placementTastes.placementLikes.Add(placement);
                    ReloadList(_placementLikesList, currentButtonSelected, 1);
                }
                else if (likeIndex == 2)
                {
                    _bestiary.monsterEntries[currentIndex].placementTastes.placementDislikes.Add(placement);
                    ReloadList(_placementDislikesList, currentButtonSelected, 2);
                }
                break;
            case ButtonSelected.ACTIVITY:
                _bestiary.monsterEntries[currentIndex].activityTastes.activityLikes.Add(activity);
                ReloadList(_activityLikesList, currentButtonSelected, 1);
                break;
            default:
                break;
        }

        _selectItemPanel.gameObject.SetActive(false);
    }

    public void RemoveElement(int likeIndex, SO_Monster monster = null, Activity activity = null, Placement placement = null, FoodTypeC foodType = null)
    {
        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                if (likeIndex == 1)
                {
                    _bestiary.monsterEntries[currentIndex].foodTastes.foodLikes.Remove(foodType);
                    ReloadList(_foodLikesList, currentButtonSelected, 1);
                }
                else if (likeIndex == 2)
                {
                    _bestiary.monsterEntries[currentIndex].foodTastes.foodDislikes.Remove(foodType);
                    ReloadList(_foodDislikesList, currentButtonSelected, 2);
                }
                break;
            case ButtonSelected.NEIGHBOURS:
                if (likeIndex == 1)
                {
                    _bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourLikes.Remove(monster);
                    ReloadList(_neighbourLikesList, currentButtonSelected, 1);
                }
                else if (likeIndex == 2)
                {
                    _bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourDislikes.Remove(monster);
                    ReloadList(_neighbourDislikesList, currentButtonSelected, 2);
                }
                break;
            case ButtonSelected.PLACEMENT:
                if (likeIndex == 1)
                {
                    _bestiary.monsterEntries[currentIndex].placementTastes.placementLikes.Remove(placement);
                    ReloadList(_placementLikesList, currentButtonSelected, 1);
                }
                else if (likeIndex == 2)
                {
                    _bestiary.monsterEntries[currentIndex].placementTastes.placementDislikes.Remove(placement);
                    ReloadList(_placementDislikesList, currentButtonSelected, 2);
                }
                break;
            case ButtonSelected.ACTIVITY:
                _bestiary.monsterEntries[currentIndex].activityTastes.activityLikes.Remove(activity);
                ReloadList(_activityLikesList, currentButtonSelected, 1);
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

    public void OpenItemPanel(int likeIndex)
    {

        foreach (Transform t in _selectItemPanelList.transform)
        {
            Destroy(t.gameObject);
        }


        switch (currentButtonSelected)
        {
            case ButtonSelected.FOOD:
                foreach(FoodTypeC t in _foodTypeList.foodTypeList)
                {
                    if(!_bestiary.monsterEntries[currentIndex].foodTastes.foodLikes.Contains(t) && !_bestiary.monsterEntries[currentIndex].foodTastes.foodDislikes.Contains(t))
                    {
                        var card = Instantiate(_cardPrefab, _selectItemPanelList.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = false;
                        bestiaryCardComponent.isEnabled = true;
                        bestiaryCardComponent._foodType = t;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.FOOD);
                    }
                }
                break;
            case ButtonSelected.NEIGHBOURS:
                foreach (SO_Monster t in _monsterList.monsterList)
                {
                    if (!_bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourLikes.Contains(t) && !_bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourDislikes.Contains(t))
                    {
                        var card = Instantiate(_cardPrefab, _selectItemPanelList.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = false;
                        bestiaryCardComponent.isEnabled = true;
                        bestiaryCardComponent._monster = t;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.NEIGHBOURS);
                    }
                }
                break;
            case ButtonSelected.PLACEMENT:
                foreach (Placement t in _placementList.placementList)
                {
                    if (!_bestiary.monsterEntries[currentIndex].placementTastes.placementLikes.Contains(t) && !_bestiary.monsterEntries[currentIndex].placementTastes.placementDislikes.Contains(t))
                    {
                        var card = Instantiate(_cardPrefab, _selectItemPanelList.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = false;
                        bestiaryCardComponent.isEnabled = true;
                        bestiaryCardComponent._placement = t;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.PLACEMENT);
                    }
                }
                break;
            case ButtonSelected.ACTIVITY:
                foreach (Activity t in _activityList.activityList)
                {
                    if (!_bestiary.monsterEntries[currentIndex].activityTastes.activityLikes.Contains(t))
                    {
                        var card = Instantiate(_cardPrefab, _selectItemPanelList.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = false;
                        bestiaryCardComponent.isEnabled = true;
                        bestiaryCardComponent._activity = t;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.ACTIVITY);
                    }
                }
                break;
            default:
                break;
        }

        _selectItemPanel.gameObject.SetActive(true);
        SelectButton();
    }

    public void ReloadList(GameObject list, ButtonSelected type, int likeIndex)
    {
        foreach(Transform t in list.transform)
        {
            if (t.gameObject.tag != "doNotDelete")
            {
                Destroy(t.gameObject);
            }
    
        }

        switch (type)
        {
            case ButtonSelected.FOOD:
                if (likeIndex == 1)
                {
                    foreach (FoodTypeC taste in _bestiary.monsterEntries[currentIndex].foodTastes.foodLikes)
                    {
                        var card = Instantiate(_cardPrefab, list.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = true;
                        bestiaryCardComponent.isEnabled = false;
                        bestiaryCardComponent._foodType = taste;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.FOOD);
                    }
 
                }
                else if (likeIndex == 2)
                {
                    foreach (FoodTypeC taste in _bestiary.monsterEntries[currentIndex].foodTastes.foodDislikes)
                    {
                        var card = Instantiate(_cardPrefab, list.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = true;
                        bestiaryCardComponent.isEnabled = false;
                        bestiaryCardComponent._foodType = taste;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.FOOD);
                    }
                }
                break;
            case ButtonSelected.NEIGHBOURS:
                if (likeIndex == 1)
                {
                    foreach (SO_Monster taste in _bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourLikes)
                    {
                        var card = Instantiate(_cardPrefab, list.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = true;
                        bestiaryCardComponent.isEnabled = false;
                        bestiaryCardComponent._monster = taste;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.NEIGHBOURS);
                    }

                }
                else if (likeIndex == 2)
                {
                    foreach (SO_Monster taste in _bestiary.monsterEntries[currentIndex].neighbourTastes.neighbourDislikes)
                    {
                        var card = Instantiate(_cardPrefab, list.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = true;
                        bestiaryCardComponent.isEnabled = false;
                        bestiaryCardComponent._monster = taste;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.NEIGHBOURS);
                    }
                }
                break;
            case ButtonSelected.PLACEMENT:
                if (likeIndex == 1)
                {
                    foreach (Placement taste in _bestiary.monsterEntries[currentIndex].placementTastes.placementLikes)
                    {
                        var card = Instantiate(_cardPrefab, list.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = true;
                        bestiaryCardComponent.isEnabled = false;
                        bestiaryCardComponent._placement = taste;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.PLACEMENT);
                    }

                }
                else if (likeIndex == 2)
                {
                    foreach (Placement taste in _bestiary.monsterEntries[currentIndex].placementTastes.placementDislikes)
                    {
                        var card = Instantiate(_cardPrefab, list.transform);
                        var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                        bestiaryCardComponent.canDelete = true;
                        bestiaryCardComponent.isEnabled = false;
                        bestiaryCardComponent._placement = taste;
                        bestiaryCardComponent.likeIndex = likeIndex;
                        bestiaryCardComponent.InitializeCard(ButtonSelected.PLACEMENT);
                    }
                }
                break;
            case ButtonSelected.ACTIVITY:
                foreach (Activity taste in _bestiary.monsterEntries[currentIndex].activityTastes.activityLikes)
                {
                    var card = Instantiate(_cardPrefab, list.transform);
                    var bestiaryCardComponent = card.GetComponent<BestiaryCard>();
                    bestiaryCardComponent.canDelete = true;
                    bestiaryCardComponent.isEnabled = false;
                    bestiaryCardComponent._activity = taste;
                    bestiaryCardComponent.likeIndex = likeIndex;
                    bestiaryCardComponent.InitializeCard(ButtonSelected.ACTIVITY);
                }
                break;
            default:
                break;
        }
    }
}
