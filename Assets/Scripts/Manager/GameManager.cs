using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MAINMENU,
        SHOP,
        STARTRUN,
        PAUSE,
        RUNEND
    }

    [Header("Game State")]
    public GameState currentState;

    public bool isMainMenu = false;
    public bool isShop = false; 
    public bool isPlay = false;
    public bool isPause = false;
    public bool isRunEnd = false;
    public bool isRunWin = false;

    [Space(10)]

    [Header("Events")]
    public UnityEvent onStart = new UnityEvent();
    public UnityEvent onShopEnter = new UnityEvent();
    public UnityEvent onShopExit = new UnityEvent();
    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onPauseEnter = new UnityEvent();
    public UnityEvent onPauseExit = new UnityEvent();
    public UnityEvent onRunLost = new UnityEvent();
    public UnityEvent onRunWin = new UnityEvent();

    [Space(10)]
    [Header("Referances")]
    [SerializeField] private ArgentSO _argent;

    private void Update()
    {
        OnStateUpdate();
    }

    private void OnStateEnter()
    {
        switch (currentState)
        {
            case GameState.MAINMENU:
                onStart.Invoke();
                Time.timeScale = 0;
                isMainMenu = true;
                break;
            case GameState.SHOP:
                onShopEnter.Invoke();
                isShop = true;
                break;
            case GameState.STARTRUN:
                Time.timeScale = 1;
                onPlay.Invoke();
                isPlay = true;
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
                onPauseEnter.Invoke();
                isPause = true;
                break;
            case GameState.RUNEND:
                Time.timeScale = 0;

                if ( isRunWin )
                {
                    onRunWin.Invoke();
                }
                else
                {
                    onRunLost.Invoke();
                }

                isRunEnd = true;
                
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (currentState)
        {
            case GameState.MAINMENU:
                if (isShop)
                {
                    TransitionToState(GameState.SHOP);
                }
                else if (isPlay)
                {
                    TransitionToState(GameState.STARTRUN);
                }
                else if (isPause)
                {
                    TransitionToState(GameState.PAUSE);
                }
                else if (isRunEnd || isRunWin)
                {
                    TransitionToState(GameState.RUNEND);
                }
                break;
            case GameState.SHOP:
                if (!isShop)
                {
                    TransitionToState(GameState.MAINMENU);
                }
                break;
            case GameState.STARTRUN:
                if (isMainMenu)
                {
                    TransitionToState(GameState.MAINMENU);
                }
                else if (isPause)
                {
                    TransitionToState(GameState.PAUSE);
                }
                else if (isRunEnd || isRunWin)
                {
                    TransitionToState(GameState.RUNEND);
                }
                break;
            case GameState.PAUSE:
                if (isMainMenu)
                {
                    TransitionToState(GameState.MAINMENU);
                }
                else if (isPlay)
                {
                    TransitionToState(GameState.STARTRUN);
                }
                else if (isRunEnd || isRunWin)
                {
                    TransitionToState(GameState.RUNEND);
                }
                break;
            case GameState.RUNEND:
                if (isMainMenu)
                {
                    TransitionToState(GameState.MAINMENU);
                }
                else if (isPlay)
                {
                    TransitionToState(GameState.STARTRUN);
                }
                else if (isPause)
                {
                    TransitionToState(GameState.PAUSE);
                }
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (currentState)
        {
            case GameState.MAINMENU:
                isMainMenu = false;
                break;
            case GameState.SHOP:
                onShopExit.Invoke();
                isShop = false;
                break;
            case GameState.STARTRUN:
                isPlay = false;
                break;
            case GameState.PAUSE:
                onPauseExit.Invoke();
                Time.timeScale = 1;
                isPause = false;
                break;
            case GameState.RUNEND:
                isMainMenu = false;
                break;
            default:
                break;
        }
    }

    private void TransitionToState(GameState state)
    {
        OnStateExit();
        currentState = state;
        OnStateEnter();
    }

    public void QuitGame()
    {
       Application.Quit();
    }

    public void StartRun()
    {
        isPlay = true;
    }

    public void PauseGame()
    {
        isPause = true;
    }

    public void ResumeGame()
    {
        isPlay = true;
    }

    public void RunOver(bool isWin)
    {
        isRunEnd = true;
        isRunWin = isWin;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void StartNewRun()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Run");
    }
}
