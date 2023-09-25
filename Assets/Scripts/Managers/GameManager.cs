using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SDD.Events;

public enum GAMESTATE { startMenu, playMenu, selectMap, play, pause, win, lose, shop, score }
public delegate void afterFunction();

public class GameManager : MonoBehaviour, IEventHandler
{
    private static GameManager m_Instance;
    public static GameManager Instance { get { return m_Instance; } }

    GAMESTATE m_State;
    public bool IsPlaying => m_State == GAMESTATE.play;


    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.AddListener<ShopButtonClickedEvent>(ShopButtonClicked);
        EventManager.Instance.AddListener<StartMenuButtonClickedEvent>(StartMenuButtonClicked);
        EventManager.Instance.AddListener<PlayMenuButtonClickedEvent>(PlayMenuButtonClicked);
        EventManager.Instance.AddListener<HasSelectedMapEvent>(HasSelectedMap);
        EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);
        EventManager.Instance.AddListener<PauseButtonClickedEvent>(PauseButtonClicked);
        EventManager.Instance.AddListener<PauseQuitButtonClickedEvent>(PauseQuitButtonClicked);
        EventManager.Instance.AddListener<ScoreLevelEvent>(ScoreLevel);       
        EventManager.Instance.AddListener<WinLevelEvent>(WinLevel);       
        EventManager.Instance.AddListener<LoseLevelEvent>(LoseLevel);       

    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.RemoveListener<ShopButtonClickedEvent>(ShopButtonClicked);
        EventManager.Instance.RemoveListener<StartMenuButtonClickedEvent>(StartMenuButtonClicked);
        EventManager.Instance.RemoveListener<PlayMenuButtonClickedEvent>(PlayMenuButtonClicked);
        EventManager.Instance.RemoveListener<ShowScoreFinishedEvent>(ShowScoreFinished);
        EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);
        EventManager.Instance.RemoveListener<PauseButtonClickedEvent>(PauseButtonClicked);
        EventManager.Instance.RemoveListener<PauseQuitButtonClickedEvent>(PauseQuitButtonClicked);
        EventManager.Instance.RemoveListener<ScoreLevelEvent>(ScoreLevel);
        EventManager.Instance.RemoveListener<WinLevelEvent>(WinLevel);
        EventManager.Instance.RemoveListener<LoseLevelEvent>(LoseLevel);
    }

    void OnEnable()
    {
        SubscribeEvents();
    }
    void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        if (!m_Instance) m_Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartMenu();
    }

    void Update()
    {
    }

    void SetState(GAMESTATE newState)
    {
        m_State = newState;
        switch (m_State)
        {
            case GAMESTATE.startMenu:
                EventManager.Instance.Raise(new StartMenuEvent());
                break;
            case GAMESTATE.playMenu:
                EventManager.Instance.Raise(new PlayMenuEvent());
                break;
            case GAMESTATE.play:
                EventManager.Instance.Raise(new StartLevelEvent());
                break;
            case GAMESTATE.pause:
                EventManager.Instance.Raise(new GamePauseEvent());
                break;
            case GAMESTATE.selectMap:
                EventManager.Instance.Raise(new SelectMapEvent());
                break;
            case GAMESTATE.win:
                EventManager.Instance.Raise(new WinMenuEvent());
                break;
            case GAMESTATE.score:
                EventManager.Instance.Raise(new ScoreLevelMenuEvent());
                break;
            case GAMESTATE.lose:
                EventManager.Instance.Raise(new LoseMenuEvent());
                break;
        }

    }

    void StartMenu()
    {
        SetState(GAMESTATE.startMenu);
    }

    void PlayMenu()
    {
        SetState(GAMESTATE.playMenu);
    }

    void Play(int scene)
    {
        StartCoroutine(LoadSceneThenFunction(scene, StartLevel));
    }

    void StartLevel()
    {
        SetState(GAMESTATE.play);
    }
    void Shop()
    {
        SetState(GAMESTATE.shop);
    }

    void Pause()
    {
        SetState(GAMESTATE.pause);
    }

    void Win()
    {
        SetState(GAMESTATE.win);
    }
    void Lose()
    {
        SetState(GAMESTATE.lose);
    }

    void SelectMap()
    {
        SetState(GAMESTATE.selectMap);
    }
    // MenuManager events' callback
    void PlayButtonClicked(PlayButtonClickedEvent e)
    {
        SelectMap();
    }

    void ShopButtonClicked(ShopButtonClickedEvent e)
    {
        Shop();
    }
    void StartMenuButtonClicked(StartMenuButtonClickedEvent e)
    {
        PlayMenu();
    }

    void ScoreLevel(ScoreLevelEvent e)
    {
        StartCoroutine(LoadSceneThenFunction(7, ShowScore));
    }
    void ShowScore()
    {
        SetState(GAMESTATE.score);
    }

    void PlayMenuButtonClicked(PlayMenuButtonClickedEvent e)
    {
        SelectMap();
    }

    void QuitButtonClicked(QuitButtonClickedEvent e)
    {
        Application.Quit();
    }

    void PauseButtonClicked(PauseButtonClickedEvent e)
    {
        Pause();
    }
    
    void HasSelectedMap(HasSelectedMapEvent e)
    {
        Play(e.scene);
    }
    void WinLevel(WinLevelEvent e)
    {
        StartCoroutine(LoadSceneThenFunction(7, Win));
    }
    public void PauseQuitButtonClicked(PauseQuitButtonClickedEvent e)
    {
        EventManager.Instance.Raise(new InitLevelsEvent());
        StartCoroutine(LoadSceneThenFunction(7, PlayMenu));
    }

    void ShowScoreFinished(ShowScoreFinishedEvent e)
    {
        SelectMap();
    }

    void LoseLevel(LoseLevelEvent e)
    {
        StartCoroutine(LoadSceneThenFunction(7, Lose));
    }

    // Coroutine pour charger la scène de manière asynchrone et appeler la fonction spécifiée
    private IEnumerator LoadSceneThenFunction(int sceneIndex, afterFunction function)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        // Attendre que la scène soit chargée
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // La scène est chargée, appeler la fonction spécifiée
        function.Invoke();
    }
}
