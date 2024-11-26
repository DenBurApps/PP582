using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddPlanMainScreenView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _addButton;
    [SerializeField] private GameObject _emptyPlane;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action AddButtonClicked;
    public event Action BackButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _addButton.onClick.AddListener(OnAddButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        _addButton.onClick.RemoveListener(OnAddButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetTransperent()
    {
        _screenVisabilityHandler.SetTransperent();
    }
    
    public void ToggleEmptyPlane(bool status)
    {
        _emptyPlane.gameObject.SetActive(status);
    }

    private void OnAddButtonClicked() => AddButtonClicked?.Invoke();
    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
}
