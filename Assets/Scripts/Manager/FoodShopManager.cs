using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FoodShopManager : MonoBehaviour
{
    public JetonSO coins;
    public TMP_Text coinsTxt;

    public SO_Food[] itemInfoSO;
    public ButtonInfo[] shopPanels;
    public GameObject[] shopSlots;
    public Button[] purchaseButtons;

    public GameObject roomPage;
    public GameObject activityPage;
    public GameObject foodPage;

    void Start()
    {
        coinsTxt.text = "Coins: " + coins.playerCoin;
        CheckBuyable();
        LoadPanels();

        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].SetActive(true);
        }
    }

    public void CheckBuyable()
    {
        for (int i = 0; i < itemInfoSO.Length; i++)
        {
            if (coins.playerCoin >= itemInfoSO[i].coinCost)
            {
                purchaseButtons[i].interactable = true;
            }
            else
            {
                purchaseButtons[i].interactable = false;
            }
        }
    }

    public void BuyItem(int btnNumber)
    {
        if (coins.playerCoin >= itemInfoSO[btnNumber].coinCost && itemInfoSO[btnNumber].quantityBuyable > 0)
        {
            coins.playerCoin -= itemInfoSO[btnNumber].coinCost;
            coinsTxt.text = "Coins: " + coins.playerCoin;
            CheckBuyable();

            itemInfoSO[btnNumber].quantityBuyable--;
            itemInfoSO[btnNumber].isUnlocked = true;
            shopPanels[btnNumber].quantityTxt.text = itemInfoSO[btnNumber].quantityBuyable.ToString();
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < itemInfoSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = itemInfoSO[i].foodName;
            shopPanels[i].priceTxt.text = "Coin(s) " + itemInfoSO[i].coinCost.ToString();
            shopPanels[i].quantityTxt.text = itemInfoSO[i].quantityBuyable.ToString();
        }
    }

    public void ShowRoomPage()
    {
        roomPage.SetActive(true);
    }

    public void HideRoomPage()
    {
        roomPage.SetActive(false);
    }

    public void ShowActivityPage()
    {
        activityPage.SetActive(true);
    }

    public void HideActivityPage()
    {
        activityPage.SetActive(false);
    }

    public void ShowFoodPage()
    {
        foodPage.SetActive(true);
    }

    public void HideFoodPage()
    {
        foodPage.SetActive(false);
    }
}
