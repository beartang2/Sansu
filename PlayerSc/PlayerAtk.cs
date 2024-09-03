using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    public enum Type { Melee };// ���� ���Ÿ� �߰� ����
    public Type type;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Awake()
    {
        //meleeArea = GetComponent<BoxCollider>();
    }

    public void Atk(float delay)
    {
        if (type == Type.Melee)//�ٰŸ� ���� Ÿ��
        {
            StopCoroutine("Swing");
            trailEffect.enabled = true;
            StartCoroutine("Swing", delay);
        }
    }

    public void Atk2(float delay2)
    {
        if (type == Type.Melee)//�ٰŸ� ���� Ÿ��
        {
            StopCoroutine("Swing2");
            StartCoroutine("Swing2", delay2);
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.5f);// 0.3�� ���
        meleeArea.enabled = true;// �ݶ��̴� Ȱ��ȭ

        //2
        yield return new WaitForSeconds(0.2f);// 0.3�� ���
        meleeArea.enabled = false;// �ݶ��̴� ��Ȱ��ȭ

        // ���� ����Ʈ
        
        yield return new WaitForSeconds(0.5f);
        trailEffect.enabled = false;

        yield break;
    }
    //�ڷ�ƾ ���
    IEnumerator Swing2()
    {
        //1
        yield return new WaitForSeconds(1f);// 0.3�� ���
        meleeArea.enabled = true;// �ݶ��̴� Ȱ��ȭ

        //2
        yield return new WaitForSeconds(1f);// 0.3�� ���
        meleeArea.enabled = false;// �ݶ��̴� ��Ȱ��ȭ

        //���� ����Ʈ �߰� ����

        yield break;
    }
}
