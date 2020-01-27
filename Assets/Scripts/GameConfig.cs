using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObject/GameConfig", order = 1)]
public class GameConfig : SingletonScriptableObject<GameConfig>
{
    public float unitSpeed;

    public float unitChanceToFindPath;
}
