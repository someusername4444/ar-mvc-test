using UnityEngine;

public interface IConfigManager
{
    public T GetConfig<T>() where T : ScriptableObject;
}

public class ConfigManager : IConfigManager
{
    private ConfigurationData configurationData;

    public ConfigManager()
    {
        //bootstrap data using resources folder 
        configurationData = Resources.Load<ConfigurationData>("ConfigurationDataPrefab");
    }

    public T GetConfig<T>() where T : ScriptableObject
    {
        foreach (var item in configurationData.scriptableObjects)
        {
            if (item is T)
            {
                return (T)item;
            }
        }
        return null;
    }
}

