using System;
using UnityEngine;

public class AddPracticeScreen : MonoBehaviour
{
    [SerializeField] private AddPracticeScreenView _view;
    [SerializeField] private ScreenStateManager _screenStateManager;

    private string _title;
    private string _hr;
    private string _min;
    private string _note;


    public event Action BackButtonClicked;
    public event Action NextButtonClicked;

    private PracticeType _type;

    public string Title => _title;
    public string Hr => _hr;
    public string Min => _min;
    public string Note => _note;
    public PracticeType Type => _type;

    private void Start()
    {
        _view.Disable();
        _type = PracticeType.None;
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

        _screenStateManager.AddPracticeOpen += OpenScreen;
        _screenStateManager.AddPracticeSavedOpen += _view.Enable;
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
        
        _screenStateManager.AddPracticeOpen -= OpenScreen;
        _screenStateManager.AddPracticeSavedOpen -= _view.Enable;
    }

    private void OpenScreen()
    {
        ResetData();
        _view.Enable();
    }
    
    private void OnTitleInputed(string title)
    {
        _title = title;
        ValidateInput();
    }

    private void OnHrInputed(string hr)
    {
        _hr = hr;
        ValidateInput();
    }

    private void OnMinIputed(string min)
    {
        _min = min;
        ValidateInput();
    }

    private void SetYogaType()
    {
        _type = PracticeType.Yoga;
        ValidateInput();
    }

    private void SetMeditationType()
    {
        _type = PracticeType.Meditation;
        ValidateInput();
    }

    private void OnNoteInputed(string note)
    {
        _note = note;
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
        NextButtonClicked?.Invoke();
        _view.Disable();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_title) && (!string.IsNullOrEmpty(_min) || !string.IsNullOrEmpty(_hr)) &&
                       !string.IsNullOrEmpty(_note) && _type != PracticeType.None;
        
        _view.SetNextButtonActive(isValid);
    }

    private void ResetData()
    {
        _note = string.Empty;
        _title = string.Empty;
        _min = string.Empty;
        _hr = string.Empty;
        _view.ResetTypeButtons();
        _view.SetHrText(_hr);
        _view.SetMinText(_min);
        _view.SetNoteText(_note);
        _view.SetTitleText(_title);
        ValidateInput();
    }
}
