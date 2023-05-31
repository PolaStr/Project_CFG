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

public class Common_AI_Base : MonoBehaviour
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

    protected Base_Navigation Navigation;

    protected Base_Interaction CurrentInteraction = null;
    protected bool StartedPerforming = false;

    public float CurrentFun { get; protected set; }
    public float CurrentSleep { get; protected set; }
    public float CurrentContact { get; protected set; }
    public float CurrentPhysiology { get; protected set; }
    public float CurrentFood { get; protected set; }


    protected virtual void Awake()
    {
        FunDisplay.value = CurrentFun = InitialFunLevel;
        SleepDisplay.value = CurrentSleep = InitialSleepLevel;
        ContactDisplay.value = CurrentContact = InitialContactLevel;
        PhysiologyDisplay.value = CurrentPhysiology = InitialPhysiologyLevel;
        FoodDisplay.value = CurrentFood = InitialFoodLevel;

        Navigation = GetComponent<Base_Navigation>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (CurrentInteraction != null)
        {
            if (Navigation.IsAtDestination && !StartedPerforming)
            {
                StartedPerforming = true;
                CurrentInteraction.Perform(this, OnInteractionFinished);
            }
        }

        CurrentFun = Mathf.Clamp01(CurrentFun - BaseFunDecayRate * Time.deltaTime);
        FunDisplay.value = CurrentFun;

        CurrentSleep = Mathf.Clamp01(CurrentSleep - BaseSleepDecayRate * Time.deltaTime);
        SleepDisplay.value = CurrentSleep = InitialSleepLevel;

        CurrentContact = Mathf.Clamp01(CurrentContact - BaseContactDecayRate * Time.deltaTime);
        ContactDisplay.value = CurrentContact = InitialContactLevel;

        CurrentPhysiology = Mathf.Clamp01(CurrentPhysiology - BasePhysiologyDecayRate * Time.deltaTime);
        PhysiologyDisplay.value = CurrentPhysiology = InitialPhysiologyLevel;

        CurrentFun = Mathf.Clamp01(CurrentFun - BaseFunDecayRate * Time.deltaTime);
        FunDisplay.value = CurrentFun = InitialFunLevel;
    }

    protected virtual void OnInteractionFinished(Base_Interaction interaction)
    {
        interaction.UnlockInteraction();
        CurrentInteraction = null;
        Debug.Log($"Finished {interaction.DisplayName}");
    }

    public void UpdateIndividualStat(EStat target, float amount)
    {
        Debug.Log($"Update {target} by {amount}");
        switch(target)
        {
            case EStat.Fun:         CurrentFun += amount; break;
            case EStat.Sleep:       CurrentSleep += amount; break;
            case EStat.Contact:     CurrentContact += amount; break;
            case EStat.Physiology:  CurrentPhysiology += amount; break;
            case EStat.Food:        CurrentFood += amount; break;
        }
    }
}
