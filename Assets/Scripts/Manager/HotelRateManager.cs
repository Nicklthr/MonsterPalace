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

    [field: SerializeField]
    public string monsterName { get; private set; }

    [field: SerializeField]
    public MonsterType type { get; private set; }

    public RateReviews(float note, string review, string monsterName, MonsterType type) 
    { 
        this.note = note;
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
        //Debug.Log(averageCurrentRating);

        hotelRating.currentStartRating = (int)averageCurrentRating;
        //Debug.Log(hotelRating.currentStartRating);

        totalReviews = listReviews.Count;
    }

    public void AddReview(RateReviews reviews)
    {
        listReviews.Add(reviews);
        RateUpdate();
        OnReviewAdd?.Invoke();
    }
}
