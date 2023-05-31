using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(Base_Navigation))]

public class Not_Simple_AI : Common_AI_Base
{
    [SerializeField] protected float DefaultInteractionScore = 0f;
    [SerializeField] protected float PickInteractionInterval = 2f;
    [SerializeField] protected int InteractionPickSize = 5;


    protected float TimeUntilNextInteractionPicked = -1f;

    protected override void Start()
    {
        base.Start();
    }

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
                PickBestInteraction();
            }
        }
    }


    float ScoreInteraction(Base_Interaction interaction)
    {
        if (interaction.StatChanges.Length == 0)
        {
            return DefaultInteractionScore;
        }

        float score = 0f;
        foreach(var change in interaction.StatChanges)
            score += ScoreChange(change.Target, change.Value);
        return score;
    }

    float ScoreChange(EStat target, float amount)
    {
        float currentValue = 0f;
        switch (target)
        {
            case EStat.Fun: currentValue = CurrentFun; break;
            case EStat.Sleep: currentValue = CurrentSleep; break;
            case EStat.Contact: currentValue = CurrentContact; break;
            case EStat.Physiology: currentValue = CurrentPhysiology; break;
            case EStat.Food: currentValue = CurrentFood; break;
        }

        return (1f - currentValue) * amount;
    }

    class ScoredInteraction
    {
        public Smart_Object TargetObject;
        public Base_Interaction Interaction;
        public float Score;
    }

    void PickBestInteraction()
    {
        //loop through all the objects
        List<ScoredInteraction> unsortedInteractions = new List<ScoredInteraction>();
        foreach (var smartObjects in Smart_Object_Manager.Instance.RegisteredObjects)
        {
            //loop through all the interactions
            foreach (var interaction in smartObjects.Interactions)
            {
                if (!interaction.CanPerform())
                    continue;

                float score = ScoreInteraction(interaction);

                unsortedInteractions.Add(new ScoredInteraction() { TargetObject= smartObjects,
                                                                 Interaction = interaction,
                                                                 Score = score});
            }
        }

        if (unsortedInteractions.Count == 0)
            return;

        //sort and pick from one of the best interactions
        var sortedInteractions = unsortedInteractions.OrderByDescending(scoredInteraction => scoredInteraction.Score).ToList();
        int maxIndex = Mathf.Min(InteractionPickSize, sortedInteractions.Count);
        var selectedIndex = Random.Range(0, maxIndex);

        var selectedObject = sortedInteractions[selectedIndex].TargetObject;
        var selectedInteraction = sortedInteractions[selectedIndex].Interaction;

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
