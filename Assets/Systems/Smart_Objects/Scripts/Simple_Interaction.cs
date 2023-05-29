using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simple_Interaction : Base_Interaction
{
    protected class PerformerInfo
    {
        public float ElapsedTime;
        public UnityEvent<Base_Interaction> OnCompleted;
    }
    [SerializeField] protected int MaxSimultaneousUsers = 1;

    protected int NumCurrentUsers = 0;
    protected List<PerformerInfo> CurrentPerformers = new List<PerformerInfo>();

    public override bool CanPerform()
    {
        return NumCurrentUsers < MaxSimultaneousUsers;
    }

    public override void LockInteraction()
    {
        ++NumCurrentUsers;

        if (NumCurrentUsers > MaxSimultaneousUsers)
            Debug.LogError($"Too many users have locked this interaction {_DisplayName}");
    }

    public override void Perform(MonoBehaviour performer, UnityEvent<Base_Interaction> onCompleted = null)
    {
        if (NumCurrentUsers <= 0)
        {
            Debug.LogError($"Trying to perform an interaction when there are no users {_DisplayName}");
            return;
        }

        //check the interaction type
        if (InteractionType == EInteractionType.Instantaneous)
        {
            if (onCompleted!= null)
                onCompleted.Invoke(this);
        }
        else if (InteractionType == EInteractionType.OverTime)
        {
            CurrentPerformers.Add(new PerformerInfo() { ElapsedTime = 0, OnCompleted = onCompleted });
        }
    }

    public override void UnlockInteraction()
    {
        if (NumCurrentUsers <= 0)
            Debug.LogError($"Trying to unlock an already unlocked interaction {_DisplayName}");

        --NumCurrentUsers;
    }

    protected virtual void Update ()
    {
        //update any current performers
        for(int index = CurrentPerformers.Count - 1; index >= 0; index--)
        {
            PerformerInfo performer = CurrentPerformers[index];

            performer.ElapsedTime += Time.deltaTime;

            if (performer.ElapsedTime >= _Duration)
            {
                performer.OnCompleted.Invoke(this);
                CurrentPerformers.RemoveAt(index);
            }



        }
    }
}
