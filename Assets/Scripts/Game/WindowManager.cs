using System.Collections.Generic;
using UnityEngine;

public class WindowManager : SingletonMonoBehaviour<WindowManager>
{
    [SerializeField] private Canvas _mainCanvas;

    private List<Window> _turnWindows = new List<Window>();

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public T GetWindow<T>() where T : Window
    {
        T loadedWindow = Resources.Load<T>("Windows/" + typeof(T).Name);

        return Instantiate<T>(loadedWindow, _mainCanvas.transform);
    }

    public void AddWindowToTurn(Window value)
    {
        if (!_turnWindows.Contains(value))
            _turnWindows.Add(value);
    }

    public void RemoveWindowOfTurn(Window value)
    {
        if (_turnWindows.Contains(value))
            _turnWindows.Remove(value);
    }

    public int OpenWindowsCount()
    {
        return _turnWindows.Count;
    }
}
