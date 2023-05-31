using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base_Navigation))]
public class Simple_AI : Common_AI_Base
{
    [SerializeField] protected float PickInteractionInterval = 2f;

    protected float TimeUntilNextInteractionPicked = -1f;

    protected override void Update()
    {
        base.Update();
        if (CurrentInteraction == null)
        {
            TimeUntilNextInteractionPicked -= Time.deltaTime;

            // time to pick an interaction
            if (TimeUntilNextInteractionPicked <= 0)
            {
                TimeUntilNextInteractionPicked = PickInteractionInterval;
                PickRandomInteraction();
            }
        }
    }

    void PickRandomInteraction()
    {
        //pick a random object
        int objectIndex = Random.Range(0, Smart_Object_Manager.Instance.RegisteredObjects.Count);
        var selectedObject = Smart_Object_Manager.Instance.RegisteredObjects[objectIndex];

        //pick a random interaction
        int interactionIndex = Random.Range(0, selectedObject.Interactions.Count);
        var selectedInteraction = selectedObject.Interactions[interactionIndex];

        //can perform the interaction?
        if (selectedInteraction.CanPerform())
        {
            CurrentInteraction = selectedInteraction;
            CurrentInteraction.LockInteraction();
            StartedPerforming = false;

            //move to the target
            if (!Navigation.SetDestination(selectedObject.InteractionPoint))
            {
                Debug.LogError($"Could not move to {selectedObject.name}");
                CurrentInteraction = null;
            }
            else
                Debug.Log($"Going to {CurrentInteraction.DisplayName} at {selectedObject.DisplayName}");
        }
    }
}
