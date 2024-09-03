using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    public GameObject _Enemy; //enemy
    public float Dist; //player와 enemy 사이의 거리를 받을 변수

    [SerializeField]
    private Slider _hpBar;
    public int _hp;

    public ParticleSystem EnemyDie;
    //private bool Particle;// 파티클
    public int deathCnt = 0;

    void Start()
    {
        _hpBar.interactable = false;
        //Particle = false;
    }
    public int Hp// 선언 먼저.
    {
        get => _hp;
        private set => _hp = Math.Clamp(value, 0, _hp);
    }

    private void Awake()
    {
        SetMaxHealth(_hp);
        Debug.Log(_hp);
    }

    void Update()
    {      
        /* 데미지
        if(Dist < 3 && Input.GetMouseButtonDown(0))
        {
            GetDamage(25);
        }
        */
        EnemyDeath();

        /* 파티클
        if(Particle)
        {
            EnemyDie.Play();
        }
        */
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sword"))
        {
            //Debug.Log(damage);
            GetDamage(10);
        }
    }

    public void SetMaxHealth(int health)
    {
        _hpBar.maxValue = health;
        _hpBar.value = health;
    }

    public void GetDamage(int damage)
    {
        int getDamageHp = Hp - damage;
        Hp = getDamageHp;
        _hpBar.value = Hp;
    }

    public void EnemyDeath()
    {
        if(_hp == 0)
        {
            gameObject.SetActive(false);
            deathCnt++;
            //Particle = true;
        }
    }
    
}
