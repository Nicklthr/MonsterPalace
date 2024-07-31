using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Michsky.UI.Dark;

public class RunManager : MonoBehaviour
{
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private MusicController _musicController;
    [SerializeField] private HotelRateManager _hotelRateManager;
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private UIDissolveEffect _dissolveEffect;

    [Space(10)]
    [SerializeField] private JetonSO _jetonSO;

    [Space(10)]
    [SerializeField] private AudioClip _runMusic;

    public UnityEvent onRunStart = new UnityEvent();
    public UnityEvent onRunPause = new UnityEvent();
    public UnityEvent onRunResume = new UnityEvent();
    public UnityEvent onRunLost = new UnityEvent(); 
    public UnityEvent onRunWin = new UnityEvent();

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
        GameManager.Instance.onRunLost.AddListener(OnRunLost);
        GameManager.Instance.onRunWin.AddListener(OnRunWin);
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
            RunOver(false);
        }
        else if (_hotelRateManager.hotelRating.currentStartRating >= 5)
        {
            RunOver(true);
        }

        if (_moneyManager.playerMoney <= 0)
        {
            RunOver(false);
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
            GameManager.Instance.onRunLost.RemoveListener(OnRunLost);
            GameManager.Instance.onRunWin.RemoveListener(OnRunWin);
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

    public void OnRunLost()
    {
        onRunLost.Invoke();
        CoinScoreAdd();
        Debug.Log("Run Lost");
        _saveManager.SaveGame();
    }

    private void CoinScoreAdd()
    {
        int amout = (int)_hotelRateManager.totalReviews;
        _jetonSO.AddCoin(amout);
    }

    public void OnRunWin()
    {
        onRunWin.Invoke();
        CoinScoreAdd();
        Debug.Log("Run Win");
        _saveManager.SaveGame();
    }

    public void RunOver(bool isWin)
    {
        if ( isWin )
        {
            GameManager.Instance.RunOver(isWin);
        }
        else
        {
            GameManager.Instance.RunOver(isWin);
        }
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }
}