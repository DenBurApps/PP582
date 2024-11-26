using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCardScreen : MonoBehaviour
{
    [SerializeField] private OpenCardScreenView _view;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private EditPracticeScreen _editPracticeScreen;
    
    private FilledPracticePlane _filledPracticePlane;

    public event Action<FilledPracticePlane> EditButtonClicked;
    public event Action BackButtonClicked;
    public event Action<PracticeData> AddToPracticeButtonClicked;
    
    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _view.EditButtonClicked += OnEditButtonClicked;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.AddToPracticePlanClicked += OnAddToPracticePlanClicked;
        _mainScreen.OnOpenFilledPracticeClicked += OnOpenWindow;
        _editPracticeScreen.BackButtonClicked += _view.Enable;
    }

    private void OnDisable()
    {
        _view.EditButtonClicked -= OnEditButtonClicked;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.AddToPracticePlanClicked -= OnAddToPracticePlanClicked;
        _mainScreen.OnOpenFilledPracticeClicked -= OnOpenWindow;
        _editPracticeScreen.BackButtonClicked -= _view.Enable;
    }

    private void OnOpenWindow(FilledPracticePlane filledPracticePlane)
    {
        if (filledPracticePlane == null)
            throw new ArgumentNullException(nameof(filledPracticePlane));

        _filledPracticePlane = filledPracticePlane;
        
        _view.SetNote(_filledPracticePlane.PracticeData.Notes);
        _view.SetTitle(_filledPracticePlane.PracticeData.Title);
        _view.SetMoodSprite(_filledPracticePlane.PracticeData.Status);
        _view.SetTypeSprite(_filledPracticePlane.PracticeData.Type);

        if (!string.IsNullOrEmpty(_filledPracticePlane.PracticeData.DurationHr))
        {
            _view.SetHr(_filledPracticePlane.PracticeData.DurationHr);
        }

        if (!string.IsNullOrEmpty(_filledPracticePlane.PracticeData.DurationMin))
        {
            _view.SetMin(_filledPracticePlane.PracticeData.DurationMin);
        }
        
        _view.SetTopImageSprite(_filledPracticePlane.PracticeData.Type);
        
        _view.Enable();
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void OnEditButtonClicked()
    {
        EditButtonClicked?.Invoke(_filledPracticePlane);
        _view.Disable();
    }

    private void OnAddToPracticePlanClicked()
    {
        AddToPracticeButtonClicked?.Invoke(_filledPracticePlane.PracticeData);
        _view.Disable();
    }
}
