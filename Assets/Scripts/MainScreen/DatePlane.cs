using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DatePlane : MonoBehaviour
{
    [SerializeField] private int _additionToDate;
    [SerializeField] private TMP_Text _dayNumber;
    [SerializeField] private TMP_Text _dayName;

    private void Start()
    {
        DateTime targetDate = DateTime.Today.AddDays(_additionToDate);
        CultureInfo englishCulture = new CultureInfo("en-US");
        _dayNumber.text = targetDate.Day.ToString();
        _dayName.text = targetDate.ToString("ddd", englishCulture);
    }
}
