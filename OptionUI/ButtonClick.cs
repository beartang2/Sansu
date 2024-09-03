using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{//ButtonClick 스크립트 : 버튼을 클릭하면 그에 대응하는 차기 행동을 지정하는 스크립트

    public BTNType currentType; //버튼 타입을 정의한 BTNType사용

    public void OnBtnClick() //버튼이 눌렸을 때
    {
        int sceneNum = 1;
        switch (currentType)
        {
            case BTNType.Start://게임 시작 버튼이 눌렸을 때 실행
                Debug.Log("시작하기");
                SceneManager.LoadScene(sceneNum); //임시 튜토씬을 불러옴.("")부분 수정하면 됨.
                break;

            case BTNType.Option: //설정 버튼이 눌려을 때 실행
                Debug.Log("설정창On"); //설정창을 켰을때 콜솔창에 '설정창On'을 출력

                //이 곳에 옵션버튼이 눌렸을 경우 추가 코드 작성.

                break;

            case BTNType.Quit://게임 종료 버튼이 눌렸을 때 실행
                Debug.Log("나가기"); // 나가기를 콘솔창에 출력. (추후 삭제해도 무관)

            #if UNITY_EDITOR //전처리기로 유니티 에디터가 실행중일때 플레이를 멈추도록함.
                             //Application.Quit()는 빌드 상태일때만 작동해, 테스트시 불편함이 있어서 작성한 코드.
                UnityEditor.EditorApplication.isPlaying = false; //어플리케이션 플레이를 false로 함.
            #else //유니티에디터가 실행중이 아닐때 작동
                Application.Quit(); //어플리케이션을 종료
            #endif
                break;
        }
    }
}
