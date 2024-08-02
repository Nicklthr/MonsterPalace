using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReviewPanelManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject _reviewsListBackground;
    [SerializeField] private GameObject _reviewsListPanel;
    [SerializeField] private RatingBarStarUI _ratingBarStar;
    [SerializeField] private Button _reveiwsBtn;

    [Space(10)]
    [SerializeField] private GameObject _reviewPrefab;
    [SerializeField] private Transform _reviewListContent;

    [Space(10)]
    [Header("Reviews")]
    [SerializeField] private TextMeshProUGUI _reviewsCount;

    [Header("Satisfaction TextMesh")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _satisfactionQuantity;
    [SerializeField] private TextMeshProUGUI _satisfactionMaxQuantity;

    [Header("Satisfaction Bar")]
    [Space(10)]
    [SerializeField] private Image _satisfactionBar;
    [SerializeField] private MMF_Player _feelUpdateBar;
    [SerializeField] private MMF_Player _feelAlertBar;

    [Space(10)]
    [Header("Manager")]
    [SerializeField] private HotelRateManager _hotelRateManager;

    public UnityEvent OnReviewAdd = new UnityEvent();

    [SerializeField] private List<SO_Monster> _monsters = new List<SO_Monster>();

    [SerializeField] private bool _debug = false;

    public void Start()
    {
        _hotelRateManager = FindObjectOfType<HotelRateManager>();

        _reviewsListBackground.SetActive(false);
        _reviewsListPanel.SetActive(false);

        _reveiwsBtn.onClick.AddListener(() =>
        {
            ToggleReviewsListPanel();
        });

        _hotelRateManager.OnInitialRating += UpdateGlobalReviewAtStart;
        _hotelRateManager.OnReviewAdd += UpdateGlobalReview;
    }

    public void Update()
    {

        if (IsSatisfactionValueInAlertRange())
        {
            _feelAlertBar.PlayFeedbacks();
        }else
        {
            _feelAlertBar.StopFeedbacks();
            _satisfactionBar.color = Color.white;
        }

        if ( _debug )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UpdateGlobalReview();
                UpdateReviewList();
            }
        }
    }

    private void UpdateGlobalReview()
    {
        int currentSatisfactionQuantity = int.Parse( _satisfactionQuantity.text );

        _feelUpdateBar.GetFeedbackOfType<MMF_ImageFill>().CurveRemapZero = ConvertTo01Range(currentSatisfactionQuantity);
        _feelUpdateBar.GetFeedbackOfType<MMF_ImageFill>().CurveRemapOne = ConvertTo01Range(_hotelRateManager.currentSatisfactionQuantity);
        _feelUpdateBar.PlayFeedbacks();

        Debug.Log(ConvertTo01Range(currentSatisfactionQuantity));


        NumericTextAnimator.Instance.AnimateTextTo( _satisfactionQuantity, (int)_hotelRateManager.currentSatisfactionQuantity);
        _satisfactionMaxQuantity.text = "/" + _hotelRateManager.MinMaxSatisfactionThreshold.y.ToString();
        
        if (_hotelRateManager.totalReviews == 0)
        {
            NumericTextAnimator.Instance.AnimateTextTo(_reviewsCount, _hotelRateManager.totalReviews);
        }
        else
        {
            NumericTextAnimator.Instance.AnimateTextTo(_reviewsCount, _hotelRateManager.totalReviews);
            gameObject.GetComponent<MMF_Player>().PlayFeedbacks();
        }

        UpdateReviewList();

        OnReviewAdd.Invoke();

    }

    private void UpdateGlobalReviewAtStart()
    {
        int currentSatisfactionQuantity = 0;

        _feelUpdateBar.GetFeedbackOfType<MMF_ImageFill>().CurveRemapZero = ConvertTo01Range(ConvertSatisfactionValue(currentSatisfactionQuantity));
        _feelUpdateBar.GetFeedbackOfType<MMF_ImageFill>().CurveRemapOne = ConvertTo01Range(_hotelRateManager.currentSatisfactionQuantity);

        _feelUpdateBar.PlayFeedbacks();

        Debug.Log(ConvertTo01Range(currentSatisfactionQuantity));


        NumericTextAnimator.Instance.AnimateTextTo(_satisfactionQuantity, (int)_hotelRateManager.currentSatisfactionQuantity);
        _satisfactionMaxQuantity.text = "/" + _hotelRateManager.MinMaxSatisfactionThreshold.y.ToString();

        if (_hotelRateManager.totalReviews == 0)
        {
            NumericTextAnimator.Instance.AnimateTextTo(_reviewsCount, _hotelRateManager.totalReviews);
        }
        else
        {
            NumericTextAnimator.Instance.AnimateTextTo(_reviewsCount, _hotelRateManager.totalReviews);
            gameObject.GetComponent<MMF_Player>().PlayFeedbacks();
        }

        UpdateReviewList();

        OnReviewAdd.Invoke();
    }

    private void ToggleReviewsListPanel()
    {
        _reviewsListBackground.SetActive(!_reviewsListBackground.activeSelf);
        _reviewsListPanel.SetActive(!_reviewsListPanel.activeSelf);
    }

    private void UpdateReviewList()
    {
        foreach ( Transform child in _reviewListContent )
        {
            Destroy( child.gameObject );
        }

        foreach (RateReviews review in _hotelRateManager.listReviews)
        {
            GameObject reviewRow = Instantiate( _reviewPrefab, _reviewListContent);

            reviewRow.GetComponent<RatingBarStarUI>().UpdateBar( review.note );
            reviewRow.GetComponent<SingleReviewMonsterUI>().SetMonsterData( review.monsterName, review.type.ToString(), review.review, GetSpriteMonsterByType(review.type) );
        }
    }

    private Sprite GetSpriteMonsterByType( MonsterType type )
    {
        return _monsters.Find( monster => monster.monsterType == type ).monsterSprite;
    }
   

    public float ConvertSatisfactionValue(float value)
    {
        // S'assurer que la valeur d'entrée est bien entre -100 et 100
        value = Mathf.Clamp(value, -100f, 100f);

        // Conversion de la plage [-100, 100] à [0, 100]
        return (value + 100f) / 2f;
    }

    public float ConvertTo01Range(float value)
    {
        // S'assurer que la valeur d'entrée est bien entre 0 et 100
        value = Mathf.Clamp(value, 0f, 100f);

        // Conversion de la plage [0, 100] à [0, 1]
        return value / 100f;
    }

    private bool IsSatisfactionValueInAlertRange()
    {
        float value = ConvertSatisfactionValue(_hotelRateManager.currentSatisfactionQuantity);

        // S'assurer que la valeur d'entrée est bien entre 0 et 100
        value = Mathf.Clamp(value, 0f, 100f);

        return value <= 25f;
    }
}
