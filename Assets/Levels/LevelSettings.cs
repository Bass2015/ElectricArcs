using UnityEngine;
public enum Dificulty
{
    Easy,
    Medium,
    Hard
}
[CreateAssetMenu]
public class LevelSettings : ScriptableObject
{
    [SerializeField] private int levelNum;
    [SerializeField] private int targetTravelTime;
    [SerializeField] private Dificulty dificulty;

    public int LevelNum { get => levelNum;}
    public int TargetTravelTime { get => targetTravelTime;}
    public Dificulty Dificulty { get => dificulty;}
    
}
