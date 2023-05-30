using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStat
{
    Fun,
    Sleep,
    Contact,
    Physiology,
    Food
}

[RequireComponent(typeof(Base_Navigation))]

public class Not_Simple_AI : MonoBehaviour
{
    [Header("Fun")]
    [SerializeField] float InitialFunLevel = 0.5f;
    [SerializeField] float BaseFunDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider FunDisplay;

    [Header("Sleep")]
    [SerializeField] float InitialSleepLevel = 0.5f;
    [SerializeField] float BaseSleepDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider SleepDisplay;

    [Header("Contact")]
    [SerializeField] float InitialContactLevel = 0.5f;
    [SerializeField] float BaseContactDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider ContactDisplay;

    [Header("Physiology")]
    [SerializeField] float InitialPhysiologyLevel = 0.5f;
    [SerializeField] float BasePhysiologyDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider PhysiologyDisplay;

    [Header("Food")]
    [SerializeField] float InitialFoodLevel = 0.5f;
    [SerializeField] float BaseFoodDecayRate = 0.005f;
    [SerializeField] UnityEngine.UI.Slider FoodDisplay;


    [SerializeField] protected float PickInteractionInterval = 2f;

    protected Base_Navigation Navigation;

    protected Base_Interaction CurrentInteraction = null;
    protected bool StartedPerforming = false;

    protected float TimeUntilNextInteractionPicked = -1f;

    public float CurrentFun { get; protected set; }
    public float CurrentSleep { get; protected set; }
    public float CurrentContact { get; protected set; }
    public float CurrentPhysiology { get; protected set; }
    public float CurrentFood { get; protected set; }

    private void Awake()
    {
        FunDisplay.value = CurrentFun = InitialFunLevel;
        SleepDisplay.value = CurrentSleep = InitialSleepLevel;
        ContactDisplay.value = CurrentContact = InitialContactLevel;
        PhysiologyDisplay.value = CurrentPhysiology = InitialPhysiologyLevel;
        FoodDisplay.value = CurrentFood = InitialFoodLevel;

        Navigation = GetComponent<Base_Navigation>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (CurrentInteraction != null && Navigation.IsAtDestination)
        {
            if (Navigation.IsAtDestination && !StartedPerforming)
            {
                StartedPerforming = true;
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

        CurrentFun -= Mathf.Clamp01(CurrentFun - BaseFunDecayRate * Time.deltaTime);
        FunDisplay.value = CurrentFun;

        CurrentSleep -= Mathf.Clamp01(CurrentSleep - BaseSleepDecayRate * Time.deltaTime);
        SleepDisplay.value = CurrentSleep = InitialSleepLevel;

        CurrentContact -= Mathf.Clamp01(CurrentContact - BaseContactDecayRate * Time.deltaTime);
        ContactDisplay.value = CurrentContact = InitialContactLevel;

        CurrentPhysiology -= Mathf.Clamp01(CurrentPhysiology - BasePhysiologyDecayRate * Time.deltaTime);
        PhysiologyDisplay.value = CurrentPhysiology = InitialPhysiologyLevel;

        CurrentFun -= Mathf.Clamp01(CurrentFun - BaseFunDecayRate * Time.deltaTime);
        FunDisplay.value = CurrentFun = InitialFunLevel;
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
