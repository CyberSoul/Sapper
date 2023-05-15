using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioSlider : MonoBehaviour
{
    Slider slider;

    const string key = "audio";

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = AudioListener.volume;
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(float newValue)
    {
        AudioPrefs.SetVolume(newValue);
    }

}
