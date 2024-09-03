using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Check : MonoBehaviour
{
    // Enum for clock directions
    public enum ClockDirection
    {
        OneOClock,
        FourOClock,
        SevenOClock,
        TwelveOClock
    }

    [System.Serializable]
    public class Statue
    {
        public string name;
        public Transform transform;
        public ClockDirection correctDirection;
    }

    public Statue[] statues;
    public Transform playerTransform;
    public float detectionRadius = 5.0f;

    // Monster spawn related variables
    public GameObject rangeObject;
    BoxCollider rangeColider;
    [SerializeField]
    private int slimeCnt;
    public GameObject slime;
    public int spawnCnt = 0;

    [SerializeField]
    private GameObject mainQuest;
    [SerializeField]
    private GameObject BossStagePortal;

    public Scene_DialogSystem sceneLog;

    private bool hadSaying = false;

    private void Awake()
    {
        rangeColider = rangeObject.GetComponent<BoxCollider>();
    }

    private void Start()
    {
        mainQuest.SetActive(false); // 메인방 문제 비활성화
        BossStagePortal.SetActive(false); // 보스방 포탈 비활성화
        spawnCnt = 0;
        sceneLog.ShowDialogue(); // 산슈 대사 시작
    }

    void Update()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) <= detectionRadius && Input.GetKeyDown(KeyCode.F) && !CanInteractWithPuzzle())
        {
            // 아직 해결해야 할 것이 남아있어.
            sceneLog.ONOFF(true);
            sceneLog.txt_Name.text = "산슈";
            sceneLog.txt_Dialogue.text = "아직 해결해야 할 것이 남아있어.";
        }

        if (!hadSaying && CanInteractWithPuzzle())
        {
            hadSaying = true;
            mainQuest.SetActive(true); // 메인방 문제 표시
            // ... 도형과 숫자.. 석상? 무엇을 의미하는 걸까? 
            sceneLog.ONOFF(true);
            sceneLog.txt_Name.text = "산슈";
            sceneLog.txt_Dialogue.text = "... 도형과 숫자.. 석상? 무엇을 의미하는 걸까?";
        }

        if (CanInteractWithPuzzle() && Vector3.Distance(playerTransform.position, transform.position) <= detectionRadius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (CheckAllStatuesCorrect())
                {
                    // 맞혔다! 방으로 가는 길이 열렸어!
                    sceneLog.ONOFF(true);
                    sceneLog.txt_Name.text = "산슈";
                    sceneLog.txt_Dialogue.text = "맞혔다! 방으로 가는 길이 열렸어!";

                    BossStagePortal.SetActive(true);
                }
                else
                {
                    // 제길, 이게 아닌가 보군. 슬라임을 잡고 다시 시도해보자.
                    sceneLog.ONOFF(true);
                    sceneLog.txt_Name.text = "산슈";
                    sceneLog.txt_Dialogue.text = "으악! 이게 아닌가 봐.";

                    spawnCnt = 0;       // 여러번 사용 가능하게 초기화
                    StartCoroutine(RandSpawn());
                }
            }
        }
    }

    bool CanInteractWithPuzzle()
    {
        GameObject[] StatueLights = GameObject.FindGameObjectsWithTag("Light");
        if (StatueLights.Length > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool CheckAllStatuesCorrect()
    {
        foreach (Statue statue in statues)
        {
            if (!IsStatueCorrect(statue))
            {
                return false;
            }
        }
        return true;
    }

    bool IsStatueCorrect(Statue statue)
    {
        float yRotation = statue.transform.eulerAngles.y;
        switch (statue.correctDirection)
        {
            case ClockDirection.OneOClock:
                return Mathf.Approximately(yRotation, 210);  // 1 세모
            case ClockDirection.FourOClock:
                return Mathf.Approximately(yRotation, 300);  // 4 네모
            case ClockDirection.SevenOClock:
                return Mathf.Approximately(yRotation, 30);   // 7 원
            case ClockDirection.TwelveOClock:
                return Mathf.Approximately(yRotation, 180);  // 12 별
        }
        return false;
    }

    Vector3 RandomPos()
    {
        // Initial position
        Vector3 originPos = rangeObject.transform.position;

        // bounds is the size of the collider
        float rangeX = rangeColider.bounds.size.x;
        float rangeZ = rangeColider.bounds.size.z;

        // Random range
        rangeX = Random.Range((rangeX / 2) * (-1), rangeX / 2);
        rangeZ = Random.Range((rangeZ / 2) * (-1), rangeZ / 2);

        // Random spawn position
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
