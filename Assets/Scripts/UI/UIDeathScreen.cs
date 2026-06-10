using UnityEngine;

public class UIDeathScreen : MonoBehaviour
{
    public void GoToCheckPointBtn()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenuBtn()
    {
		GameManager.instance.ChangeScene("MainMenu", RespawnType.None);
	}
}
