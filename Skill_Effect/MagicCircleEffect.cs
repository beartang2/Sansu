using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleEffect : MonoBehaviour
{
    public float duration = 1f; // �������� �����Ǵ� �ð�
    private Quaternion startRotation;
    private Quaternion endRotation;

    private float timer = 0f;

    void Start()
    {
        startRotation = transform.rotation;
        endRotation = startRotation * Quaternion.Euler(0, 180, 0); // �ð� ���� 90�� ȸ��
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= duration)
        {
            // ��Ÿ���� ���� �ð� �������� 90�� ȸ��
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, timer / duration);
        }
        else
        {
            // 1�� �� �ݽð� �������� 90�� ȸ���ϸ鼭 �����
            float disappearTimer = timer - duration;
            transform.rotation = Quaternion.Lerp(endRotation, startRotation, disappearTimer / duration);

            if (disappearTimer >= duration)
            {
                Destroy(gameObject); // ������ �ı�
            }
        }
    }
}

