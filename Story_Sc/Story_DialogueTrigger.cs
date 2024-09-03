using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_DialogueTrigger : MonoBehaviour
{
    public Story_Dialogue info;

    public void Start()
    {
        var system = FindObjectOfType<Story_DialogueSystem>();
        system.Begin(info);
    }
}