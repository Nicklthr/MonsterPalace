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
    public event Action OnInitialRating;
    public SO_HotelRating hotelRating;

    private void Start()
    {
        hotelRating.InitializeRateing();

        AddReview(new RateReviews(2, "Great hotel", "Dracula", MonsterType.VAMPIRE), false);
        AddReview(new RateReviews(2, "Not bad", "Frankenstein", MonsterType.GHOUL), false);
        AddReview(new RateReviews(2, "I love it", "Wolfy", MonsterType.WEREWOLF), false);
        AddReview(new RateReviews(2, "Good place", "Mother", MonsterType.WITCH), false);
        AddReview(new RateReviews(2, "I will come back", "Bhou", MonsterType.YOKAI), false);

        averageCurrentRating = hotelRating.intialStartRating;
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
        RateUpdate();
        if( useEvent) {
            OnReviewAdd?.Invoke();
        }
    }
}
