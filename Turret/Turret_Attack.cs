using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Attack : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public float attackRange = 50f;
    public float attackInterval = 1f;
    public int damageAmount = 20;
    public float heightOffset = 1f; // 총알이 발사될 높이 오프셋

    private bool playerInRange = false;
    private bool canAttack = true; // 터렛이 공격 가능한지 여부

    public GameObject[] turretLights;

    private void Update()
    {
        DetectPlayer();

        // 플레이어를 감지하고, 공격이 가능한 상태이며, TurretOff와 TurretOff2가 false인 경우에만 공격합니다.
        if (playerInRange && canAttack && !AreTurretsDisabled())
        {
            Attack();
        }
        else if(AreTurretsDisabled())
        {
            turretLights[0].SetActive(false);
            turretLights[1].SetActive(false);
            turretLights[2].SetActive(false);
        }
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
                playerInRange = false;
                return;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        playerInRange = distanceToPlayer <= attackRange;
    }

    private bool AreTurretsDisabled()
    {
        Orb orbScript = FindObjectOfType<Orb>();
        Orb2 orbScript2 = FindObjectOfType<Orb2>();

        if (orbScript != null && orbScript2 != null)
        {
            // 터렛이 공격을 멈춰야 하는지 여부를 반환합니다.
            return orbScript.TurretOff && orbScript2.TurretOff2;
        }

        return false; // 어떤 경우에도 공격을 멈추지 않음
    }

    private void Attack()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab is not assigned.");
            return;
        }

        Vector3 shootDirection = (player.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(shootDirection);

        Vector3 spawnPosition = transform.position + Vector3.up * heightOffset;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, rotation);
        // 탄환에 대한 추가 설정 (예: 탄속, 이펙트 등)

        // 공격 간격을 준수하기 위해 공격 후 공격 가능 상태를 false로 설정
        canAttack = false;
        Invoke("ResetAttack", attackInterval);
    }

    // 공격 가능 상태를 재설정하는 함수
    private void ResetAttack()
    {
        canAttack = true;
    }
}