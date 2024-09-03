using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 16f;
    public float deleteRange = 100f; // 총알이 사라지는 범위
    public float deleteTime = 3f;
    public float delTime;

    private void Update()
    {
        // 정면 방향으로 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 총알 삭제 조건 체크
        CheckDeleteCondition();
    }

    private void CheckDeleteCondition()
    {
        delTime += Time.deltaTime;
        //if (transform.position.magnitude > deleteRange)
        if (delTime >= deleteTime)
        {
            Debug.Log("삭제");
            // 총알 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 데미지 적용
            newPlayerHP playerHP = other.GetComponent<newPlayerHP>();
            if (playerHP != null)
            {
                Debug.Log("데미지 입음25");
                playerHP.GetDamage(25);
            }

            // 총알 삭제
            Destroy(gameObject);
        }
        else if(other.name == "TurretStatue") // 포탑인 경우 통과
        {
            return;
        }
        else // 다른 물체에 부딪히면
        {
            Destroy(gameObject);
        }
    }
}
