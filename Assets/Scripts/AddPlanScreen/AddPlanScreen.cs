using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlanScreen : MonoBehaviour
{
    [SerializeField] private AddPlanScreenView _view;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private OpenCardScreen _openCardScreen;

    private string _planTitle;
    private string _practiceTitle;
    private PracticeType _type;
    private DurationVariants _durationType;
    private string _hr;
    private string _min;

    public event Action BackButtonClicked;
    public event Action<PracticePlanData> Saved; 
    
    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _screenStateManager.AddPlanScreenOpen += OpenScreen;

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

        _openCardScreen.AddToPracticeButtonClicked += OpenScreen;
    }

    private void OnDisable()
    {
        _screenStateManager.AddPlanScreenOpen -= OpenScreen;

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
        
        _openCardScreen.AddToPracticeButtonClicked -= OpenScreen;
    }

    private void OpenScreen()
    {
        ResetData();
        _view.Enable();
    }

    private void OpenScreen(PracticeData practiceData)
    {
        ResetData();
        
        if (practiceData == null)
            throw new ArgumentNullException(nameof(practiceData));

        _practiceTitle = practiceData.Title;
        _view.SetPracticeTitleText(_practiceTitle);

        if (practiceData.Type == PracticeType.Meditation)
        {
            _view.OnMeditationButtonPressed();
        }
        else
        {
            _view.OnYogaButtonPressed();
        }
        
        _type = practiceData.Type;
        
        _view.SetHrText(practiceData.DurationHr);
        _view.SetMinText(practiceData.DurationMin);

        _hr = practiceData.DurationHr;
        _min = practiceData.DurationMin;
        ValidateInput();
        _view.Enable();
    }
    
    private void OnPlanTitleInputed(string title)
    {
        _planTitle = title;
        ValidateInput();
    }
    
    private void OnPracticeTitleInputed(string title)
    {
        _practiceTitle = title;
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

    private void Set10Days()
    {
        _durationType = DurationVariants.Days10;
        ValidateInput();
    }

    private void Set20Days()
    {
        _durationType = DurationVariants.Days20;
        ValidateInput();
    }

    private void Set30Days()
    {
        _durationType = DurationVariants.Days30;
        ValidateInput();
    }
    
    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_planTitle) && (!string.IsNullOrEmpty(_min) || !string.IsNullOrEmpty(_hr)) &&
                       !string.IsNullOrEmpty(_practiceTitle) && _type != PracticeType.None && _durationType != DurationVariants.None;
        
        _view.SetSaveButtonActive(isValid);
    }

    private void SaveData()
    {
        PracticeData practiceData =
            new PracticeData(_practiceTitle, _hr, _min, string.Empty, PracticeStatus.None, _type);
        PracticePlanData planDataToSave = new PracticePlanData(_planTitle, practiceData, _durationType);
        
        Saved?.Invoke(planDataToSave);
        OnBackButtonClicked();
    }
    
    private void ResetData()
    {
        _planTitle = string.Empty;
        _practiceTitle = string.Empty;
        _min = string.Empty;
        _hr = string.Empty;
        _view.ResetTypeButtons();
        _view.ResetDurationButtons();
        _view.SetHrText(_hr);
        _view.SetMinText(_min);
        _view.SetPlanTitleText(_planTitle);
        _view.SetPracticeTitleText(_practiceTitle);
        ValidateInput();
    }

    private void OnBackButtonClicked()
    {
        ResetData();
        BackButtonClicked?.Invoke();
        _view.Disable();
    }
}
