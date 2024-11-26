using System;

[Serializable]
public class PracticeData
{
    public string Title;
    public string DurationHr;
    public string DurationMin;
    public string Notes;
    public PracticeStatus Status;
    public PracticeType Type;

    public PracticeData(string title, string durationHr, string durationMin, string notes, PracticeStatus status, PracticeType type)
    {
        Title = title;
        DurationHr = durationHr;
        DurationMin = durationMin;
        Notes = notes;
        Status = status;
        Type = type;
    }
}

public enum PracticeStatus
{
    Relieved,
    Neutral,
    Suspicious,
    Tired,
    Smile,
    None
}

public enum PracticeType
{
    Meditation,
    Yoga,
    None
}