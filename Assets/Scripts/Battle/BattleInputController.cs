using UnityEngine;

public class BattleInputController : MonoBehaviour
{
    [SerializeField] private Camera _sceneCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(_sceneCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);

            for (int loop = 0; loop < hits.Length; loop++)
                hits[loop].collider.gameObject.GetComponent<TroopObject>()?.OnClickTroop();
        }
    }
}
