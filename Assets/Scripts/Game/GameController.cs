using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitsStorage))]
public class GameController : SingletonMonoBehaviour<GameController>
{
    [SerializeField] private UnitsStorage _unitsStorage;

    public UnitsStorage UnitsStorage => _unitsStorage;
    public PlayerData PlayerData { get; private set; }
    public List<EnemyData> EnemysDatas { get; private set; } = new List<EnemyData>();
    public List<EnemyData> DefeatedEnemys { get; private set; } = new List<EnemyData>();
    public int CurrentEnemy { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
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
        EnemysDatas.Clear();
        DefeatedEnemys.Clear();

        for (int loop = 0; loop < GameConfig.Instance.enemysCount; loop++)
        {
            int enemyTroopsCount = UnityEngine.Random.Range(1, _unitsStorage.Units.Length + 1);
            List<UnitStats> allTroops = new List<UnitStats>(_unitsStorage.Units);
            List<Troop> troops = new List<Troop>();

            for (int troopIndex = 0; troopIndex < enemyTroopsCount; troopIndex++)
            {
                int randomTroopIndex = UnityEngine.Random.Range(0, allTroops.Count);
                Troop troop = new Troop(allTroops[randomTroopIndex], UnityEngine.Random.Range(GameConfig.Instance.minEnemyUnitsCount, GameConfig.Instance.maxEnemyUnitsCount));

                troops.Add(troop);
                allTroops.Remove(allTroops[randomTroopIndex]);
            }

            EnemysDatas.Add(new EnemyData(loop, troops));
        }
    }

    public void StartFight(int enemyGroup)
    {
        MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
        messageWindow.onClickOkButton = () =>
        {
            messageWindow.Hide();

            CurrentEnemy = enemyGroup;

            SceneLoader.Instance.LoadScene(GameConfig.BATTLE_SCENE_PATH);
        };
        messageWindow.Show(Localization.Instance.Get("start_fight"));
    }

    public void EndBattle(List<Troop> group)
    {
        PlayerData.SetGroup(group);

        DefeatedEnemys.Add(EnemysDatas[CurrentEnemy]);

        CurrentEnemy = -1;

        if (EnemysDatas.Count == DefeatedEnemys.Count)
            EndGame();
        else
            SceneLoader.Instance.LoadScene(GameConfig.MAP_SCENE_PATH);
    }

    public void EndGame()
    {
        MessageWindow messageWindow = WindowManager.Instance.GetWindow<MessageWindow>();
        messageWindow.onClickOkButton = () =>
        {
            messageWindow.Hide();

            Start();
        };
        messageWindow.Show(Localization.Instance.Get("end_game"));
    }
}
