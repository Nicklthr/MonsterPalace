using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleReviewMonsterUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _monsterName;
    [SerializeField] private TextMeshProUGUI _monsterType;
    [SerializeField] private Image _monsterImage;
    [SerializeField] private TextMeshProUGUI _review;

    public void SetMonsterData(string name, string type, string review, Sprite monsterImage = null)
    {
        _monsterName.text = name;
        _monsterType.text = type;
        _review.text = review;

        if (monsterImage != null)
        {
            _monsterImage.sprite = monsterImage;
        }

    }
}
