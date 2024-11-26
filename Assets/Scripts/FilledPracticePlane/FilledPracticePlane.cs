using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilledPracticePlane : MonoBehaviour
{
    private const string HrAddText = " h ";
    private const string MinAddText = " min";

    [SerializeField] private Color _practiceColor;
    [SerializeField] private Color _practicePlanColor;
    [SerializeField] private Sprite _yogaSprite;
    [SerializeField] private Sprite _meditationSprite;

    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private Image _statusImage;
    [SerializeField] private Image _typeImage;

    [SerializeField] private Button _openButton;


    public bool IsActive { get; private set; }
    public PracticeData PracticeData { get; private set; }

    public event Action<FilledPracticePlane> OpenButtonClicked;

    private void OnEnable()
    {
        _openButton.onClick.AddListener(OnOpenButttonClicked);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(OnOpenButttonClicked);
    }

    public void SetPracticeData(PracticeData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        PracticeData = data;
        _openButton.image.color = _practiceColor;
        UpdateUI();
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

    private void UpdateUI()
    {
        _title.text = PracticeData.Title;

        if (!string.IsNullOrEmpty(PracticeData.DurationHr))
        {
            _time.text = PracticeData.DurationHr + HrAddText;
        }

        if (!string.IsNullOrEmpty(PracticeData.DurationMin))
        {
            _time.text += PracticeData.DurationMin + MinAddText;
        }

        if (PracticeData.Type == PracticeType.Meditation)
        {
            _typeImage.sprite = _meditationSprite;
        }
        else
        {
            _typeImage.sprite = _yogaSprite;
        }
    }

    public void SetStatusSprite(Sprite statusSprite)
    {
        _statusImage.sprite = statusSprite;
    }

    public void ResetData()
    {
        PracticeData = null;
        _title.text = string.Empty;
        _time.text = string.Empty;
        _typeImage.sprite = null;
        _statusImage.sprite = null;
    }

    private void OnOpenButttonClicked() => OpenButtonClicked?.Invoke(this);
}