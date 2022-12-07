using System.Collections.Generic;

public interface IFactory
{
    void AddDependency<DepT>(object dependency);
    void ResolveDependencies(object instance);
}

public class Factory : IFactory
{

    private Dictionary<string, object> dependencies = new Dictionary<string, object>();

    public void AddDependency<DepT>(object dependency)
    {
        dependencies.Add(typeof(DepT).Name, dependency);
    }

    public void ResolveDependencies(object instance)
    {
        var type = instance.GetType();
        foreach (var property in type.GetProperties())
        {
            if (property.GetValue(instance, null) != null)
            {
                // Dependency already set
                continue;
            }
            var key = property.PropertyType.Name;
            if (dependencies.ContainsKey(key))
            {
                var dependency = dependencies[key];
                if (dependency != null)
                {
                    property.SetValue(instance, dependency, null);
                }
            }
        }
    }
}

