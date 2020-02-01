using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig", order = 1)]
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    public const string MAP_SCENE_PATH = "Scenes/MapScene";
    public const string BATTLE_SCENE_PATH = "Scenes/BattleScene";

    [Header("Map Settings")]
    public int enemysCount;

    [Header("Battle Settings")]
    public Vector2 basePlayerGroupPosition;
    public Vector2 baseEnemyGroupPosition;
    public Vector2 groupPositionOffset;

    [Header("Units Stats")]
    public float unitSpeed;
    public float unitChanceToFindPath;

    [Header("Resources Paths")]
    public string resourcesSpritesPath;
    public string troopsPrefabsPath;
}
