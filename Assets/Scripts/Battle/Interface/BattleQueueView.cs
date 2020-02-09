using UnityEngine;
using UnityEngine.UI;

public class BattleQueueView : MonoBehaviour
{
    [SerializeField] private Image _troopImage;
    [SerializeField] private Image _selectedFrame;
    [SerializeField] private Image _viewBackground;

    private BattleQueueElement _battleElement;

    public void Init(BattleQueueElement element)
    {
        _battleElement = element;

        _troopImage.sprite = Resources.Load<Sprite>($"{GameConfig.Instance.troopsImagePath}TroopIcon_{_battleElement.Troop.UnitStats.id}");
        _viewBackground.color = _battleElement.Team == E_BatteElementTeam.Player ? Color.white : Color.red;

        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        _selectedFrame.gameObject.SetActive(isSelected);
    }
}
