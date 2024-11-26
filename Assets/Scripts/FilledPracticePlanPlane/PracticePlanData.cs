using System;

[Serializable]
public class PracticePlanData
{
   public string PlanTitle;
   public PracticeData PracticeData;
   public DurationVariants Duration;
   public string CompletionDate;
   public float CompleteCount;
   public bool IsCompleted;

   public PracticePlanData(string planTitle, PracticeData practiceData, DurationVariants duration)
   {
      PlanTitle = planTitle;
      PracticeData = practiceData;
      Duration = duration;
   }
}

public enum DurationVariants
{
   Days10,
   Days20,
   Days30,
   None
}
