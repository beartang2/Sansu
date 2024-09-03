using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    public enum Type { Melee };// 이후 원거리 추가 가능
    public Type type;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Awake()
    {
        //meleeArea = GetComponent<BoxCollider>();
    }

    public void Atk(float delay)
    {
        if (type == Type.Melee)//근거리 공격 타입
        {
            StopCoroutine("Swing");
            trailEffect.enabled = true;
            StartCoroutine("Swing", delay);
        }
    }

    public void Atk2(float delay2)
    {
        if (type == Type.Melee)//근거리 공격 타입
        {
            StopCoroutine("Swing2");
            StartCoroutine("Swing2", delay2);
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.5f);// 0.3초 대기
        meleeArea.enabled = true;// 콜라이더 활성화

        //2
        yield return new WaitForSeconds(0.2f);// 0.3초 대기
        meleeArea.enabled = false;// 콜라이더 비활성화

        // 공격 이펙트
        
        yield return new WaitForSeconds(0.5f);
        trailEffect.enabled = false;

        yield break;
    }
    //코루틴 사용
    IEnumerator Swing2()
    {
        //1
        yield return new WaitForSeconds(1f);// 0.3초 대기
        meleeArea.enabled = true;// 콜라이더 활성화

        //2
        yield return new WaitForSeconds(1f);// 0.3초 대기
        meleeArea.enabled = false;// 콜라이더 비활성화

        //공격 이펙트 추가 가능

        yield break;
    }
}
