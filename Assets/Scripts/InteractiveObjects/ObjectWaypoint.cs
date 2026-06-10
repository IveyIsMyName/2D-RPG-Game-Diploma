using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectWaypoint : MonoBehaviour
{
	[SerializeField] private string transferToScene;
	[Space]
	[SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType connectedWaypoint;
	[SerializeField] private bool canBeTriggered = true;
	[SerializeField] private Transform respawnPoint;

	public RespawnType GetWaypointType() => waypointType;

	public Vector3 GetPositionAndSetTriggerFalse()
	{
		canBeTriggered = false;
		return respawnPoint == null ? transform.position : respawnPoint.position;
	}

	private void OnValidate()
	{
		gameObject.name = "Object_Waypoint - " + waypointType.ToString() + " - " + transferToScene;

		if (waypointType == RespawnType.Enter)
			connectedWaypoint = RespawnType.Exit;

		if (waypointType == RespawnType.Exit)
			connectedWaypoint = RespawnType.Enter;

				
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (canBeTriggered == false)
			return;

		SaveManager.instance.SaveGame();
		GameManager.instance.ChangeScene(transferToScene, connectedWaypoint);

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		canBeTriggered = true;
	}
}
