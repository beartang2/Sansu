using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Sansu_Player_Interaction : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 컴포넌트
    public bool killEnemy = false; // 플레이어가 설정할 수 있는 적 오브젝트를 처치했는가?

    public Scene_DialogSystem sceneLog;

    private bool hadSaying = false;

    private GameObject bossEnemy;
    private BossEnemy bossEnemySc;
    private bool isBossLive = false;

    private GameObject campFire;

    private void Start()
    {
        bossEnemy = GameObject.FindGameObjectWithTag("Boss");
        if (bossEnemy != null)
        {
            isBossLive = true;
            bossEnemySc = bossEnemy.GetComponent<BossEnemy>();
        }
        campFire = GameObject.FindGameObjectWithTag("Fire");
    }
    void Update()
    {
        KillCheck();

        // E 키가 눌렸을 때 상호작용 1을 실행
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckInteraction1();
        }
        /*
        // R 키가 눌렸을 때 상호작용 2를 실행
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckInteraction2();
        }
        */
    }


    // 상호작용 1 체크
    void CheckInteraction1()
    {
        GameObject[] statues = GameObject.FindGameObjectsWithTag("Statue"); // 'Statue' 태그를 가진 모든 게임 오브젝트 가져오기

        foreach (GameObject statue in statues)
        {
            Vector3 direction = statue.transform.position - transform.position;
            float distanceToEnemy = direction.magnitude;

            // 적과 플레이어의 거리가 5 이하이고, 적과 Statue의 거리가 5 이하인 경우에만 RotateStatue 함수 호출
            if (distanceToEnemy <= 7f)
            {
                // 죽은 적 오브젝트의 수가 설정된 처치 수 이상일 경우 상호작용 실행
                if (killEnemy)
                {
                    if (!hadSaying) // 대사가 한번만 나오도록
                    {
                        // 석상을 돌릴 수 있게 됐어
                        sceneLog.ONOFF(true);
                        sceneLog.txt_Name.text = "산슈";
                        sceneLog.txt_Dialogue.text = "석상을 돌릴 수 있게 됐어!";

                        hadSaying = true;
                    }

                    RotateStatue(statue);
                }
                else
                {
                    // 아직 몬스터가 다 죽지 않았어.
                    sceneLog.ONOFF(true);
                    sceneLog.txt_Name.text = "산슈";
                    sceneLog.txt_Dialogue.text = "아직 몬스터가 다 죽지 않았어.";
                }
            }
        }

        if (campFire != null)
        {
            Vector3 dirFire = campFire.transform.position - transform.position;
            float distanceToFire = dirFire.magnitude;

            if (distanceToFire <= 5f)
            {
                campFire.SetActive(false);
            }
        }
    }

    /*
    // 상호작용 2 체크
    void CheckInteraction2()
    {
        // 죽은 적 오브젝트의 수가 설정된 처치 수 이상일 경우 상호작용 실행
        if (killEnemy)
        {
            Debug.Log("Interaction activated Statue2!"); // 디버깅 로그 추가

            GameObject[] statues = GameObject.FindGameObjectsWithTag("Statue"); // 'Statue' 태그를 가진 모든 게임 오브젝트 가져오기

            foreach (GameObject statue in statues)
            {
                Vector3 direction = statue.transform.position - transform.position;
                float distanceToEnemy = direction.magnitude;

                // 적과 플레이어의 거리가 5 이하이고, 적과 Statue의 거리가 5 이하인 경우에만 비활성화
                if (distanceToEnemy <= 7f)
                {
                    statue.SetActive(false);
                }
            }
        }
    }*/

    // 죽은 적 오브젝트의 수 가져오기
    public void KillCheck()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject statues = GameObject.FindGameObjectWithTag("Statue");

        // 적 오브젝트의 개수가 0 이하일 경우 죽은 적으로 간주
        if (statues != null && !killEnemy && enemies.Length == 0)
        {
            killEnemy = true;
        }
        else if (statues != null && enemies.Length > 0)
        {
            killEnemy = false;
        }

        if (isBossLive && bossEnemySc.Hp <= 0)
        {
            if (sceneLog.Fcount < sceneLog.partDialogue.Length)
            {
                sceneLog.txt_Dialogue.text = sceneLog.partDialogue[sceneLog.Fcount].partDialogue; // 다음 대사 미리 설정해주기
                sceneLog.txt_Name.text = sceneLog.partDialogue[sceneLog.Fcount].partName;
                sceneLog.ONOFF(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    //Debug.Log(count + ", " + dialogue.Length);
                    sceneLog.NextFinalDialogue(); //다음 대사가 진행됨
                }
            }
            else
            {
                sceneLog.ONOFF(false); //대사가 끝남
                SceneManager.LoadScene(6); // 엔딩씬
            }
        }
    }

    // Statue를 회전하는 함수
    void RotateStatue(GameObject statue)
    {
        // Statue의 y 각도를 30씩 증가시킴
        statue.transform.Rotate(0f, 30f, 0f);

        float yRotation = statue.transform.eulerAngles.y;

        Debug.Log(yRotation);
    }
}