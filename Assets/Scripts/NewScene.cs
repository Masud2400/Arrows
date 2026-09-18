using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class NewScene : MonoBehaviour
{
	private Data gameData;
	private Image blackScreen;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		blackScreen = AssetManager.Instance.BlackScreen;
	}
	
    private IEnumerator ActivateRestart(string scene)
	{
		gameData.ResetData();
		Time.timeScale = 1f;
		
		blackScreen.gameObject.SetActive(true);
		
		yield return StartCoroutine(FadeBlackScreen.FadeBlack(1, blackScreen));
		
		SceneManager.LoadScene(scene);
	}
	
	public void RestartCurrentScene()
	{
		StartCoroutine(ActivateRestart("Game"));
	}
	
	public void Exit()
	{
		StartCoroutine(ActivateRestart("Main"));
	}
}
