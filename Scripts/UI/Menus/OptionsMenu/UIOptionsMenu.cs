using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIOptionsMenu : UIAnimatedElement
{
    [Header("Options Menu Elements")] 
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;
    public TMP_Dropdown GraphicsQualityDropdown;
    public Slider VolumeSlider;
    public Button GoBackButton;

    private List<Resolution> _resolutions = new List<Resolution>();
    private List<string> _textResolutions = new List<string>();
    
    private Action _onGoBack;
    
    protected override void EventHandlerRegister()
    {
        UIOptionsMenuHandlerData.OnShowMenu += ShowMenu;
        GameManagerHandlerData.OnGameResumed += HideMenu;
        
        ResolutionDropdown.onValueChanged.AddListener(OnResolutionSelected);
        FullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        GraphicsQualityDropdown.onValueChanged.AddListener(OnGraphicsQualitySelected);
        VolumeSlider.onValueChanged.AddListener(OnVolumeUpdated);
        GoBackButton.onClick.AddListener(GoBack);
        
    }
    
    protected override void EventHandlerUnRegister()
    {
        UIOptionsMenuHandlerData.OnShowMenu -= ShowMenu;
        GameManagerHandlerData.OnGameResumed -= HideMenu;
        
        ResolutionDropdown.onValueChanged.RemoveAllListeners();
        FullscreenToggle.onValueChanged.RemoveAllListeners();
        GraphicsQualityDropdown.onValueChanged.RemoveAllListeners();
        VolumeSlider.onValueChanged.RemoveAllListeners();
        GoBackButton.onClick.RemoveAllListeners();
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        InitializeResolutionDropdown();
        FullscreenToggle.isOn = Screen.fullScreen;
        GraphicsQualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        VolumeSlider.SetValueWithoutNotify(AudioServiceDataHandler.GetVolume());
    }

    private void ShowMenu(Action onGoBack)
    {
        _onGoBack = onGoBack;
        
        // For unknown reasons, the dropdown is sometimes not placed properly, the rebuild fix this
        LayoutRebuilder.MarkLayoutForRebuild((RectTransform)GraphicsQualityDropdown.transform);
        
        PlayShowAnimation();
    }

    private void InitializeResolutionDropdown()
    {
        _resolutions.Clear();
        _resolutions = GetDistinctResolutions();
        UpdateResolutionDropdownValues();
    }

    private List<Resolution> GetDistinctResolutions()
    {
        return Screen.resolutions.Select(res => new Resolution{ width = res.width, height = res.height }).Distinct().ToList();
    }

    private void UpdateResolutionDropdownValues()
    {
        _textResolutions.Clear();
        ResolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        
        for (var i = _resolutions.Count - 1; i >= 0 ; i--)
        {
            Resolution resolution = _resolutions[i];
            string textResolution = $"{resolution.width} x {resolution.height}";
            _textResolutions.Add(textResolution);
            
            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        ResolutionDropdown.AddOptions(_textResolutions);
        ResolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
    }
    
    private void OnRectTransformDimensionsChange()
    {
        List<Resolution> resolutions = GetDistinctResolutions();
        if (resolutions.Count != _resolutions.Count)
        {
            _resolutions = resolutions;
            UpdateResolutionDropdownValues();
        }
    }

    private void OnResolutionSelected(int index)
    {
        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
        AudioServiceDataHandler.UpdateVolume(value);
    }

    private void GoBack()
    {
        if(!IsVisible) return;
        PlayHideAnimation();
        _onGoBack?.Invoke();
    }

    private void HideMenu()
    {
        if(!IsVisible) return;
        PlayHideAnimation();
    }
}