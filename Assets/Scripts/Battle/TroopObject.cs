using System;
using UnityEngine;

public class TroopObject : MonoBehaviour
{
    public Action onClickTroop;

    [SerializeField] private SpriteRenderer _unitSprite;
    [SerializeField] private SpriteRenderer _highlightImage;
    [SerializeField] private SpriteRenderer _currentTroopImage;
    [SerializeField] private TextMesh _unitsCount;

    public SpriteRenderer UnitSprite => _unitSprite;
    public SpriteRenderer HighlightImage => _highlightImage;
    public SpriteRenderer ActiveImage => _currentTroopImage;
    public TextMesh UnitsCount => _unitsCount;

    public void OnClickTroop()
    {
        onClickTroop?.Invoke();
    }
}
