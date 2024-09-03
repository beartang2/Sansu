using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    //private int _hp = 50;  사용안함
    public bool killEnemy = false; // 플레이어가 설정할 수 있는 적 오브젝트를 처치했는가?

    public GameObject[] statueLights;

    void Update()
    {
        KillCheck();

        /*
        if (_hp == 0)
        {
            gameObject.SetActive(false);
            //Particle = true;
        }
        */
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (killEnemy && other.gameObject.CompareTag("sword"))
        {
            //Debug.Log(damage);
            GetDamage(10);
        }
    }
    public void GetDamage(int damage)
    {
        _hp -= damage;
    }*/

    public void KillCheck()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        // 적 오브젝트의 HP가 0 이하일 경우 죽은 적으로 간주
        if (!killEnemy && enemies.Length == 0)
        {
            Debug.Log("All enemy are gone!");
            statueLights[0].SetActive(false);
            statueLights[1].SetActive(false);
            killEnemy = true;
        }
    }
}
