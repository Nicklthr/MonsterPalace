using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private SO_Monster monsterDatas;
    [SerializeField] private SO_Hotel hotelDatas;
    [SerializeField] private NavMeshAgent agent;
    public string monsterName;
    private float patienceMax;
    [Range(0, 100)]
    public int satisfaction = 50;


    [Header("Planning")]
    public int eatingHour;
    public int activityHour;
    public int arrivalHour;

    [Header("Time of the Day")]
    [Range(0, 23)]
    [SerializeField] private int currentHour;
 

    [Header("Stay Duration")]
    [SerializeField] private int stayDurationMin;
    [SerializeField] private int stayDurationMax;
    public int stayDuration;
    [SerializeField] private int currentStayDuration = 0;

    [Header("Positions")]
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform receptionPosition;
    [SerializeField] public Transform roomPosition;
    private Vector3 startPositionVector;
    private Vector3 receptionPositionVector;
    private Vector3 roomPositionVector;

    public bool controlDone = false;

    //Beahviour tree variables
    #region Beahviour tree variables
    public float MyPatienceMax { get { return patienceMax; } set { patienceMax = value; } }
    public int MycurrentHour { get { return currentHour; } set { currentHour = value; } }
    public int MystayDuration { get { return stayDuration; } set { stayDuration = value; } }
    public int MycurrentStayDuration { get { return currentStayDuration; } set { currentStayDuration = value; } }
    public Vector3 MystartPositionVector { get { return startPositionVector; } set { startPositionVector = value; } }
    public Vector3 MyreceptionPosition { get { return receptionPositionVector; } set { receptionPositionVector = value; } }
    public Vector3 MyroomPosition { get { return roomPositionVector; } set { roomPositionVector = value; } }
    #endregion


    private void OnEnable()
    {
        //On récupère les informations du monstre par rapport à celles de son espèce

        patienceMax = monsterDatas.patienceMax;
        currentStayDuration = 0;
        roomPosition = null;
        satisfaction = 50;

        //Name
        monsterName = monsterDatas.monsterNameList.Name[Random.Range(0, monsterDatas.monsterNameList.Name.Length)];

        //Eating Hour
        if (monsterDatas.eatingHourMax < monsterDatas.eatingHourMin)
        {
            eatingHour = Random.Range(monsterDatas.eatingHourMin, (24 + monsterDatas.eatingHourMax));

            if (eatingHour > 23) 
            {
                eatingHour = eatingHour - 24;
            }
        }
        else
        {
            eatingHour = Random.Range(monsterDatas.eatingHourMin, monsterDatas.eatingHourMax);
        }

        //Activity Hour
        if (monsterDatas.activityHourMax < monsterDatas.activityHourMin)
        {
            activityHour = Random.Range(monsterDatas.activityHourMin, (24 + monsterDatas.activityHourMax));

            if (activityHour > 23)
            {
                activityHour = activityHour - 24;
            }
        }
        else
        {
            activityHour = Random.Range(monsterDatas.activityHourMin, monsterDatas.activityHourMax);
        }

        //Preventing Activity and Eating Hour to Overlap
        if(activityHour == eatingHour)
        {
            if (eatingHour == monsterDatas.eatingHourMax)
            {
                eatingHour--;
                if (eatingHour < 0)
                {
                    eatingHour = 23;
                }
            }
            else
            {
                eatingHour++; 
                if (eatingHour > 23)
                {
                    eatingHour = 0;
                }
            }
        }

        //StayDuration
        stayDuration = Random.Range(stayDurationMin, stayDurationMax);

        //ReceptionPosition
        //agent.destination = receptionPosition.position;
        receptionPositionVector = receptionPosition.position;
        startPositionVector = startPosition.position;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (roomPosition != null)
        {
            roomPositionVector = roomPosition.position;
        }
        
    }


    private void OnDisable()
    {
        transform.position = startPosition.position;
        GiveEvaluation();
    }

    public void changeHour(int hour)
    {
        currentHour = hour;
        controlDone = false;

        if ( currentHour == arrivalHour)
        {
            currentStayDuration++;
        }
    }

    public bool searchEating()
    {
        foreach (ActivityType activity in monsterDatas.activityLike)
        {
            foreach(Room room in hotelDatas.rooms)
            {
                if(activity == room.activityType)
                {

                }
            }
        }
        return true;
    }

    public bool searchActivity()
    {
        return true;
    }


    public void Happy(int value)
    {
        satisfaction += value;
        if (satisfaction > 100)
        {
            satisfaction = 100;
        }
    }

    public void notHappy(int value)
    {
        satisfaction -= value;
        if(satisfaction < 0)
        {
            satisfaction = 0;
        }
    }

    public void foodControl()
    {

    }

    public void roomControl()
    {

    }

    public void Pay()
    {

    }

    public void GiveEvaluation()
    {

    }

}


