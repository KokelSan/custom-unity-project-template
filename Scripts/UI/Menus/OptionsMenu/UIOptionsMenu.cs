using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOptionsMenu : UIButtonMenu
{
    [Header("Options Menu Elements")] 
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;
    public TMP_Dropdown GraphicsQualityDropdown;
    public Slider VolumeSlider;

    private List<Resolution> _resolutions = new List<Resolution>();
    private List<string> _textResolutions = new List<string>();
    
    private Action _onGoBack;
    
    protected override void EventHandlerRegister()
    {
        base.EventHandlerRegister();
        
        UIMenuManagerHandlerData.OnShowOptionsMenu += ShowMenu;
        GameManagerHandlerData.OnGameResumed += OnGameResumed;
        
        ResolutionDropdown.onValueChanged.AddListener(OnResolutionSelected);
        FullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        GraphicsQualityDropdown.onValueChanged.AddListener(OnGraphicsQualitySelected);
        VolumeSlider.onValueChanged.AddListener(OnVolumeUpdated);
    }
    
    protected override void EventHandlerUnRegister()
    {
        base.EventHandlerUnRegister();
        
        UIMenuManagerHandlerData.OnShowOptionsMenu -= ShowMenu;
        GameManagerHandlerData.OnGameResumed -= OnGameResumed;
        
        ResolutionDropdown.onValueChanged.RemoveAllListeners();
        FullscreenToggle.onValueChanged.RemoveAllListeners();
        GraphicsQualityDropdown.onValueChanged.RemoveAllListeners();
        VolumeSlider.onValueChanged.RemoveAllListeners();
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        UpdateResolutionDropdownValues();
        FullscreenToggle.isOn = Screen.fullScreen;
        GraphicsQualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        VolumeSlider.SetValueWithoutNotify(AudioManagerDataHandler.GetVolume());
    }

    private void ShowMenu(Action onGoBack)
    {
        _onGoBack = onGoBack;
        
        // For unknown reasons, the dropdown is sometimes not placed properly, the rebuild fix this
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform)GraphicsQualityDropdown.transform);
        
        PlayShowAnimation();
    }
    
    public override void HideMenu()
    {
        if(!IsVisible) return;
        PlayHideAnimation();
        
        if (_onGoBack == null) return;
        _onGoBack.Invoke();
        _onGoBack = null;
    }

    private void OnGameResumed()
    {
        if(!IsVisible) return;
        PlayHideAnimation();
        _onGoBack = null;
    }

    private List<Resolution> GetDistinctResolutions()
    {
        List<Resolution> resolutions = Screen.resolutions.Select(res => new Resolution{ width = res.width, height = res.height }).Distinct().ToList();
        resolutions.Reverse();
        return resolutions;
    }

    private void UpdateResolutionDropdownValues()
    {
        _resolutions.Clear();
        _textResolutions.Clear();
        ResolutionDropdown.ClearOptions();
        
        _resolutions = GetDistinctResolutions();
        int currentResolutionIndex = -1;
        
        for (var i = 0; i < _resolutions.Count ; i++)
        {
            Resolution resolution = _resolutions[i];
            string textResolution = $"{resolution.width} x {resolution.height}";
            _textResolutions.Add(textResolution);
            
            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        if (currentResolutionIndex == -1)
        {
            AddResolutionToList(Screen.width, Screen.height, out currentResolutionIndex);
        }
        
        ResolutionDropdown.AddOptions(_textResolutions);
        ResolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
    }

    private void AddResolutionToList(int width, int height, out int index)
    {
        index = 0;
        
        for (int i = 0; i < _resolutions.Count; i++)
        {
            Resolution res = _resolutions[i];
            
            if (width > res.width)
            {
                index = i;
                break;
            }

            if (width == res.width)
            {
                for (int j = i; j < _resolutions.Count; j++)
                {
                    res = _resolutions[j];
                    
                    if (height > res.height || width != res.width)
                    {
                        index = j;
                        break;
                    }
                }

                if (index != 0) break;
            }
        }

        Resolution resolution = new Resolution { width = width, height = height };
        string resText = $"{width} x {height}";

        if (index == 0 && width < _resolutions[0].width)
        {
            _resolutions.Add(resolution);
            _textResolutions.Add(resText);
            index = _resolutions.Count - 1;
            return;
        }

        _resolutions.Insert(index, resolution);
        _textResolutions.Insert(index, resText);
    }
    
    private void OnRectTransformDimensionsChange()
    {
        UpdateResolutionDropdownValues();
    }

    private void OnResolutionSelected(int index)
    {
        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log($"Resolution set to {resolution.width} x {resolution.height}");
    }

    private void OnFullscreenToggled(bool newValue)
    {
        Screen.fullScreen = newValue;
    }

    private void OnGraphicsQualitySelected(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    private void OnVolumeUpdated(float value)
    {
        AudioManagerDataHandler.UpdateVolume(value);
    }
}