using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField]
    private int cnt = 1; // 다음 씬 번호

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Player")))
        {
            SceneManager.LoadScene(cnt);
        }
    }
}