using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Sansu_DialogController : MonoBehaviour
{
	[SerializeField]
	private	List<Sansu_DialogBase>	Dialogs;
	[SerializeField]
	//private	string				nextSceneName = "";

	private Sansu_DialogBase		currentDialog = null;
	private	int						currentIndex = -1;

	public Image Text;				// Q스킬을 써보자 띄울 텍스트창 
	public Animator anim;			// 애니메이션

    public bool killEnemy = false; // 플레이어가 설정할 수 있는 적 오브젝트를 처치했는가?
    public GameObject portal;

    public Sansu_DialogSystem sansuDialog;
    public Scene_DialogSystem sceneDialog;

    private void Start()
	{
        portal.SetActive(false);
        anim.SetBool("IsOpen", false);
		SetNextDialog();
	}

	private void Update()
	{
		if ( currentDialog != null )
		{
			currentDialog.Execute(this);
		}

		OnAllEnemiesDestroyedCheck();
	}

	public void SetNextDialog()
	{
		// 현재 과정의 Exit() 메소드 호출
		if ( currentDialog != null )
		{
			//anim.SetBool("IsOpen", true);
		}

		// 마지막 과정을 진행했다면 메소드 호출
		if ( currentIndex >= Dialogs.Count-1 )
		{
			anim.SetBool("IsOpen", true);
			StartCoroutine(HideUIDelay(15f));
			
			return;
		}

		// 다음 과정을 currentDialog로 등록
		currentIndex++;
		currentDialog = Dialogs[currentIndex];

		// 새로 바뀐 과정의 Enter() 메소드 호출
		currentDialog.Enter();
	}

	IEnumerator HideUIDelay(float delay)
	{
		// delay 만큼 대기
		yield return new WaitForSeconds(delay);

		// UI 요소를 비활성화하여 숨김
		anim.SetBool("IsOpen", false);

		Invoke("UIHIde", 2);
	}

	private void UIHIde() // 튜토리얼 UI
    {
		Text.gameObject.SetActive(false);
    }

	private void OnAllEnemiesDestroyedCheck()
	{
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        if (!killEnemy && enemies.Length == 0)
        {
            // 모든 적이 제거되었을 때 할 일을 여기에 작성
            Debug.Log("모든 적이 제거되었다");

            // 전투 후 대사
            sceneDialog.ShowDialogue();
            sansuDialog.FkeyEnabled(false);

            portal.SetActive(true);
            killEnemy = true;
        }
	}

	/*public void CompletedAllTutorials()
	{
		currentDialog = null;

		// 행동 양식이 여러 종류가 되었을 때 코드 추가 작성
		// 현재는 씬 전환

		Debug.Log("Complete All");

		if ( !nextSceneName.Equals("") )
		{
			SceneManager.LoadScene(nextSceneName);
		}
	}*/
}

