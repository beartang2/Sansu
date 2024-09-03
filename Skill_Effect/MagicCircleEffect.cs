using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleEffect : MonoBehaviour
{
    public float duration = 1f; // 마법진이 유지되는 시간
    private Quaternion startRotation;
    private Quaternion endRotation;

    private float timer = 0f;

    void Start()
    {
        startRotation = transform.rotation;
        endRotation = startRotation * Quaternion.Euler(0, 180, 0); // 시계 방향 90도 회전
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= duration)
        {
            // 나타나는 동안 시계 방향으로 90도 회전
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, timer / duration);
        }
        else
        {
            // 1초 후 반시계 방향으로 90도 회전하면서 사라짐
            float disappearTimer = timer - duration;
            transform.rotation = Quaternion.Lerp(endRotation, startRotation, disappearTimer / duration);

            if (disappearTimer >= duration)
            {
                Destroy(gameObject); // 마법진 파괴
            }
        }
    }
}

