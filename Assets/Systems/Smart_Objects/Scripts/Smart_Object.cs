using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smart_Object : MonoBehaviour
{
    [SerializeField] protected string _DisplayName;
    [SerializeField] protected Transform _InteractionMarker;

    protected List<Base_Interaction> CachedInteractions = null;

    public Vector3 InteractionPoint => _InteractionMarker != null ? _InteractionMarker.position : transform.position;

    public string DisplayName => _DisplayName;
    public List <Base_Interaction> Interactions
    {
        get
        {
            if (CachedInteractions == null)
                CachedInteractions = new List<Base_Interaction>(GetComponents<Base_Interaction>());

            return CachedInteractions;
        }
    }


    void Start()
    {
        Smart_Object_Manager.Instance.RegisterSmartObject(this);
    }

    private void OnDestroy()
    {
        Smart_Object_Manager.Instance.DeregisterSmartObject(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
