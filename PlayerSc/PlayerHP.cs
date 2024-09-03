using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Slider playerHpBar;
    private float playerHp;

    private float healRate = 1.5f; // 1초당 회복되는 양

    private bool isHealing = false;

    void Start()
    {
        
    }
    public float Hp// 선언 먼저.
    {
        get => playerHp;
        private set => playerHp = Math.Clamp(value, 0, playerHp);
    }
    private void Awake()
    {
        playerHp = 100;
        SetMaxHealth(playerHp);
    }
    private void Update()
    {
        PlayerDeath();
    }
    public void GetDamage(int damage)
    {
        float getDamageHp = Hp - damage;
        Hp = getDamageHp;
        playerHpBar.value = Hp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Debug.Log("아야! n 데미지를 입었다!");
            GetDamage(5);
            if (isHealing)
            {
                Debug.Log("힐링 멈춤");
                StartCoroutine(HealDelay(4f));
                isHealing = false;
            }
        }
    }
    public void SetMaxHealth(float health)
    {
        playerHpBar.maxValue = health;
        playerHpBar.value = health;
    }
    public void PlayerDeath()
    {
        if (Hp == 0)
        {
            Debug.Log("사망");
            gameObject.SetActive(false);
            //Particle = true;
        }
    }

    // 자동 회복 코루틴
    private IEnumerator HealOverTime()
    {
        StopCoroutine("HealDelay");
        while (Hp < 100 && isHealing)
        {
            if (Hp == 100)
            {
                //StopCoroutine(HealOverTime());
                isHealing = false;
            }
            else
            {
                yield return new WaitForSecondsRealtime(2f);
                float healingHp = playerHp + healRate;
                playerHp = Mathf.Clamp(healingHp, 0f, 100f);
                playerHpBar.value = playerHp;
                Debug.Log("오토 힐링 가동 중. 현재 체력 : " + playerHp);
            }

            /*
            if (!isHealing)
            {
                StartCoroutine(HealDelay(4f));
            }
            else
            {
                

            }*/
        }
        
    }

    // 일정 시간 후에 자동 회복 코루틴을 시작
    private IEnumerator HealDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        isHealing = true;
        StartCoroutine(HealOverTime());
    }
}
