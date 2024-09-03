using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider rangeColider;
    [SerializeField]
    private int slimeCnt;
    public GameObject slime;
    public int spawnCnt = 0;

    private void Start()
    {
        spawnCnt = 0;
        StartCoroutine(RandSpawn());
    }
    private void Awake()
    {
        rangeColider = rangeObject.GetComponent<BoxCollider>();
    }


    Vector3 RandomPos()
    {
        //처음 위치
        Vector3 originPos = rangeObject.transform.position;

        //bounds는 콜라이더 사이즈를 가져옴
        float rangeX = rangeColider.bounds.size.x;
        float rangeZ = rangeColider.bounds.size.z;

        //랜덤의 범위
        rangeX = Random.Range((rangeX / 2) * (-1), rangeX / 2);
        rangeZ = Random.Range((rangeZ / 2) * (-1), rangeZ / 2);

        //랜덤 위치 스폰
        Vector3 randPos = new Vector3(rangeX, 0f, rangeZ);
        Vector3 respawnPos = originPos + randPos;

        return respawnPos;
    }
    IEnumerator RandSpawn()
    {
        while (spawnCnt < slimeCnt)
        {
            GameObject instantSlime = Instantiate(slime, RandomPos(), Quaternion.identity);
            spawnCnt++;

            yield return new WaitForSeconds(1f);
        }
    }
}