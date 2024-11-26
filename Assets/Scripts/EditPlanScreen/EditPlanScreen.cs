using System;
using UnityEngine;

public class EditPlanScreen : MonoBehaviour
{
    [SerializeField] private EditPlanScreenView _view;
    [SerializeField] private AddPlanMainScreen _addPlanMainScreen;
    
    private string _newPlanTitle;
    private string _newPracticeTitle;
    private PracticeType _newType;
    private DurationVariants _newDurationType;
    private string _newHr;
    private string _newMin;

    private FilledPracticePlanPlane _filledPracticePlanPlane;

    public event Action BackButtonClicked;
    public event Action Saved;
    public event Action<FilledPracticePlanPlane> Deleted;

    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _view.HrInputed += OnHrInputed;
        _view.PlanTitleInputed += OnPlanTitleInputed;
        _view.PracticeTitleInputed += OnPracticeTitleInputed;
        _view.MinInputed += OnMinIputed;
        _view.YogaButtonClicked += SetYogaType;
        _view.MeditationButtonClicked += SetMeditationType;
        _view.Days10ButtonClicked += Set10Days;
        _view.Days20ButtonClicked += Set20Days;
        _view.Days30ButtonClicked += Set30Days;

        _view.BackButtonClicked += OnBackButtonClicked;
        _view.SaveButtonClicked += SaveData;
        _view.DeleteButtonClicked += OnDeleted;

        _addPlanMainScreen.EditPracticePlanClicked += OpenScreen;
    }

    private void OnDisable()
    {
        _view.HrInputed -= OnHrInputed;
        _view.PlanTitleInputed -= OnPlanTitleInputed;
        _view.PracticeTitleInputed -= OnPracticeTitleInputed;
        _view.MinInputed -= OnMinIputed;
        _view.YogaButtonClicked -= SetYogaType;
        _view.MeditationButtonClicked -= SetMeditationType;
        _view.Days10ButtonClicked -= Set10Days;
        _view.Days20ButtonClicked -= Set20Days;
        _view.Days30ButtonClicked -= Set30Days;

        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.SaveButtonClicked -= SaveData;
        _view.DeleteButtonClicked -= OnDeleted;

        _addPlanMainScreen.EditPracticePlanClicked -= OpenScreen;
    }
    

    private void OpenScreen(FilledPracticePlanPlane filledPracticePlanPlane)
    {
        ResetData();

        if (filledPracticePlanPlane == null)
            throw new ArgumentNullException(nameof(filledPracticePlanPlane));

        _filledPracticePlanPlane = filledPracticePlanPlane;

        _view.SetPracticeTitleText(_filledPracticePlanPlane.PracticeData.Title);
        _view.SetPlanTitleText(_filledPracticePlanPlane.PracticePlanData.PlanTitle);

        if (_filledPracticePlanPlane.PracticeData.Type == PracticeType.Meditation)
        {
            _view.OnMeditationButtonPressed();
        }
        else
        {
            _view.OnYogaButtonPressed();
        }

        _view.SetHrText(_filledPracticePlanPlane.PracticeData.DurationHr);
        _view.SetMinText(_filledPracticePlanPlane.PracticeData.DurationMin);

        if (_filledPracticePlanPlane.PracticePlanData.Duration == DurationVariants.Days10)
        {
            _view.On10DaysClicked();
        }
        else if (_filledPracticePlanPlane.PracticePlanData.Duration == DurationVariants.Days20)
        {
            _view.On20DaysButtonClicked();
        }
        else
        {
            _view.On30DaysButtonClicked();
        }
        
        ValidateInput();
        _view.Enable();
    }

    private void OnPlanTitleInputed(string title)
    {
        _newPlanTitle = title;
        ValidateInput();
    }

    private void OnPracticeTitleInputed(string title)
    {
        _newPracticeTitle = title;
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

    private void Set10Days()
    {
        _newDurationType = DurationVariants.Days10;
        ValidateInput();
    }

    private void Set20Days()
    {
        _newDurationType = DurationVariants.Days20;
        ValidateInput();
    }

    private void Set30Days()
    {
        _newDurationType = DurationVariants.Days30;
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_view.PlanTitleInput.text) &&
                       !string.IsNullOrEmpty(_view.PracticeTitleInput.text) && (!string.IsNullOrEmpty(_view.HrInput.text) ||
                       !string.IsNullOrEmpty(_view.MinInput.text)) && _newType != PracticeType.None &&
                       _newDurationType != DurationVariants.None;

        _view.SetSaveButtonActive(isValid);
    }

    private void SaveData()
    {
        PracticeData practiceData =
            new PracticeData(_newPracticeTitle, _newHr, _newMin, string.Empty, PracticeStatus.None, _newType);
        PracticePlanData planDataToSave = new PracticePlanData(_newPlanTitle, practiceData, _newDurationType);

        _filledPracticePlanPlane.SetPlanData(planDataToSave);
        _filledPracticePlanPlane.SetCompleteButtonSprite();
        
        Saved?.Invoke();
        _view.Disable();
    }

    private void ResetData()
    {
        _newPlanTitle = string.Empty;
        _newPracticeTitle = string.Empty;
        _newMin = string.Empty;
        _newHr = string.Empty;
        _view.ResetTypeButtons();
        _view.ResetDurationButtons();
        _view.SetHrText(_newHr);
        _view.SetMinText(_newMin);
        _view.SetPlanTitleText(_newPlanTitle);
        _view.SetPracticeTitleText(_newPracticeTitle);
        ValidateInput();
    }

    private void OnBackButtonClicked()
    {
        _view.Disable();
        BackButtonClicked?.Invoke();
    }

    private void OnDeleted()
    {
        Deleted?.Invoke(_filledPracticePlanPlane);
        _view.Disable();
    }
}