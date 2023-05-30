using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Base_Navigation))]
public class Simple_AI : MonoBehaviour
{
    [SerializeField] protected float PickInteractionInterval = 2f;

    protected Base_Navigation Navigation;

    protected Base_Interaction CurrentInteraction = null;
    protected bool StartedPerforming = false;

    protected float TimeUntilNextInteractionPicked = -1f;

    private void Awake()
    {
        Navigation = GetComponent<Base_Navigation>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (CurrentInteraction != null && Navigation.IsAtDestination)
        {
            if (Navigation.IsAtDestination && ! StartedPerforming)
            {
                StartedPerforming= true;
                CurrentInteraction.Perform(this, OnInteractionFinished);
            }
        }
        else
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

    void OnInteractionFinished(Base_Interaction interaction)
    {
        interaction.UnlockInteraction();
        CurrentInteraction = null;
        Debug.Log($"Finished {interaction.DisplayName}");
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
