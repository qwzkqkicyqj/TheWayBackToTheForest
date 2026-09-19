using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections;



public class PlayerData
{
    public int SlotNumber;
    public int HP = 0;
    public int PotionCount = 0;
    public int SceneNumber = 0;
    public bool IsClear = false;
}

public class SoundData
{
    public float MasterVolume;
    public float SFXVolume;
    public float BGMVolume;
    public bool MasterVolumeMute;
    public bool SFXVolumeMute;
    public bool BGMVolumeMute;
}

public class DataManager : MonoBehaviour
{
    public string SaveData;
    public string LoadData;
    public static DataManager Instance = null;
    public int SlotNumber;
    public string EncryptAndDecryptKey = "chungkang";
    public PlayerData PlayerData = new PlayerData();
    SoundData SoundData = new SoundData();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SoundDataLoad();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void ReSetData()
    {
        PlayerData.HP = 0;
        PlayerData.SceneNumber = 1;
        PlayerData.PotionCount = 0;
        PlayerData.IsClear = false;
    }
    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        PlayerDataSave();
    }

    void PlayerDataSave()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerData.SceneNumber = SceneManager.GetActiveScene().buildIndex;
            PlayerData.SlotNumber = this.SlotNumber;
            SaveData = JsonUtility.ToJson(PlayerData);
            File.WriteAllText(Application.persistentDataPath + "/game" + PlayerData.SlotNumber, EncryptAndDecrypt(SaveData));
            Debug.Log(SaveData);
            Debug.Log(EncryptAndDecrypt(SaveData));
            return;
        }
        PlayerData.HP = Player.s_HP;
        PlayerData.PotionCount = Player.s_PotionCount;
        PlayerData.SceneNumber = SceneManager.GetActiveScene().buildIndex;
        PlayerData.SlotNumber = this.SlotNumber;
        SaveData = JsonUtility.ToJson(PlayerData);    
        File.WriteAllText(Application.persistentDataPath + "/game" + PlayerData.SlotNumber, EncryptAndDecrypt(SaveData));
        Debug.Log(SaveData);
    }

    public void PlayerDataLoad(int i)
    {
        LoadData = File.ReadAllText(Application.persistentDataPath + "/game" + i);
        PlayerData = JsonUtility.FromJson<PlayerData>(EncryptAndDecrypt(LoadData));
        this.SlotNumber = PlayerData.SlotNumber;
        // 이미 클리어를 성공한 게임 데이터
        if (PlayerData.IsClear)
        {
            Debug.Log("탈출을 성공한 게임입니다. 처음부터 다시 하시겠습니다?");
            return;
        }
        SceneManager.LoadScene(PlayerData.SceneNumber);
        PauseManager.Instance.gameObject.SetActive(true);
        if (PlayerData.SceneNumber != 1)
        {
            Player.s_HP = PlayerData.HP;
            Player.s_PotionCount = PlayerData.PotionCount;
        }
    }

    public void SoundDataSave()
    {
        SoundData.MasterVolume = SoundManager.s_MasterVolume;
        SoundData.SFXVolume = SoundManager.s_SFXVolume;
        SoundData.BGMVolume = SoundManager.s_BGMVolume;
        SoundData.MasterVolumeMute = SoundManager.s_MasterVolumeMute;
        SoundData.SFXVolumeMute = SoundManager.s_SFXVolumeMute;
        SoundData.BGMVolumeMute = SoundManager.s_BGMVolumeMute;
        SaveData = JsonUtility.ToJson(SoundData);
        File.WriteAllText(Application.persistentDataPath + "/Option", SaveData);
        Debug.Log("설정 완료");
    }
    void SoundDataLoad()
    {
        LoadData = File.ReadAllText(Application.persistentDataPath + "/Option");
        SoundData = JsonUtility.FromJson<SoundData>(LoadData);
        SoundManager.s_MasterVolume = SoundData.MasterVolume;
        SoundManager.s_SFXVolume = SoundData.SFXVolume;
        SoundManager.s_BGMVolume = SoundData.BGMVolume;
        SoundManager.s_MasterVolumeMute = SoundData.MasterVolumeMute;
        SoundManager.s_SFXVolumeMute = SoundData.SFXVolumeMute;
        SoundManager.s_BGMVolumeMute = SoundData.BGMVolumeMute;
    }
    string EncryptAndDecrypt(string data)
    {
        string _result = "";
        for (int i = 0; i < data.Length; i++)
        {
            _result += (char)(data[i] ^ EncryptAndDecryptKey[i % EncryptAndDecryptKey.Length]);
        }
        return _result;
    }
}