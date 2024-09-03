using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class Boss : MonoBehaviour
{
    public GameObject jumpEffectPrefab; // 표시할 이미지의 프리팹
    private GameObject jumpEffectInstance; // 생성된 이미지 오브젝트를 저장하기 위한 변수

    private Rigidbody bossRigidbody;
    public float jumpForce = 3;

    public GameObject ground; // Plane
    public float jumpEffectScale = 10.0f; // 이미지 크기

    public bool isJumping; //현재 점프중인지를 확인할 변수
    public bool isBressing; // 현재 브레스를 쏘는 중인지 확인할 변수

    public GameObject player;
    private Transform playerTransform;
    private Transform bossTransform; // 보스 위치 정보를 받을 변수
    private float distanceToPlayer;

    private newPlayerHP playerHpSc; // 데미지 적용 스크립트

    //bool conditionMet = false; // 점프 조건이 만족되었는지를 나타내는 변수
    //bool conditionMet2 = false; // 브레스 조건이 만족되었는지를 나타내는 변수
    bool jumpCooldown = false; // 점프 쿨다운을 나타내는 변수
    bool bressCooldown = false; // 브레스 쿨다운을 나타내는 변수

    private float rotSpeed; //보스 고개 돌리는 속도

    public Animator anim;

    public ParticleSystem Bress; //브레스 파티클

    public DecalProjector BossJumpImage; //보스피격범위

    public float attackInterval = 10.0f; // 랜덤으로 패턴 뽑을 쿨타임
    private float timeSinceLastAttack;

    public Scene_DialogSystem sceneLog;
    public GameObject miniBossImg; // 중간보스 실루엣
    private bool hadSaying = false;

    private void Awake()
    {
        bossRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rotSpeed = 10.0f;
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerHpSc = GameObject.FindWithTag("Player").GetComponent<newPlayerHP>();
        bossTransform = transform;
        distanceToPlayer = Vector3.Distance(playerTransform.position, bossTransform.position);
        //StartCoroutine(CheckConditionPeriodically()); //보스와 플레이어 간의 거리를 구하기
        BossJumpImage.enabled = false; //처음에 프로젝터 비활성화
        timeSinceLastAttack = 0f;
        miniBossImg.SetActive(false);
    }

    void Update()
    {
        /*
        if (conditionMet && !isJumping && !jumpCooldown && !isBressing) // 플레이어와의 거리가 11이하일 때 / 점프중이 아닐 때 / 점프 쿨타임이 돌았을 때 / 브레스 시전중이 아닐 때
        {
            Jump(); //점프
            StartCoroutine(StartJumpCooldown()); //점프 쿨타임 
        }

        if (conditionMet2 && !isJumping && !bressCooldown) // 플레이어와의 거리가 11부터 15미만일 때 / 점프중이 아닐 때 / 브레스 쿨타임이 돌았을 때
        {
            Skill_Bress(); 
            StartCoroutine(StartBressCooldown());
        }
        */
        timeSinceLastAttack += Time.deltaTime;

        distanceToPlayer = Vector3.Distance(playerTransform.position, bossTransform.position);

        if(!hadSaying && distanceToPlayer < 20)
        {
            hadSaying = true;
            // 중간 보스 조우 시 대사
            miniBossImg.SetActive(true);
            sceneLog.ONOFF(true);
            sceneLog.txt_Name.text = "소울이터";
            sceneLog.txt_Dialogue.text = "감히 보스의 물건을 훔친 것도 모자라, 보스를 해칠 계획을 꾸미고 있다니.. " +
                "여기서 끝을 내주마!";
        }
        if (hadSaying && Input.GetKeyDown(KeyCode.F))
        {
            miniBossImg.SetActive(false);
        }
        if (!isJumping && !isBressing && distanceToPlayer < 17) 
        {
            BossToPlayer();

            if (timeSinceLastAttack >= attackInterval)
            {
                PerformRandomAttack();
                timeSinceLastAttack = 0f;
            }
        }
        
    }

    public void BossToPlayer() //Boss가 플레이어를 따라가게 하는 함수
    {
        anim.SetBool("Bress", false); // 브레스 애니메이션 비활성화

        Vector3 tarPos = playerTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        float distanceWithPlayer = dirRot.magnitude;  // 플레이어와의 거리 계산
        float minDistance = 2.4f;  // 플레이어와의 최소 거리

        anim.SetBool("FindPlayer", true);

        // 보스가 플레이어와의 최소 거리보다 멀리 있을 때만 이동
        if (distanceWithPlayer > minDistance)
        {
            Quaternion tarRot = Quaternion.LookRotation(dirRot);
            transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

            // 이동할 거리를 계산하여 최소 거리보다 더 멀리 이동하지 않도록 조정
            Vector3 moveAmount = dirRot.normalized * 5f * Time.deltaTime;
            if (distanceWithPlayer - moveAmount.magnitude < minDistance)
            {
                moveAmount = dirRot.normalized * (distanceWithPlayer - minDistance);
            }

            transform.position += moveAmount;
        }
    }

    private void OnCollisionEnter(Collision collision) //점프 후 땅에 닿으면 피격범위 이미지를 없애기 위함 + 콘솔창에 피격 구현
    {
        if (isJumping && collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            BossJumpImage.enabled = false; //보스 피격범위 비활성화

            if (!isJumping && distanceToPlayer < 7)
            {
                playerHpSc.GetDamage(25);
            }
        }
    }

    public void Jump()
    {
        anim.SetBool("Bress", false); // 브레스 애니메이션 비활성화
        bossRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프
        isJumping = true;
        BossJumpImage.enabled = true; //보스 피격범위 활성화
        anim.SetBool("FindPlayer", false); // 보스 걸어다니는 애니메이션 비활성화
    }

    public void Skill_Bress()
    {
        if(!isJumping)
        {
            Vector3 direction = player.transform.position - transform.position; //플레이어 방향 구하기

            //플레이어 방향으로 부드럽게 돌기                                                                   
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

            Bress.Play(); //브레스 파티클 시작
            anim.SetBool("Bress", true); // 브레스 애니메이션 활성화
            //anim.SetBool("Bress", false); // 브레스 애니메이션 비활성화
            isBressing = true;
            anim.SetBool("FindPlayer", false); // 보스 걸어다니는 애니메이션 비활성화
        }
    }

    /*void ShowJumpEffect()
    {
        // 스프라이트 이미지를 표시할 위치를 계산합니다.
        Vector3 spawnPosition = new Vector3(bossTransform.position.x + 14.5f, ground.transform.position.y + 0.1f, bossTransform.position.z - 6);

        // 스프라이트 이미지를 생성하고 표시합니다.
        jumpEffectInstance = Instantiate(jumpEffectPrefab, spawnPosition, Quaternion.Euler(90f, 0f, 0f));

        // 이미지의 크기를 조절합니다.
        jumpEffectInstance.transform.localScale *= jumpEffectScale;
    }*/

    public void PerformRandomAttack()
    {
        int randomAttack = Random.Range(0, 2);

        switch (randomAttack)
        {
            case 0:
                AttackPattern1();
                break;
            case 1:
                AttackPattern2();
                break;
            default:
                Debug.Log("알 수 없는 공격 패턴입니다.");
                break;
        }
    }

    public void AttackPattern1()
    {
        Jump(); // 보스 스크립트 내의 점프 시키는 함수 불러오기
        StartCoroutine(StartJumpCooldown()); //점프 쿨타임 
        Debug.Log("보스가 공격 패턴 1을 사용합니다."); //확인용
    }

    public void AttackPattern2()
    {

        Skill_Bress(); // 보스 스크립트 내의 브레스 쏘는 함수 불러오기
        StartCoroutine(StartBressCooldown()); // 브레스 쿨타임
        Debug.Log("보스가 공격 패턴 2를 사용합니다."); //확인용
    }

    /*
    IEnumerator CheckConditionPeriodically()
    {
        while (true)
        {
            // 플레이어와 보스 사이의 거리를 계산합니다.
            distanceToPlayer = Vector3.Distance(playerTransform.position, bossTransform.position);

            // 거리가 11 이하인 경우 조건을 충족시킵니다.
            if (distanceToPlayer < 11)
            {
                conditionMet = true;
            }
            else
            {
                conditionMet = false;
            }

            if (distanceToPlayer >= 11 &&  distanceToPlayer < 15)
            {
                conditionMet2 = true;
            }
            else
            {
                conditionMet2 = false;
            }

            yield return null; // 다음 프레임까지 대기합니다.
        }
    }
    */

    IEnumerator StartJumpCooldown()
    {
        jumpCooldown = true; // 점프 쿨다운 시작

        yield return new WaitForSeconds(10.0f); // 10초 대기

        jumpCooldown = false; // 점프 쿨다운 종료
    }

    IEnumerator StartBressCooldown()
    {
        anim.SetBool("Bress", false); // 브레스 애니메이션 비활성화
        bressCooldown = true; // 브레스 쿨다운 시작

        yield return new WaitForSeconds(10.0f); // 10초 대기

        bressCooldown = false; // 브레스 쿨다운 종료
        isBressing = false; // 브레스와 점프가 같이 실행되지 않게 하기 위한 변수
    }
}