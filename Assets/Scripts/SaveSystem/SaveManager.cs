using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public static SaveManager instance;

	private FileDataHandler dataHandler;
	private GameData gameData;
	private List<ISaveable> allSaveables;

	[SerializeField] private string fileName = "savefile.json";
	[SerializeField] private bool encryptData = true;

	private void Awake()
	{
		instance = this;
	}

	private IEnumerator Start()
	{
		Debug.Log(Application.persistentDataPath);
		dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
		allSaveables = FindIsaveables();

		yield return null;
		LoadGame();
	}

	private void LoadGame()
	{
		gameData = dataHandler.LoadData();

		if (gameData == null)
		{
			Debug.Log("No Save data found, creating a new save!");
			gameData = new GameData();
			return;
		}

		foreach (var saveable in allSaveables)
			saveable.LoadData(gameData);
	}

	public void SaveGame()
	{
		foreach (var saveable in allSaveables)
			saveable.SaveData(ref gameData);

		dataHandler.SaveData(gameData);
	}

	[ContextMenu("*** Delete Save Data ***")]
	public void DeleteSavedData()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
		dataHandler.Delete();

		LoadGame();
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}

	public GameData GetGameData() => gameData;

	private List<ISaveable> FindIsaveables()
	{
		return
			FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include)
			.OfType<ISaveable>()
			.ToList();
	}
}
