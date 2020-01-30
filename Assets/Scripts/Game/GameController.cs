using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonMonoBehaviour<GameController>
{
    [SerializeField] private UnitsStorage _unitsStorage;

    public UnitsStorage UnitsStorage => _unitsStorage;

    public PlayerData PlayerData { get; private set; }

    public Dictionary<int, List<Troop>> EnemysDatas { get; private set; } = new Dictionary<int, List<Troop>>();

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

            PlayerData = new PlayerData(group);

            GenerateEnemys();

            SceneLoader.Instance.LoadScene(GameConfig.MAP_SCENE_PATH);
        };
        createGroupWindow.Show();
    }

    private void GenerateEnemys()
    {
        for (int loop = 0; loop < GameConfig.Instance.enemysCount; loop++)
        {
            int enemyTroopsCount = Random.Range(1, _unitsStorage.Units.Length);
            List<UnitStats> allTroops = new List<UnitStats>(_unitsStorage.Units);
            List<Troop> troops = new List<Troop>();

            for (int troopIndex = 0; troopIndex < enemyTroopsCount; troopIndex++)
            {
                int randomTroopIndex = Random.Range(0, allTroops.Count);
                Troop troop = new Troop(allTroops[randomTroopIndex], Random.Range(1, 200));

                troops.Add(troop);
                allTroops.Remove(allTroops[randomTroopIndex]);
            }

            EnemysDatas.Add(loop, troops);
        }
    }

    public void StartFight(int enemyGroup)
    {
        Debug.Log($"Fight with enemy {enemyGroup}, troops count {EnemysDatas[enemyGroup].Count}, first is {EnemysDatas[enemyGroup][0].UnitStats.id}");
    }

    [ContextMenu("Show Window")]
    private void ShowWindow()
    {
        MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
        messageWindow.onClickOkButton = () => messageWindow.Hide();
        messageWindow.Show("rer");
    }
}
