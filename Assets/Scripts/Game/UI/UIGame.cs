using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [Header("Top Bar")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button doneButton;
    [SerializeField] private TextMeshProUGUI levelLabel;

    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button backToLevelsButton;

    [Header("Settings Panel")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Button resumeSettingButton;

    [Header("Win Panel")]
    [SerializeField] private GameObject winGamePanel;
    [SerializeField] private TextMeshProUGUI winAccuracyLabel;
    [SerializeField] private Image[] winStarImages;
    [SerializeField] private Sprite starOnSprite;
    [SerializeField] private Sprite starOffSprite;
    [SerializeField] private Button winNextLevelButton;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverAccuracyLabel;
    [SerializeField] private Button gameOverRetryButton;

    public event Action OnSubmitRequested;
    public event Action OnRetryRequested;
    public event Action OnNextLevelRequested;
    public event Action OnPauseRequested;
    public event Action OnResumeRequested;

    private IAudioService audioService;
    private ISceneService sceneService;

    public void Initialize(IAudioService audio, ISceneService scene)
    {
        audioService = audio;
        sceneService = scene;

        homeButton.onClick.AddListener(HandleHome);
        pauseButton.onClick.AddListener(HandlePause);
        settingButton.onClick.AddListener(HandleOpenSettings);
        doneButton.onClick.AddListener(HandleDone);

        resumeButton.onClick.AddListener(HandleResume);
        resumeSettingButton.onClick.AddListener(HandleResume);
        replayButton.onClick.AddListener(HandleRetry);
        backToLevelsButton.onClick.AddListener(HandleBackToLevels);

        winNextLevelButton.onClick.AddListener(HandleNext);
        gameOverRetryButton.onClick.AddListener(HandleRetry);

        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        winGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void OnLevelStarted(int oneBasedIndex)
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        winGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        if (levelLabel != null) levelLabel.text = $"Level {oneBasedIndex}";
        SetDoneInteractable(true);
    }

    public void SetDoneInteractable(bool interactable)
    {
        if (doneButton != null) doneButton.interactable = interactable;
    }

    public void ShowResult(LevelResult result)
    {
        Time.timeScale = 0f;
        if (result.IsCleared) ShowWinPanel(result);
        else ShowGameOverPanel(result);
    }

    private void ShowWinPanel(LevelResult result)
    {
        gameOverPanel.SetActive(false);
        winGamePanel.SetActive(true);

        if (winAccuracyLabel != null)
            winAccuracyLabel.text = $"{Mathf.RoundToInt(result.Accuracy * 100f)}%";

        for (int i = 0; i < winStarImages.Length; i++)
            winStarImages[i].sprite = i < result.Stars ? starOnSprite : starOffSprite;

        if (winNextLevelButton != null)
            winNextLevelButton.gameObject.SetActive(true);

        audioService.PlayWinBGM();
    }

    private void ShowGameOverPanel(LevelResult result)
    {
        winGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        if (gameOverAccuracyLabel != null)
            gameOverAccuracyLabel.text = $"{Mathf.RoundToInt(result.Accuracy * 100f)}%";

        if (gameOverRetryButton != null)
            gameOverRetryButton.gameObject.SetActive(true);

        audioService.PlayGameOverBGM();
    }

    private void HandleHome()
    {
        audioService.PlayClickSFX();
        sceneService.LoadStartScene();
    }

    private void HandlePause()
    {
        audioService.PlayClickSFX();
        pausePanel.SetActive(true);
        OnPauseRequested?.Invoke();
    }

    private void HandleOpenSettings()
    {
        audioService.PlayClickSFX();
        settingPanel.SetActive(true);
        OnPauseRequested?.Invoke();
    }

    private void HandleResume()
    {
        audioService.PlayClickSFX();
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        OnResumeRequested?.Invoke();
    }

    private void HandleDone()
    {
        audioService.PlayClickSFX();
        SetDoneInteractable(false);
        OnSubmitRequested?.Invoke();
    }

    private void HandleRetry()
    {
        audioService.PlayClickSFX();
        OnRetryRequested?.Invoke();
    }

    private void HandleNext()
    {
        audioService.PlayClickSFX();
        OnNextLevelRequested?.Invoke();
    }

    private void HandleBackToLevels()
    {
        audioService.PlayClickSFX();
        sceneService.LoadLevelSelectScene();
    }
}
