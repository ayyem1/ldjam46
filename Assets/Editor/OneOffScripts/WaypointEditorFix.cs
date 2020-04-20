using UnityEngine;

[ExecuteInEditMode]
public class WaypointEditorFix : MonoBehaviour
{
    private void Update()
    {
        Waypoint originalWaypoint = GetComponentInChildren<Waypoint>();

        originalWaypoint.dialogTextMesh.text = originalWaypoint.waypointInfo.dialog;
        originalWaypoint.waypointName.text = originalWaypoint.waypointInfo.waypointName;
        originalWaypoint.waypointSprite.sprite = originalWaypoint.waypointInfo.waypointImage;
    }
}
