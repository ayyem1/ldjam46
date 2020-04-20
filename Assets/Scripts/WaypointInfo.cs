using UnityEngine;

[CreateAssetMenu(fileName = "WaypointInfo", menuName = "ScriptableObjects/WaypointInfo", order = 1)]
public class WaypointInfo : ScriptableObject
{
    public string waypointName = string.Empty;
    public string dialog = string.Empty;
    public Sprite waypointImage;
}
