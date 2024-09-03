using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable] //직접 만든 class에 접근할 수 있도록 해줌. 
public class Dialogue
{
    [TextArea]//한줄 말고 여러 줄 쓸 수 있게 해줌
    public string dialogue;
    public string name;

}
[System.Serializable]
public class PartDialogue
{
    [TextArea]//한줄 말고 여러 줄 쓸 수 있게 해줌
    public string partDialogue;
    public string partName;

}
public class Scene_DialogSystem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI txt_Name; //이름
    [SerializeField] private Image sprite_DialogueBox; //대사창 이미지(crop)를 제어하기 위한 변수
    [SerializeField] public TextMeshProUGUI txt_Dialogue; // 텍스트를 제어하기 위한 변수

    public bool isDialogue = false; //대화가 진행중인지 알려줄 변수
    public int count = 0; //대사가 얼마나 진행됐는지 알려줄 변수
    public int Fcount = 0;

    [SerializeField] public Dialogue[] dialogue;
    [SerializeField] public PartDialogue[] partDialogue;
    public void ShowDialogue() // <- 이 메서드를 호출하면 다이얼로그가 시작된다.
    {
        ONOFF(true); //대화가 시작됨
        count = 0;
        Fcount = 0;
        NextDialogue(); //호출하자마자 대사가 진행될 수 있도록 
    }

    public void ONOFF(bool _flag)
    {
        sprite_DialogueBox.gameObject.SetActive(_flag);
        txt_Name.gameObject.SetActive(_flag);
        txt_Dialogue.gameObject.SetActive(_flag);
        isDialogue = _flag;
    }

    private void NextDialogue()
    {
        //첫번째 대사와 첫번째 cg부터 계속 다음 cg로 진행되면서 화면에 보이게 된다. 
        txt_Dialogue.text = dialogue[count].dialogue;
        //Debug.Log(txt_Dialogue.text); // debugging

        txt_Name.text = dialogue[count].name;
        //Debug.Log(txt_Name.text);

        count++; //다음 대사와 cg가 나오도록
        //Debug.Log(count);
    }

    public void NextFinalDialogue()
    {
        //첫번째 대사와 첫번째 cg부터 계속 다음 cg로 진행되면서 화면에 보이게 된다. 
        txt_Dialogue.text = partDialogue[Fcount].partDialogue;
        //Debug.Log(txt_Dialogue.text); // debugging

        txt_Name.text = partDialogue[Fcount].partName;
        //Debug.Log(txt_Name.text);

        Fcount++; //다음 대사와 cg가 나오도록
        //Debug.Log(count);
    }

    // Update is called once per frame
    void Update()
    {
        //키를 누를 때마다 대사가 진행되도록. 
        if (isDialogue) //활성화가 되었을 때만 대사가 진행되도록
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("Input F from Scene Dialog");
                //대화의 끝을 알아야함.
                if (count < dialogue.Length)
                {
                    //Debug.Log(count + ", " + dialogue.Length);
                    NextDialogue(); //다음 대사가 진행됨
                }
                else
                {
                    ONOFF(false); //대사가 끝남
                }
            }
        }

    }
}