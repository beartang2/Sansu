using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossEnemy_MoveJump : MonoBehaviour
{
    public GameObject jumpEffectPrefab; // 표시할 이미지의 프리팹
    private GameObject jumpEffectInstance; // 생성된 이미지 오브젝트를 저장하기 위한 변수

    private Rigidbody bossRigidbody;
    public float jumpForce = 3;

    public GameObject ground; // Plane
    public float jumpEffectScale = 10.0f; // 이미지 크기
    public bool isJumping; //현재 점프중인지를 확인할 변수

    public GameObject player;
    private Transform playerTransform;
    private Transform bossTransform; // 보스 위치 정보를 받을 변수
    private float distanceToPlayer;

    bool conditionMet = false; // 조건이 만족되었는지를 나타내는 변수
    bool jumpCooldown = false; // 점프 쿨다운을 나타내는 변수
    //bool nvAgentTF = true;

    private float rotSpeed;

    private void Awake()
    {
        bossRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rotSpeed = 2.0f;
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        bossTransform = transform;
        StartCoroutine(CheckConditionPeriodically());
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(playerTransform.position, bossTransform.position);

        if (!isJumping && distanceToPlayer < 12) 
        {
            BossToPlayer();
        }
        
        if (conditionMet && !isJumping && !jumpCooldown)
        {
            Jump();
            StartCoroutine(StartJumpCooldown());
        }

        if (!isJumping)
        {
            Destroy(jumpEffectInstance);
        }
    }

    public void BossToPlayer() //Boss가 플레이어를 따라가게 하는 함수
    {
        Vector3 tarPos = playerTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        Quaternion tarRot = Quaternion.LookRotation(dirRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

        // transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
        transform.position += dirRot.normalized * 3f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) //점프 후 땅에 닿으면 피격범위 이미지를 없애기 위함 + 콘솔창에 피격 구현
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;

            if (distanceToPlayer < 7)
            {
                Debug.Log("아야");
            }
        }
    }

    public void Jump()
    {
        bossRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프
        jumpEffectPrefab.SetActive(true); // 피격 범위 이미지 활성화
        ShowJumpEffect(); // 이미지 생성
        isJumping = true;
    }

    void ShowJumpEffect()
    {
        // 스프라이트 이미지를 표시할 위치를 계산합니다.
        Vector3 spawnPosition = new Vector3(bossTransform.position.x, bossTransform.position.y - 0.4f, bossTransform.position.z);

        // 스프라이트 이미지를 생성하고 표시합니다.
        jumpEffectInstance = Instantiate(jumpEffectPrefab, spawnPosition, Quaternion.Euler(90f, 0f, 0f));

        // 이미지의 크기를 조절합니다.
        jumpEffectInstance.transform.localScale *= jumpEffectScale;
    }

    IEnumerator CheckConditionPeriodically()
    {
        while (true)
        {
            // 플레이어와 보스 사이의 거리를 계산합니다.
            distanceToPlayer = Vector3.Distance(playerTransform.position, bossTransform.position);

            // 거리가 11 이하인 경우 조건을 충족시킵니다.
            if (distanceToPlayer < 6)
            {
                conditionMet = true;
            }
            else
            {
                conditionMet = false;
            }

            yield return null; // 다음 프레임까지 대기합니다.
        }
    }

    IEnumerator StartJumpCooldown()
    {
        jumpCooldown = true; // 점프 쿨다운 시작

        yield return new WaitForSeconds(5.0f); // 5초 대기

        jumpCooldown = false; // 점프 쿨다운 종료
    }
}