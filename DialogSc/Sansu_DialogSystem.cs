using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum Speaker { 산슈 = 0, 사제 }

public class Sansu_DialogSystem : MonoBehaviour
{
	[SerializeField]
	private	Dialog[]			dialogs;						// 현재 분기의 대사 목록
	[SerializeField]
	private	Image   		imageDialogs;                   // 대화창 Image UI
	[SerializeField]
	private Image[]				charactorImages;				// 대화창 캐릭터 Image UI
	[SerializeField]
	private	TextMeshProUGUI	textNames;						// 현재 대사중인 캐릭터 이름 출력 Text UI
	[SerializeField]
	private	TextMeshProUGUI	textDialogues;					// 현재 대사 출력 Text UI
	[SerializeField]
	private	GameObject		objectArrows;					// 대사가 완료되었을 때 출력되는 커서 오브젝트
	[SerializeField]
	private	float				typingSpeed;					// 텍스트 타이핑 효과의 재생 속도
	[SerializeField]
	private	KeyCode				keyCodeSkip = KeyCode.F;	// 타이핑 효과를 스킵하는 키

	private	int					currentIndex = -1;
	private	bool				isTypingEffect = false;			// 텍스트 타이핑 효과를 재생중인지
	private	Speaker				currentSpeaker = Speaker.산슈;

    [SerializeField]
    private GameObject background;

    private bool isGamePaused = false;

    private bool isFkeyEnabled = true; // 키 입력 제한

    [SerializeField]
    private GameObject[] defaultImgs;

    public PlayerController playerController;
    public MyCamera cameraController;

    public void Setup()
	{
        if (!isGamePaused)
        { 
            PauseGame();
        }
        background.SetActive(true);
		for ( int i = 0; i < 2; ++ i )
		{
			// 모든 대화 관련 게임오브젝트 비활성화
			InActiveObjects();
		}
		SetNextDialog();
	}

	public bool UpdateDialog()
	{
        if (!isFkeyEnabled)
        {
            return false;
        }
        if (Input.GetKeyDown(keyCodeSkip))
		{
            //Debug.Log("Input G from Sansu Dialog");
			// 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
			if ( isTypingEffect == true )
			{
				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("TypingText");
				isTypingEffect = false;
				textDialogues.text = dialogs[currentIndex].dialogue;
				// 대사가 완료되었을 때 출력되는 커서 활성화
				objectArrows.SetActive(true);

				return false;
			}

			// 다음 대사 진행
			if ( dialogs.Length > currentIndex + 1 )
			{
				SetNextDialog();
                if(currentIndex == 9)
                {
                    charactorImages[9].gameObject.SetActive(true);
                }
                else
                { 
                    charactorImages[currentIndex-1].gameObject.SetActive(false);
                }
            }
			// 대사가 더 이상 없을 경우 true 반환
			else
			{
				// 모든 캐릭터 이미지를 어둡게 설정
				for ( int i = 0; i < 3; ++ i )
				{
					// 모든 대화 관련 게임오브젝트 비활성화
					InActiveObjects();
                    background.SetActive(false);
                    defaultImgs[i].SetActive(false);
                    charactorImages[9].gameObject.SetActive(false);
                }
                if(isGamePaused)
                {
                    StartCoroutine(ResumeGame());
                }

                return true;
			}
		}

		return false;
	}

	private void SetNextDialog()
	{
        playerController.SetInputEnabled(false);
        cameraController.SetCameraMovementEnabled(false);

        // 이전 화자의 대화 관련 오브젝트 비활성화
        InActiveObjects();

		currentIndex ++;

		// 현재 화자 설정
		currentSpeaker = dialogs[currentIndex].speaker;

		// 대화창 활성화
		imageDialogs.gameObject.SetActive(true);
        
        // 캐릭터 이미지 활성화
        charactorImages[currentIndex].gameObject.SetActive(true);

		// 현재 화자 이름 텍스트 활성화 및 설정
		textNames.gameObject.SetActive(true);
		textNames.text = dialogs[currentIndex].speaker.ToString();

		// 화자의 대사 텍스트 활성화 및 설정 (Typing Effect)
		textDialogues.gameObject.SetActive(true);
		StartCoroutine(nameof(TypingText));

        if(currentIndex > 0)
        {
            //npc가 이전에 웃음 짓고 대화했을 때, 기본 표정도 다르게 설정
            if(charactorImages[currentIndex-1].gameObject.name == "NPC_image2")
            {
                defaultImgs[2].SetActive(true);
            }
            else
            {
                defaultImgs[2].SetActive(false);
                defaultImgs[1].SetActive(true);
            }
        }
	}

	private void InActiveObjects()
	{
		imageDialogs.gameObject.SetActive(false);
		textNames.gameObject.SetActive(false);
		textDialogues.gameObject.SetActive(false);
		objectArrows.SetActive(false);
	}

    // 게임 일시 정지
    void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // 게임 시간을 정지
    }

    // 게임 재개
    IEnumerator ResumeGame()
    {
        yield return new WaitForSecondsRealtime(0.7f);
        isGamePaused = false;
        Time.timeScale = 1f; // 재개
        playerController.SetInputEnabled(true);
        cameraController.SetCameraMovementEnabled(true);
    }

    private IEnumerator TypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// 텍스트를 한글자씩 타이핑치듯 재생
		while ( index < dialogs[currentIndex].dialogue.Length )
		{
			textDialogues.text = dialogs[currentIndex].dialogue.Substring(0, index);

			index ++;

			yield return new WaitForSecondsRealtime(typingSpeed);
		}

		isTypingEffect = false;

		// 대사가 완료되었을 때 출력되는 커서 활성화
		objectArrows.SetActive(true);
	}

    public void FkeyEnabled(bool enabled)
    {
        isFkeyEnabled = enabled;
    }
}

[System.Serializable]
public struct Dialog
{
	public	Speaker		speaker;	// 화자
	[TextArea(3, 5)]
	public	string		dialogue;	// 대사
}

