using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : SingletonMonoBehaviour<WindowManager>
{
    [SerializeField] private Canvas _mainCanvas;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public T GetWindow<T>() where T: Window
    {
        T loadedWindow = Resources.Load<T>("Windows/" + typeof(T).Name);

        return Instantiate<T>(loadedWindow, _mainCanvas.transform);
    }
}
