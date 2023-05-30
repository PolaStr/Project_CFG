using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smart_Object_Manager : MonoBehaviour
{
    public static Smart_Object_Manager Instance { get; private set; } = null;

    public List<Smart_Object> RegisteredObjects { get; private set; } = new List<Smart_Object>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Trying to create second SmartObjectManager on {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterSmartObject(Smart_Object toRegister)
    {
        RegisteredObjects.Add(toRegister);

        Debug.Log(toRegister.DisplayName);
    }

    public void DeregisterSmartObject(Smart_Object toDeregister)
    {
        RegisteredObjects.Remove(toDeregister);
    }
}
