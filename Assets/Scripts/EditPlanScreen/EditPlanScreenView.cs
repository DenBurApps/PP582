using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EditPlanScreenView : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSaveButtonSprite;
    [SerializeField] private Sprite _selectedSaveButtonSprite;
    [SerializeField] private Color _defaultTextSaveButtonColor;
    [SerializeField] private Color _selectedTextSaveButtonColor;

    [SerializeField] private Sprite _selectedTypeButtonSprite;
    [SerializeField] private Sprite _defaultTypeButtonSprite;

    [SerializeField] private TMP_Text _saveButtonText;

    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_InputField _planTitleInput;
    [SerializeField] private TMP_InputField _practiceTitleInput;
    [SerializeField] private Button _meditationButton;
    [SerializeField] private Button _yogaButton;
    [SerializeField] private Button _10DaysButton;
    [SerializeField] private Button _20DaysButton;
    [SerializeField] private Button _30DaysButton;
    [SerializeField] private TMP_InputField _hrInput;
    [SerializeField] private TMP_InputField _minInput;


    [SerializeField] private TMP_Text _10DaysButtonText;
    [SerializeField] private TMP_Text _20DaysButtonText;
    [SerializeField] private TMP_Text _30DaysButtonText;

    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _deleteButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Color _defaultDaysButtonsTextColor;

    public event Action BackButtonClicked;
    public event Action YogaButtonClicked;
    public event Action MeditationButtonClicked;
    public event Action SaveButtonClicked;
    public event Action Days10ButtonClicked;
    public event Action Days20ButtonClicked;
    public event Action Days30ButtonClicked;
    public event Action DeleteButtonClicked;

    public event Action<string> PlanTitleInputed;
    public event Action<string> PracticeTitleInputed;
    public event Action<string> HrInputed;
    public event Action<string> MinInputed;

    public TMP_InputField PlanTitleInput => _planTitleInput;
    public TMP_InputField PracticeTitleInput => _practiceTitleInput;
    public TMP_InputField HrInput => _hrInput;
    public TMP_InputField MinInput => _minInput;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
        _planTitleInput.onValueChanged.AddListener(OnPlanTitleInputed);
        _practiceTitleInput.onValueChanged.AddListener(OnPracticeTitleInputed);
        _hrInput.onValueChanged.AddListener(OnHrInputed);
        _minInput.onValueChanged.AddListener(OnMinIputed);

        _yogaButton.onClick.AddListener(OnYogaButtonPressed);
        _meditationButton.onClick.AddListener(OnMeditationButtonPressed);

        _10DaysButton.onClick.AddListener((() => OnDaysButtonClicked(_10DaysButton)));
        _20DaysButton.onClick.AddListener((() => OnDaysButtonClicked(_20DaysButton)));
        _30DaysButton.onClick.AddListener((() => OnDaysButtonClicked(_30DaysButton)));

        _defaultDaysButtonsTextColor = _10DaysButtonText.color;

        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        _planTitleInput.onValueChanged.RemoveListener(OnPlanTitleInputed);
        _practiceTitleInput.onValueChanged.RemoveListener(OnPracticeTitleInputed);
        _hrInput.onValueChanged.RemoveListener(OnHrInputed);
        _minInput.onValueChanged.RemoveListener(OnMinIputed);

        _yogaButton.onClick.RemoveListener(OnYogaButtonPressed);
        _meditationButton.onClick.RemoveListener(OnMeditationButtonPressed);

        _10DaysButton.onClick.RemoveListener((() => OnDaysButtonClicked(_10DaysButton)));
        _20DaysButton.onClick.RemoveListener((() => OnDaysButtonClicked(_20DaysButton)));
        _30DaysButton.onClick.RemoveListener((() => OnDaysButtonClicked(_30DaysButton)));

        _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetPlanTitleText(string text)
    {
        _planTitleInput.text = text;
    }

    public void SetPracticeTitleText(string text)
    {
        _practiceTitleInput.text = text;
    }

    public void SetHrText(string text)
    {
        _hrInput.text = text;
    }

    public void SetMinText(string text)
    {
        _minInput.text = text;
    }

    public void SetSaveButtonActive(bool status)
    {
        if (status)
        {
            _saveButton.image.sprite = _selectedSaveButtonSprite;
            _saveButtonText.color = _selectedTextSaveButtonColor;
            _saveButton.enabled = true;
        }
        else
        {
            _saveButton.image.sprite = _defaultSaveButtonSprite;
            _saveButtonText.color = _defaultTextSaveButtonColor;
            _saveButton.enabled = false;
        }
    }

    public void ResetDurationButtons()
    {
        _10DaysButton.image.sprite = _defaultTypeButtonSprite;
        _20DaysButton.image.sprite = _defaultTypeButtonSprite;
        _30DaysButton.image.sprite = _defaultTypeButtonSprite;

        _10DaysButtonText.color = _defaultDaysButtonsTextColor;
        _20DaysButtonText.color = _defaultDaysButtonsTextColor;
        _30DaysButtonText.color = _defaultDaysButtonsTextColor;
    }

    public void ResetTypeButtons()
    {
        _yogaButton.image.sprite = _defaultTypeButtonSprite;
        _meditationButton.image.sprite = _defaultTypeButtonSprite;
    }

    public void OnYogaButtonPressed()
    {
        _yogaButton.image.sprite = _selectedTypeButtonSprite;
        _meditationButton.image.sprite = _defaultTypeButtonSprite;
        YogaButtonClicked?.Invoke();
    }

    public void OnMeditationButtonPressed()
    {
        _meditationButton.image.sprite = _selectedTypeButtonSprite;
        _yogaButton.image.sprite = _defaultTypeButtonSprite;
        MeditationButtonClicked?.Invoke();
    }

    public void On10DaysClicked()
    {
        OnDaysButtonClicked(_10DaysButton);
    }

    public void On20DaysButtonClicked()
    {
        OnDaysButtonClicked(_20DaysButton);
    }

    public void On30DaysButtonClicked()
    {
        OnDaysButtonClicked(_30DaysButton);
    }

    private void OnDaysButtonClicked(Button button)
    {
        if (button == _10DaysButton)
        {
            _10DaysButton.image.sprite = _selectedSaveButtonSprite;
            _10DaysButtonText.color = _selectedTextSaveButtonColor;
            _20DaysButton.image.sprite = _defaultTypeButtonSprite;
            _20DaysButtonText.color = _defaultDaysButtonsTextColor;
            _30DaysButton.image.sprite = _defaultTypeButtonSprite;
            _30DaysButtonText.color = _defaultDaysButtonsTextColor;
            Days10ButtonClicked?.Invoke();
        }
        else if (button == _20DaysButton)
        {
            _20DaysButton.image.sprite = _selectedSaveButtonSprite;
            _20DaysButtonText.color = _selectedTextSaveButtonColor;
            _10DaysButton.image.sprite = _defaultTypeButtonSprite;
            _10DaysButtonText.color = _defaultDaysButtonsTextColor;
            _30DaysButton.image.sprite = _defaultTypeButtonSprite;
            _30DaysButtonText.color = _defaultDaysButtonsTextColor;
            Days20ButtonClicked?.Invoke();
        }
        else
        {
            _30DaysButton.image.sprite = _selectedSaveButtonSprite;
            _30DaysButtonText.color = _selectedTextSaveButtonColor;
            _10DaysButton.image.sprite = _defaultTypeButtonSprite;
            _10DaysButtonText.color = _defaultDaysButtonsTextColor;
            _20DaysButton.image.sprite = _defaultTypeButtonSprite;
            _20DaysButtonText.color = _defaultDaysButtonsTextColor;
            Days30ButtonClicked?.Invoke();
        }
    }

    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnSaveButtonClicked() => SaveButtonClicked?.Invoke();
    private void OnPracticeTitleInputed(string text) => PracticeTitleInputed?.Invoke(text);
    private void OnPlanTitleInputed(string text) => PlanTitleInputed?.Invoke(text);
    private void OnHrInputed(string text) => HrInputed?.Invoke(text);
    private void OnMinIputed(string text) => MinInputed?.Invoke(text);
    private void OnDeleteButtonClicked() => DeleteButtonClicked?.Invoke();
}