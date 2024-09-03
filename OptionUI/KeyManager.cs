using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { UP, DOWN, LEFT, RIGHT, SKILL1, SKILL2, RUN, ROLL, KEYCOUNT }
public static class KeySetting { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }
// ���� Ű���� �Է����� ����ؾ� �ϹǷ� �˸��� ���� Input.GetKey(KeySetting.Keys[KeyAction.�˸��� �׼��̸� ex) KeyAction.UP]) �� �Ἥ ����ϸ� �ȴ�.
public class KeyManager : MonoBehaviour
{
    KeyCode[] defalutKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.E, KeyCode.LeftShift, KeyCode.LeftControl };
    // �⺻ Ű �Է� ���� �ٲٰ� ������ ���� Ű�ڵ带 �����ϸ� �ȴ�.
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
