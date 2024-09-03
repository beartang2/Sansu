using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    private Stack<GameObject> settingsStack = new Stack<GameObject>();

    // UI 패널들을 에디터에서 연결하기 위한 public 변수로 선언합니다.
    public GameObject optionSettingsPanel;
    public GameObject keySettingsPanel;
    public GameObject audioSettingsPanel;

    private GameObject player;
    private PlayerController playerController;
    private GameObject mouseManager;
    private MouseManagerSc mouseManagerSc;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        mouseManager = GameObject.FindGameObjectWithTag("Mouse");
        mouseManagerSc = mouseManager.GetComponent<MouseManagerSc>();
    }
    void Update()
    {
        // Esc 키를 눌렀을 때 처리
        if (SceneManager.GetActiveScene().name != "StartScene" && Input.GetKeyDown(KeyCode.Escape))
        {
            mouseManagerSc.VisibleCursor();
            if (player != null)
            {
                playerController.SetInputEnabled(false);
            }
            Time.timeScale = 0f; // 시간 정지
            if (settingsStack.Count == 0)
            {
                // 설정창이 열려 있지 않을 경우 옵션 설정창을 엶
                OpenOption();
            }
            else
            {
                // 설정창이 열려 있을 경우 현재 설정창을 닫음
                CloseCurrentSettings();
                
            }
        }
    }

    // 새로운 설정창을 스택에 추가하고 활성화하는 메서드
    public void OpenSettings(GameObject settingsPanel)
    {
        // 현재 활성화된 설정창을 스택에 푸시하고 비활성화
        if (settingsStack.Count > 0)
        {
            settingsStack.Peek().SetActive(false);
        }

        // 새로운 설정창을 스택에 푸시하고 활성화
        settingsStack.Push(settingsPanel);
        settingsPanel.SetActive(true);
    }

    // 현재 설정창을 스택에서 제거하고 이전 설정창을 활성화하는 메서드
    public void CloseCurrentSettings()
    {
        if (settingsStack.Count > 0)
        {
            // 현재 설정창을 팝하고 비활성화
            GameObject topPanel = settingsStack.Pop();
            topPanel.SetActive(false);

            // 스택의 이전 설정창을 다시 활성화
            if (settingsStack.Count > 0)
            {
                settingsStack.Peek().SetActive(true);
            }
            else
            {
                if(SceneManager.GetActiveScene().name != "StartScene")
                {
                    mouseManagerSc.InvisibleCursor();
                }
                if (player != null)
                {
                    playerController.SetInputEnabled(true);
                }
            }
        }

        Time.timeScale = 1f; // 시간 재개
    }

    public void CloseAllSettings()
    {
        while (settingsStack.Count > 0)
        {
            GameObject topPanel = settingsStack.Pop();
            topPanel.SetActive(false);
        }
        if (player != null)
        {
            playerController.SetInputEnabled(true);
        }
        if(SceneManager.GetActiveScene().name != "StartScene")
        {
            mouseManagerSc.InvisibleCursor();
        }
        Time.timeScale = 1f; // 시간 재개
    }

    // 각 설정창을 여는 메서드 (UI 버튼에 연결)
    public void OpenOption()
    {
        OpenSettings(optionSettingsPanel);
    }
    public void OpenKeySettings()
    {
        OpenSettings(keySettingsPanel);
    }

    public void OpenAudioSettings()
    {
        OpenSettings(audioSettingsPanel);
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(0); // <- 로드되는 씬 이름에 맞기 바꿔서 쓰기
    }
}