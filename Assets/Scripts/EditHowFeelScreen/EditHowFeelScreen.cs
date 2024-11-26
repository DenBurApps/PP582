using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditHowFeelScreen : MonoBehaviour
{
    [SerializeField] private EditPracticeScreen _editPracticeScreen;
    [SerializeField] private EditHowFeelScreenView _view;
    [SerializeField] private MoodButton[] _moodButtons;
    [SerializeField] private StatusSpriteProvider _statusSpriteProvider;

    private FilledPracticePlane _filledPracticePlane;
    private PracticeStatus _newStatus;
    private MoodButton _currentButton;

    public event Action BackButtonClicked;
    public event Action SavedButtonClicked;
    public event Action<FilledPracticePlane> DeleteButtonClicked;

    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _view.SaveButtonClicked += SaveData;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.DeleteButtonClicked += OnDeleteButtonClicked;
        _editPracticeScreen.NextButtonClicked += OpenScreen;

        foreach (var button in _moodButtons)
        {
            button.ButtonClicked += ProcessMoodButtonClicked;
        }
    }

    private void OnDisable()
    {
        _view.SaveButtonClicked -= SaveData;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.DeleteButtonClicked -= OnDeleteButtonClicked;
        _editPracticeScreen.NextButtonClicked -= OpenScreen;
        
        foreach (var button in _moodButtons)
        {
            button.ButtonClicked -= ProcessMoodButtonClicked;
        }
    }

    private void OpenScreen(FilledPracticePlane filledPracticePlane)
    {
        if (filledPracticePlane == null)
            throw new ArgumentNullException(nameof(filledPracticePlane));

        _filledPracticePlane = filledPracticePlane;
        _view.SetStatusSprite(_filledPracticePlane.PracticeData.Status);
        
        _view.Enable();
    }

    private void ProcessMoodButtonClicked(MoodButton button)
    {
        if (button == null)
            throw new ArgumentNullException(nameof(button));

        if (_currentButton != null)
            _currentButton.ResetButton();

        _currentButton = button;
        _newStatus = _currentButton.ButtonStatus;
        _view.SetStatusSprite(_newStatus);
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool isValid = _newStatus != _filledPracticePlane.PracticeData.Status;
        _view.SetSaveButtonActive(isValid);
    }

    private void SaveData()
    {
        PracticeData dataToSave = new PracticeData(_editPracticeScreen.NewTitle, _editPracticeScreen.NewHr,
            _editPracticeScreen.NewMin, _editPracticeScreen.NewNote, _newStatus, _editPracticeScreen.NewType);


        _filledPracticePlane.SetPracticeData(dataToSave);
        _filledPracticePlane.SetStatusSprite(_statusSpriteProvider.GetNormalSprite(dataToSave.Status));
        SavedButtonClicked?.Invoke();
        _view.Disable();
    }
    
    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void OnDeleteButtonClicked()
    {
        DeleteButtonClicked?.Invoke(_filledPracticePlane);
        _view.Disable();
    }
}