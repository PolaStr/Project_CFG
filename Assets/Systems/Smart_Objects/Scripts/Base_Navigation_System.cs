using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base_Navigation_System : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;

    public bool IsAtDestination { get; internal set; }

    internal bool SetDestination(Vector3 position)
    {
        throw new NotImplementedException();
    }

    private void Awake()
    {
        navMeshAgent= GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        navMeshAgent.destination = movePositionTransform.position;
    }
}
