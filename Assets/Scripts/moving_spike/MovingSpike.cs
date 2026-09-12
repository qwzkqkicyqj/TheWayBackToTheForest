    using System;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    int _i;
    public float Speed = 3f;
    public Transform[] Point;
    public Transform Transform;
    float _q = 0;
    void Start()
    {
        Application.targetFrameRate = 60;
        transform.position = Point[0].position;
    }


    private void Update()
    {
        _q += Time.deltaTime * 500;
        if (Vector2.Distance(transform.position, Point[_i].position) < 0.01f) //현재 플랫폼의 위치와 포인트 지점의 위치가 거의 비슷할 때
        {
            _i++;
            if (_i == Point.Length)
            {
                _i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, Point[_i].position, Speed * Time.deltaTime); //플랫폼 점차 증가
        Transform.rotation = Quaternion.Euler(0, 0, _q % 360f);
    }
}
