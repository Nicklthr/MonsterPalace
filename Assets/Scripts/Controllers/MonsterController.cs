using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UI;
using BehaviorDesigner.Runtime.Tasks.MonsterTask;

public class MonsterController : MonoBehaviour, ISelectable
{
    [Header("Datas")]
    public SO_Monster monsterDatas;
    [SerializeField] private SO_Hotel hotelDatas;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audiosource;
    [SerializeField] private AudioClip happySound;
    [SerializeField] private AudioClip angrySound;
    [SerializeField] private Image reactionImage;
    [SerializeField] public SO_ReactionImage reactionImageDatas;

    private float defaultSpeed;

    public string monsterName;
    public string monsterID;
    private float patienceMax;
    [Range(-100, 100)]
    public int satisfaction = 0;

    public float starsRate = 0f;
    public string review = "";

    public MoneyManager moneyManager;

    /*[Header("Planning")]
    public int eatingHour;
    public int activityHour;
    public int arrivalHour;

    [Header("Time of the Day")]
    [Range(0, 23)]
    [SerializeField] private int currentHour;
    [SerializeField] private DayNightCycle timer;*/

    [Header("Needs values")]
    public float hungerValue = 100f;
    public float hungerMultiplyValue = 1f;
    public bool hungerIsPaused = true;
    public bool hungerCheck = false;
    public float hungerFirstCheckValue = 50;
    public float hungerCurrentCheckValue = 50;
    public float boredomValue = 100f;
    public float boredomMultiplyValue = 1f;
    public bool boredomIsPaused = true;
    public bool boredomCheck = false;
    public float boredomFirstCheckValue = 50;
    public float boredomCurrentCheckValue = 50;


    [Header("Stay Duration")]
    [SerializeField] private int stayDurationMin;
    [SerializeField] private int stayDurationMax;
    public int stayDuration;
    public int currentStayDuration = 0;

    [Header("Positions")]
    [SerializeField] private Transform startPosition;
    public Vector3 roomPosition;
    private Vector3 startPositionVector;

    private Vector3 lookDirection;

    public bool controlDone = false;
    private bool roomFind = false;

    private Room currentRoom;
    private TargetInRoom currentTarget;

    public bool canLeave = false;
    public bool timeToMove = false;
    public bool timeToEat = false;
    public bool timeToActivity = false;
    public bool canAssignRoom = false;
    public bool waitingQ = false;
    public bool endLine = false;
    public bool roomAssigned = false;

    //Monster commentary
    public List<string> commentaries = new List<string>();
    public static event Action<MonsterController> OnNewCommentaire;

    private Outline _outline;
    public void OnHoverEnter() => SetOutline(true);
    public void OnHoverExit() => SetOutline(false);
    public void OnSelect() => SetOutline(true);
    public void OnDeselect() => SetOutline(false);

    //Beahviour tree variables
    #region Beahviour tree variables
    public float MyPatienceMax { get { return patienceMax; } set { patienceMax = value; } }
   // public int MycurrentHour { get { return currentHour; } set { currentHour = value; } }
    public int MystayDuration { get { return stayDuration; } set { stayDuration = value; } }
    public int MycurrentStayDuration { get { return currentStayDuration; } set { currentStayDuration = value; } }
    public Vector3 MystartPositionVector { get { return startPositionVector; } set { startPositionVector = value; } }
    //public Vector3 MyreceptionPosition { get { return receptionPositionVector; } set { receptionPositionVector = value; } }
    public Vector3 MyroomPosition { get { return roomPosition; } set { roomPosition = value; } }
    #endregion


