using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutsceneSkip : MonoBehaviour
{
    public Image Gauge;
    public bool IsFHold = false;
    public float IsFHoldTimer = 0;
    public bool IsCutsceneEnd = false;

    void Update()
    {
        if(IsFHold)
        {
            IsFHoldTimer += Time.deltaTime;
            Gauge.fillAmount = IsFHoldTimer / 3;
            if (IsFHoldTimer > 3)
            {
                IsCutsceneEnd = true;
            }
        }
        else
        {
            IsFHoldTimer = 0;
            Gauge.fillAmount = IsFHoldTimer;
        }
    }

    public void OnHold(InputValue value)
    {
        if (value.isPressed)
        {
            IsFHold = true;
        }
        else
        {
            IsFHold = false;
        }
    }
}
