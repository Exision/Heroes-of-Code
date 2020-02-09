using UnityEngine;
using UnityEngine.UI;

public class LocalizadText : MonoBehaviour
{
    [SerializeField] string localizedKey = "INSERT_KEY_HERE";

    void Start()
    {
        Text text = GetComponent<Text>();
        text.text = Localization.Instance.Get(localizedKey);
    }
}
