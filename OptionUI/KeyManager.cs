using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { UP, DOWN, LEFT, RIGHT, SKILL1, SKILL2, RUN, ROLL, KEYCOUNT }
public static class KeySetting { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }
// 위의 키들을 입력으로 사용해야 하므로 알맞은 곳에 Input.GetKey(KeySetting.Keys[KeyAction.알맞은 액션이름 ex) KeyAction.UP]) 을 써서 사용하면 된다.
public class KeyManager : MonoBehaviour
{
    KeyCode[] defalutKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.E, KeyCode.LeftShift, KeyCode.LeftControl };
    // 기본 키 입력 값을 바꾸고 싶으면 위에 키코드를 변경하면 된다.
    private void Awake()
    {
        for (int i = 0; i < (int) KeyAction.KEYCOUNT; i++)
        {
            KeySetting.keys.Add((KeyAction)i, defalutKeys[i]);
        }
    }

    private void OnGUI()
    {
        Event KeyEvent = Event.current;
        if (KeyEvent.isKey && key != -1)
        {
            KeyCode newKey = KeyEvent.keyCode;
            if (!KeySetting.keys.ContainsValue(newKey))
            {
                KeySetting.keys[(KeyAction)key] = newKey;
            }
            key = -1;
        }
    }
    int key = -1;
    public void ChangeKey(int num)
    {
        key = num;
    }
}
