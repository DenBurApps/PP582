using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class HowFeelScreenView : MonoBehaviour
{
    [SerializeField] private Button _backButton;

    [SerializeField] private Sprite _defaultSaveButtonSprite;
    [SerializeField] private Sprite _selectedSaveButtonSprite;
    [SerializeField] private Color _defaultTextSaveButtonColor;
    [SerializeField] private Color _selectedTextSaveButtonColor;
    
    [SerializeField] private Sprite _bigCircleDefaultSprite;
    [SerializeField] private Sprite _bigCircleSelectedSprite;
    [SerializeField] private Sprite _smallCircleDefaultSprite;
    
    [SerializeField] private Image _bigCircle;
    [SerializeField] private Image _smallCircle;

    [SerializeField] private Button _saveButton;
    [SerializeField] private TMP_Text _saveButtonText;

    [SerializeField] private StatusSpriteProvider _statusSpriteProvider;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action BackButtonClicked;
    public event Action SaveButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }
    
    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
    }
    
    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetStatusSprite(PracticeStatus status)
    {
        _smallCircle.sprite = _statusSpriteProvider.GetGlowingSprite(status);
        _bigCircle.sprite = _bigCircleSelectedSprite;
    }

    public void SetDefaultStatusSprites()
    {
        _bigCircle.sprite = _bigCircleDefaultSprite;
        _smallCircle.sprite = _smallCircleDefaultSprite;
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

    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnSaveButtonClicked() => SaveButtonClicked?.Invoke();
}
