using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private MainScreenView _view;
    [SerializeField] private List<FilledPracticePlane> _filledPracticePlanes;
    [SerializeField] private StatusSpriteProvider _statusSpriteProvider;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private HowFeelScreen _howFeelScreen;
    [SerializeField] private EditPracticeScreen _editPracticeScreen;
    [SerializeField] private EditHowFeelScreen _editHowFeelScreen;
    [SerializeField] private SettingsScreen _settingsScreen;

    private string _filePath => Path.Combine(Application.persistentDataPath, "savefileMain.json");

    private List<int> _availableIndexes = new List<int>();

    public event Action SettingsClicked;
    public event Action AddPracticeClicked;
    public event Action AddPracticePlanClicked;
    public event Action<FilledPracticePlane> OnOpenFilledPracticeClicked;

    private void Start()
    {
        _view.Enable();
        DisableAllWindows();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPracticePlanes.Count);
        LoadPractices();
    }

    private void OnEnable()
    {
        _view.AddPracticeClikced += OnAddPracticeClicked;
        _view.AddPracticePlanClicked += OnAddPracicePlanClicked;
        _view.SettingsButtonClicked += OnSettingsClicked;

        _screenStateManager.MainScreenOpen += _view.Enable;

        _howFeelScreen.SaveButtonClicked += EnableItem;

        _editPracticeScreen.DeleteButtonClicked += DeleteItemPlane;

        _editHowFeelScreen.DeleteButtonClicked += DeleteItemPlane;
        _editHowFeelScreen.SavedButtonClicked += _view.Enable;
        _editHowFeelScreen.SavedButtonClicked += SavePracticePlanes;

        _settingsScreen.SettingsDisabled += _view.Disable;
        _settingsScreen.SettingsOpened += _view.MakeTransperent;
    }

    private void OnDisable()
    {
        _view.AddPracticeClikced -= OnAddPracticeClicked;
        _view.AddPracticePlanClicked -= OnAddPracicePlanClicked;
        _view.SettingsButtonClicked -= OnSettingsClicked;

        _screenStateManager.MainScreenOpen -= _view.Enable;
        _howFeelScreen.SaveButtonClicked -= EnableItem;
        _editPracticeScreen.DeleteButtonClicked -= DeleteItemPlane;

        _editHowFeelScreen.DeleteButtonClicked -= DeleteItemPlane;
        _editHowFeelScreen.SavedButtonClicked -= _view.Enable;
        _editHowFeelScreen.SavedButtonClicked -= SavePracticePlanes;

        _settingsScreen.SettingsDisabled -= _view.Disable;
        _settingsScreen.SettingsOpened -= _view.MakeTransperent;
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
        _view.MakeTransperent();
    }

    private void OnAddPracticeClicked()
    {
        AddPracticeClicked?.Invoke();
        _view.Disable();
    }

    private void OnAddPracicePlanClicked()
    {
        AddPracticePlanClicked?.Invoke();
        _view.Disable();
    }

    private void EnableItem(PracticeData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        if (_availableIndexes.Count > 0)
        {
            int availableIndex = _availableIndexes[0];
            var currentFilledItemPlane = _filledPracticePlanes[availableIndex];
            _availableIndexes.RemoveAt(0);

            if (!currentFilledItemPlane.IsActive)
            {
                currentFilledItemPlane.Enable();
                currentFilledItemPlane.SetPracticeData(data);
                currentFilledItemPlane.SetStatusSprite(_statusSpriteProvider.GetNormalSprite(data.Status));
                currentFilledItemPlane.OpenButtonClicked += OpenPractice;
            }
        }

        _view.Enable();
        SavePracticePlanes();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPracticePlanes.Count);
    }

    private void DeleteItemPlane(FilledPracticePlane plane)
    {
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));

        _view.Enable();

        int index = _filledPracticePlanes.IndexOf(plane);

        if (index >= 0 && !_availableIndexes.Contains(index))
        {
            _availableIndexes.Add(index);
        }

        plane.OpenButtonClicked -= OpenPractice;
        plane.ResetData();
        plane.Disable();
        
        Debug.Log(_availableIndexes.Count);
        Debug.Log(_filledPracticePlanes.Count);

        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPracticePlanes.Count);

        SavePracticePlanes();
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _filledPracticePlanes.Count; i++)
        {
            _filledPracticePlanes[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void OpenPractice(FilledPracticePlane filledPracticePlane)
    {
        OnOpenFilledPracticeClicked?.Invoke(filledPracticePlane);
        _view.Disable();
    }

    public void SavePracticePlanes()
    {
        List<PracticeData> practiceDatas = new List<PracticeData>();

        foreach (var window in _filledPracticePlanes)
        {
            if (window.PracticeData != null)
            {
                practiceDatas.Add(window.PracticeData);
            }
        }

        PracticeDataList itemDataList = new PracticeDataList(practiceDatas);
        string json = JsonUtility.ToJson(itemDataList, true);

        try
        {
            File.WriteAllText(_filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save trip data: " + e.Message);
        }
    }

    private void LoadPractices()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                PracticeDataList loadedTripDataList = JsonUtility.FromJson<PracticeDataList>(json);

                int windowIndex = 0;
                foreach (PracticeData data in loadedTripDataList.PracticeDatas)
                {
                    if (windowIndex < _filledPracticePlanes.Count)
                    {
                        if (_availableIndexes.Count > 0)
                        {
                            int availableIndex = _availableIndexes[0];
                            var currentFilledItemPlane = _filledPracticePlanes[availableIndex];
                            _availableIndexes.RemoveAt(0);

                            if (!currentFilledItemPlane.IsActive)
                            {
                                currentFilledItemPlane.Enable();
                                currentFilledItemPlane.SetPracticeData(data);
                                currentFilledItemPlane.SetStatusSprite(_statusSpriteProvider.GetNormalSprite(data.Status));
                                currentFilledItemPlane.OpenButtonClicked += OpenPractice;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load trip data: " + e.Message);
            }
        }

        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPracticePlanes.Count);
    }
}

[Serializable]
public class PracticeDataList
{
    public List<PracticeData> PracticeDatas;

    public PracticeDataList(List<PracticeData> practiceDatas)
    {
        PracticeDatas = practiceDatas;
    }
}