using UnityEngine;

public class StatusSpriteProvider : MonoBehaviour
{
    [SerializeField] private Sprite _relieved;
    [SerializeField] private Sprite _neutral;
    [SerializeField] private Sprite _suspicious;
    [SerializeField] private Sprite _tired;
    [SerializeField] private Sprite _smile;
    
    [SerializeField] private Sprite _relievedGlowing;
    [SerializeField] private Sprite _neutralGlowing;
    [SerializeField] private Sprite _suspiciousGlowing;
    [SerializeField] private Sprite _tiredGlowing;
    [SerializeField] private Sprite _smileGlowing;

    public Sprite GetNormalSprite(PracticeStatus status)
    {
        switch (status)
        {
            case PracticeStatus.Neutral: return _neutral;
            case PracticeStatus.Relieved: return _relieved;
            case PracticeStatus.Smile: return _smile;
            case PracticeStatus.Suspicious: return _suspicious;
            case PracticeStatus.Tired: return _tired;
            default: return null;
        }
    }
    
    public Sprite GetGlowingSprite(PracticeStatus status)
    {
        switch (status)
        {
            case PracticeStatus.Neutral: return _neutralGlowing;
            case PracticeStatus.Relieved: return _relievedGlowing;
            case PracticeStatus.Smile: return _smileGlowing;
            case PracticeStatus.Suspicious: return _suspiciousGlowing;
            case PracticeStatus.Tired: return _tiredGlowing;
            default: return null;
        }
    }
}
