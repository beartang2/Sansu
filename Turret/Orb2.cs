using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb2 : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 4f; // 플레이어 감지 범위

    public bool TurretOff2 = false; // 외부에서 접근 가능한 public bool 변수
    Animator buttonAnimator; // 버튼의 애니메이터 컴포넌트

    private void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                return;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            if (Input.GetKeyDown(KeyCode.E)) // E 키 감지
            {
                ActivateButton();
            }
        }
    }

    private void ActivateButton()
    {
        // 애니메이션 트리거를 활성화하여 애니메이션 실행
        if (buttonAnimator != null)
        {
            buttonAnimator.SetTrigger("ButtonPress"); // 'Activate'는 애니메이션 트리거 이름입니다.
        }

        // TurretOff1 변수를 true로 설정
        TurretOff2 = true;
    }
}
