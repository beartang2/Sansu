using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    //정지
    public bool isGamePaused = false;

    // 스킬 구현
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
    /*
    public Text problemText;    // UI 텍스트 요소 - 사칙연산 문제를 표시.
    public InputField answerInput;  // UI 입력 필드 - 답을 입력.
    public Text resultText; // UI 텍스트 요소 - 정답 여부를 표시.

    private bool isProblemActive = false;   // 현재 문제가 활성화되었는지 여부.
    */

    void Start()
    {
        problemUI.SetActive(false);
        answerResultText.gameObject.SetActive(false);

        checkButton.onClick.AddListener(CheckAnswer);
    }

    void Update()
    {
        CheckProblem();
    }

    // 스킬 구현
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
                num1 = Random.Range(1, 10);
                num2 = Random.Range(1, 10);
                problemText.text = $"{num1} + {num2} = ?";
                break;
            case 1:
                num1 = Random.Range(5, 15);
                num2 = Random.Range(1, 10);
                problemText.text = $"{num1} - {num2} = ?";
                break;
            case 2:
                num1 = Random.Range(2, 10);
                num2 = Random.Range(2, 10);
                problemText.text = $"{num1} × {num2} = ?";
                break;
            case 3:
                num2 = Random.Range(2, 6);
                num1 = num2 * Random.Range(2, 6);
                problemText.text = $"{num1} ÷ {num2} = ?";
                break;
            default:
                Debug.LogError("Invalid operator index: " + operatorIndex);
                return;
        }

        // 문제가 나왔으므로 isProblemEnd를 true로 설정
        isProblemEnd = true;
    }

    void CheckProblem()
    {
        if (PlayerController.skillKey && !isProblemEnd) // 문제가 나오지 않은 경우에만 문제 표시
        {
            ShowProblem();
            if (!isGamePaused)
            {
                PauseGame();
            }
        }
    }

    IEnumerator DeactivateProblemUIDelayed2()
    {
        yield return new WaitForSeconds(15f); // 15초 대기
        DeactivateProblemUI(); // 문제 UI 비활성화
    }

    void DeactivateProblemUI()
    {
        problemUI.SetActive(false);
    }

    void DeactivateAnswerResultText()
    {
        answerResultText.gameObject.SetActive(false);
    }

    public void CheckAnswer()
    {
        isAnswered = false;// true로 되어있던 isAnswered를 다시 false로

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
                    Debug.Log("크리티컬! 40 데미지");
                }
                else if (userAnswer != correctAnswer)
                {
                    answerResultText.text = "틀렸습니다!";
                    Debug.Log("실패! 20 데미지");
                    //isProblemEnd = false;
                }

                // 문제 해결 후 문제가 더 이상 나오지 않도록 isProblemEnd를 false로 설정
                isProblemEnd = false;

                // 정답 결과 텍스트가 출력된 후 3초 후에 문제 UI 비활성화 및 게임 재개
                StartCoroutine(DeactivateProblemUIDelayed());
                isAnswered = true;
                answerResultText.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator DeactivateProblemUIDelayed()
    {
        yield return new WaitForSecondsRealtime(3f); // 3초 대기
        DeactivateProblemUI(); // 문제 UI 비활성화
        ResumeGame(); // 게임 재개
    }

    // 게임 일시 정지
    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // 게임 시간을 정지
        StartCoroutine(ResumeAfterDelay(15f)); // 15초 후에 게임 재개
    }


    IEnumerator ResumeAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 실제 시간 기준으로 대기
        ResumeGame();
    }

    void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // 게임 시간을 정상으로 되돌림
    }

    // 수정
    /*
    // 사칙연산 문제를 생성하여 UI에 표시하고, 15초 후에 문제를 제거
    void DisplayArithmeticProblem()
    {
        isProblemActive = true;

        // 랜덤한 숫자 및 연산자 생성
        int num1 = Random.Range(1, 11);
        int num2 = Random.Range(1, 11);
        char[] operators = { '+', '-', '*', '/' };
        char op = operators[Random.Range(0, operators.Length)];

        // 문제 생성
        string problem = num1 + " " + op + " " + num2 + " = ?";
        problemText.text = problem;

        StartCoroutine(RemoveProblemAfterDelay(15f));
    }

    // 일정 시간 후에 UI에서 문제를 제거
    IEnumerator RemoveProblemAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        problemText.text = "";
        resultText.text = "";
        isProblemActive = false;
    }

    // 입력된 답을 확인하여 정답 여부를 표시
    public void CheckAnswer()
    {
        if (!isProblemActive) return;   // 문제가 활성화되지 않은 경우 처리하지 않음

        string answerStr = answerInput.text;
        if (string.IsNullOrEmpty(answerStr)) return;    // 입력된 답이 없는 경우 처리하지 않음

        int answer;
        if (int.TryParse(answerStr, out answer))
        {
            int expectedAnswer = CalculateExpectedAnswer(problemText.text);
            if (answer == expectedAnswer)
            {
                resultText.text = "정답!";  // 정답인 경우 결과를 표시
            }
            else
            {
                resultText.text = "틀렸습니다!";    // 오답인 경우 결과를 표시
            }
        }
    }

    // 주어진 사칙연산 문제 문자열로부터 예상 답을 계산
    int CalculateExpectedAnswer(string problem)
    {
        // 문제에서 연산자와 숫자를 분리하여 계산
        string[] parts = problem.Split(' ');
        int num1 = int.Parse(parts[0]);
        int num2 = int.Parse(parts[2]);
        char op = parts[1][0];

        // 연산자에 따라 예상 답을 계산
        switch (op)
        {
            case '+':
                return num1 + num2;
            case '-':
                return num1 - num2;
            case '*':
                return num1 * num2;
            case '/':
                return num1 / num2; // 나눗셈에서 나머지는 무시
            default:
                return 0;
        }
    }*/
}
