using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);
                instance = gameObject.AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = GetComponent<T>();
    }
}
