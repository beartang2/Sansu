using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue_Puzzle : MonoBehaviour
{
    public Transform playerTransform;
    public float detectionRadius = 5.0f;
    private float rotationAngle = 30.0f;

    void Update()
    {
        if (CanInteractWithPuzzle())
        {
            if (Vector3.Distance(playerTransform.position, transform.position) <= detectionRadius)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    RotateStatue();
                }
            }
        }
    }

    bool CanInteractWithPuzzle()
    {
        // Check if there are no GameObjects named "MonsterStatue"
        GameObject monsterStatue = GameObject.Find("MonsterStatue");
        if (monsterStatue != null)
        {
            return false;
        }

        // Check if there are no GameObjects with the "Orb" tag
        GameObject[] orbs = GameObject.FindGameObjectsWithTag("Orb");
        if (orbs.Length > 0)
        {
            return false;
        }

        return true;
    }

    void RotateStatue()
    {
        transform.Rotate(0, rotationAngle, 0);
    }
}
