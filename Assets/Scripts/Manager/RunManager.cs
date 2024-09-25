using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Michsky.UI.Dark;
using System;
using System.Collections;
using UnityEngine.Rendering;
public class RunManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private MusicController _musicController;
    [SerializeField] private HotelRateManager _hotelRateManager;
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private UIDissolveEffect _dissolveEffect;

    [Space(10)]
    [Header("Jeton")]
    [SerializeField] private JetonSO _jetonSO;

    [Space(10)]
    [Header("Music")]
    [SerializeField] private AudioClip _runMusic;
    [SerializeField] private AudioClip _runWinMusic;
    [SerializeField] private AudioClip _runLostMusic;

    [Space(10)]
    [Header("Events")]
    public UnityEvent onRunStart = new UnityEvent();
    public UnityEvent onRunPause = new UnityEvent();
    public UnityEvent onRunResume = new UnityEvent();
    public UnityEvent onRunLost = new UnityEvent(); 
    public UnityEvent onRunWin = new UnityEvent();
    public event Action OnAddCoin;

    [Space(10)]
    [Header("Debug Mode")]
    [SerializeField] private bool _debugMode = false;

    [Space(10)]
    [Header("End Screen Stats")]
    public float runChrono = 0f;     
    public bool isGameRunning = false;
    public int coinsGranted;

    private void Awake()
    {
        if ( SceneManager.GetActiveScene().name != "Run" )
        {
            Debug.LogError("RunManager is not in the Run Scene");
            return;
        }

        _saveManager = FindObjectOfType<SaveManager>();
        _hotelRateManager = FindObjectOfType<HotelRateManager>();
        _moneyManager = FindObjectOfType<MoneyManager>();

        if (_dissolveEffect != null)
        {
            GameObject dissolveObject = _dissolveEffect.gameObject;
            if (!dissolveObject.activeSelf)
            {
                dissolveObject.SetActive(true);
            }
        }

        CheckRef();

        GameManager.Instance.onPlay.AddListener(OnRunStart);
        GameManager.Instance.onPauseEnter.AddListener(OnRunPause);
        GameManager.Instance.onPauseExit.AddListener(OnRunResume);
    }

    public void Start()
    {
        _musicController.PlayMusic( _runMusic, true );
        StartCoroutine(DissolveOutCoroutine());
    }


    private void Update()
    {
        if ( _hotelRateManager.hotelRating.satisfactionQuantity <= 0 )
        {
            OnRunLost();
        }
        else if ( _hotelRateManager.hotelRating.satisfactionQuantity >= 100f )
        {
            OnRunWin();
        }

        if ( _moneyManager.playerMoney <= 0 )
        {
            OnRunLost();
        }

        if ( _debugMode )
        {
            DebugMode();
        }

        if (isGameRunning)
        {
            runChrono += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if ( GameManager.Instance != null )
        {
            GameManager.Instance.onPlay.RemoveListener(OnRunStart);
            GameManager.Instance.onPauseEnter.RemoveListener(OnRunPause);
            GameManager.Instance.onPauseExit.RemoveListener(OnRunResume);
        }
    }

    private void CheckRef()
    {
        if ( _saveManager == null )
        {
            Debug.LogError("SaveManager is missing");
        }

        if ( _hotelRateManager == null )
        {
            Debug.LogError("HotelRateManager is missing");
        }

        if ( _moneyManager == null )
        {
            Debug.LogError("MoneyManager is missing");
        }
    }

    private void OnRunStart()
    {
        onRunStart.Invoke();
       _saveManager.SaveGame();
        isGameRunning = true;
        coinsGranted = 0;
    }

    private void OnRunPause()
    {
        onRunPause.Invoke();
    }

    private void OnRunResume()
    {
        onRunResume.Invoke();
    }

    [ContextMenu("Run Lost")]
    public void OnRunLost()
    {
        _saveManager.SaveGame();
        _musicController.StopMusic(true);
        onRunLost.Invoke();
        isGameRunning = false;

        StartCoroutine(WaitAndPause(3f));

        CoinScoreAdd();
        Debug.Log("Run Lost");
    }

    private void CoinScoreAdd()
    {
        //int amout = (int)_hotelRateManager.totalReviews;
        //_jetonSO.AddCoin(amout);
        coinsGranted = (int)_hotelRateManager.totalReviews;
        _jetonSO.AddCoin(coinsGranted);
        OnAddCoin?.Invoke();
    }

    [ContextMenu("Run Win")]
    public void OnRunWin()
    {
        _saveManager.SaveGame();
        _musicController.StopMusic(true);
        onRunWin.Invoke();
        isGameRunning = false;

        StartCoroutine(WaitAndPause(3f));

        CoinScoreAdd();
        Debug.Log("Run Win");
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }

    private IEnumerator WaitAndPause(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.PauseGame();
    }

    private void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _moneyManager.PayTaxe(50f);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            _moneyManager.AddMoney(100f);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            _hotelRateManager.AddReview(new RateReviews(5, "Wowwww dev good", "Dev", MonsterType.DEMON, 10));
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            _hotelRateManager.AddReview(new RateReviews(0, "Wowwww dev bad", "Dev", MonsterType.WITCH, -10));
        }
    }

    public float ConvertSatisfactionValue(float value)
    {
        // S'assurer que la valeur d'entrée est bien entre -100 et 100
        value = Mathf.Clamp(value, -100f, 100f);

        // Conversion de la plage [-100, 100] à [0, 100]
        return (value + 100f) / 2f;
    }

    private IEnumerator DissolveOutCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _dissolveEffect.DissolveOut();
    }
}