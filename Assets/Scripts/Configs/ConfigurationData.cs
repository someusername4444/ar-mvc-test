using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigurationData", menuName = "Matterless/ConfiguationData")]
public class ConfigurationData : ScriptableObject
{
    public List<ScriptableObject> scriptableObjects;
}
