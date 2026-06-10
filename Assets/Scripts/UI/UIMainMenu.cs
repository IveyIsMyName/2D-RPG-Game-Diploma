using UnityEngine;

public class UIMainMenu : MonoBehaviour
{

	private void Start()
	{
		transform.root.GetComponentInChildren<UIFadeScreen>().DoFadeIn();
	}

	public void PlayBtn()
    {
        GameManager.instance.ContinuePlay();
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
