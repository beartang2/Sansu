using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjector : MonoBehaviour
{
    public GameObject boss; // 보스 객체 참조
    public float yOffset = 0f;
    void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); // 프로젝터의 각도 조절
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = boss.transform.position; // 프로젝터가 보스를 따라다니게 함

        Vector3 newPosition = boss.transform.position;
        newPosition.y -= yOffset; // yOffset는 원하는 만큼의 높이 감소값입니다.
        transform.position = newPosition;
    }
}
