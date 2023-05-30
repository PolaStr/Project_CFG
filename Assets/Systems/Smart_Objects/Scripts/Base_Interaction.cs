using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EInteractionType
{
    Instantaneous = 0,
    OverTime = 1
}


[System.Serializable]
public class InteractionStatChange
{
    public EStat Target;
    public float Value;
}

public abstract class Base_Interaction : MonoBehaviour
{

    [SerializeField] protected string _DisplayName;
    [SerializeField] protected EInteractionType _InteractionType = EInteractionType.Instantaneous;
    [SerializeField] protected float _Duration = 0f;
    [SerializeField] protected InteractionStatChange[] StatChanges;

    public string DisplayName => _DisplayName;
    public EInteractionType InteractionType => _InteractionType;
    public float Duration => _Duration;

    public abstract bool CanPerform();
    public abstract void LockInteraction();

    public abstract void Perform(MonoBehaviour performer, UnityAction<Base_Interaction> onCompleted);
    public abstract void UnlockInteraction();
}
