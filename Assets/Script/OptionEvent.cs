using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionEvent : MonoBehaviour
{
    public Toggle toggle;
    public Slider slider;

    public void SoundMute()
    {
        if (toggle.isOn)
        {
            AudioListener.volume = 0;
            slider.value = 0;
        }
        else
        {
            AudioListener.volume = 1;
            slider.value = 1;
        }
    }

    public void SoundSlider()
    {
        AudioListener.volume = slider.value;

        if (slider.value == 0)
            toggle.isOn = true;
        else
            toggle.isOn = false;
    }
}
