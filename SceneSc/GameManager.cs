using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트 참조

    public void CoroutineRelivePlayerAfterDelay()
    {
        StartCoroutine(RelivePlayerAfterDelay());
    }

    private IEnumerator RelivePlayerAfterDelay()
    {
        player.SetActive(false);
        Debug.Log("플레이어 비활성화");
        yield return new WaitForSeconds(0.5f);

        newPlayerHP playerHp = player.GetComponent<newPlayerHP>();
        if(playerHp != null)
        {
            playerHp.Hp = 100; // 체력 초기화
            playerHp.playerHpBar.value = playerHp.Hp;
        }
        Debug.Log("플레이어 체력 초기화");
        yield return new WaitForSecondsRealtime(1.5f);
        player.SetActive(true);
        Debug.Log("플레이어 활성화");
    }
}
