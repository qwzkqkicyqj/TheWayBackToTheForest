using TMPro;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public GameObject _pauseWindow;
    bool _isPause = false;
    bool _isMainMenu = false;
    public Slider MasterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider BGMVolumeSlider;
    public TextMeshProUGUI MasterVolumeStateText;
    public TextMeshProUGUI SFXVolumeStateText;
    public TextMeshProUGUI BGMVolumeStateText;
    public GameObject MasterVolumeMuteGameObject;
    public GameObject SFXVolumeMuteGameObject;
    public GameObject BGMVolumeMuteGameObject;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnLoadedScene;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        MasterVolumeSlider.value = SoundManager.s_MasterVolume;
        SFXVolumeSlider.value = SoundManager.s_SFXVolume;
        BGMVolumeSlider.value = SoundManager.s_BGMVolume;
    }
    void OnLoadedScene(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameObject.SetActive(false);
            return;
        }
    }
    private void Update()
    {
        SoundStateUpdate();
    }

    public void OnClickMainMenu()
    {
        OnCloseWindowAndPause();
        StartCoroutine(FadeEffectManager.Instance.FadeOut(0));
    }

    void OnCloseWindowAndPause()
    {
        _isPause = _isPause ? false : true; 
        if (_isPause)
        {
            _pauseWindow.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            DataManager.Instance.SoundDataSave();
            _pauseWindow.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void OnClickMuteButton(string sound)
    {
        if (sound == "Master")
        {
            SoundManager.s_MasterVolumeMute = SoundManager.s_MasterVolumeMute ? false : true;
        }
        if (sound == "SFX")
        {
            SoundManager.s_SFXVolumeMute = SoundManager.s_SFXVolumeMute ? false : true;
        }
        if (sound == "BGM")
        {
            SoundManager.s_BGMVolumeMute = SoundManager.s_BGMVolumeMute ? false : true;
        }
    }
    public void SoundStateUpdate()
    {
        MasterVolumeStateText.text = $"{MasterVolumeSlider.value}/100";
        SoundManager.s_MasterVolume = MasterVolumeSlider.value;
        BGMVolumeStateText.text = $"{BGMVolumeSlider.value}/100";
        SoundManager.s_BGMVolume = BGMVolumeSlider.value;
        SFXVolumeStateText.text = $"{SFXVolumeSlider.value}/100";
        SoundManager.s_SFXVolume = SFXVolumeSlider.value;
        if (SoundManager.s_MasterVolumeMute)
        {
            MasterVolumeMuteGameObject.SetActive(true);
        }
        else
        {
            MasterVolumeMuteGameObject.SetActive(false);
        }
        if (SoundManager.s_SFXVolumeMute)
        {
            SFXVolumeMuteGameObject.SetActive(true);
        }
        else
        {
            SFXVolumeMuteGameObject.SetActive(false);
        }
        if (SoundManager.s_BGMVolumeMute)
        {
            BGMVolumeMuteGameObject.SetActive(true);
        }
        else
        {
            BGMVolumeMuteGameObject.SetActive(false);
        }
    }
}