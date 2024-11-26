using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button _addPracticeButton;
    [SerializeField] private Button _addPracticePlanButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private GameObject _emptyPlane;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action SettingsButtonClicked;
    public event Action AddPracticeClikced;
    public event Action AddPracticePlanClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _addPracticeButton.onClick.AddListener(OnAddPracticeClicked);
        _addPracticePlanButton.onClick.AddListener(OnAddPracticePlanClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnDisable()
    {
        _addPracticeButton.onClick.RemoveListener(OnAddPracticeClicked);
        _addPracticePlanButton.onClick.RemoveListener(OnAddPracticePlanClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    public void ToggleEmptyPlane(bool status)
    {
        _emptyPlane.gameObject.SetActive(status);
    }

    public void MakeTransperent()
    {
        _screenVisabilityHandler.SetTransperent();
    }

    private void OnSettingsClicked() => SettingsButtonClicked?.Invoke();
    private void OnAddPracticeClicked() => AddPracticeClikced?.Invoke();
    private void OnAddPracticePlanClicked() => AddPracticePlanClicked?.Invoke();
}
