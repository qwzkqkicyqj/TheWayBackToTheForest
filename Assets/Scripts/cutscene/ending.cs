using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public Transform GoMainMenu;
    public Transform Player;
    public Transform BackGround;
    void Start()
    {
        StartCoroutine(FadeEffectManager.Instance.FadeIn());
    }

    void Update()
    {
        if(Player.position.x >= GoMainMenu.position.x)
        {
            Player.position = new Vector2(-2.969164f, 0);
            BackGround.position = new Vector2(-2.969164f, BackGround.position.y);
        }
    }
}
