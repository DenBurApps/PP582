using System;
using UnityEngine;

public class ScreenStateManager : MonoBehaviour
{
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private AddPracticeScreen _addPracticeScreen;
    [SerializeField] private HowFeelScreen _howFeelScreen;
    [SerializeField] private OpenCardScreen _openCardScreen;
    [SerializeField] private AddPlanMainScreen _addPlanMainScreen;
    [SerializeField] private AddPlanScreen _addPlanScreen;
    [SerializeField] private SettingsScreen _settingsScreen;

    public event Action MainScreenOpen;
    public event Action AddPracticeOpen;
    public event Action AddPracticeSavedOpen;
    public event Action AddPlanMainScreenOpen;
    public event Action AddPlanScreenOpen;
    public event Action SettingsScreenOpen;
    
    private void OnEnable()
    {
        _mainScreen.AddPracticeClicked += OnAddPracticeOpen;
        _mainScreen.AddPracticePlanClicked += OnAddPlanMainScreenOpen;
        _mainScreen.SettingsClicked += OnSettingsOpen;
        
        _addPracticeScreen.BackButtonClicked += OnMainScreenOpen;
        _howFeelScreen.BackButtonClicked += OnAddPracticeSavedOpen;
        _openCardScreen.BackButtonClicked += OnMainScreenOpen;

        _addPlanMainScreen.BackButtonClicked += OnMainScreenOpen;
        _addPlanMainScreen.AddPlanClicked += OnAddPlanScreenOpen;

        _addPlanScreen.BackButtonClicked += OnAddPlanMainScreenOpen;

        _settingsScreen.BackButtonClicked += OnMainScreenOpen;
    }

    private void OnDisable()
    {
        _mainScreen.AddPracticeClicked -= OnAddPracticeOpen;
        _mainScreen.AddPracticePlanClicked -= OnAddPlanMainScreenOpen;
        _mainScreen.SettingsClicked -= OnSettingsOpen;
        
        _addPracticeScreen.BackButtonClicked -= OnMainScreenOpen;
        _howFeelScreen.BackButtonClicked -= OnAddPracticeSavedOpen;
        _openCardScreen.BackButtonClicked -= OnMainScreenOpen;
        
        _addPlanMainScreen.BackButtonClicked -= OnMainScreenOpen;
        _addPlanMainScreen.AddPlanClicked -= OnAddPlanScreenOpen;
        
        _addPlanScreen.BackButtonClicked -= OnAddPlanMainScreenOpen;
        
        _settingsScreen.BackButtonClicked -= OnMainScreenOpen;
    }

    private void OnMainScreenOpen() => MainScreenOpen?.Invoke();
    private void OnAddPracticeOpen() => AddPracticeOpen?.Invoke();
    private void OnAddPracticeSavedOpen() => AddPracticeSavedOpen?.Invoke();
    private void OnAddPlanMainScreenOpen() => AddPlanMainScreenOpen?.Invoke();
    private void OnAddPlanScreenOpen() => AddPlanScreenOpen?.Invoke();
    private void OnSettingsOpen() => SettingsScreenOpen?.Invoke();
}
