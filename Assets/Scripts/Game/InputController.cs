using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    public static Action<Vector3> onPositionSelected;

    private EventSystem _eventSystem;

    void Start()
    {
       _eventSystem = FindObjectOfType<EventSystem>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _eventSystem.currentSelectedGameObject == null)
            onPositionSelected?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
