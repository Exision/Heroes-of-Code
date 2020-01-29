using UnityEngine;

public class SingletonResources<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance = null;

    public static T Instance
    {
        get
        {
            if (!instance)
                instance = Instantiate<T>(Resources.Load<T>(typeof(T).Name));

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = GetComponent<T>();
    }
}