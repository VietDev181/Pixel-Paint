using UnityEngine;
using UnityEngine.Audio;

public class AudioService : MonoBehaviour, IAudioService
{
    private const string BgmKey = "BGMVolume";
    private const string SfxKey = "SFXVolume";
    private const string BgmMixerParam = "BGMVolume";
    private const string SfxMixerParam = "SFXVolume";

    [Header("Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip brushClip;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private AudioClip levelBGM;
    [SerializeField] private AudioClip gameBGM;
    [SerializeField] private AudioClip winBGM;
    [SerializeField] private AudioClip gameOverBGM;

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    private void Start()
    {
        ApplyVolume(BgmMixerParam, GetBGMVolume());
        ApplyVolume(SfxMixerParam, GetSFXVolume());
    }

    private void ApplyVolume(string param, float value)
    {
        if (mixer != null) mixer.SetFloat(param, LinearToDb(value));
    }

    public void PlayClickSFX() => PlaySFX(clickClip);
    public void PlayBrushSFX() => PlaySFX(brushClip);

    public void PlayMainMenuBGM() => PlayBGM(mainMenuBGM);
    public void PlayLevelBGM() => PlayBGM(levelBGM);
    public void PlayGameBGM() => PlayBGM(gameBGM);
    public void PlayWinBGM() => PlayBGM(winBGM);
    public void PlayGameOverBGM() => PlayBGM(gameOverBGM);

    public void SetBGMVolume(float value)
    {
        if (mixer != null) mixer.SetFloat(BgmMixerParam, LinearToDb(value));
        PlayerPrefs.SetFloat(BgmKey, value);
    }

    public void SetSFXVolume(float value)
    {
        if (mixer != null) mixer.SetFloat(SfxMixerParam, LinearToDb(value));
        PlayerPrefs.SetFloat(SfxKey, value);
    }

    public float GetBGMVolume() => PlayerPrefs.GetFloat(BgmKey, 1f);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SfxKey, 1f);

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }

    private void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    private static float LinearToDb(float linear)
    {
        return Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }
}
