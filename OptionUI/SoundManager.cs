using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicsource;
    //public AudioSource soudnsource;

    public Slider bgm_slider;
    //public Slider sfx_slider;

    private void Awake()
    {
        bgm_slider = bgm_slider.GetComponent<Slider>();
        //sfx_slider = sfx_slider.GetComponent<Slider>();

        bgm_slider.onValueChanged.AddListener(SetMusicVolume);
        //sfx_slider.onValueChanged.AddListener(SetSoundVoluem);
    }

    private void Start()
    {
        float currentVolume = musicsource.volume;
        bgm_slider.value = currentVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicsource.volume = volume;
    }

    public void SetSoundVoluem(float volume)
    {
        //soudnsource.volume = volume;
    }
}
