using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sansu_Interaction : MonoBehaviour
{
    // 플레이어 오브젝트
    public GameObject player;

    // 플레이어와 상호작용할 때 사용할 키
    public KeyCode interactionKey = KeyCode.E;

    // 회전할 각도
    public float rotationAngle = 90f;

    // 플레이어와 상호작용할 수 있는 거리
    public float interactionDistance = 4f;

    // MonsterKill 값이 5 이상인지 확인할 변수
    private bool canInteract = false;

    private void Update()
    {
        // 상호작용 키를 눌렀을 때
        if (Input.GetKeyDown(interactionKey))
        {
            // 특정 조건을 만족하는지 확인
            if (canInteract)
            {
                // 주변의 Statue를 찾아서 상호작용
                Collider[] colliders = Physics.OverlapSphere(player.transform.position, interactionDistance);

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Statue"))
                    {
                        // Statue 오브젝트와 플레이어의 거리를 계산
                        float distance = Vector3.Distance(player.transform.position, collider.transform.position);

                        // 일정 범위 내에 있는 경우 상호작용
                        if (distance <= interactionDistance)
                        {
                            // Statue 오브젝트의 회전을 증가시킴
                            collider.transform.Rotate(Vector3.up, rotationAngle);
                        }
                    }
                }
            }
        }
    }

    // 다른 스크립트에서 호출하여 MonsterKill 값을 설정할 수 있는 함수
    public void SetMonsterKill(int killCount)
    {
        // 조건을 만족하는지 확인하고 canInteract 값을 설정
        canInteract = (killCount >= 5);
    }
}
