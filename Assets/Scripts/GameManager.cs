using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
	public static GameManager instance;

	private Vector3 lastPlayerPosition;
	private string lastScenePlayed;
	private bool dataLoaded = false;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	//public void SetLastPlayerPosition(Vector3 position) => lastPlayerPosition = position;

	public void RestartScene()
	{
		SaveManager.instance.SaveGame();

		string sceneName = SceneManager.GetActiveScene().name;
		ChangeScene(sceneName, RespawnType.None);
	}

	public void ChangeScene(string sceneName, RespawnType respawnType)
	{
		SaveManager.instance.SaveGame();
		Time.timeScale = 1;
		StartCoroutine(ChangeSceneCo(sceneName, respawnType));
	}

	private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
	{
		UIFadeScreen fadeScreen = FindFadeScreenUI();

		fadeScreen.DoFadeOut();
		yield return fadeScreen.fadeEffectCo;

		SceneManager.LoadScene(sceneName);

		dataLoaded = false; // data loaded becomes true when you load game from save manager
		yield return null;

		while(dataLoaded == false)
		{
			yield return null;
		}

		fadeScreen = FindFadeScreenUI();
		fadeScreen.DoFadeIn();

		Player player = Player.instance;

		if (player == null)
			yield break;

		Vector3 position = GetNewPlayerPosition(respawnType);

		if (position != Vector3.zero)
			player.TeleportPlayer(position);
	}

	private UIFadeScreen FindFadeScreenUI()
	{
		if (UI.instance != null)
			return UI.instance.fadeScreenUI;
		else
			return FindAnyObjectByType<UIFadeScreen>();
	}

	private Vector3 GetWayPointPosition(RespawnType type)
	{
		var waypoints = FindObjectsByType<ObjectWaypoint>();

		foreach (var point in waypoints)
		{
			if (point.GetWaypointType() == type)
				return point.GetPositionAndSetTriggerFalse();
		}

		return Vector3.zero;
	}

	private Vector3 GetNewPlayerPosition(RespawnType type)
	{
		if (type == RespawnType.None)
		{
			var data = SaveManager.instance.GetGameData();
			var checkpoints = FindObjectsByType<ObjectCheckpoint>();
			var unlockedCheckpoints = checkpoints
				.Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckPointID(), out bool unlocked) && unlocked)
				.Select(cp => cp.GetPosition())
				.ToList();

			var enterWaypoints = FindObjectsByType<ObjectWaypoint>()
				.Where(wp => wp.GetWaypointType() == RespawnType.Enter)
				.Select(wp => wp.GetPositionAndSetTriggerFalse())
				.ToList();

			var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList(); //combine two lists into one

			if (selectedPositions.Count == 0)
				return Vector3.zero;

			return selectedPositions.OrderBy(position => Vector3.Distance(position, lastPlayerPosition)).First(); //arrange form lowest to highest by comparing distance
		}

		return GetWayPointPosition(type);
	}

	public void ContinuePlay()
	{
		ChangeScene(lastScenePlayed, RespawnType.None);
	}

	public void LoadData(GameData data)
	{
		lastScenePlayed = data.lastScenePlayed;
		lastPlayerPosition = data.lastPlayerPosition;

		if (string.IsNullOrEmpty(lastScenePlayed))
			lastScenePlayed = "Level_0";

		dataLoaded = true;
	}

	public void SaveData(ref GameData data)
	{
		string currentScene = SceneManager.GetActiveScene().name;

		if (currentScene == "MainMenu")
			return;

		data.lastPlayerPosition = Player.instance.transform.position;
		data.lastScenePlayed = currentScene;
		dataLoaded = false;
	}
}
