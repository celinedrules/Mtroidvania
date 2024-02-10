using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string unlockedText;
    [SerializeField] private bool acquired;

    public Sprite Sprite => sprite;
    public string UnlockedText => unlockedText;
    protected internal Animator Animator { get; set; }
    protected bool CanTrigger { get; private set; }

    public bool Acquired
    {
        get => acquired;
        set => acquired = value;
    }

    public virtual bool Perform()
    {
        if (!CanTrigger)
            return false;

        CanTrigger = false;

        return true;
    }

    public virtual bool ResetAbility()
    {
        return CanTrigger = true;
    }
}