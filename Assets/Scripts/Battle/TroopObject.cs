using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopObject : MonoBehaviour
{
    public Action onClickTroop;

    [SerializeField] private SpriteRenderer _highlightImage;

    public SpriteRenderer HighlightImage
    {
        get => _highlightImage;
    }

    public void OnClickTroop()
    {
        onClickTroop?.Invoke();
    }
}
