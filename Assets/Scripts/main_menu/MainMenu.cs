using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [Header("메인 메뉴 UI 오브젝트")]
    public GameObject LoadGame;
    public GameObject Option;
    public GameObject QuitGame;
    public GameObject Title;
    public GameObject NewGame;

    [Header("플레이어 씬 전환 오브젝트")]
    public GameObject Player;
    public GameObject[] Background;

    public bool TitleFadeEnd = false;
    public Image TitleImage;
    float _fadeTime = 2.5f;
    float _fadeTimer;
    public Button LoadButton;
    public GameObject LoadGaemSaveSlot;
    public CanvasGroup[] LoadGameSlotButtonCanvasGroup; 
    public TextMeshProUGUI[] LoadGameSlotButtonText;
    Stack<GameObject> _openWindow = new Stack<GameObject>();
    public GameObject DeleteWarning;
    public TextMeshProUGUI DeleteWarningText;
    public Button DeleteYesButton;
    public Button DeleteNoButton;
    bool _isDeleting = false;
    public int DeleteDataNumber;
    public CanvasGroup[] NewGameSlotButtonCanvasGroup;
    public TextMeshProUGUI[] NewGameSlotButtonText;
    public GameObject NewGaemSaveSlot;
    public CanvasGroup[] NewGameSlotDeleteButton;
    public GameObject OptionWindow;
    public Slider MasterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider BGMVolumeSlider;
    public TextMeshProUGUI MasterVolumeStateText;
    public TextMeshProUGUI SFXVolumeStateText;
    public TextMeshProUGUI BGMVolumeStateText;
    public GameObject MasterVolumeMuteGameObject;
    public GameObject SFXVolumeMuteGameObject;
    public GameObject BGMVolumeMuteGameObject;
    public Button MasterVolumeMuteButton;
    public Button SFXVolumeMuteButton;
    public Button BGMVolumeMuteButton;
    public GameObject FadeObject;
    public Image FadeObjectImage;
    private void Start()
    {
        StartCoroutine(FadeEffectManager.Instance.FadeIn());
        ReLoadData();
        MasterVolumeSlider.value = SoundManager.s_MasterVolume;
        SFXVolumeSlider.value = SoundManager.s_SFXVolume;
        BGMVolumeSlider.value = SoundManager.s_BGMVolume;
    }

    public void OnClickNewGame()
    {
        Debug.Log("새 게임");
        NewGaemSaveSlot.SetActive(true);
        _openWindow.Push(NewGaemSaveSlot);
    }

    public void OnClickLoadGame()
    {
        LoadGaemSaveSlot.SetActive(true);
        _openWindow.Push(LoadGaemSaveSlot);
    }

    public void OnClickOption()
    {
        OptionWindow.SetActive(true);
        _openWindow.Push(OptionWindow);
    }
    public void OnClickQuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    public void OnClickNewSlot(int i)
    {
        NewGame.SetActive(false);
        LoadGame.SetActive(false);
        Option.SetActive(false);
        QuitGame.SetActive(false);
        _openWindow.Pop().SetActive(false);
        DataManager.Instance.SlotNumber = i;
        StartCoroutine(TitleFadeOut());
    }

    public void OnClickLoadSlot(int i)
    {
        StartCoroutine(FadeEffectManager.Instance.LoadGameFadeOut(i));
    }

    public void OnClickDataDelete(int i)
    {
        DeleteDataNumber = i;
        DeleteWarning.SetActive(true);
        _openWindow.Push(DeleteWarning);
    }

    public void OnClickDataDeleteYes()
    {
        File.Delete(Application.persistentDataPath + "/game" + DeleteDataNumber);
        StartCoroutine(SlotDataDeleteAndCheck(DeleteDataNumber));
    }
    public void OnClickDataDeleteNo()   
    {
        DeleteWarningText.text = "정말 데이터를 삭제하시겠습니까?";
        DeleteYesButton.interactable = true;
        DeleteWarning.SetActive(false);
        _openWindow.Pop();
    }
    void OnCloseWindowAndPause()
    {
        if(_openWindow.Count > 0 && !_isDeleting)
        {
            
            if(_openWindow.Peek().CompareTag("Option"))
            { 
                DataManager.Instance.SoundDataSave();
            }
            _openWindow.Pop().SetActive(false);
        }
    }

    IEnumerator TitleFadeOut()
    {
        yield return TimeManager.s_Wait_1s;
        Color color = TitleImage.color;
        while (_fadeTimer < _fadeTime)
        {
            _fadeTimer += Time.deltaTime;
            color.a =1 -  _fadeTimer/_fadeTime;
            TitleImage.color = color;
            yield return null;
        }
        TitleFadeEnd = true;
    }
    IEnumerator SlotDataDeleteAndCheck(int i)
    {
        _isDeleting = true;
        File.Delete(Application.persistentDataPath + "/game" + i);
        DeleteYesButton.interactable = false;
        DeleteNoButton.interactable = false;
        while (_isDeleting)
        {
            if (File.Exists(Application.persistentDataPath + "/game" + i))
            {
                DeleteWarningText.text = "삭제 진행중입니다.";
            }
            else
            {
                DeleteWarningText.text = "삭제가 완료되었습니다.";
                DeleteNoButton.interactable = true;
                _isDeleting = false;
            }
            yield return null;
        }
        LoadButton.interactable = true;
        for (int j = 0; j < 3; j++)
        {
            if (File.Exists(Application.persistentDataPath + "/game" + (j + 1)))
            {
                LoadGameSlotButtonCanvasGroup[j].interactable = true;
                LoadGameSlotButtonCanvasGroup[j].alpha = 1f;
                LoadGameSlotButtonText[j].text = "게임" + (j + 1) + " 불러오기";
            }
            else
            {
                LoadGameSlotButtonCanvasGroup[j].interactable = false;
                LoadGameSlotButtonCanvasGroup[j].alpha = 0.7f;
                LoadGameSlotButtonText[j].text = "게임" + (j + 1) + " 데이터 파일 없음";
            }
        }
        ReLoadData();
    }
    void ReLoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/game1") || File.Exists(Application.persistentDataPath + "/game2") || File.Exists(Application.persistentDataPath + "/game3"))
        {
            LoadButton.interactable = true;
            for (int i = 0; i < 3; i++)
            {
                if (File.Exists(Application.persistentDataPath + "/game" + (i + 1)))
                {
                    LoadGameSlotButtonCanvasGroup[i].interactable = true;
                    LoadGameSlotButtonCanvasGroup[i].alpha = 1f;
                    LoadGameSlotButtonText[i].text = "게임" + (i + 1) + " 불러오기";  
                }
                else
                {
                    
                    LoadGameSlotButtonCanvasGroup[i].interactable = false;
                    LoadGameSlotButtonCanvasGroup[i].alpha = 0.7f;
                    LoadGameSlotButtonText[i].text = "게임" + (i + 1) + " 데이터 파일 없음";
                }
            }
        }
        else
        {
            LoadButton.interactable = false;
        }
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/game" + (i + 1)))
            {
                NewGameSlotButtonCanvasGroup[i].interactable = false;
                NewGameSlotButtonCanvasGroup[i].alpha = 0.7f;
                NewGameSlotButtonText[i].text = "게임 진행중";
                NewGameSlotDeleteButton[i].interactable = true;
                NewGameSlotDeleteButton[i].alpha = 1;
            }
            else
            {
                NewGameSlotButtonCanvasGroup[i].interactable = true;
                NewGameSlotButtonCanvasGroup[i].alpha = 1f; 
                NewGameSlotButtonText[i].text = "새로운 게임 시작하기";
                NewGameSlotDeleteButton[i].interactable = false;
                NewGameSlotDeleteButton[i].alpha = 0.7f;
            }
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
