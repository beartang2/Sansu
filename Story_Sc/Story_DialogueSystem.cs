using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Story_DialogueSystem : MonoBehaviour
{
    int i = 0;
    public Text txtName;            // 이름
    public Text txtSentence;        // 문장
    public Image image;             // 현재 뜨는 이미지
    public Sprite[] spriteimage;    // 교체할 이미지
    public string[] names;          // 이름 

    Queue<string> sentences = new Queue<string>();
   
    public Animator anim;

    public int sceneNum = 2;
    public bool isEnd = false;

    public GameObject arrow;

    private void Update()
    {
        if (!isEnd && Input.GetKeyDown(KeyCode.F))
        {
            Next();
            Imagechange();
        }
    }

    public void Begin(Story_Dialogue info)
    {     
        anim.SetBool("IsOpen", true);

        sentences.Clear();
      
        txtName.text = names[i];

        foreach (var sentence in info.sentence)
        {
            sentences.Enqueue(sentence);
        }

        Next();
    }

    public void Next()
    {
        if (sentences.Count == 0)
        {
            End();
            return;
        }

        image.sprite = spriteimage[i];
              
        txtName.text = names[i];

        //txtSentence.text = sentences.Dequeue();
        txtSentence.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences.Dequeue()));
    }

    public void Imagechange()
    {
        if(i < spriteimage.Length - 1)
        {
            i++;
            image.sprite = spriteimage[i];
        }
        else
        {
            return;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        foreach (var letter in sentence)
        {
            txtSentence.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void End()
    {
        isEnd = true;
        arrow.SetActive(false);
        anim.SetBool("IsOpen", false);
        txtSentence.text = string.Empty;
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene(sceneNum);
    }
}
