using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class EditPracticeScreenView : MonoBehaviour
{
    [SerializeField] private Sprite _defaultNextButtonSprite;
    [SerializeField] private Sprite _selectedNextButtonSprite;
    [SerializeField] private Color _defaultTextNextButtonColor;
    [SerializeField] private Color _selectedTextNextButtonColor;

    [SerializeField] private Sprite _selectedTypeButtonSprite;
    [SerializeField] private Sprite _defaultTypeButtonSprite;

    [SerializeField] private TMP_Text _nextButtonText;
    
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_InputField _titleInput;
    [SerializeField] private Button _meditationButton;
    [SerializeField] private Button _yogaButton;
    [SerializeField] private TMP_InputField _hrInput;
    [SerializeField] private TMP_InputField _minInput;
    [SerializeField] private TMP_InputField _noteInput;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _deleteButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;
    public event Action YogaButtonClicked;
    public event Action MeditationButtonClicked;
    public event Action NextButtonClicked;
    public event Action DeleteButtonClicked;
    public event Action<string> TitleInputed;
    public event Action<string> HrInputed;
    public event Action<string> MinInputed;
    public event Action<string> NoteInputed;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _nextButton.onClick.AddListener(OnNextButtonClicked);
        _meditationButton.onClick.AddListener(OnMeditationButtonPressed);
        _yogaButton.onClick.AddListener(OnYogaButtonPressed);
        _titleInput.onValueChanged.AddListener(OnTitleInputed);
        _hrInput.onValueChanged.AddListener(OnHrInputed);
        _minInput.onValueChanged.AddListener(OnMinIputed);
        _noteInput.onValueChanged.AddListener(OnNotesInputed);
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _nextButton.onClick.RemoveListener(OnNextButtonClicked);
        _meditationButton.onClick.RemoveListener(OnMeditationButtonPressed);
        _yogaButton.onClick.RemoveListener(OnYogaButtonPressed);
        _titleInput.onValueChanged.RemoveListener(OnTitleInputed);
        _hrInput.onValueChanged.RemoveListener(OnHrInputed);
        _minInput.onValueChanged.RemoveListener(OnMinIputed);
        _noteInput.onValueChanged.RemoveListener(OnNotesInputed);
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

    public void SetTitleText(string text)
    {
        _titleInput.text = text;
    }

    public void SetHrText(string text)
    {
        _hrInput.text = text;
    }

    public void SetMinText(string text)
    {
        _minInput.text = text;
    }

    public void SetNoteText(string text)
    {
        _noteInput.text = text;
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

    public void ResetTypeButtons()
    {
        _yogaButton.image.sprite = _defaultTypeButtonSprite;
        _meditationButton.image.sprite = _defaultTypeButtonSprite;
    }

    public void SetNextButtonActive(bool status)
    {
        if (status)
        {
            _nextButton.image.sprite = _selectedNextButtonSprite;
            _nextButtonText.color = _selectedTextNextButtonColor;
            _nextButton.enabled = true;
        }
        else
        {
            _nextButton.image.sprite = _defaultNextButtonSprite;
            _nextButtonText.color = _defaultTextNextButtonColor;
            _nextButton.enabled = false;
        }
    }

    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnNextButtonClicked() => NextButtonClicked?.Invoke();
    private void OnTitleInputed(string text) => TitleInputed?.Invoke(text);
    private void OnHrInputed(string text) => HrInputed?.Invoke(text);
    private void OnMinIputed(string text) => MinInputed?.Invoke(text);
    private void OnNotesInputed(string text) => NoteInputed?.Invoke(text);
    private void OnDeleteButtonClicked() => DeleteButtonClicked?.Invoke();
}
