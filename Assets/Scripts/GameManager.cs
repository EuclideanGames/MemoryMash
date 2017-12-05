using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables
    public CanvasGroup MenuPanel;
    public Text AuthenticationText;

    public CanvasGroup GamePanel;
    public Text ScoreText;
    public Text TimerText;
    public Text LivesText;

    public CanvasGroup SummaryPanel;
    public TextMeshProUGUI SummaryTitleText;
    public RectTransform SummaryStatPanel;
    public SummaryStatUIObject SummaryStatPrefab;

    public SnapScrollRect GameModeSelectPanel;
    public SnapScrollRect DifficultySelectPanel;

    public float MaxTimeBetweenAds;

    public Square SquarePrefab;
    public List<Difficulty> Difficulties;

    private int selectedGameModeIndex;
    private int selectedDifficultyIndex;

    private GameMode activeGameMode;
    
    private float timeSinceLastAd;
    #endregion

    #region MonoBehaviour Functions
    void Awake()
    {
        selectedGameModeIndex = 0;
        selectedDifficultyIndex = 0;
        
        timeSinceLastAd = 0.0f;
    }

    void Start()
    {
        InitializeGPGS();

        GameModeSelectPanel.OnPageChanged.AddListener(SelectGameMode);
        DifficultySelectPanel.OnPageChanged.AddListener(SelectDifficulty);
    }

    void Update()
    {
        Vector3 screen_to_world;
        Vector2 touch_pos;

        if (activeGameMode != null && activeGameMode.Running)
        {
            if (Application.isMobilePlatform)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        screen_to_world = Camera.main.ScreenToWorldPoint(touch.position);
                        touch_pos = new Vector2(screen_to_world.x, screen_to_world.y);

                        RaycastHit2D hit = Physics2D.Raycast(touch_pos, Camera.main.transform.forward);

                        if (hit.collider != null)
                        {
                            Square square = hit.transform.GetComponent<Square>();

                            if (square != null)
                            {
                                square.Hit();
                            }
                        }
                    }
                    else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        screen_to_world = Camera.main.ScreenToWorldPoint(touch.position);
                        touch_pos = new Vector2(screen_to_world.x, screen_to_world.y);

                        RaycastHit2D hit = Physics2D.Raycast(touch_pos, Camera.main.transform.forward);

                        if (hit.collider != null)
                        {
                            Square square = hit.transform.GetComponent<Square>();

                            if (square != null)
                            {
                                square.Hold();
                            }
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    screen_to_world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touch_pos = new Vector2(screen_to_world.x, screen_to_world.y);

                    RaycastHit2D hit = Physics2D.Raycast(touch_pos, Camera.main.transform.forward);

                    if (hit.collider != null)
                    {
                        Square square = hit.transform.GetComponent<Square>();

                        if (square != null)
                        {
                            square.Hit();
                        }
                    }
                }
                else if(Input.GetMouseButton(0))
                {
                    screen_to_world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touch_pos = new Vector2(screen_to_world.x, screen_to_world.y);

                    RaycastHit2D hit = Physics2D.Raycast(touch_pos, Camera.main.transform.forward);

                    if (hit.collider != null)
                    {
                        Square square = hit.transform.GetComponent<Square>();

                        if (square != null)
                        {
                            square.Hold();
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                activeGameMode.EndGame();

            activeGameMode.OnUpdate();
        }

        timeSinceLastAd += Time.deltaTime;
    }
    #endregion

    #region GPGS
    public void InitializeGPGS()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        SignIn();
    }

    public void SignIn()
    {
        if (Social.localUser.authenticated)
        {
            return;
        }

        Social.localUser.Authenticate((bool success) =>
        {
            //on success, allow user to interact with menu
            //on failure, show error message

            //MenuPanel.interactable = success;
            //AuthenticationText.gameObject.SetActive(!success);
        });
    }

    public void OpenAchievements()
    {
        if (Social.localUser.authenticated)
        {
            Social.Active.ShowAchievementsUI();
        }
    }

    public void OpenLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.Active.ShowLeaderboardUI();
        }
    }
    #endregion

    #region Game Logic
    public void SelectGameMode(int modeIndex)
    {
        selectedGameModeIndex = modeIndex;
    }

    public void SelectDifficulty(int difficultyIndex)
    {
        selectedDifficultyIndex = difficultyIndex;
    }

    public void StartGame()
    {
        SwitchToGame();

        ScoreText.text = string.Empty;
        LivesText.text = string.Empty;

        switch (selectedGameModeIndex)
        {
            case 0:
                activeGameMode = new TimeAttack();
                break;
            case 1:
                activeGameMode = new Survival();
                break;
            case 2:
                activeGameMode = new Frenzy();
                break;
            case 3:
                activeGameMode = new Anchor();
                break;
            case 4:
                activeGameMode = new ColorCode();
                break;
            default:
                break;
        }

        activeGameMode.OnScoreChanged += HandleScoreChanged;
        activeGameMode.OnTimerChanged += HandleTimerChanged;
        activeGameMode.OnLivesChanged += HandleLivesChanged;
        activeGameMode.OnGameEnded += HandleGameEnded;

        activeGameMode.SelectedDifficulty = Difficulties[selectedDifficultyIndex];
        activeGameMode.ActiveFactory = SquareFactory.CreateFactory();
        activeGameMode.ActiveFactory.SquarePrefab = SquarePrefab;

        activeGameMode.InitializeGame();
        
        StartCoroutine(activeGameMode.InitializeGrid());
    }
    #endregion

    #region UI
    public void SwitchToGame()
    {
        GamePanel.Show();
        MenuPanel.Hide();
        SummaryPanel.Hide();

        AdManager.Instance.CloseBannerAd();
    }

    public void SwitchToMenu()
    {
        GamePanel.Hide();
        MenuPanel.Show();
        SummaryPanel.Hide();

        AdManager.Instance.GetBannerAd();
    }

    public void SwitchToSummary()
    {
        GamePanel.Hide();
        MenuPanel.Hide();
        SummaryPanel.Show();
    }

    public void PopulateSummaryData()
    {
        SummaryTitleText.text = activeGameMode.SummaryTitle;

        foreach (Transform child in SummaryStatPanel)
            Destroy(child.gameObject);

        foreach (KeyValuePair<string, string> pair in activeGameMode.GetGameSummary())
        {
            SummaryStatUIObject new_stat = Instantiate(SummaryStatPrefab);
            new_stat.transform.SetParent(SummaryStatPanel, false);
            new_stat.UpdateStat(pair);
        }
    }

    public void HandleScoreChanged(object sender, OnScoreChangedEventArgs args)
    {
        UpdateScoreText(args.NewScore);
    }

    public void HandleTimerChanged(object sender, OnTimerChangedEventArgs args)
    {
        UpdateTimerText(args.NewTime);
    }

    public void HandleLivesChanged(object sender, OnLivesChangedEventArgs args)
    {
        UpdateLivesText(args.NewLives);
    }

    public void HandleGameEnded(object sender, OnGameEndedEventArgs args)
    {
        PopulateSummaryData();
        SwitchToSummary();

        AchievementManager.IncrementGamesAchievements(1);

        if (timeSinceLastAd > MaxTimeBetweenAds)
        {
            AdManager.Instance.GetInterstitialAd();
            timeSinceLastAd = 0.0f;
        }
    }

    public void UpdateScoreText(int score)
    {
        ScoreText.text = score.ToString().PadLeft(5, '0');
    }

    public void UpdateTimerText(float time)
    {
        TimerText.text = time.ToString("F2") + "s";
    }

    public void UpdateLivesText(int lives)
    {
        LivesText.text = "Lives: " + lives.ToString();
    }
    #endregion
}
