using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sansu_InventoryUI : MonoBehaviour
{
    public Image[] images;
  
    void Start()
    {
        // 처음에 모든 이미지를 끕니다.
        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if ( Player_Inventory.KeyCount == 0  )
        {
            images[0].gameObject.SetActive(false);
            images[1].gameObject.SetActive(false);
            images[2].gameObject.SetActive(false);

        } else if ( Player_Inventory.KeyCount == 1)
        {
            images[0].gameObject.SetActive(true);

            //images[1].gameObject.SetActive(false);

        } else if (Player_Inventory.KeyCount == 2)
        {
            images[0].gameObject.SetActive(true);
            images[1].gameObject.SetActive(true);

            //images[2].gameObject.SetActive(false);

        } else if (Player_Inventory.KeyCount == 3)
        {
            images[0].gameObject.SetActive(true);
            images[1].gameObject.SetActive(true);
            images[2].gameObject.SetActive(true);

            //images[3].gameObject.SetActive(false);

        }
    }
}
