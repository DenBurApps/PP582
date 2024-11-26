using System;
using UnityEngine;

public class HowFeelScreen : MonoBehaviour
{
    [SerializeField] private HowFeelScreenView _view;
    [SerializeField] private MoodButton[] _moodButtons;
    [SerializeField] private AddPracticeScreen _addPracticeScreen;
    
    private PracticeStatus _status;
    private MoodButton _currentButton;

    public event Action BackButtonClicked;
    public event Action<PracticeData> SaveButtonClicked;
    
    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        foreach (var button in _moodButtons)
        {
            button.ButtonClicked += ProcessMoodButtonClicked;
        }

        _addPracticeScreen.NextButtonClicked += OpenScreen;
        
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.SaveButtonClicked += SaveData;
    }

    private void OnDisable()
    {
        foreach (var button in _moodButtons)
        {
            button.ButtonClicked -= ProcessMoodButtonClicked;
        }

        _addPracticeScreen.NextButtonClicked -= OpenScreen;
        
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.SaveButtonClicked -= SaveData;
    }

    private void OpenScreen()
    {
        ResetData();
        ValidateInput();
        _view.Enable();
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

    private void SaveData()
    {
        PracticeData dataToSave = new PracticeData(_addPracticeScreen.Title, _addPracticeScreen.Hr,
            _addPracticeScreen.Min, _addPracticeScreen.Note, _status, _addPracticeScreen.Type);
        
        SaveButtonClicked?.Invoke(dataToSave);
        ResetData();
        _view.Disable();
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
        ResetData();
    }

    private void ResetData()
    {
        if(_currentButton != null)
            _currentButton.ResetButton();
        
        _status = PracticeStatus.None;
        _view.SetDefaultStatusSprites();
    }
}