    private void OnEnable()
    {
        //On récupère les informations du monstre par rapport à celles de son espèce
        patienceMax = monsterDatas.patienceMax;
        currentStayDuration = 0;
        //roomPosition = null;
        satisfaction = 0;
        canLeave = false;
        timeToMove = false;
        timeToEat = false;
        timeToActivity = false;
        commentaries.Clear();
        canAssignRoom = false;
        waitingQ = true;
        defaultSpeed = agent.speed;
        endLine = false;
        DeactivateReaction();

        hungerValue = 100f;
        hungerMultiplyValue = 1f;
        hungerIsPaused = true;
        hungerCheck = false;
        hungerCurrentCheckValue = hungerFirstCheckValue;
        boredomValue = 100f;
        boredomMultiplyValue = 1f;
        boredomIsPaused = true;
        boredomCheck = false;
        boredomCurrentCheckValue = boredomFirstCheckValue;



        //Name
        monsterName = monsterDatas.monsterNameList.Name[UnityEngine.Random.Range(0, monsterDatas.monsterNameList.Name.Length)];

        //Eating Hour
        /*if (monsterDatas.eatingHourMax < monsterDatas.eatingHourMin)
        {
            eatingHour = UnityEngine.Random.Range(monsterDatas.eatingHourMin, (24 + monsterDatas.eatingHourMax));

            if (eatingHour > 23) 
            {
                eatingHour = eatingHour - 24;
            }
        }
        else
        {
            eatingHour = UnityEngine.Random.Range(monsterDatas.eatingHourMin, monsterDatas.eatingHourMax);
        }

        //Activity Hour
        if (monsterDatas.activityHourMax < monsterDatas.activityHourMin)
        {
            activityHour = UnityEngine.Random.Range(monsterDatas.activityHourMin, (24 + monsterDatas.activityHourMax));

            if (activityHour > 23)
            {
                activityHour = activityHour - 24;
            }
        }
        else
        {
            activityHour = UnityEngine.Random.Range(monsterDatas.activityHourMin, monsterDatas.activityHourMax);
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
        }*/

        //StayDuration
        stayDuration = UnityEngine.Random.Range(stayDurationMin, stayDurationMax);

        //ReceptionPosition
        //agent.destination = receptionPosition.position;
        //receptionPositionVector = receptionPosition.position;
        startPositionVector = startPosition.position;

        WaitingQ.instance.Sub(gameObject);

    }

