using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinalBoss : MonoBehaviour
{

    public Transform playerTransform;
    private Transform bossTransform;

    private Rigidbody bossRigidbody;
    public float jumpForce = 3;

    private float rotSpeed = 10.0f;

    public Animator anim;

    private float distanceToPlayer;

    bool atkCooldown = false;

    public float attackInterval = 5.0f; // 랜덤으로 패턴 뽑을 쿨타임
    private float timeSinceLastAttack;

    public bool isJumping; // 현재 점프 중인지를 확인할 변수
    bool jumpCooldown = false; // 점프 쿨다운을 나타내는 변수

    private bool isAtk = false;
    private bool isJumpAtk = false;

    [SerializeField]
    BoxCollider swordCol; // 검 콜라이더

    public GameObject rangeObject;
    BoxCollider rangeCollider;
    public GameObject capsul; // 소환할 오브젝트

    private Coroutine respawnCoroutine; // 코루틴을 제어할 변수

    public newPlayerHP playerHp;

    public Scene_DialogSystem sceneLog;


    private void Awake()
    {
        bossRigidbody = GetComponent<Rigidbody>();
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    Vector3 Return_RandomPosition(float yPosition)
    {
        Vector3 originPosition = rangeObject.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 randomPosition = new Vector3(range_X, yPosition, range_Z);

        Vector3 respawnPosition = originPosition + randomPosition;
        return respawnPosition;
    }

    void Start()
    {
        bossTransform = transform;
        timeSinceLastAttack = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(playerTransform.position, bossTransform.position);
        if (distanceToPlayer <= 20 && !isJumping)
        {
            BossToPlayer();
        }

        timeSinceLastAttack += Time.deltaTime;

        if (distanceToPlayer < 10 && timeSinceLastAttack >= attackInterval)
        {
            PerformRandomAttack();
            timeSinceLastAttack = 0f;
        }
    }

    public void PerformRandomAttack()
    {
        int randomAttack = Random.Range(0, 3);

        switch (randomAttack)
        {
            case 0:
                AttackPattern1();
                break;
            case 1:
                AttackPattern2();
                break;
            case 2:
                AttackPattern3();
                break;
            default:
                Debug.Log("알 수 없는 공격 패턴입니다.");
                break;
        }
    }

    public void AttackPattern1()
    {
        isJumpAtk = true;
        anim.SetBool("JumpAtk", true);
        anim.SetBool("FindPlayer", false); // 보스 걸어다니는 애니메이션 비활성화
        Debug.Log("보스가 공격 패턴 1을 사용합니다.");
        bossRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프
        StartCoroutine(ResetAttackTrigger());
    }

    public void AttackPattern2()
    {
        anim.SetBool("FindPlayer", false); // 보스 걸어다니는 애니메이션 비활성화
        Debug.Log("보스가 공격 패턴 2를 사용합니다.");

        // 코루틴 시작
        if (respawnCoroutine == null)
        {
            respawnCoroutine = StartCoroutine(RandomRespawn_Coroutine());
        }

        // 10초 후 코루틴 중지
        StartCoroutine(StopRespawnCoroutineAfterDelay(10f));
    }

    public void AttackPattern3()
    {
        anim.SetBool("FindPlayer", false); // 보스 걸어다니는 애니메이션 비활성화
        BossBaseAttack();
        Debug.Log("보스가 공격 패턴 3를 사용합니다.");
    }

    public void BossToPlayer() // Boss가 플레이어를 따라가게 하는 함수
    {
        Vector3 tarPos = playerTransform.position;
        tarPos.y = transform.position.y;
        Vector3 dirRot = tarPos - transform.position;

        float distanceToPlayer = dirRot.magnitude;  // 플레이어와의 거리 계산
        float minDistance = 1.4f;  // 플레이어와의 최소 거리

        anim.SetBool("FindPlayer", true);

        // 보스가 플레이어와의 최소 거리보다 멀리 있을 때만 이동
        if (distanceToPlayer > minDistance)
        {
            Quaternion tarRot = Quaternion.LookRotation(dirRot);
            transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);

            // 이동할 거리를 계산하여 최소 거리보다 더 멀리 이동하지 않도록 조정
            Vector3 moveAmount = dirRot.normalized * 5f * Time.deltaTime;
            if (distanceToPlayer - moveAmount.magnitude < minDistance)
            {
                moveAmount = dirRot.normalized * (distanceToPlayer - minDistance);
            }

            transform.position += moveAmount;
        }
    }

    public void BossBaseAttack()
    {
        anim.SetBool("FindPlayer", false);
        anim.SetBool("attack", true);
        StartCoroutine(SwingSword());
        isAtk = true;
        StartCoroutine(ResetAttackTrigger());
    }

    public void Jump()
    {
        bossRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프
        isJumpAtk = true;
        anim.SetBool("FindPlayer", false);
        anim.SetBool("JumpAtk", true);
        StartCoroutine(ResetAttackTrigger());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isJumpAtk)
            {
                playerHp.GetDamage(35);
                isJumpAtk = false;
            }
            
            if (isAtk)
            {
                playerHp.GetDamage(15);
                isAtk = false;
            }
        }
    }

    IEnumerator SwingSword()
    {
        //1
        yield return new WaitForSeconds(1.0f);// 0.3초 대기
        swordCol.enabled = true;// 콜라이더 활성화

        //2
        yield return new WaitForSeconds(0.6f);// 0.3초 대기
        swordCol.enabled = false;// 콜라이더 비활성화

        //공격 이펙트 추가 가능

        yield break;
    }

    IEnumerator ResetAttackTrigger()
    {
        // 공격 애니메이션 시간 동안 대기
        yield return new WaitForSeconds(0.3f); // 공격 애니메이션 시간이 지나면
        anim.SetBool("JumpAtk", false);
        anim.SetBool("FindPlayer", true);
        anim.SetBool("attack", false);

        // 다음 공격이 가능한 상태로 만들기 위해 약간의 지연 시간을 추가
        //yield return new WaitForSeconds(0.5f); // 애니메이션 상태가 업데이트될 시간을 확보

        StartCoroutine(StartAttackCooldown()); // 공격 쿨다운 시작
        StartCoroutine(StartJumpCooldown());
    }

    IEnumerator StartAttackCooldown()
    {
        atkCooldown = true;
        yield return new WaitForSeconds(5.0f);
        atkCooldown = false;
        isAtk = false;
        anim.SetBool("attack", false);
    }

    IEnumerator StartJumpCooldown()
    {
        jumpCooldown = true; // 점프 쿨다운 시작
        yield return new WaitForSeconds(5.0f); // 5초 대기
        jumpCooldown = false; // 점프 쿨다운 종료
        isJumpAtk = false;
        anim.SetBool("JumpAtk", false);
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.7f);

            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            float yPosition = 20.0f;
            GameObject instantCapsul = Instantiate(capsul, Return_RandomPosition(yPosition), Quaternion.identity);
        }
    }

    IEnumerator StopRespawnCoroutineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }
}
