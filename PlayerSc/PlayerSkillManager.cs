using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour
{
    //정지
    public bool isGamePaused = false;

    // 문제 생성 스킬
    public GameObject problemUI;
    public Text problemText;
    public InputField answerInput;
    public Text answerResultText;
    public Button checkButton;

    int operatorIndex;

    public bool isProblemEnd = false;

    private bool isAnswered = false;
    private int num1;
    private int num2;

    public Enemy enemy; // Enemy 스크립트를 가진 게임 오브젝트에 대한 참조
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        answerResultText.gameObject.SetActive(false);

        checkButton.onClick.AddListener(CheckAnswer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isProblemEnd) // 문제가 종료되지 않은 경우에만 문제 표시
            {
                ShowProblem();
                playerController.SetInputEnabled(false);

                if (!isGamePaused)
                {
                    PauseGame();
                }
            }
        }
    }

    //정답 확인 기능
    public void CheckAnswer()
    {
        if (!isAnswered)
        {

            int userAnswer;
            if (int.TryParse(answerInput.text, out userAnswer))
            {

                int correctAnswer = 0;

                // 연산 기호에 따라 정답 계산
                switch (operatorIndex)
                {
                    case 0:
                        correctAnswer = num1 + num2;
                        break;
                    case 1:
                        correctAnswer = num1 - num2;
                        break;
                    case 2:
                        correctAnswer = num1 * num2;
                        break;
                    case 3:
                        correctAnswer = num1 / num2;
                        break;
                }

                if (userAnswer == correctAnswer)
                {
                    answerResultText.text = "정답입니다!";
                    ApplyDamageToEnemies(40); // 정답인 경우 적에게 40의 데미지 적용
                }
                else
                {
                    answerResultText.text = "틀렸습니다!";
                    ApplyDamageToEnemies(10); // 오답인 경우 적에게 10의 데미지 적용
                }

                // 문제 해결 후 문제가 더 이상 나오지 않도록 isProblemEnd를 false로 설정
                isProblemEnd = false;

                // 정답 결과 텍스트가 출력된 후 3초 후에 문제 UI 비활성화 및 게임 재개
                StartCoroutine(DeactivateProblemUIDelayed());
                isAnswered = true;
                answerResultText.gameObject.SetActive(true);
            }
        }
        isAnswered = false;
    }

    // 적에게 데미지 적용 함수
    void ApplyDamageToEnemies(int damage)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
        {
            // 플레이어와 적의 위치 차이 계산
            Vector3 direction = enemy.transform.position - transform.position;
            float distance = direction.magnitude;

            // 적과 플레이어의 거리가 5 이하인 경우에만 데미지 적용
            if (distance <= 5f)
            {
                // 적에게 데미지 적용
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.GetDamage(damage);
                }
            }
        }
    }

    // UI 비활성화 및 딜레이 주기
    IEnumerator DeactivateProblemUIDelayed()
    {
        yield return new WaitForSecondsRealtime(3f); // 3초 대기
        DeactivateProblemUI(); // 문제 UI 비활성화
        ResumeGame(); // 게임 재개
    }

    // 문제 생성 스킬
    void ShowProblem()
    {
        // 문제 UI 활성화
        problemUI.SetActive(true);

        // 입력 필드 초기화
        answerInput.text = "";

        // 정답 결과 텍스트 초기화
        answerResultText.text = "";

        // 연산 기호를 무작위로 선택
        operatorIndex = Random.Range(0, 4); // 0: 덧셈, 1: 뺄셈, 2: 곱셈, 3: 나눗셈

        // 무작위로 선택된 연산 기호에 따라 문제 생성
        switch (operatorIndex)
        {
            case 0:
                num1 = Random.Range(5, 20);
                num2 = Random.Range(5, 20);
                problemText.text = $"{num1} + {num2} = ?";
                break;
            case 1:
                num1 = Random.Range(5, 15);
                num2 = Random.Range(1, 10);
                problemText.text = $"{num1} - {num2} = ?";
                break;
            case 2:
                num1 = Random.Range(2, 15);
                num2 = Random.Range(2, 10);
                problemText.text = $"{num1} × {num2} = ?";
                break;
            case 3:
                num2 = Random.Range(2, 11);
                num1 = num2 * Random.Range(1, 11);
                problemText.text = $"{num1} ÷ {num2} = ?";
                break;
            default:
                Debug.LogError("Invalid operator index: " + operatorIndex);
                return;
        }

        // 문제가 나왔으므로 isProblemEnd를 true로 설정
        isProblemEnd = true;
    }

    // UI 비활성화 함수
    void DeactivateProblemUI()
    {
        problemUI.SetActive(false);
        isProblemEnd = false;
        playerController.SetInputEnabled(true); // 키 입력 제한 해제
    }

    // 게임 일시 정지
    void PauseGame()
    {
        isGamePaused = true;
        Debug.Log("This game is Paused!");
        Time.timeScale = 0f; // 게임 시간을 정지
        StartCoroutine(ResumeAfterDelay(15f)); // 15초 후에 게임 재개
    }

    // 일정 시간 후 게임 재개
    IEnumerator ResumeAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 실제 시간 기준으로 대기
        ResumeGame();
        DeactivateProblemUI();
    }

    // 게임 재개
    void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // 게임 시간을 정상으로 되돌림
    }
}