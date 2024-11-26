using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class OpenCardScreenView : MonoBehaviour
{
    private const string HrAddText = " h ";
    private const string MinAddText = " min";

    [SerializeField] private Image _topImage;
    [SerializeField] private Sprite _topYogaSprite;
    [SerializeField] private Sprite _topMediatationSprite;
    
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _addPracticePlanButton;

    [SerializeField] private Sprite _yogaSprite;
    [SerializeField] private Sprite _meditationSprite;
    [SerializeField] private Image _moodImage;
    
    [SerializeField] private Image _typeImage;

    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _noteText;

    [SerializeField] private StatusSpriteProvider _statusSpriteProvider;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;
    public event Action EditButtonClicked;
    public event Action AddToPracticePlanClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }
    
    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _editButton.onClick.AddListener(OnEditButtonClicked);
        _addPracticePlanButton.onClick.AddListener(OnAddToPracticePlanClicked);
    }
    
    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
        _addPracticePlanButton.onClick.RemoveListener(OnAddToPracticePlanClicked);
    }

    public void SetTitle(string text)
    {
        _titleText.text = text;
    }

    public void SetNote(string note)
    {
        _noteText.text = note;
    }

    public void SetHr(string hr)
    {
        _timeText.text = hr + HrAddText;
    }

    public void SetMin(string min)
    {
        _timeText.text += min + MinAddText;
    }

    public void SetTopImageSprite(PracticeType type)
    {
        if (type == PracticeType.Meditation)
        {
            _topImage.sprite = _topMediatationSprite;
        }
        else
        {
            _topImage.sprite = _topYogaSprite;
        }
    }

    public void SetMoodSprite(PracticeStatus status)
    {
        _moodImage.sprite = _statusSpriteProvider.GetNormalSprite(status);
    }

    public void SetTypeSprite(PracticeType type)
    {
        if (type == PracticeType.Meditation)
        {
            _typeImage.sprite = _meditationSprite;
        }
        else
        {
            _typeImage.sprite = _yogaSprite;
        }
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }
    
    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnEditButtonClicked() => EditButtonClicked?.Invoke();
    private void OnAddToPracticePlanClicked() => AddToPracticePlanClicked?.Invoke();
}
