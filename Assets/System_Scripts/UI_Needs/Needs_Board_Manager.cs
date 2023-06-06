using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENeedsBoardKey
{
    Character_Stat_Sleep,
    Character_Stat_Fun,
    Character_Stat_Contact,
    Character_Stat_Physiology,
    Character_Stat_Food,
    Character_FocusObject

}


public class Needsboard
{
    Dictionary<ENeedsBoardKey, int>         IntValues             = new Dictionary<ENeedsBoardKey, int>();
    Dictionary<ENeedsBoardKey, float>       FloatValues           = new Dictionary<ENeedsBoardKey, float>();
    Dictionary<ENeedsBoardKey, bool>        BoolValues            = new Dictionary<ENeedsBoardKey, bool>();
    Dictionary<ENeedsBoardKey, string>      StringValues          = new Dictionary<ENeedsBoardKey, string>();
    Dictionary<ENeedsBoardKey, Vector3>     Vector3Values         = new Dictionary<ENeedsBoardKey, Vector3>();
    Dictionary<ENeedsBoardKey, GameObject>  GameObjectValues      = new Dictionary<ENeedsBoardKey, GameObject>();
    Dictionary<ENeedsBoardKey, object>      GenericValues         = new Dictionary<ENeedsBoardKey, object>();

    public void SetGeneric<T>(ENeedsBoardKey key, T value)
    {
        GenericValues[key] = value;
    }

    public T GetGeneric<T>(ENeedsBoardKey key)
    {
        if (!GenericValues.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for {key} in IntValues");

        return (T)GenericValues[key];

    }

    public bool TryGet<T>(ENeedsBoardKey key, out T value, T defaultValue)
    {
        if (GenericValues.ContainsKey(key))
        {
            value = (T)GenericValues[key];
            return true;
        }

        value = defaultValue;
        return false;

    }

    //1
    public void Set(ENeedsBoardKey key, int value)
    {
        IntValues[key] = value;
    }

    public int GetInt(ENeedsBoardKey key)
    {
        if (!IntValues.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for {key} in IntValues");

        return IntValues[key];
    }

    public bool TryGet(ENeedsBoardKey key, out int value, int defaultValue = 0)
    {
        if (IntValues.ContainsKey(key))
        {
            value = IntValues[key];
            return true;
        }

        value = defaultValue;
        return false;

    }

    //2
    public void Set(ENeedsBoardKey key, float value)
    {
        FloatValues[key] = value;
    }

    public float GetFloat(ENeedsBoardKey key)
    {
        if (!FloatValues.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for {key} in FloatValues");

        return FloatValues[key];
    }

    public bool TryGet(ENeedsBoardKey key, out float value, float defaultValue = 0)
    {
        if (FloatValues.ContainsKey(key))
        {
            value = FloatValues[key];
            return true;
        }

        value = defaultValue;
        return false;

    }

    //3
    public void Set(ENeedsBoardKey key, bool value)
    {
        BoolValues[key] = value;
    }

    public bool GetBool(ENeedsBoardKey key)
    {
        if (!BoolValues.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for {key} in BoolValues");

        return BoolValues[key];
    }

    public bool TryGet(ENeedsBoardKey key, out bool value, bool defaultValue = false)
    {
        if (BoolValues.ContainsKey(key))
        {
            value = BoolValues[key];
            return true;
        }

        value = defaultValue;
        return false;

    }

    //4
    public void Set(ENeedsBoardKey key, string value)
    {
        StringValues[key] = value;
    }

    public string GetString(ENeedsBoardKey key)
    {
        if (!StringValues.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for {key} in StringValues");

        return StringValues[key];
    }

    public bool TryGet(ENeedsBoardKey key, out string value, string defaultValue = "")
    {
        if (StringValues.ContainsKey(key))
        {
            value = StringValues[key];
            return true;
        }

        value = defaultValue;
        return false;

    }

    //5
    public void Set(ENeedsBoardKey key, Vector3 value)
    {
        Vector3Values[key] = value;
    }

    public Vector3 GetVector3(ENeedsBoardKey key)
    {
        if (!Vector3Values.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for {key} in Vector3Values");

        return Vector3Values[key];
    }

    public bool TryGet(ENeedsBoardKey key, out Vector3 value, Vector3 defaultValue)
    {
        if (Vector3Values.ContainsKey(key))
        {
            value = Vector3Values[key];
            return true;
        }

        value = defaultValue;
        return false;

    }

    //6
    public void Set(ENeedsBoardKey key, GameObject value)
    {
        GameObjectValues[key] = value;
    }

    public GameObject GetGameObject(ENeedsBoardKey key)
    {
        if (!GameObjectValues.ContainsKey(key))
            throw new System.ArgumentException($"Could not find value for {key} in GameObjectValues");

        return GameObjectValues[key];
    }

    public bool TryGet(ENeedsBoardKey key, out GameObject value, GameObject defaultValue = null)
    {
        if (GameObjectValues.ContainsKey(key))
        {
            value = GameObjectValues[key];
            return true;
        }

        value = defaultValue;
        return false;

    }

}

public class Needs_Board_Manager : MonoBehaviour
{
    public static Needs_Board_Manager Instance { get; private set; } = null;

    Dictionary<MonoBehaviour, Needsboard> Individual_Needsboards = new Dictionary<MonoBehaviour, Needsboard>();
    Dictionary<int, Needsboard> Shared_Needsboards = new Dictionary<int, Needsboard>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Trying to create second Needs_Board_Manager on {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Needsboard GetIndividualBlackboard(MonoBehaviour requestor)
    {
        if (!Individual_Needsboards.ContainsKey(requestor))
            Individual_Needsboards[requestor] = new Needsboard();

        return Individual_Needsboards[requestor];
    }

    public Needsboard GetSharedBlackBorad(int uniqueID)
    {
        if (!Shared_Needsboards.ContainsKey(uniqueID))
            Shared_Needsboards[uniqueID] = new Needsboard();

        return Shared_Needsboards[uniqueID];
    }
}
