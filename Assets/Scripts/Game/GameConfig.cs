using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig", order = 1)]
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    public const string MAIN_SCENE_PATH = "Scenes/MainScene";
    public const string MAP_SCENE_PATH = "Scenes/MapScene";
    public const string BATTLE_SCENE_PATH = "Scenes/BattleScene";

    [Header("Game Settings")]
    public int minEnemyUnitsCount;
    public int maxEnemyUnitsCount;

    [Header("Map Settings")]
    public int enemysCount;
    public Vector3 playerStartPosition;

    [Header("Battle Settings")]
    public Vector2 basePlayerGroupPosition;
    public Vector2 baseEnemyGroupPosition;
    public Vector2 groupPositionOffset;
    public float enemyUseSkillChance;

    [Header("Units Stats")]
    public float unitSpeed;
    public float unitChanceToFindPath;

    [Header("Resources Paths")]
    public string troopsPrefabsPath;
    public string troopsImagePath;
    public string skillImagePath;

    public static int FindMultiple(int first, int second)
    {
        int multiple = first * second;

        for (int loop = 0; loop <= (first * second); loop++)
        {
            if (loop % first == 0 && loop % second == 0)
            {
                multiple = loop;

                if (loop != 0)
                    break;
            }
        }

        return multiple;
    }
}
