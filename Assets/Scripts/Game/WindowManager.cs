using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : SingletonMonoBehaviour<WindowManager>
{
    private List<Window> _windows = new List<Window>();

    protected override void Awake()
    {
        base.Awake();
    }

    public T GetWindow<T>() where T: Window
    {
        foreach (Window window in _windows)
        {
            if (window is T)
                if (window.gameObject.activeInHierarchy)
                    return Instantiate<T>((T)window);
                else
                    return (T)window;
        }

        T loadedWindow = Resources.Load<T>("Windows/" + typeof(T).Name);

        if (loadedWindow != null && !_windows.Contains(loadedWindow))
            _windows.Add(loadedWindow);


        return Instantiate<T>(loadedWindow);
    }
}
