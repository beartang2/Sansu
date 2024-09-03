using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    //public GameObject obj;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) // 바닥에 닿았을 때
        {
            Destroy(this.gameObject);
        }

        else if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}