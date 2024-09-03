using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class PlayerController : MonoBehaviour
{
    public float speed = 8f;    //8에서 8f로 수정됨

    //달리기
    private float normalSpeed; // 원래의 속도
    private float runSpeed; // 뛰는 중인 속도

    float hAxis;
    float vAxis;
    public bool isAtk1 = false;// 일반공격 (좌)
    //bool atk2;// 강공격 (우)
    bool isAtk1Ready = true;// 일반공격 준비 완료 (좌)
    //bool isAtk2Ready = true;// 강공격 준비 완료 (우)
    public static bool skillKey = false; // 스킬키

    [SerializeField]
    float atk1Delay = 1f;// 일반공격 딜레이

    PlayerAttack currentWeapon;
    Sansu_SkillManager skill;

    Vector2 moveVec;    //앞뒤좌우 이동시 Vector3이 아닌 Vector2 사용!

    private Transform cameraTransform;
    private float rotationV;    //회전 속도
    [SerializeField]
    private float smoothRotationTime = 0.25f;   //회전 시간

    //플레이어 점프
    public float jumpForce = 3;
    private bool IsJumping;
    Rigidbody player;
    bool jumpKey;

    //구르기
    bool isDodge;   //회피중인가?

    private float dodgeCooldown = 3f; // 회피 쿨다운 시간
    private float lastDodgeTime = -9999f; // 마지막 회피 시간

    Animator anim;

    bool sDown;  //달리기 키를 눌렀는지 확인

    public bool isInputEnabled = true; // 키 입력 제한 변수

    void Start()
    {
        cameraTransform = Camera.main.transform;

        currentWeapon = GetComponent<PlayerAttack>();
        skill = GetComponent<Sansu_SkillManager>();

        if (currentWeapon == null)
        {
            Debug.LogError("Attack component is missing!");
        }

        player = GetComponent<Rigidbody>();
        IsJumping = false;

        normalSpeed = speed; // 원래의 속도 설정
        runSpeed = speed * 2f; // 뛰는 중인 속도 설정
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        GetKey();
        Jump();
        Run();
        Dodge();
        UseWeapon();
        Animation();
    }

    //  키 입력
    void GetKey()
    {
        if (!isInputEnabled)
        {
            return;
        }
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jumpKey = Input.GetKeyDown(KeyCode.Space);
        isAtk1 = Input.GetMouseButtonDown(0);
        skillKey = Input.GetKeyDown(KeyCode.Q);
        sDown = Input.GetKey(KeyCode.LeftShift);   // 왼쪽 쉬프트 눌렀을 때 sRun 활성화
    }

    // 이동 구현
    void Movement()
    {
        //플레이어 이동
        moveVec = new Vector2(hAxis, vAxis).normalized;

        if(!isAtk1Ready)
        {
            moveVec = Vector2.zero; // 초기화
        }
        

        if (moveVec != Vector2.zero)
        {
            //플레이어가 이동하는 방향에 따라 회전
            //Atan2는 라디안을 반환하는 친구, 각도로 바꿔주기 위해 Mathf.Rad2Deg 곱해줌 + 카메라 각도
            float rotation = Mathf.Atan2(moveVec.x, moveVec.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            //카메라 부드럽게 회전
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationV, smoothRotationTime);

            //카메라가 보는 방향 이동
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    // 쉬프트 키를 눌렀을 때 속도를 변경하는 함수
    void Run()
    {
        // Shift 키를 누르면 뛰는 중인 속도로 변경
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetSpeed(runSpeed);
        }

        // Shift 키를 떼면 원래의 속도로 변경
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SetSpeed(normalSpeed);
        }
    }

    // 속도 설정 함수
    void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    //회피
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && moveVec != Vector2.zero 
                && !IsJumping && !isDodge && Time.time - lastDodgeTime >= dodgeCooldown)
        {
            speed *= 4;
            isDodge = true;
            anim.SetTrigger("IsDodge");     //회피를 사용할 때, 트리거 설정
            Invoke("DodgeOut", 0.25f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.25f;
        isDodge = false;

        lastDodgeTime = Time.time; // 마지막 회피 시간 갱신
    }

    // 점프 구현
    void Jump()
    {
        if (jumpKey && !IsJumping)  // 점프 키 입력시 && 점프 중이 아닐 때
        {
            //Debug.Log("점프중");
            IsJumping = true;   // 점프 중
            player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프
            anim.SetBool("IsJump", true);
            anim.SetTrigger("doJump");

            Invoke("Land", 0.85f);
        }
    }

    //착지 애니메이션 용 함수 따로 실행해야 해서 빼둠
    void Land()
    {
        anim.SetBool("IsJump", false);
    }

    //  애니메이션 구현
    void Animation()
    {
        //  애니메이션
        anim.SetBool("isWalk", moveVec != Vector2.zero);    // 가만히 있을 때, 대기 모션
        anim.SetBool("IsJump", IsJumping);    // IsJumping 활성화 되었을 때, 점프 모션
        if(hAxis != 0 || vAxis != 0)
        {
            anim.SetBool("IsRun", sDown);    // sRun 활성화 되었을 때, 달리기 모션 
        }
        else
        {
            anim.SetBool("IsRun", false);    // sRun 활성화 되었을 때, 달리기 모션 
        }

        if (isAtk1)
        {
            anim.SetTrigger("IsAtk1");     // 기본 공격
        }
        else
        {
            return;
        }
    }

    // 무기 사용
    void UseWeapon()
    {
        if (isAtk1 && !skill.isGamePaused)
        {
            //Debug.Log("leftClick");
            currentWeapon.Atk(atk1Delay);// Attack 스크립트의 Atk()함수 호출
        }
        /*
        if(atk2 && !isGamePaused)
        {
            Debug.Log("rightClick");
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // 땅에 닿았으면
        {
            //Debug.Log("땅에 닿음 점프중이 아님");
            IsJumping = false;  // 점프 중이 아님
        }
    }
    
    public void SetInputEnabled(bool enabled)
    {
        isInputEnabled = enabled;
    }
}