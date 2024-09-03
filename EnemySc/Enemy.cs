using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Transform _transform; //enemy의 transform을 받을 변수
    private Transform playerTransform; // player의 transform을 받을 변수
    private NavMeshAgent nvAgent; //NavMeshAgent컴포넌트를 받을 변수

    public GameObject Player; //player
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
        _transform = this.gameObject.GetComponent<Transform>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
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
        _hp = 100;
        SetMaxHealth(_hp);
    }

    void Update()
    {
        Dist = Vector3.Distance(playerTransform.position, _transform.transform.position);

        if (Dist < 10)
        {
            nvAgent.destination = playerTransform.position;
        }

        
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
