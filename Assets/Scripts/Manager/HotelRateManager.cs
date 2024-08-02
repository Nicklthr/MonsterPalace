using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RateReviews
{
    [field: SerializeField]
    public float note { get; private set; }

    [field: SerializeField]
    public float satisfactionQuantity { get; private set; }

    [field: SerializeField]
    public string review { get; private set; }

    [field: SerializeField]
    public string monsterName { get; private set; }

    [field: SerializeField]
    public MonsterType type { get; private set; }

    public RateReviews(float note, string review, string monsterName, MonsterType type, float satisfactionQuantity = 0) 
    { 
        this.note = note;
        this.satisfactionQuantity = satisfactionQuantity;
        this.review = "- "+ review + " -";
        this.monsterName = monsterName;
        this.type = type;
    }
}

public class HotelRateManager : MonoBehaviour
{
    public List<RateReviews> listReviews = new List<RateReviews>();

    public float averageCurrentRating;
    public int totalReviews;
    public float currentSatisfactionQuantity;
    public Vector2 MinMaxSatisfactionThreshold = new Vector2(-100, 100);

    public event Action OnReviewAdd;
    public event Action OnInitialRating;
    public SO_HotelRating hotelRating;

    private void Start()
    {
        hotelRating.InitializeRateing();

        averageCurrentRating = hotelRating.intialStartRating;
        currentSatisfactionQuantity = hotelRating.startSatisfactionQuantity;
        OnInitialRating?.Invoke();
    }

    public void RateUpdate()
    {
        averageCurrentRating = 0;

        foreach(var note in listReviews)
        {
            averageCurrentRating += note.note;
        }

        averageCurrentRating = averageCurrentRating / listReviews.Count;
        hotelRating.currentStartRating = (int)averageCurrentRating;

        totalReviews = listReviews.Count;
    }

    public void AddReview(RateReviews reviews, bool useEvent = true)
    {
        listReviews.Add(reviews);

        hotelRating.satisfactionQuantity += reviews.satisfactionQuantity;
        currentSatisfactionQuantity = hotelRating.satisfactionQuantity;

        RateUpdate();
        if( useEvent) {
            OnReviewAdd?.Invoke();
        }
    }
}
