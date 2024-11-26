using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MoodButton : MonoBehaviour
{
    [SerializeField] private PracticeStatus _buttonStatus;

    private Button _button;

    public event Action<MoodButton> ButtonClicked;
    
    public PracticeStatus ButtonStatus => _buttonStatus;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ProcessButtonClick);
    }

    public void ResetButton()
    {
        _button.onClick.AddListener(ProcessButtonClick);
    }
    
    private void ProcessButtonClick()
    {
        ButtonClicked?.Invoke(this);
        _button.onClick.RemoveListener(ProcessButtonClick);
    }
}
