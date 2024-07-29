using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class RoomShopManager : MonoBehaviour
{
    public JetonSO coins;
    public TextMeshProUGUI coinsTxt;

    [Space(10)]
    public SO_RoomType[] itemsBedroom;
    public SO_RoomType[] itemsActivity;
    public SO_Food[] itemsFood;

    [Space(10)]
    public GameObject bedroomPanel;
    public GameObject activityPanel;
    public GameObject foodPanel;

    [Space (10)]
    [Header("Button Prefab")]
    public GameObject buttonFoodPrefab;
    public GameObject buttonRoomPrefab;

    [Space(10)]
    [Header("Event")]
    public UnityEvent OnBuyItem = new UnityEvent();
    public UnityEvent OnCantBuyItem = new UnityEvent();

    void Start()
    {
        int coin = (int)coins.playerCoin;
        NumericTextAnimator.Instance.AnimateTextTo(coinsTxt, coin, 1f);

        CreateBedroomItems();
        CreateActivityItems();
        CreateFoodItems();
        
    }

    private void CreateFoodItems()
    {

       foreach (SO_Food item in itemsFood)
        {
            GameObject button = Instantiate(buttonFoodPrefab, foodPanel.transform);
            ButtonInfo buttonInfo = button.GetComponent<ButtonInfo>();

            buttonInfo.titleTxt.text = item.foodName;

            if (item.isUnlocked)
            {
                buttonInfo.priceTxt.gameObject.SetActive(false);
                buttonInfo.priceInt.text = "Unlocked";
                button.GetComponent<Button>().interactable = false;
            }
            else
            {
                buttonInfo.priceInt.text = item.coinCost.ToString() + " coins";
                button.GetComponent<Button>().onClick.AddListener(() => BuyItemFood(item));
            }
        }
    }

    private void CreateActivityItems()
    {
        foreach (SO_RoomType item in itemsActivity)
        {
            GameObject button = Instantiate(buttonRoomPrefab, activityPanel.transform);
            ButtonInfo buttonInfo = button.GetComponent<ButtonInfo>();

            buttonInfo.titleTxt.text = item.roomName;

            if (item.isUnlocked)
            {
                buttonInfo.priceTxt.gameObject.SetActive(false);
                buttonInfo.priceInt.text = "Unlocked";
                button.GetComponent<Button>().interactable = false;
            }
            else
            {
                buttonInfo.priceInt.text = item.coinCost.ToString() + " coins";
                button.GetComponent<Button>().onClick.AddListener(() => BuyItem(item));
            }
        }
    }

    private void CreateBedroomItems()
    {
        foreach (SO_RoomType item in itemsBedroom)
        {
            GameObject button = Instantiate(buttonRoomPrefab, bedroomPanel.transform);
            ButtonInfo buttonInfo = button.GetComponent<ButtonInfo>();

            buttonInfo.titleTxt.text = item.roomName;

            if (item.isUnlocked)
            {
                buttonInfo.priceTxt.gameObject.SetActive(false);
                buttonInfo.priceInt.text = "Unlocked";
                button.GetComponent<Button>().interactable = false;
            }
            else
            {
                buttonInfo.priceInt.text = item.coinCost.ToString() + " coins";
                button.GetComponent<Button>().onClick.AddListener(() => BuyItem(item));
            }
        }
    }

    public void BuyItemFood(SO_Food item)
    {
        if (coins.playerCoin >= item.coinCost)
        {
            if (coins.playerCoin + item.coinCost < 0)
            {
                coins.playerCoin = 0;
                NumericTextAnimator.Instance.AnimateTextTo(coinsTxt, (int)coins.playerCoin, 1f);
            }
            else
            {
                coins.playerCoin -= item.coinCost;
                NumericTextAnimator.Instance.AnimateTextTo(coinsTxt, (int)coins.playerCoin, 1f);

            }
            item.isUnlocked = true;

            UpdateUI();
            OnBuyItem.Invoke();
        }
        else
        {
            OnCantBuyItem.Invoke();
        }

    }

    public void BuyItem(SO_RoomType item)
    {
        if (coins.playerCoin >= item.coinCost)
        {
            if (coins.playerCoin + item.coinCost < 0)
            {
                coins.playerCoin = 0;
                NumericTextAnimator.Instance.AnimateTextTo(coinsTxt, (int)coins.playerCoin, 1f);
            }
            else
            {
                coins.playerCoin -= item.coinCost;
                NumericTextAnimator.Instance.AnimateTextTo(coinsTxt, (int)coins.playerCoin, 1f);

            }

            item.isUnlocked = true;

            UpdateUI();
            OnBuyItem.Invoke();
        }
        else
        {
            OnCantBuyItem.Invoke();
        }

    }

    private void UpdateUI()
    {
        foreach (Transform child in bedroomPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in activityPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in foodPanel.transform)
        {
            Destroy(child.gameObject);
        }

        CreateBedroomItems();
        CreateActivityItems();
        CreateFoodItems();
    }

}
