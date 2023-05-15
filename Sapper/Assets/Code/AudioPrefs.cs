using UnityEngine;

public class AudioPrefs : MonoBehaviour
{
    static string c_key = "audio";


    void Start()
    {
        float volume = PlayerPrefs.GetFloat(c_key, 0.5f);
        AudioListener.volume = volume;
    }

    public static void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(c_key, value);
    }
}
