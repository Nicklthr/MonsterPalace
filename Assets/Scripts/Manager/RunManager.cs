using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Michsky.UI.Dark;

public class RunManager : MonoBehaviour
{
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private MusicController _musicController;
    [SerializeField] private UIDissolveEffect _dissolveEffect;

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
            return;
        }

        _saveManager = FindObjectOfType<SaveManager>();

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

    private void OnRunLost()
    {
        onRunLost.Invoke();
        Debug.Log("Run Lost");
        _saveManager.SaveGame();
    }

    private void OnRunWin()
    {
        onRunWin.Invoke();
        _saveManager.SaveGame();
    }
}