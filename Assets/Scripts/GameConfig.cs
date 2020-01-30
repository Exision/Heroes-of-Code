using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig", order = 1)]
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    public const string MAP_SCENE_PATH = "Scenes/MapScene";

    [Header("Map Settings")]
    public int enemysCount;

    [Header("Units Stats")]
    public float unitSpeed;
    public float unitChanceToFindPath;

    [Header("Resources Paths")]
    public string resourcesSpritesPath;
}
