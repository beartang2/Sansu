using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class newPlayerHP : MonoBehaviour
{
    [SerializeField]
    private float playerHp;
    public Slider playerHpBar;

    private float maxPlayerHp = 100f;

    private float lastDamageTime; // 마지막 피해를 받은 시간
    private float healCooldown = 5f; // 자동 회복 쿨다운 시간
    private float healRate = 5f; // 1초당 회복되는 양

    private bool isHealing = true; // 자동 회복 여부를 나타내는 플래그

    private int playerLife = 3; // 플레이어 목숨
    [SerializeField]
    private Image[] deadHearts; // 플레이어 목숨 이미지

    public GameManager gameManager;

    Vector3 firstPlayerPos;

    void Start()
    {
        firstPlayerPos = gameObject.transform.position;
        playerHpBar.interactable = false;
        lastDamageTime = Time.time; // 시작 시간으로 초기화

        foreach (Image deadHeart in deadHearts)
        {
            deadHeart.enabled = false; // 목숨 이미지 초기화
        }
    }
    public float Hp// 선언 먼저.
    {
        get => playerHp;
        set => playerHp = Mathf.Clamp(value, 0, maxPlayerHp); // 최대 체력은 maxPlayerHp로 제한
    }
    private void Awake()
    {
        playerHp = maxPlayerHp;
        SetMaxHealth(playerHp);
    }
    private void Update()
    {
        // 자동 회복 실행 여부를 확인하고 실행
        if (isHealing)
        {
            if (Hp < maxPlayerHp && Time.time >= lastDamageTime + healCooldown)
            {
                HealOverTime();
                // 현재 체력이 최대 체력과 같을 때 "자동 회복 완료!" 출력
                if (Hp == maxPlayerHp)
                {
                    Debug.Log("자동 회복 완료!");
                }
            }
        }

        PlayerDeath();
    }

    public void GetDamage(float damage)
    {
        float getDamageHp = Hp - damage;
        Hp = getDamageHp;
        playerHpBar.value = Hp;

        // 피해를 받았을 때 자동 회복 시작
        /*if (!isHealing)
        {
            StartCoroutine(HealDelay(4f));
        }*/

        lastDamageTime = Time.time; // 피해를 받은 시간 업데이트
        isHealing = false; // 자동 회복 중지
        Invoke("StartHealing", healCooldown); // 일정 시간 후에 자동 회복 시작
        Debug.Log("플레이어가 " + damage + " 의 데미지를 입었습니다. 현재 체력 : " + Hp);
        Debug.Log("5초 후 자동 회복이 시작됩니다. 피격시 다시 5초를 카운트 합니다.");
    }

    public void SetMaxHealth(float health)
    {
        playerHpBar.maxValue = health;
        playerHpBar.value = health;
    }

    private void StartHealing()
    {
        isHealing = true;
    }

    private void HealOverTime()
    {
        Hp += healRate * Time.deltaTime;
        Hp = Mathf.Clamp(Hp, 0, maxPlayerHp); // 회복 후 최대치 초과 방지
        playerHpBar.value = playerHp;
    }

    // 보스 검에 닿은 데미지 처리
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bossSword"))
        {
            GetDamage(20);
        }
        if (other.gameObject.CompareTag("Stone"))
        {
            GetDamage(30);
        }
        /*
        if(other.gameObject.CompareTag("Bress"))
        {
            GetDamage(2);
        }*/
    }

    // 몬스터와 보스에 직접 닿은 데미지 처리
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            GetDamage(5);
        }
        if (collision.gameObject.CompareTag("miniBoss"))
        {
            GetDamage(7);
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            GetDamage(10);
        }
    }

    public void PlayerDeath()
    {
        if(gameObject.transform.position.y < -5f)
        {
            Debug.Log("맵 밖으로 떨어짐");
            gameObject.transform.position = firstPlayerPos; // 처음 플레이어 위치로
        }
        if (Hp == 0 && playerLife > 0)
        {
            Debug.Log("사망");
            playerLife--;
            deadHearts[playerLife].enabled = true;
            Debug.Log(playerLife);
            //Particle = true;
            gameManager.CoroutineRelivePlayerAfterDelay();
        }
        if(playerLife <= 0)
        {
            Debug.Log("목숨 0");
            deadHearts[playerLife].enabled = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 시작
        }
    }
}
