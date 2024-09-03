using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    [SerializeField]
    private Animator doorAnimator;

    public Scene_DialogSystem sceneDialog;

    public static int KeyCount = 0;

    private bool hadSaying = false;
    [SerializeField]
    private BoxCollider doorCol;
    private void Start()
    {
        if(sceneDialog != null)
        {
            sceneDialog.ShowDialogue(); // 대사 시작
        }
    }

    private void Update()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        // f 키를 눌렀을 때 아이템 획득 기능 호출
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupKey();
        }

        if(doors != null)
        {
            // 플레이어 주변에 Door 태그가 있는지 확인하여 열쇠 사용 기능 호출
            foreach (GameObject door in doors)
            {
                if (Vector3.Distance(transform.position, door.transform.position) <= 4f && Input.GetKeyDown(KeyCode.E))
                {
                    UseKey();
                }
            }
        }
    }

    public void PickupKey()
    {
        GameObject[] keys = GameObject.FindGameObjectsWithTag("Key");

        foreach (GameObject key in keys)
        {
            if (Vector3.Distance(transform.position, key.transform.position) <= 4f)
            {
                Destroy(key);
                KeyCount++;
                Debug.Log("열쇠를 획득했습니다! 현재 Key Count: " + KeyCount);
                
                if(keys.Length > 0)
                {
                    // 열쇠를 얻었다! 남은 열쇠 개수는 n개야.
                    sceneDialog.ONOFF(true);
                    sceneDialog.txt_Name.text = "산슈";
                    sceneDialog.txt_Dialogue.text = "열쇠를 얻었다! 남은 열쇠 개수는 " + (keys.Length - 1) + "개야.";
                    Debug.Log("열쇠를 얻었다! 남은 열쇠 개수는 " + (keys.Length - 1) + "개야.");
                }

                if(KeyCount == 3)
                {
                    // 열쇠를 모두 얻었어! 문을 열러 가자.
                    sceneDialog.ONOFF(true);
                    sceneDialog.txt_Name.text = "산슈";
                    sceneDialog.txt_Dialogue.text = "정말 힘든 싸움이었어.." +
                        "열쇠를 모두 얻었으니, 어서 문을 열러 가자.";
                }
            }
        }
    }

    public void UseKey()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject door in doors)
        {
            if (enemies.Length <= 0)
            {
                if (Vector3.Distance(transform.position, door.transform.position) <= 4f)
                {
                    if (KeyCount >= 3)
                    {
                        // 문이 열리는 소리가 들렸어!
                        sceneDialog.ONOFF(true);
                        sceneDialog.txt_Name.text = "산슈";
                        sceneDialog.txt_Dialogue.text = "문이 열렸어!";

                        // 여기에 문을 열기 위한 코드 추가
                        doorAnimator.SetTrigger("isOpen");
                        doorCol.enabled = false;
                        //door.SetActive(false); // 문 비활성화 (임시)
                    }
                    else if(!hadSaying)
                    {
                        hadSaying = true;
                        // 열쇠가 없어..
                        sceneDialog.ONOFF(true);
                        sceneDialog.txt_Name.text = "산슈";
                        sceneDialog.txt_Dialogue.text = "열쇠가 없어..";
                        Debug.Log("열쇠가 없어..");
                    }
                }
            }
            else
            {
                // 아직 몬스터가 남아있나봐.
                sceneDialog.ONOFF(true);
                sceneDialog.txt_Name.text = "산슈";
                sceneDialog.txt_Dialogue.text = "아직 몬스터가 남아있나봐.";
                Debug.Log("아직 몬스터가 남아있나봐.");
            }
        }
    }
}
