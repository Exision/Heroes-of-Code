using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapInputController : MonoBehaviour
{
    public static Action<Vector3> onPositionSelected;

    [SerializeField] private Camera _sceneCamera;
    [SerializeField] private EventSystem _eventSystem;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _eventSystem.currentSelectedGameObject == null)
            onPositionSelected?.Invoke(_sceneCamera.ScreenToWorldPoint(Input.mousePosition));
    }
}
