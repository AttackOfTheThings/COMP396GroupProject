using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeSaveController : MonoBehaviour
{
    public Slider volumeSlider = null;
    public Text volumeTextUI = null;
    public void VolumeSlider (float volume)
    {
        volumeTextUI.text = volume.ToString("0.0");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveVolumeButton()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeValue);
        LoadValue();
    }
    void LoadValue()
    {
        float volumeValue = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}
