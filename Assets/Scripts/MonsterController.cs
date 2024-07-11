using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private SO_Monster monsterDatas;

    public string monsterName;

    public int eatingHour;
    public int activityHour;

    public int arrivalHour;

    public int stayDuration;
    [SerializeField] private int stayDurationMin;
    [SerializeField] private int stayDurationMax;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform receptionPosition;
    [SerializeField] public Transform roomPosition;

    private Vector3 startPositionVector;
    public Vector3 MystartPositionVector { get { return startPositionVector; } set { startPositionVector = value; } }


    private Vector3 receptionPositionVector;
    public Vector3 MyreceptionPosition { get { return receptionPositionVector; } set { receptionPositionVector = value; } }

    private Vector3 roomPositionVector;
    public Vector3 MyroomPosition { get { return roomPositionVector; } set { roomPositionVector = value; } }

    [SerializeField] private NavMeshAgent agent;

    public int currentTime;

    private float patienceMax;
    public float MyPatienceMax { get { return patienceMax; } set { patienceMax = value; } }

    private void OnEnable()
    {
        //On récupère les informations du monstre par rapport à celles de son espèce

        patienceMax = monsterDatas.patienceMax;

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
    }

}
