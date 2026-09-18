using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject CutsceneBackground;
    public Transform GoMainMenu;
    public Transform Player;

    void Start()
    {
        StartCoroutine(FadeEffectManager.Instance.FadeIn());
    }

    void Update()
    {
        if(Player.position.x >= GoMainMenu.position.x)
        {
            SceneManager.LoadScene(0);
        }
    }
}
