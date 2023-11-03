using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PropFactory : MonoBehaviour
{
    [SerializeField] private Prop[] props;
    private Dictionary<string, Prop> propsByName;

    private void Awake()
    {
        propsByName = new Dictionary<string, Prop>();
        foreach (var prop in props)
        {
            propsByName.Add(prop.propName, prop);
        }
    }

    public Prop CreateProp(string propName, Transform spawn)
    {
        if (propsByName.TryGetValue(propName, out Prop propPrefab))
        {
            Prop propInstance = Instantiate(propPrefab, spawn.position, Quaternion.identity);
            return propInstance;
        }
        else
        {
            Debug.LogWarning($"The prop '{propName}' doesnt exists in the props database");
            return null;
        }
    }
}
