using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseManagerSc : MonoBehaviour
{
    public static CursorLockMode lockState;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "StartScene")
        {
            VisibleCursor();
        }
        else
        {
            InvisibleCursor();
        }
    }

    public void InvisibleCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void VisibleCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
