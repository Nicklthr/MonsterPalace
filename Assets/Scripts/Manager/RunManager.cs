using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Michsky.UI.Dark;
using System;
using System.Collections;
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
        
        CheckRef();

        GameManager.Instance.onPlay.AddListener(OnRunStart);
        GameManager.Instance.onPauseEnter.AddListener(OnRunPause);
        GameManager.Instance.onPauseExit.AddListener(OnRunResume);
    }

    public void Start()
    {
        _musicController.PlayMusic( _runMusic, true );
        _dissolveEffect.DissolveOut();
    }


    private void Update()
    {
        if (_hotelRateManager.hotelRating.currentStartRating < 1)
        {
            OnRunLost();
        }
        else if (_hotelRateManager.hotelRating.currentStartRating >= 5)
        {
            OnRunWin();
        }

        if (_moneyManager.playerMoney <= 0)
        {
            OnRunLost();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _moneyManager.PayTaxe(50);
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

        StartCoroutine(WaitAndPause(3f));

        CoinScoreAdd();
        Debug.Log("Run Lost");
    }

    private void CoinScoreAdd()
    {
        int amout = (int)_hotelRateManager.totalReviews;
        _jetonSO.AddCoin(amout);
        OnAddCoin?.Invoke();
    }

    [ContextMenu("Run Win")]
    public void OnRunWin()
    {
        _saveManager.SaveGame();
        _musicController.StopMusic(true);
        onRunWin.Invoke();

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
}