using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonMonoBehaviour<GameController>
{
    [SerializeField] private UnitsManager _unitsManager;

    public UnitsManager UnitsManager => _unitsManager;

    private PlayerData _playerData;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        CreateGroupWindow createGroupWindow = WindowManager.Instance.GetWindow<CreateGroupWindow>();
        createGroupWindow.onGroupReady = (Dictionary<int, int> group) =>
        {
            createGroupWindow.Hide();
        };
        createGroupWindow.Show();


    }

    [ContextMenu("Show Window")]
    private void ShowWindow()
    {
        MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
        messageWindow.onClickOkButton = () => messageWindow.Hide();
        messageWindow.Show("rer");
    }
}
