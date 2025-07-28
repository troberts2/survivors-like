using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resDropDown;
    [SerializeField] private Toggle fullscreenToggle;

    Resolution[] allResolutions;
    bool isFullscreen;
    int selectedResolution;
    List<Resolution> selectedResolutionList = new List<Resolution>();

    // Start is called before the first frame update
    void Start()
    {
        isFullscreen = false;
        List<Resolution> resolutions = new List<Resolution>
        {
            CreateNewResolution(640, 360),
            CreateNewResolution(1280, 720),
            CreateNewResolution(1920, 1080)
        };

        List<string> resolutionStringList = new List<string>
        {
            "640 x 360",
            "1280 x 720",
            "1920 x 1080"
        };

        resDropDown.AddOptions(resolutionStringList);
        selectedResolutionList = resolutions;
        SetOriginalScale();// remove this later and replace with what player chose previously
    }

    private Resolution CreateNewResolution(int width, int height)
    {
        Resolution newRes = new Resolution();
        newRes.width = width;
        newRes.height = height;
        return newRes;
    }

    public void ChangeResolution()
    {
        selectedResolution = resDropDown.value;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }

    public void ChangeFullscreen()
    {
        isFullscreen = fullscreenToggle.isOn;
        if(isFullscreen)
        {
            //resDropDown.value = 2; //change res to 1920x1080 if changing to full screen
            selectedResolution = resDropDown.value;
        }
        else
        {
            selectedResolution = resDropDown.value;
        }
        
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, isFullscreen);
    }

    public void SetOriginalScale()
    {
        Screen.SetResolution(640, 360, false);
        resDropDown.value = 0;
        fullscreenToggle.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
