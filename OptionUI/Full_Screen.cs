using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class Full_Screen : MonoBehaviour
{
    FullScreenMode screenMode;
    public Toggle fullscreenBtn;

    void Start()
    {
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
        fullscreenBtn.onValueChanged.AddListener(FullScreenBtn);
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OKBtnClick()
    {
        Screen.SetResolution(1920, 1080, screenMode == FullScreenMode.FullScreenWindow);
        Screen.fullScreenMode = screenMode;
    }
}