    private void Awake()
    {
        //timer = GameObject.FindGameObjectWithTag("Time").GetComponent<DayNightCycle>();
        startPosition = GameObject.FindGameObjectWithTag("StartPosition").transform;
        //receptionPosition = GameObject.FindGameObjectWithTag("ReceptionPosition").transform;
        agent.updateRotation = false;
        lookDirection = new Vector3(0, 0, -90);
        moneyManager = FindObjectOfType<MoneyManager>();
        _outline = GetComponent<Outline>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (roomPosition != null)
        {
            roomPositionVector = roomPosition.position;
        }*/
        satisfaction = (currentStayDuration * 100) / stayDuration;

        if (!hungerIsPaused && !canLeave)
        {
            hungerValue -= Time.deltaTime / hungerMultiplyValue;
            if (hungerValue < 0)
            {
                hungerValue = 0;
            }

            if(hungerValue < hungerCurrentCheckValue && hungerValue > 0)
            {
                goToEat();
            }

            if (hungerValue == 0) 
            {
                notHappy(0, "noroom", true);
                canLeave = true;
            }
        }

        if (!boredomIsPaused && !canLeave)
        {
            boredomValue -= Time.deltaTime / boredomMultiplyValue;
            if (boredomValue < 0)
            {
                boredomValue = 0;
            }

            if(boredomValue < boredomCurrentCheckValue && boredomValue > 0)
            {
                goToActivity();
            }

            if (boredomValue == 0)
            {
                notHappy(0, "noroom", true);
                canLeave = true;
            }

        }


        if (agent.remainingDistance > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

       /* if(currentHour != timer.currentHour)
        {
            changeHour(timer.currentHour);
        }*/

        if (currentStayDuration == stayDuration && !timeToMove)
        {
            canLeave = true;
        }


    }

    private void LateUpdate()
    {
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
        else
        {
            transform.LookAt(lookDirection);
        }

    }


    private void OnDisable()
    {
        transform.position = startPosition.position;
        GiveEvaluation();
    }

    private void SetOutline(bool enabled)
    {
        if (_outline != null)
        {
            _outline.enabled = enabled;
        }
    }

    /*public void changeHour(int hour)
    {
        currentHour = hour;
        controlDone = false;

        if ( currentHour == arrivalHour && roomAssigned)
        {
            currentStayDuration++;
            neighbourControl();
        }

        if (currentHour == eatingHour && roomAssigned) 
        {
            timeToMove = true;
            timeToEat = true;
            
        }else if (currentHour == activityHour && roomAssigned)
        {
            timeToMove = true;
            timeToActivity = true;
        }
    }*/

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
                    message = LanguageHandler.Instance.GetTranslation("nocanteenplace");
                }

            }
        }


        if (roomFind)
        {
            setReaction(reactionImageDatas.returnEmote("eat"));
            roomFind = false;
            return true;
        }
        else
        {
            setReaction(reactionImageDatas.returnEmote("noDiner"));
            if (message == "")
            {
                message = LanguageHandler.Instance.GetTranslation("nocanteen");
            }
            notHappy(4, message);
            return false;
        }
        
    }

    public bool searchActivity()
    {
        string message = "";
        string typeString = "";
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
                        typeString = LanguageHandler.Instance.GetTranslation(activity.ToString());
                        message = string.Format(LanguageHandler.Instance.GetTranslation("noactivityplace"), typeString);
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
            setReaction(reactionImageDatas.returnEmote("activity"));
            roomFind = false;
            return true;
        }
        else
        {
            setReaction(reactionImageDatas.returnEmote("noActivity"));
            if (message == "")
            {
                message = LanguageHandler.Instance.GetTranslation("noactivity");
            }
            notHappy(5, message);
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


    public void Happy(int value, string message, bool id = false)
    {
        if (value == 1)
        {
            //Multiplicateur
            hungerMultiplyValue += 1;
            boredomMultiplyValue += 1;
        }
        else if (value == 2)
        {
            //Nourriture Correct
            hungerValue += 30;
            hungerCurrentCheckValue += 30;
            if (hungerCurrentCheckValue > hungerFirstCheckValue)
            {
                hungerCurrentCheckValue = hungerFirstCheckValue;
            }
        }
        else if (value == 3)
        {
            //Nourriture excellente
            hungerValue += 50;
            hungerCurrentCheckValue += 50;
            if (hungerCurrentCheckValue > hungerFirstCheckValue)
            {
                hungerCurrentCheckValue = hungerFirstCheckValue;
            }
        }
        else if (value == 4)
        {
            //Activité ok
            boredomValue += 30;
            boredomCurrentCheckValue += 30;
            if(boredomCurrentCheckValue > boredomFirstCheckValue)
            {
                boredomCurrentCheckValue = boredomFirstCheckValue;
            }
        }
 

        ActivateReaction();
        animator.SetTrigger("isHappy");
        audiosource.clip = happySound;
        audiosource.Play();

        if (id)
        {
            message = LanguageHandler.Instance.GetTranslation(message);
        }

        commentaries.Add(message);
        OnNewCommentaire?.Invoke(this);
        //Debug.Log(message);
    }

    public void notHappy(int value, string message, bool id = false)
    {

        if (value == 1)
        {
            //Multiplicateur
            hungerMultiplyValue = hungerMultiplyValue/2;
            boredomMultiplyValue = boredomMultiplyValue/2;
        }
        else if (value == 2)
        {
            //Nourriture inCorrect
            hungerValue -= 10;
            hungerCurrentCheckValue -= 10;

        }
        else if (value == 3)
        {
            //Nourriture horrible ou Pas de repas
            hungerValue -= 20;
            hungerCurrentCheckValue -= 20;

            if (hungerValue < 0)
            {
                hungerValue = 0;
            }

        }
        else if (value == 4)
        {
            //Pas de cantine
            //hungerValue -= 20;
            //hungerCurrentCheckValue -= 10;

            if (hungerValue < 0)
            {
                hungerValue = 0;
            }
            hungerIsPaused = false;
            boredomIsPaused = false;
        }
        else if (value == 5)
        {
            //Pas d'activité
            //Pas de cantine
            //boredomValue -= 20;
            //boredomCurrentCheckValue -= 10;

            if (boredomValue < 0)
            {
                boredomValue = 0;
            }
            hungerIsPaused = false;
            boredomIsPaused = false;
        }


        ActivateReaction();
        animator.SetTrigger("isAngry");
        audiosource.clip = angrySound;
        audiosource.Play();

        if (id)
        {
            message = LanguageHandler.Instance.GetTranslation(message);
        }

        commentaries.Add(message);
        //Debug.Log(message);
    }

    public void foodControl()
    {
                
        int satisfood = 0;
        int numberlike = 0;
        int numberdislike = 0;
        string message = "";
        string typeString = "";
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
                                typeString = LanguageHandler.Instance.GetTranslation(type.ToString());
                                message = string.Format(LanguageHandler.Instance.GetTranslation("foodnotlike"), typeString);
                                //message = $"Je n'aime pas la nourriture {type} !";
                            }
                        }

                        if (numberdislike == room.foodAssigned.typeList.Length && numberdislike > 1)
                        {
                            message = LanguageHandler.Instance.GetTranslation("foodhate");
                            satisfood = -3;
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
                                    typeString = LanguageHandler.Instance.GetTranslation(type.ToString());
                                    message = string.Format(LanguageHandler.Instance.GetTranslation("foodlike"), typeString);
                                }
                            }

                            if (numberlike == monsterDatas.foodLike.Length && numberlike > 1)
                            {
                                message = LanguageHandler.Instance.GetTranslation("foodlove");
                                satisfood = 3;
                            }

                        }
                    }

                    if(satisfood > 0)
                    {
                        setReaction(reactionImageDatas.returnEmote("like"));
                        if (satisfood == 3)
                        {
                            Happy(satisfood, message);
                        }
                        else
                        {
                            Happy(2, message);
                        }
                       
                    }
                    else if(satisfood < 0)
                    {
                        setReaction(reactionImageDatas.returnEmote("dislike"));
                        if (satisfood == -3)
                        {
                            notHappy(-satisfood, message);
                        }
                        else
                        {
                            notHappy(2, message);
                        }
                        
                    }

                }
                else
                {
                    setReaction(reactionImageDatas.returnEmote("noMenu"));
                    message = LanguageHandler.Instance.GetTranslation("nofood");
                    notHappy(3, message);
                }
                
                break;
            }
        }
    }

    public void roomControl()
    {
        //arrivalHour = currentHour;

        string message = "";
        foreach (Room room in hotelDatas.rooms)
        {
            if (room.type == RoomType.BEDROOM && room.monsterID == monsterID)
            {

                if (room.monsterTypeOfRoom == monsterDatas.monsterType)
                {
                    message = LanguageHandler.Instance.GetTranslation("roomTypeLike");
                    setReaction(reactionImageDatas.returnEmote("happy"));
                    Happy(1, message);
                }
                else if(room.monsterTypeOfRoom != MonsterType.NONE)
                {
                    setReaction(reactionImageDatas.returnEmote("angry"));
                    message = LanguageHandler.Instance.GetTranslation("roomTypeDislike");
                    notHappy(1, message);
                }

                break;
            }

        }

    }

    public void placementControl()
    {

        int satisplac = 0;
        int numberlike = 0;
        int numberdislike = 0;
        string message = "";
        string typeString = "";
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
                            typeString = LanguageHandler.Instance.GetTranslation(type.ToString());
                            message = string.Format(LanguageHandler.Instance.GetTranslation("roomnotlike"), typeString);
                            }
                        }

                        if (numberdislike == monsterDatas.roomPlacementsDislike.Length && numberdislike > 1)
                        {
                            message = LanguageHandler.Instance.GetTranslation("roomhate");
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
                                    typeString = LanguageHandler.Instance.GetTranslation(type.ToString());
                                    message = string.Format(LanguageHandler.Instance.GetTranslation("roomlike"), typeString);
                                }
                            }

                            if (numberlike == monsterDatas.foodLike.Length && numberlike > 1)
                            {
                                message = LanguageHandler.Instance.GetTranslation("roomlove");
                            }

                        }
                    }

                    if (satisplac > 0)
                    {
                        setReaction(reactionImageDatas.returnEmote("happy"));
                        Happy(1, message);
                    }
                    else if (satisplac < 0)
                    {
                        setReaction(reactionImageDatas.returnEmote("angry"));
                        notHappy(1, message);
                    }

                break;
            }
        }
    }

    public void neighbourControl()
    {

        int satisneighboor = 0;
        int numberlike = 0;
        int numberdislike = 0;
        string message = "";
        string typeString = "";

        //On récupère les infos de la chambre
        foreach (Room room in hotelDatas.rooms)
        {
            if (room.type == RoomType.BEDROOM && room.monsterID == monsterID)
            {

                //On récupère les pièces voisines
                foreach (Room neighbourRoom in hotelDatas.rooms)
                {
                    if (neighbourRoom.type == RoomType.BEDROOM && neighbourRoom.monsterID != null && neighbourRoom.positionInGrid.y == room.positionInGrid.y && (neighbourRoom.positionInGrid.x == room.positionInGrid.x + room.roomSize.x || neighbourRoom.positionInGrid.x == room.positionInGrid.x - neighbourRoom.roomSize.x) )
                    {


                        //On parcours les voisins que le monstre n'aime pas
                        foreach (MonsterType dislike in monsterDatas.neighboorDislike)
                        {
  
                            if (dislike == neighbourRoom.monsterDataCurrentCustomer.monsterType)
                                {
                                    if(satisneighboor > 0)
                                    {
                                        satisneighboor = 0;
                                    }
                                    satisneighboor -= 10;
                                    numberdislike++;
                                    typeString = LanguageHandler.Instance.GetTranslation(dislike.ToString());
                                    message = string.Format(LanguageHandler.Instance.GetTranslation("neighbourDislike"), typeString);
                                //message = $"Je n'aime pas la nourriture {type} !";

                                if (numberdislike == monsterDatas.neighboorDislike.Length && numberdislike > 1)
                                {
                                    message = LanguageHandler.Instance.GetTranslation("neighbourHate");
                                }

                                break;

                            }


                        }

                        if (satisneighboor >= 0)
                        {
                            //On parcours les voisins que le monstre aime
                            foreach (MonsterType like in monsterDatas.neighboorLike)
                            {



                                if (like == neighbourRoom.monsterDataCurrentCustomer.monsterType)
                                {
                                        satisneighboor += 10;
                                        numberlike++;
                                        typeString = LanguageHandler.Instance.GetTranslation(like.ToString());
                                        message = string.Format(LanguageHandler.Instance.GetTranslation("neighbourLike"), typeString);
                                }
                                

                                if (numberlike == monsterDatas.neighboorLike.Length && numberlike > 1)
                                {
                                    message = LanguageHandler.Instance.GetTranslation("neighbourLove");
                                }

                            }
                        }




                        //Debug.Log("You Have neighbours");

                    }

                }

                break;
            }

        }



        if (satisneighboor > 0)
        {
            setReaction(reactionImageDatas.returnEmote("happy"));
            Happy(1, message);
        }
        else if (satisneighboor < 0)
        {
            setReaction(reactionImageDatas.returnEmote("angry"));
            notHappy(1, message);
        }
    }

    public void Pay()
    {
        moneyManager.Payment(stayDuration);
        if (EndSceenStats.Instance) {
            EndSceenStats.Instance.CustomersCountAdd(1);
        }
    }

    public void GiveEvaluation()
    {
        starsRate = ConvertValue(satisfaction);
        HotelRateManager hotelratemanager = FindObjectOfType<HotelRateManager>();
        hotelratemanager.AddReview(new RateReviews(starsRate, "Test", monsterName, monsterDatas.monsterType, (satisfaction / 10f)));
    }
    public float ConvertValue(float inputValue)
    {
        if (inputValue < -100f || inputValue > 100f)
        {
            return -1f;
        }

        float outputValue = (inputValue + 100f) / 40f;

        return outputValue;
    }

    public void CheckOut()
    {
        Room assignedRoom = hotelDatas.rooms.Find(room => room.type == RoomType.BEDROOM && room.monsterID == monsterID);
        if (assignedRoom != null)
        {
            if (assignedRoom.CheckOutMonster(monsterID))
            {
                // Réinitialiser les données spécifiques au monstre après avoir quitté la chambre
                roomAssigned = false;
                roomPosition = new Vector3(0, 0, 0);
            }
        }
        else
        {
            Debug.LogError("Monster: No assigned room found for this monster");
        }
    }

    public void QuitWaitingQueue()
    {
        WaitingQ.instance.Quit(gameObject);
    }

    public void MoveInQueue(Transform nextPosition, bool reception, bool boolEndLine)
    {

        agent.SetDestination(nextPosition.position);
        

        if ( reception )
        {
            waitingQ = false;
           
        }
        else if(boolEndLine)
        {
            endLine = true;
        }
    }

    public void ActivateReaction()
    {
        reactionImage.enabled = true;
    }

    public void DeactivateReaction()
    {
        reactionImage.enabled = false;
    }

    public void setReaction(Sprite sprite)
    {
        reactionImage.sprite = sprite;
    }

    public void goToEat()
    {
        //hungerIsPaused = true;
        //boredomIsPaused = true;
        timeToEat = true;
        timeToMove = true;
        hungerCurrentCheckValue -= 10;
    }

    public void goToActivity()
    {
        //hungerIsPaused = true;
        //boredomIsPaused = true;
        timeToActivity = true;
        timeToMove = true;
        boredomCurrentCheckValue -= 10;
    }

}


