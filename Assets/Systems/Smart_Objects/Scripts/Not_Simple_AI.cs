using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Base_Navigation))]

public class Not_Simple_AI : Common_AI_Base
{
    [SerializeField] protected float PickInteractionInterval = 2f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {

    }
}
