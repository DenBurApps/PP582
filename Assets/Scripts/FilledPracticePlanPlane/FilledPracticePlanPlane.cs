using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilledPracticePlanPlane : MonoBehaviour
{
    private const string HrAddText = " h ";
    private const string MinAddText = " min";

    [SerializeField] private Sprite _defaultCompleteButtonSprite;
    [SerializeField] private Sprite _selectedCompleteButtonSprite;

    [SerializeField] private Image _filledProcessImage;

    [SerializeField] private TMP_Text _planTitle;
    [SerializeField] private TMP_Text _duration;
    [SerializeField] private TMP_Text _practiceTitle;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Button _completeButton;
    [SerializeField] private Button _editButton;

    private float _maxDuration;
    private float _currentDuration;
    private bool _isCompleted;

    public event Action<FilledPracticePlanPlane> PlanCompleted;
    public event Action<FilledPracticePlanPlane> EditButtonClicked;
    public event Action CompleteButtonClicked;

    public PracticePlanData PracticePlanData { get; private set; }
    public PracticeData PracticeData { get; private set; }
    public bool IsActive { get; private set; }

    private void Awake()
    {
        _isCompleted = false;
    }

    private void OnEnable()
    {
        _completeButton.onClick.AddListener(OnCompleteButtonClicked);
        _editButton.onClick.AddListener(OnEditButtonClicked);
    }

    private void OnDisable()
    {
        _completeButton.onClick.RemoveListener(OnCompleteButtonClicked);
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
    }


    public void Enable()
    {
        gameObject.SetActive(true);
        IsActive = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void SetPlanData(PracticePlanData practicePlanData)
    {
        if (practicePlanData == null)
            throw new ArgumentNullException(nameof(practicePlanData));

        PracticePlanData = practicePlanData;

        _planTitle.text = string.IsNullOrEmpty(practicePlanData.PlanTitle)
            ? "Untitled Plan"
            : practicePlanData.PlanTitle;

        SetDuration(practicePlanData.Duration);

        if (_currentDuration > 0)
        {
            PracticePlanData.CompleteCount = _currentDuration;
        }
        else
        {
            _currentDuration = Mathf.Max(0, practicePlanData.CompleteCount);
        }

        if (practicePlanData.PracticeData == null)
            throw new ArgumentNullException(nameof(practicePlanData.PracticeData));

        PracticeData = PracticePlanData.PracticeData;

        if (_isCompleted)
        {
            PracticePlanData.IsCompleted = _isCompleted;
        }
        else
        {
            _isCompleted = PracticePlanData.IsCompleted;
        }

        SetDurationText();
        SetTimeText();
        SetPracticeTitleText();
        SetFilledImage();
        TestDataChange();
    }


    public void ResetData()
    {
        _planTitle.text = string.Empty;
        _duration.text = "0/0";
        _practiceTitle.text = string.Empty;
        _timeText.text = string.Empty;
        _filledProcessImage.fillAmount = 0f;

        _completeButton.image.sprite = _defaultCompleteButtonSprite;
        _editButton.gameObject.SetActive(false);

        _maxDuration = 0f;
        _currentDuration = 0f;
        _isCompleted = false;

        PracticePlanData = null;
        PracticeData = null;

        Disable();
    }

    private void SetDuration(DurationVariants duration)
    {
        _maxDuration = duration switch
        {
            DurationVariants.Days10 => 10,
            DurationVariants.Days20 => 20,
            DurationVariants.Days30 => 30
        };
    }

    private void SetFilledImage()
    {
        if (_maxDuration > 0 && _currentDuration > 0)
        {
            float currentFilledAmount = _currentDuration / _maxDuration;
            _filledProcessImage.fillAmount = currentFilledAmount;
        }
        else
        {
            _filledProcessImage.fillAmount = 0;
        }
    }

    private void SetDurationText()
    {
        if (_maxDuration > 0)
            _duration.text = _currentDuration + "/" + _maxDuration;
    }

    private void SetPracticeTitleText()
    {
        _practiceTitle.text = PracticeData.Title;
    }

    public void SetCompleteButtonSprite()
    {
        if (_isCompleted)
        {
            _completeButton.image.sprite = _selectedCompleteButtonSprite;
        }
        else
        {
            _completeButton.image.sprite = _defaultCompleteButtonSprite;
        }
    }

    private void TestDataChange()
    {
        DateTime currentDate = DateTime.Now;
        DateTime parsedPreviousDate;
        
        if (!string.IsNullOrEmpty(PracticePlanData.CompletionDate))
        {
            parsedPreviousDate = DateTime.Parse(PracticePlanData.CompletionDate);
            
            if (currentDate.Date > parsedPreviousDate.Date)
            {
                _isCompleted = false;
                PracticePlanData.IsCompleted = false;
            }
            else
            {
                _isCompleted = true;
                PracticePlanData.IsCompleted = true;
            }
        }
        
        SetCompleteButtonSprite();
    }

    private void SetTimeText()
    {
        if (!string.IsNullOrEmpty(PracticeData.DurationHr))
        {
            _timeText.text = PracticeData.DurationHr + HrAddText;
        }

        if (!string.IsNullOrEmpty(PracticeData.DurationMin))
        {
            _timeText.text += PracticeData.DurationMin + MinAddText;
        }
    }

    private void OnCompleteButtonClicked()
    {
        DateTime parsedPreviousDate;
        DateTime currentDate = DateTime.Now;

        if (string.IsNullOrEmpty(PracticePlanData.CompletionDate) && !_isCompleted)
        {
            ProcessCompletion();
        }
        else if (!string.IsNullOrEmpty(PracticePlanData.CompletionDate)
                 && !PracticePlanData.IsCompleted
                 && !_isCompleted
                 && DateTime.TryParse(PracticePlanData.CompletionDate, out parsedPreviousDate))
        {
            if (currentDate.Date > parsedPreviousDate.Date)
            {
                ProcessCompletion();
            }
        }
    }

    private void ProcessCompletion()
    {
        if (_currentDuration < _maxDuration)
        {
            _currentDuration++;
            PracticePlanData.CompleteCount = _currentDuration;
            _isCompleted = true;
            PracticePlanData.IsCompleted = _isCompleted;
            PracticePlanData.CompletionDate = DateTime.Now.ToString("yyyy-MM-dd");
            
            if (_currentDuration >= _maxDuration)
            {
                PlanCompleted?.Invoke(this);
                return;
            }

            SetFilledImage();
            SetCompleteButtonSprite();
            SetDurationText();
            CompleteButtonClicked?.Invoke();
        }
    }

    private void OnEditButtonClicked()
    {
        EditButtonClicked?.Invoke(this);
    }
}