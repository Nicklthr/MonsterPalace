using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RateReviews
{
    [field: SerializeField]
    public float note { get; private set; }

    [field: SerializeField]
    public string review { get; private set; }

    public RateReviews(float note, string review) 
    { 
        this.note = note;
        this.review = review;
    }
}

public class HotelRateManager : MonoBehaviour
{
    [SerializeField]
    private List<RateReviews> listReviews = new List<RateReviews>();

    public float averageCurrentRating;

    public event Action OnReviewAdd;

    public SO_HotelRating hotelRating;

    public void RateUpdate()
    {
        averageCurrentRating = 0;

        foreach(var note in listReviews)
        {
            averageCurrentRating += note.note;
        }

        averageCurrentRating = averageCurrentRating / listReviews.Count;
        Debug.Log(averageCurrentRating);

        hotelRating.currentStartRating = (int)averageCurrentRating;
        Debug.Log(hotelRating.currentStartRating);
    }

    public void AddReview(RateReviews reviews)
    {
        listReviews.Add(reviews);
        RateUpdate();
        OnReviewAdd?.Invoke();
    }
}
