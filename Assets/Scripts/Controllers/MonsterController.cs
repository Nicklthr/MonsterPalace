using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private SO_Monster monsterDatas;
    [SerializeField] private SO_Hotel hotelDatas;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audiosource;
    [SerializeField] private AudioClip happySound;
    [SerializeField] private AudioClip angrySound;

    public string monsterName;
    public string monsterID;
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
    [SerializeField] private DayNightCycle timer;


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
    private bool roomFind = false;

    private Room currentRoom;
    private TargetInRoom currentTarget;

    public bool canLeave = false;
    public bool timeToMove = false;
    public bool timeToEat = false;
    public bool timeToActivity = false;

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

        timer = GameObject.FindGameObjectWithTag("Time").GetComponent<DayNightCycle>();
        patienceMax = monsterDatas.patienceMax;
        currentStayDuration = 0;
        roomPosition = null;
        satisfaction = 50;
        canLeave = false;
        timeToMove = false;
        timeToEat = false;
        timeToActivity = false;

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

        if (agent.remainingDistance > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if(currentHour != timer.currentHour)
        {
            changeHour(timer.currentHour);
        }

        if (currentStayDuration == stayDuration && !timeToMove)
        {
            canLeave = true;
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
            neighbourControl();
        }

        if (currentHour == eatingHour ) 
        {
            timeToMove = true;
            timeToEat = true;
            
        }else if (currentHour == activityHour)
        {
            timeToMove = true;
            timeToActivity = true;
        }
    }

    public bool searchEating()
    {
        string message = "";
        foreach (Room room in hotelDatas.rooms)
        {
            if (room.type == RoomType.DINING)
            {
                if (room.currentUsers < room.maxUsers)
                {
                    foreach (TargetInRoom target in room.targets)
                    {
                        if (!target.isOccupied)
                        {
                            target.SetIsOccupied(true);
                            currentRoom = room;
                            currentTarget = target;
                            room.currentUsers++;
                            agent.SetDestination(currentTarget.target);
                            break;
                        }
                    }
                    roomFind = true;
                    break;
                }
                else
                {
                    message = "Toutes les places pour manger sont occupées !";
                }

            }
        }


        if (roomFind)
        {
            roomFind = false;
            return true;
        }
        else
        {
            if (message == "")
            {
                message = "Il n'y pas d'endroit pour manger !";
            }
            notHappy(10, message);
            return false;
        }
        
    }

    public bool searchActivity()
    {
        string message = "";
        foreach (ActivityType activity in monsterDatas.activityLike)
        {
            foreach (Room room in hotelDatas.rooms)
            {
                if (activity == room.activityType && room.type == RoomType.ACTIVITY)
                {
                    if(room.currentUsers < room.maxUsers)
                    {
                        foreach (TargetInRoom target in room.targets)
                        {
                            if (!target.isOccupied)
                            {
                                target.SetIsOccupied(true);
                                currentRoom = room;
                                currentTarget = target;
                                room.currentUsers++;
                                agent.SetDestination(currentTarget.target);
                                break;
                            }
                        }
                        roomFind = true;
                        break;
                    }
                    else
                    {
                        message = $"Il n'y avait plus de place dans la {activity}";
                    }
 
                }
            }

            if (roomFind)
            {
                break;
            }

        }

        if (roomFind)
        {
            roomFind = false;
            return true;
        }
        else
        {
            if (message == "")
            {
                message = "Il n'y a pas d'activité pour moi dans cet hôtel";
            }
            notHappy(10, message);
            return false;
        }
    }

    public void freeRoom()
    {
        currentRoom.currentUsers--;
        currentTarget.SetIsOccupied(false);
        currentRoom = null;
        currentTarget = null;
    }


    public void Happy(int value, string message)
    {
        satisfaction += value;
        if (satisfaction > 100)
        {
            satisfaction = 100;
        }
        animator.SetTrigger("isHappy");
        audiosource.clip = happySound;
        audiosource.Play();
        Debug.Log(message);
    }

    public void notHappy(int value, string message)
    {
        satisfaction -= value;
        if(satisfaction < 0)
        {
            satisfaction = 0;
        }
        animator.SetTrigger("isAngry");
        audiosource.clip = angrySound;
        audiosource.Play();
        Debug.Log(message);
    }

    public void foodControl()
    {
                
        int satisfood = 0;
        int numberlike = 0;
        int numberdislike = 0;
        string message = "";
        //On récupère la room assignée au monstre
        foreach (Room room in hotelDatas.rooms)
        {
            if(room.type == RoomType.BEDROOM && room.monsterID == monsterID)
            {
                //On regarde si un repas est assigné
                if (room.foodAssigned != null)
                {
                    //On parcours la nourriture que le monstre n'aime pas
                    foreach (FoodType dislike in monsterDatas.foodDislike)
                    {

                        foreach (FoodType type in room.foodAssigned.typeList)
                        {
                            if(dislike == type)
                            {
                                satisfood-= 10;
                                numberdislike++;
                                message = $"Je n'aime pas la nourriture {type} !";
                            }
                        }

                        if (numberdislike == monsterDatas.foodDislike.Length && numberdislike > 1)
                        {
                            message = $"J'ai détesté le repas !";
                        }

                    }

                    if(satisfood == 0)
                    {
                        //On parcours la nourriture que le monstre aime
                        foreach (FoodType like in monsterDatas.foodLike)
                        {

                            foreach (FoodType type in room.foodAssigned.typeList)
                            {
                                if (like == type)
                                {
                                    satisfood += 10;
                                    numberlike++;
                                    message = $"J'aime la nourriture {type} !";
                                }
                            }

                            if (numberlike == monsterDatas.foodLike.Length)
                            {
                                message = $"J'ai adoré le repas !";
                            }

                        }
                    }

                    if(satisfood > 0)
                    {
                        Happy(satisfood, message);
                    }
                    else if(satisfood < 0)
                    {
                        notHappy(-satisfood, message);
                    }

                }
                else
                {
                    message = "Comment ça pas de repas prévu !";
                    notHappy(30, message);
                }
                
                break;
            }
        }
    }

    public void roomControl()
    {
        arrivalHour = currentHour;
    }

    public void placementControl()
    {

        int satisplac = 0;
        int numberlike = 0;
        int numberdislike = 0;
        string message = "";
        //On récupère la room assignée au monstre
        foreach (Room room in hotelDatas.rooms)
        {
            if (room.type == RoomType.BEDROOM && room.monsterID == monsterID)
            {
  
                    //On parcours les emplacements que le monstre n'aime pas
                    foreach (RoomPlacement dislike in monsterDatas.roomPlacementsDislike)
                    {

                        foreach (RoomPlacement type in room.roomPlacement)
                        {
                            if (dislike == type)
                            {
                                satisplac -= 10;
                                numberdislike++;
                                message = $"Je n'aime pas les emplacements {type} !";
                            }
                        }

                        if (numberdislike == monsterDatas.foodDislike.Length && numberdislike > 1)
                        {
                            message = $"Je déteste l'emplacement de la chambre !";
                        }

                    }

                    if (satisplac == 0)
                    {
                    //On parcours les emplacements que le monstre aime
                    foreach (RoomPlacement like in monsterDatas.roomPlacementsLike)
                        {

                            foreach (RoomPlacement type in room.roomPlacement)
                            {
                                if (like == type)
                                {
                                    satisplac += 10;
                                    numberlike++;
                                    message = $"J'aime les emplacements {type} !";
                                }
                            }

                            if (numberlike == monsterDatas.foodLike.Length)
                            {
                                message = $"J'adore l'emplacement de la chambre !";
                            }

                        }
                    }

                    if (satisplac > 0)
                    {
                        Happy(satisplac, message);
                    }
                    else if (satisplac < 0)
                    {
                        notHappy(-satisplac, message);
                    }

                break;
            }
        }
    }

    public void neighbourControl()
    {

    }

    public void Pay()
    {

    }

    public void GiveEvaluation()
    {

    }

}


