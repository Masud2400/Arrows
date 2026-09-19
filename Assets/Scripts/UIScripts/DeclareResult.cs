using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class DeclareResult : MonoBehaviour
{
    private Data gameData;
	private Image heartOne;
	private Image heartTwo;
	private Image heartThree;
	private CanvasGroup winOrGameOver;
	private CanvasGroup gameMenu;
	private GameObject background;
	private TextMeshProUGUI resultText;
	private Transform spawnParent;
	private ParticleSystem firework;
	
    private bool isLost = false;
	private bool isWin = false;
	
	public static event Action OnFireworkStart;

    void Start()
    {
        gameData = AssetManager.Instance.GameData;
		heartOne = AssetManager.Instance.HeartOne;
		heartTwo = AssetManager.Instance.HeartTwo;
		heartThree = AssetManager.Instance.HeartThree;
		winOrGameOver = AssetManager.Instance.WinOrGameOver;
		gameMenu = AssetManager.Instance.GameMenu;
		background = AssetManager.Instance.Background;
		resultText = winOrGameOver.GetComponentInChildren<TextMeshProUGUI>();
		spawnParent = AssetManager.Instance.SpawnParent;
		firework = AssetManager.Instance.Firework;
		
		gameData.OnHealthChanged += UpdateHealthNum;
    }

    void Update()
    {
        if (!isLost && gameData.attempts < 0)
        {
            DeclareGameResult("Game Over");
            isLost = true;
        }
		
		if(!isWin && gameData.gameInitialized && gameData.lineCount <= 0)
		{
			DeclareGameResult("You Won !!!");
			isWin = true;
			firework.Play();
			OnFireworkStart?.Invoke();
		}
    }
	
	private void DeclareGameResult(string text)
	{
		if(gameMenu.gameObject.activeSelf == true)
			gameMenu.gameObject.SetActive(false);
		
		SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
		//spriteRenderer.sortingOrder = 1;
		
		resultText.text = text;
		
		StartCoroutine(UIAppear.ShowUI(winOrGameOver));
	}

    private void OnDisable()
    {
        gameData.OnHealthChanged -= UpdateHealthNum;
    }
	
	private void UpdateHealthNum(int attempts)
	{
		heartOne.gameObject.SetActive(attempts >= 1);
		heartTwo.gameObject.SetActive(attempts >= 2);
		heartThree.gameObject.SetActive(attempts >= 3);
	}
}
