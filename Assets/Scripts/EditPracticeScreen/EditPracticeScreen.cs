using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditPracticeScreen : MonoBehaviour
{
    [SerializeField] private EditPracticeScreenView _view;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private OpenCardScreen _openCardScreen;
    [SerializeField] private EditHowFeelScreen _editHowFeelScreen;

    private string _newTitle;
    private string _newHr;
    private string _newMin;
    private string _newNote;
    private PracticeType _newType;

    private FilledPracticePlane _filledPracticePlane;

    public event Action BackButtonClicked;
    public event Action<FilledPracticePlane> NextButtonClicked;
    public event Action<FilledPracticePlane> DeleteButtonClicked; 

    public string NewTitle => _newTitle;
    public string NewHr => _newHr;
    public string NewMin => _newMin;
    public string NewNote => _newNote;
    public PracticeType NewType => _newType;

    private void Start()
    {
        _view.Disable();
        _newType = PracticeType.None;
    }

    private void OnEnable()
    {
        _view.TitleInputed += OnTitleInputed;
        _view.HrInputed += OnHrInputed;
        _view.MinInputed += OnMinIputed;
        _view.NoteInputed += OnNoteInputed;
        _view.YogaButtonClicked += SetYogaType;
        _view.MeditationButtonClicked += SetMeditationType;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.NextButtonClicked += OnNextButtonClicked;
        _view.DeleteButtonClicked += OnDeleteButtonClicked;

        _openCardScreen.EditButtonClicked += OpenScreen;

        _editHowFeelScreen.BackButtonClicked += _view.Enable;
    }

    private void OnDisable()
    {
        _view.TitleInputed -= OnTitleInputed;
        _view.HrInputed -= OnHrInputed;
        _view.MinInputed -= OnMinIputed;
        _view.NoteInputed -= OnNoteInputed;
        _view.YogaButtonClicked -= SetYogaType;
        _view.MeditationButtonClicked -= SetMeditationType;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.NextButtonClicked -= OnNextButtonClicked;
        _view.DeleteButtonClicked -= OnDeleteButtonClicked;
        
        _openCardScreen.EditButtonClicked -= OpenScreen;
        _editHowFeelScreen.BackButtonClicked -= _view.Enable;
    }

    private void OpenScreen(FilledPracticePlane filledPracticePlane)
    {
        if (filledPracticePlane == null)
            throw new ArgumentNullException(nameof(filledPracticePlane));

        ResetData();
        
        _filledPracticePlane = filledPracticePlane;
        _view.Enable();

        if (!string.IsNullOrEmpty(_filledPracticePlane.PracticeData.DurationHr))
        {
            _view.SetHrText(_filledPracticePlane.PracticeData.DurationHr);
        }
        else
        {
            _view.SetHrText(string.Empty);
        }
        
        if (!string.IsNullOrEmpty(_filledPracticePlane.PracticeData.DurationMin))
        {
            _view.SetMinText(_filledPracticePlane.PracticeData.DurationMin);
        }
        else
        {
            _view.SetMinText(string.Empty);
        }
        
        _view.SetTitleText(_filledPracticePlane.PracticeData.Title);
        _view.SetNoteText(_filledPracticePlane.PracticeData.Notes);

        if (_filledPracticePlane.PracticeData.Type == PracticeType.Meditation)
        {
            _view.OnMeditationButtonPressed();
        }
        else
        {
            _view.OnYogaButtonPressed();
        }
    }

    private void OnTitleInputed(string title)
    {
        _newTitle = title;
        ValidateInput();
    }

    private void OnHrInputed(string hr)
    {
        _newHr = hr;
        ValidateInput();
    }

    private void OnMinIputed(string min)
    {
        _newMin = min;
        ValidateInput();
    }

    private void SetYogaType()
    {
        _newType = PracticeType.Yoga;
        ValidateInput();
    }

    private void SetMeditationType()
    {
        _newType = PracticeType.Meditation;
        ValidateInput();
    }

    private void OnNoteInputed(string note)
    {
        _newNote = note;
        ValidateInput();
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        ResetData();
        _view.Disable();
    }

    private void OnNextButtonClicked()
    {
        if(_filledPracticePlane == null)
            return;
        
        NextButtonClicked?.Invoke(_filledPracticePlane);
        _view.Disable();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_newTitle) ||
                       (!string.IsNullOrEmpty(_newMin) || !string.IsNullOrEmpty(_newHr)) ||
                       !string.IsNullOrEmpty(_newNote) && _newType != PracticeType.None;

        _view.SetNextButtonActive(isValid);
    }

    private void ResetData()
    {
        _newNote = string.Empty;
        _newTitle = string.Empty;
        _newMin = string.Empty;
        _newHr = string.Empty;
        _view.ResetTypeButtons();
        _view.SetHrText(_newHr);
        _view.SetMinText(_newMin);
        _view.SetNoteText(_newNote);
        _view.SetTitleText(_newTitle);
        _filledPracticePlane = null;
        ValidateInput();
    }

    private void OnDeleteButtonClicked()
    {
        DeleteButtonClicked?.Invoke(_filledPracticePlane);
        _view.Disable();
    }
}