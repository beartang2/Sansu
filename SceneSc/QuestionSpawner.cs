using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSpawner : MonoBehaviour
{
    //List<float> problemList = new List<float>();//중복 방지
    public GameObject problem;
    public int spawnCnt = 0;
    public RectTransform canvas;


     
    // Start is called before the first frame update
    void Start()
    {
        spawnCnt = 0;
        StartCoroutine(RandomSpawn());
    }
    Vector2 RandomPos()
    {
        //처음 위치
        Vector3 originPos = new Vector3(0f, 0f, 0f);

        //화면 범위
        float rangeX = Random.Range(0f, 1500f);
        float rangeY = Random.Range(0f, 700f);

        //랜덤 위치 스폰
        Vector3 randPos = new Vector3(rangeX, rangeY, 0f);
        Vector3 respawnPos = originPos + randPos;

        return respawnPos;
    }
    private void Update()
    {
        RandomPos();
    }

    IEnumerator RandomSpawn()
    {
        while(spawnCnt < 3)
        {
            yield return new WaitForSeconds(0.3f);

            GameObject question = Instantiate(problem, RandomPos(), Quaternion.identity, canvas);
            spawnCnt++;
        }
    }
}
