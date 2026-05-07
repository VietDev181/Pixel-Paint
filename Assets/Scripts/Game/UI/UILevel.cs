using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
    private const string SelectedLevelKey = "SelectedLevel";

    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private int totalLevels = 15;

    [Header("Buttons")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject resetPanel;

    private IAudioService audioService;
    private ISceneService sceneService;
    private LevelProgressUseCase progress;

    public void Initialize(IAudioService audio, ISceneService scene, LevelProgressUseCase progress)
    {
        this.audioService = audio;
        this.sceneService = scene;
        this.progress = progress;

        homeButton.onClick.AddListener(OnHome);
        settingButton.onClick.AddListener(OnOpenSetting);
        resetButton.onClick.AddListener(OnAskReset);
        resumeButton.onClick.AddListener(OnClosePanels);
        yesButton.onClick.AddListener(OnConfirmReset);
        noButton.onClick.AddListener(OnClosePanels);

        settingPanel.SetActive(false);
        resetPanel.SetActive(false);

        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        for (int i = 0; i < totalLevels; i++)
        {
            int levelIndex = i;
            int oneBased = i + 1;

            var btnObj = Instantiate(levelButtonPrefab, buttonContainer);
            var btn = btnObj.GetComponent<Button>();
            var label = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            Transform lockTransform = btnObj.transform.Find("LockIcon");
            Image lockIcon = lockTransform != null ? lockTransform.GetComponent<Image>() : null;
            var starUI = btnObj.GetComponent<UILevelButton>();

            if (label != null) label.text = oneBased.ToString();

            btnObj.transform.localScale = Vector3.zero;
            btnObj.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetDelay(i * 0.05f);

            bool unlocked = progress.IsUnlocked(levelIndex);
            btn.interactable = unlocked;
            if (lockIcon != null) lockIcon.gameObject.SetActive(!unlocked);

            if (starUI != null && starUI.starContainer != null)
                starUI.starContainer.SetActive(unlocked);

            if (unlocked)
            {
                btn.onClick.AddListener(() => SelectLevel(levelIndex));
                if (starUI != null) starUI.SetStars(progress.GetStars(levelIndex));
            }
        }
    }

    private void SelectLevel(int levelIndex)
    {
        audioService.PlayClickSFX();
        PlayerPrefs.SetInt(SelectedLevelKey, levelIndex);
        PlayerPrefs.Save();
        sceneService.LoadGameScene();
    }

    private void OnHome()
    {
        audioService.PlayClickSFX();
        sceneService.LoadStartScene();
    }

    private void OnOpenSetting()
    {
        audioService.PlayClickSFX();
        Time.timeScale = 0f;
        ShowPanel(settingPanel);
    }

    private void OnAskReset()
    {
        audioService.PlayClickSFX();
        Time.timeScale = 0f;
        ShowPanel(resetPanel);
    }

    private void OnClosePanels()
    {
        audioService.PlayClickSFX();
        Time.timeScale = 1f;
        HidePanel(settingPanel);
        HidePanel(resetPanel);
    }

    private void OnConfirmReset()
    {
        audioService.PlayClickSFX();
        Time.timeScale = 1f;
        progress.Reset();
        sceneService.ReloadCurrentScene();
    }

    private static void ShowPanel(GameObject panel)
    {
        if (panel == null) return;
        panel.SetActive(true);
        if (!panel.TryGetComponent<CanvasGroup>(out var cg)) cg = panel.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        cg.DOFade(1f, 0.3f);
        panel.transform.localScale = Vector3.one * 0.9f;
        panel.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
    }

    private static void HidePanel(GameObject panel)
    {
        if (panel == null || !panel.activeSelf) return;
        var cg = panel.GetComponent<CanvasGroup>();
        if (cg != null) cg.DOFade(0f, 0.2f).OnComplete(() => panel.SetActive(false));
        else panel.SetActive(false);
    }
}
