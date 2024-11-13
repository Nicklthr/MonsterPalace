using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.UI;
using BehaviorDesigner.Runtime.Tasks.MonsterTask;

public class MonsterControllerV2 : MonoBehaviour, ISelectable
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



    //Monster commentary
    public List<string> commentaries = new List<string>();
    public static event Action<MonsterControllerV2> OnNewCommentaire;

    private Outline _outline;
    public void OnHoverEnter() => SetOutline(true);
    public void OnHoverExit() => SetOutline(false);
    public void OnSelect() => SetOutline(true);
    public void OnDeselect() => SetOutline(false);


    private void OnEnable()
    {
        hungerValue = 100f;
        hungerMultiplyValue = 1f;
        hungerIsPaused = true;
        hungerCheck = false;
        boredomValue = 100f;
        boredomMultiplyValue = 1f;
        boredomIsPaused = true;
        boredomCheck = false;

    }

    private void Awake()
    {
        startPosition = GameObject.FindGameObjectWithTag("StartPosition").transform;
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

        if (!hungerIsPaused && hungerValue > 0)
        {
            hungerValue -= Time.deltaTime / hungerMultiplyValue;
            if (hungerValue < 0)
            {
                hungerValue = 0;
            }
        }

        if(!boredomIsPaused && boredomValue > 0)
        {
            boredomValue -= Time.deltaTime / boredomMultiplyValue;
            if (boredomValue < 0)
            {
                boredomValue = 0;
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

    public bool searchEating()
    {
        return true;
    }

    public bool searchActivity()
    {
        return true;
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

    }

    public void notHappy(int value, string message, bool id = false)
    {

    }

    public void foodControl()
    {

    }

    public void roomControl()
    {

    }

    public void placementControl()
    {

    }

    public void neighbourControl()
    {

    }

    public void Pay()
    {
        moneyManager.Payment(stayDuration);
        if (EndSceenStats.Instance)
        {
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

    #region Waiting queue
    public void QuitWaitingQueue()
    {
        WaitingQ.instance.Quit(gameObject);
    }

    public void MoveInQueue(Transform nextPosition, bool reception, bool boolEndLine)
    {

        agent.SetDestination(nextPosition.position);


        if (reception)
        {
            waitingQ = false;

        }
        else if (boolEndLine)
        {
            endLine = true;
        }
    }
    #endregion

    #region emote reaction
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
    #endregion

    #region Needs
    public void hungerDecrease()
    {


    }

    public void boredomDecrease()
    {

    }

    #endregion
}



