using System.Collections;
using UnityEngine;

public class TextFly : MonoBehaviour
{
    [SerializeField] private TextMesh _text;

    public void Play(string text, Vector3 fromPosition, Color color)
    {
        _text.text = text;
        _text.color = color;

        Vector3 toPosition = fromPosition;
        toPosition.y += 3f;

        StartCoroutine(Animation(fromPosition, toPosition));
    }

    IEnumerator Animation(Vector3 fromPosition, Vector3 toPosition)
    {
        float factor = 0f;

        while (factor < 1f)
        {
            factor += Time.deltaTime;

            transform.position = Vector3.Lerp(fromPosition, toPosition, factor);

            yield return null;
        }

        Destroy(gameObject);

        yield break;
    }
}