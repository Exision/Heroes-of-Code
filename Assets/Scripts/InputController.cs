using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static Action<Vector3> onPositionSelected;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            onPositionSelected?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
