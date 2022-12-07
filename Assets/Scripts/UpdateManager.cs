using System.Collections.Generic;
using UnityEngine;


public interface IUpdateManager
{
    void RegisterUpdateable(IUpdateable updateable);
}

public interface IUpdateable
{
    void UpdateMe();
    bool Active { get; }
}

public class UpdateManager : MonoBehaviour, IUpdateManager
{
    List<IUpdateable> updateables = new List<IUpdateable>();
    
    void Start()
    {
        App.Startup(this);
    }

    void Update()
    {
        foreach (var updateable in updateables)
        {
            if (updateable.Active)
            {
                updateable.UpdateMe();
            }
        }
    }

    public void RegisterUpdateable(IUpdateable updateable)
    {
        updateables.Add(updateable);
    }
}






