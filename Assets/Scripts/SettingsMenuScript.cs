using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;


public class SettingsMenuScript : MonoBehaviour

{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDD;
    //Create an array called resolutions.
    Resolution[] resolutions;

    private int graphicsQualityIndex;
    private string qualityLevel;

    private void Start()
    {
        GetAspectRatio();
    }

    public void GetAspectRatio()
    {   
        // 
        resolutions = Screen.resolutions;

        // determine the defualt ratio and initialize its index to 0
        string defaultResolution = StringRatios(Screen.currentResolution);
        int defaultResIindex = 0;
        
        // create empty string list to place in dropdown
        var ratios = new List<string>();

        for ( int i = 0; i < resolutions.Length; i++)
        {
            // convert ratios to strings to add to dropdown list.
            string ratio = StringRatios(resolutions[i]);
            ratios.Add(ratio);

            // compare string ratio to string default ratio.
            if (ratio == defaultResolution)
            {
                // if the same update defualt index to the aspect ratios index.
                defaultResIindex = i;
            }

        }
        // clear and repopulate dropdown with ratios list.
        resolutionDD.ClearOptions();
        resolutionDD.AddOptions(ratios);

        // set default ratio
        resolutionDD.value = defaultResIindex;
        resolutionDD.RefreshShownValue();
    }
    /// helper function for start, this function takes the aspect ratios passed in and 
    /// returns a strring of the aspect ratio that the start function can compare.
    public string StringRatios(Resolution ratio)
    {
       // make string from ratio object passed through parameter.
       string ratioString = ratio.width + " x " + ratio.height;
       return ratioString;
    }

    /// the selected aspect ratio index that was selected in the 
    /// drop down is passed into this unction and the Screen class is called
    /// which calls function SetResolution
    public void SetRatio(int resIndex)
    {
        // locate resolution that game resolution will be set to.
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// simple boolean function that passes the toggle interaction 
    /// into this function and sets if screen is fullscreen or not
    /// 
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    /// takes float from volume slider and sets the volume paramater 
    /// of the main audio mixer to the passed in float val
    /// 
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    /// Takes a float from the graphics slider, based on the 
    /// index value a case is determined and the graphics
    /// settings are changed accourdingly.
    public void SetGraphicsQuality (float sliderValue)
    {
        int index = Mathf.RoundToInt(sliderValue);
        switch (index)
        {
            case 1:
                graphicsQualityIndex = 1;
                qualityLevel = "Low";
                break;
                            
            case 2:
                graphicsQualityIndex = 3;
                qualityLevel = "Medium";
                break;

            case 3: 
                graphicsQualityIndex = 5;
                qualityLevel = "High";
                break;

            default:
                Debug.Log("Invalid Graphics Quality Index");
                break;
        }

        QualitySettings.SetQualityLevel(graphicsQualityIndex);
        Debug.Log($"Grpahics quality set to {qualityLevel}");

    }


}
