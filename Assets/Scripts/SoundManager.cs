using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip ForestBGMClip;
    public AudioClip DungeonBGMClip;
    public AudioSource BGMAudioSource;
    public AudioSource SFXAudioSource;
    public static float s_BGMVolume;
    public static float s_SFXVolume;
    public static float s_MasterVolume;
    public static bool s_BGMVolumeMute;
    public static bool s_SFXVolumeMute;
    public static bool s_MasterVolumeMute;
    public AudioMixer AudioMixer;
    public AudioSource BossSayEffectAudioSource;
    public AudioSource UnlockSoundAudioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnLoadedScene;
    }
    void Start()
    {
        BGMAudioSource.clip = ForestBGMClip;
        BGMAudioSource.Play();
    }
    private void Update()
    {   
        if(s_MasterVolumeMute)
        {
            AudioMixer.SetFloat("Master", -80);
        }
        else
        {
            if (s_MasterVolume == 0) AudioMixer.SetFloat("Master", -80);
            else AudioMixer.SetFloat("Master", Mathf.Lerp(-20, 20, s_MasterVolume / 100f));
        }
        if (s_SFXVolumeMute)
        {
            AudioMixer.SetFloat("SFX", -80);
        }
        else
        {
            if (s_SFXVolume == 0) AudioMixer.SetFloat("SFX", -80);
            else AudioMixer.SetFloat("SFX", Mathf.Lerp(-20, 20, s_SFXVolume / 100f));
        }
        if (s_BGMVolumeMute)
        {
            AudioMixer.SetFloat("BGM", -80);
        }
        else
        {
            if (s_BGMVolume == 0) AudioMixer.SetFloat("BGM", -80);
            else AudioMixer.SetFloat("BGM", Mathf.Lerp(-20, 20, s_BGMVolume / 100f));
        }
    }
    void OnLoadedScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            BGMAudioSource.clip = ForestBGMClip;
            BGMAudioSource.Play();
        }
        else
        {
            BGMAudioSource.clip = DungeonBGMClip;
            BGMAudioSource.Play();
        }
        if (scene.buildIndex == 5)
        {
            BGMAudioSource.clip = ForestBGMClip;
            BGMAudioSource.Play();
        }
        //SFXAudioSource.PlayOneShot
    }
    public void SFXPlay(AudioClip clip)
    {
        SFXAudioSource.PlayOneShot(clip);
    }

    public void FootStepPlay(AudioClip clip)
    {
        SFXAudioSource.clip = clip;
        SFXAudioSource.Play();
    }

    public void FootStepStop()
    {
        SFXAudioSource.Stop();
    }

    public void BossSayEffectPlay()
    {
        BossSayEffectAudioSource.Play();
    }
    public void BossSayEffectStop()
    {
        BossSayEffectAudioSource.Stop();
    }
    public void UnlockSoundPlay()
    {
        UnlockSoundAudioSource.Play();
    }
    public void UnlockSoundStop()
    {
        UnlockSoundAudioSource.Stop();
    }
}
