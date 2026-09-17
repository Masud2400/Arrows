using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
	private Data gameData;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
	}
	
    public void RestartCurrentScene()
	{
		gameData.ResetData();
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
