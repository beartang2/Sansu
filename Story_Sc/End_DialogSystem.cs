using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End_DialogSystem : MonoBehaviour
{
    int i = 0;
    public Image image;             // 현재 뜨는 이미지
    public Sprite[] spriteimage;    // 교체할 이미지

    public bool isEnd = false;

    private void Start()
    {
        Imagechange();
    }

    public void Imagechange()
    {
        if (i < spriteimage.Length - 1)
        {
            i++;
            image.sprite = spriteimage[i];
            Invoke("Imagechange", 5.5f);
        }
        else
        {
            Invoke("End", 3f);
            return;
        }
    }

    private void End()
    {
        isEnd = true;
        SceneManager.LoadScene(0);
    }

}
