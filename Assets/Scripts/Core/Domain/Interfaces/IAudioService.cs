public interface IAudioService
{
    void PlayClickSFX();
    void PlayBrushSFX();

    void PlayMainMenuBGM();
    void PlayLevelBGM();
    void PlayGameBGM();
    void PlayWinBGM();
    void PlayGameOverBGM();

    void SetBGMVolume(float value);
    void SetSFXVolume(float value);
    float GetBGMVolume();
    float GetSFXVolume();
}
