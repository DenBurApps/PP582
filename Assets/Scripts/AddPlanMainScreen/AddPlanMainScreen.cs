using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AddPlanMainScreen : MonoBehaviour
{
    private const string SaveFileName = "SavedDataPlan.json";

    [SerializeField] private AddPlanMainScreenView _view;
    [SerializeField] private List<FilledPracticePlanPlane> _filledPracticePlanPlanes;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private CongratulationsScreen _congratulationsScreen;
    [SerializeField] private AddPlanScreen _addPlanScreen;
    [SerializeField] private EditPlanScreen _editPlanScreen;
    [SerializeField] private CompletedPlanScreen _completedPlanScreen;

    private List<int> _availableIndexes = new List<int>();
    private string _saveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public event Action BackButtonClicked;
    public event Action AddPlanClicked;
    public event Action CompletePlanClicked;
    public event Action<FilledPracticePlanPlane> EditPracticePlanClicked;

    private void Start()
    {
        DisableAllWindows();
        _view.Disable();
        _congratulationsScreen.Disable();
        LoadFilledWindowsData();
    }

    private void OnEnable()
    {
        _screenStateManager.AddPlanMainScreenOpen += OpenWindow;

        _view.BackButtonClicked += OnBackButtonClikced;
        _view.AddButtonClicked += OnAddPlanClicked;

        _addPlanScreen.Saved += EnablePlan;

        _editPlanScreen.BackButtonClicked += OpenWindow;
        _editPlanScreen.Saved += OpenWindow;
        _editPlanScreen.Saved += SaveFilledWindowsData;
        _editPlanScreen.Deleted += DeletePlan;

        _completedPlanScreen.BackButtonClicked += OpenWindow;
        _completedPlanScreen.SaveButtonClicked += EnableCongratulations;
        _completedPlanScreen.SaveButtonClicked += SaveFilledWindowsData;

        _congratulationsScreen.BackButtonClicked += DisableCongratualtions;
    }

    private void OnDisable()
    {
        _screenStateManager.AddPlanMainScreenOpen -= OpenWindow;

        _view.BackButtonClicked -= OnBackButtonClikced;
        _view.AddButtonClicked -= OnAddPlanClicked;

        _addPlanScreen.Saved -= EnablePlan;

        _editPlanScreen.BackButtonClicked -= OpenWindow;
        _editPlanScreen.Saved -= OpenWindow;
        _editPlanScreen.Saved -= SaveFilledWindowsData;
        _editPlanScreen.Deleted -= DeletePlan;

        _completedPlanScreen.BackButtonClicked -= OpenWindow;
        _completedPlanScreen.SaveButtonClicked -= EnableCongratulations;
        _completedPlanScreen.SaveButtonClicked -= SaveFilledWindowsData;

        _congratulationsScreen.BackButtonClicked -= DisableCongratualtions;
    }

    private void OpenWindow()
    {
        _view.Enable();
    }

    private void DeletePlan(FilledPracticePlanPlane plane)
    {
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));

        _view.Enable();

        int index = _filledPracticePlanPlanes.IndexOf(plane);

        if (index >= 0 && !_availableIndexes.Contains(index))
        {
            _availableIndexes.Add(index);
        }

        plane.EditButtonClicked -= EditPlan;
        plane.CompleteButtonClicked -= OnCompletePlanClicked;
        plane.PlanCompleted -= PlanCompleted;
        plane.ResetData();
        plane.Disable();

        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPracticePlanPlanes.Count);

        SaveFilledWindowsData();
    }

    private void EnablePlan(PracticePlanData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        if (_availableIndexes.Count > 0)
        {
            int availableIndex = _availableIndexes[0];
            _availableIndexes.RemoveAt(0);

            var currentFilledItemPlane = _filledPracticePlanPlanes[availableIndex];

            if (!currentFilledItemPlane.IsActive)
            {
                currentFilledItemPlane.Enable();
                currentFilledItemPlane.SetPlanData(data);
                currentFilledItemPlane.EditButtonClicked += EditPlan;
                currentFilledItemPlane.CompleteButtonClicked += OnCompletePlanClicked;
                currentFilledItemPlane.PlanCompleted += PlanCompleted;
            }
        }

        _view.Enable();
        SaveFilledWindowsData();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPracticePlanPlanes.Count);
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _filledPracticePlanPlanes.Count; i++)
        {
            _filledPracticePlanPlanes[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void EditPlan(FilledPracticePlanPlane filledPracticePlanPlane)
    {
        EditPracticePlanClicked?.Invoke(filledPracticePlanPlane);
        _view.Disable();
    }

    private void OnCompletePlanClicked()
    {
        CompletePlanClicked?.Invoke();
        _view.Disable();
    }

    private void OnBackButtonClikced()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }

    private void OnAddPlanClicked()
    {
        AddPlanClicked?.Invoke();
        _view.Disable();
    }

    private void PlanCompleted(FilledPracticePlanPlane filledPracticePlanPlane)
    {
        EnableCongratulations();
        DeletePlan(filledPracticePlanPlane);
    }

    private void EnableCongratulations()
    {
        _congratulationsScreen.Enable();
        _view.SetTransperent();
    }

    private void DisableCongratualtions()
    {
        _congratulationsScreen.Disable();
        _view.Enable();
    }

    private void SaveFilledWindowsData()
    {
        List<PracticePlanData> itemsToSave = new List<PracticePlanData>();

        foreach (var window in _filledPracticePlanPlanes)
        {
            if (window.PracticePlanData != null)
            {
                itemsToSave.Add(window.PracticePlanData);
            }
        }

        PracticePlanDataList itemDataList = new PracticePlanDataList(itemsToSave);
        string json = JsonUtility.ToJson(itemDataList, true);

        try
        {
            File.WriteAllText(_saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save trip data: " + e.Message);
        }
    }

    private void LoadFilledWindowsData()
    {
        if (File.Exists(_saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(_saveFilePath);
                PracticePlanDataList loadedTripDataList = JsonUtility.FromJson<PracticePlanDataList>(json);

                int windowIndex = 0;
                foreach (PracticePlanData data in loadedTripDataList.PlanDatas)
                {
                    if (windowIndex < _filledPracticePlanPlanes.Count)
                    {
                        if (_availableIndexes.Count > 0)
                        {
                            int availableIndex = _availableIndexes[0];
                            _availableIndexes.RemoveAt(0);

                            var currentFilledItemPlane = _filledPracticePlanPlanes[availableIndex];

                            if (!currentFilledItemPlane.IsActive)
                            {
                                currentFilledItemPlane.Enable();
                                currentFilledItemPlane.SetPlanData(data);
                                currentFilledItemPlane.EditButtonClicked += EditPlan;
                                currentFilledItemPlane.CompleteButtonClicked += OnCompletePlanClicked;
                                currentFilledItemPlane.PlanCompleted += PlanCompleted;
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
        
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPracticePlanPlanes.Count);
    }
}

[Serializable]
public class PracticePlanDataList
{
    public List<PracticePlanData> PlanDatas;

    public PracticePlanDataList(List<PracticePlanData> planDatas)
    {
        PlanDatas = planDatas;
    }
}

