using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Image hpbar;
    public Animator HPBarStateImage;
    public TextMeshProUGUI HPBarStateText;
    public TextMeshProUGUI PotionCountText;
    public GameObject Key;
    public Player PlayerCode;
    public Image PotionGauge;
    int TestHp = 9999;
    private void OnEnable()
    {
        HPBarUpdate();
        PotionUpdate();
        //Player.s_HP = TestHp;
    }

    void Update()
    {
        HPBarUpdate();
        KeyUpdate();
        PotionUpdate();
        if(PlayerCode.IsUsePotion)
        {
            PotionGauge.fillAmount = PlayerCode.UsePotionTimer / PlayerCode.UsePotionTime;
        }
        else
        {
            PotionGauge.fillAmount = 0;
        }
    }

    void HPBarUpdate()
    {
        if (Player.s_HP < 0)
        {
            HPBarStateText.text = "0/100";
        }
        else if(Player.s_HP >= 0)
        {   
            HPBarStateText.text = Player.s_HP.ToString() + "/100";
        }
        hpbar.fillAmount = Player.s_HP / 100f;
        HPBarStateText.text = Player.s_HP.ToString() + "/100";
        if (Player.s_HP == 100)
        {
            HPBarStateImage.SetInteger("hpbar", 100);
        }
        else if (100 > Player.s_HP && Player.s_HP >= 75)
        {
            HPBarStateImage.SetInteger("hpbar", 75);
        }
        else if (75 > Player.s_HP && Player.s_HP >= 50)
        {
            HPBarStateImage.SetInteger("hpbar", 50);
        }
        else if (50 > Player.s_HP && Player.s_HP >= 25)
        {
            HPBarStateImage.SetInteger("hpbar", 25);
        }
        else if (25 > Player.s_HP && Player.s_HP > 0)
        {
            HPBarStateImage.SetInteger("hpbar", 1);
        }
        else
        {
            HPBarStateImage.SetInteger("hpbar", 0);
        }
    }

    void PotionUpdate()
    {
        PotionCountText.text = "x "+ Player.s_PotionCount;
    }

    void KeyUpdate()
    {
        if(Player.s_IsKey)
        {
            Key.SetActive(true);
        }
        else
        {
            Key.SetActive(false);
        }
    }

    //void OnLoadedScene(Scene scene, LoadSceneMode mode) 
    //{
    //    if(scene.buildIndex == 0 || scene.buildIndex == 5)
    //    {
    //        Destroy(gameObject);
    //        SceneManager.sceneLoaded -= OnLoadedScene;
    //        return;
    //    }
    //    else
    //    {
    //        PlayerCode = GameObject.Find("Player").GetComponent<Player>();
    //    }
    //}
}
