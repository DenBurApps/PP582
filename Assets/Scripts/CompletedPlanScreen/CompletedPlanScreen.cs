using System;
using UnityEngine;

public class CompletedPlanScreen : MonoBehaviour
{
    [SerializeField] private CompletedPlanScreenView _view;
    [SerializeField] private AddPlanMainScreen _addPlanMainScreen;
    [SerializeField] private MoodButton[] _moodButtons;
    
    private PracticeStatus _status;
    private MoodButton _currentButton;

    public event Action BackButtonClicked;
    public event Action SaveButtonClicked;
    
    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _addPlanMainScreen.CompletePlanClicked += OpenScreen;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.SaveButtonClicked += OnSavedButtonClicked;
        
        foreach (var button in _moodButtons)
        {
            button.ButtonClicked += ProcessMoodButtonClicked;
        }
    }

    private void OnDisable()
    {
        _addPlanMainScreen.CompletePlanClicked -= OpenScreen;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.SaveButtonClicked -= OnSavedButtonClicked;
        
        foreach (var button in _moodButtons)
        {
            button.ButtonClicked -= ProcessMoodButtonClicked;
        }
    }

    private void OpenScreen()
    {
        _view.Enable();
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        ResetData();
        _view.Disable();
    }

    private void OnSavedButtonClicked()
    {
        SaveButtonClicked?.Invoke();
        ResetData();
        _view.Disable();
    }
    
    private void ProcessMoodButtonClicked(MoodButton button)
    {
        if (button == null)
            throw new ArgumentNullException(nameof(button));
        
        if(_currentButton != null)
            _currentButton.ResetButton();

        _currentButton = button;
        _status = _currentButton.ButtonStatus;
        _view.SetStatusSprite(_status);
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool isValid = _status != PracticeStatus.None;
        _view.SetSaveButtonActive(isValid);
    }
    
    private void ResetData()
    {
        if(_currentButton != null)
            _currentButton.ResetButton();
        
        _status = PracticeStatus.None;
        _view.SetDefaultStatusSprites();
        _view.SetNote(string.Empty);
    }
}
