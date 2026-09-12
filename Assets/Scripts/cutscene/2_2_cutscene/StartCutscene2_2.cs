using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class StartCutscene2_2 : MonoBehaviour
{
    public CinemachineCamera PlayerCameraCinemachineCamera;
    public CinemachineCamera CutsceneCameraCinemachineCamera;
    public Player PlayerCode;
    public Player PlayerObject;
    public Transform[] CameraPoint;
    public GameObject Barrier;
    public CutsceneSkip CutsceneSkipCode;
    public GameObject CutsceneSkipObject;
    public GameObject UI;
    Coroutine _cutsceneSkip;
    public Transform PlayerCameraTransform;
    public Transform CutsceneCameraTransform;


    void Start()
    {
        _cutsceneSkip = StartCoroutine(StartCutscene());
    }

    void Update()
    {
        if(CutsceneSkipCode.IsCutsceneEnd)
        {
            FadeEffectManager.Instance.FadeSkip();
            StopCoroutine(_cutsceneSkip);
            Barrier.SetActive(true);
            PlayerCode.IsStop = false;
            UI.SetActive(true);
            CutsceneCameraTransform.position = PlayerCameraTransform.position;
            PlayerCameraCinemachineCamera.Priority = 100;
            Destroy(gameObject);
        }
    }

    IEnumerator StartCutscene()
    {
        UI.SetActive(false);
        PlayerCode.IsStop = true;
        PlayerObject.GetComponent<Rigidbody2D>().linearVelocityX = 10f;
        yield return TimeManager.s_Wait_0_5s;
        StartCoroutine(FadeEffectManager.Instance.FadeIn());
        while (!FadeEffectManager.Instance.FadeInEnd)
        {
            yield return null;
        }
        CutsceneCameraTransform.position = PlayerCameraTransform.position;
        CutsceneCameraCinemachineCamera.Priority = 2;
        PlayerObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
        CutsceneSkipObject.SetActive(true);
        yield return TimeManager.s_Wait_0_5s;

        while (Vector2.Distance(CutsceneCameraTransform.position, CameraPoint[0].position) > 0.01f)
        {
            CutsceneCameraTransform.position = Vector3.MoveTowards(CutsceneCameraTransform.position, CameraPoint[0].transform.position, 10 * Time.deltaTime);
            yield return null;
        }
        yield return TimeManager.s_Wait_2s;
        while (Vector2.Distance(CutsceneCameraTransform.position, CameraPoint[1].position) > 0.01f)
        {
            CutsceneCameraTransform.position = Vector3.MoveTowards(CutsceneCameraTransform.position, CameraPoint[1].transform.position, 10 * Time.deltaTime);
            yield return null;
        }
        yield return TimeManager.s_Wait_2s;
        while (Vector2.Distance(CutsceneCameraTransform.position, PlayerObject.transform.position) > 0.001f)
        {
            CutsceneCameraTransform.position = Vector3.MoveTowards(CutsceneCameraTransform.position, PlayerCameraTransform.position, 10 * Time.deltaTime);
            yield return null;
        }
        CutsceneCameraTransform.position = PlayerCameraTransform.position;
        CutsceneCameraCinemachineCamera.Priority = 0;
        Barrier.SetActive(true);
        PlayerCode.IsStop = false;
        UI.SetActive(true);
        Destroy(gameObject);
    }
}
