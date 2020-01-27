using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T instance = null;

    public static T Instance
    {
        get
        {
            if (!instance)
                instance = Resources.Load<T>(typeof(T).Name);

            return instance;
        }
    }
}