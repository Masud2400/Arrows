using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

public class NewScene : MonoBehaviour
{
	private Data gameData;
	private Image blackScreen;
	private ParticleSystem firework;
	
	public static event Action OnFireworkStop;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		blackScreen = AssetManager.Instance.BlackScreen;
		firework = AssetManager.Instance.Firework;
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
		
		firework.Stop();
		OnFireworkStop?.Invoke();
	}
	
	public void Exit()
	{
		StartCoroutine(ActivateRestart("Main"));
		
		firework.Stop();
		OnFireworkStop?.Invoke();
	}
}
