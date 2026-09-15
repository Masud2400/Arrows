using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Data gameData;
	private Image heartOne;
	private Image heartTwo;
	private Image heartThree;
	private CanvasGroup gameOverMenu;
	private CanvasGroup gameMenu;
	private GameObject background;
	
    private bool isLost = false;

    void Start()
    {
        gameData = AssetManager.Instance.GameData;
		heartOne = AssetManager.Instance.HeartOne;
		heartTwo = AssetManager.Instance.HeartTwo;
		heartThree = AssetManager.Instance.HeartThree;
		gameOverMenu = AssetManager.Instance.GameOverMenu;
		gameMenu = AssetManager.Instance.GameMenu;
		background = AssetManager.Instance.Background;
		
		gameData.OnHealthChanged += UpdateHealthNum;
    }

    void Update()
    {
        if (!isLost && gameData.attempts < 0)
        {
            DeclareGameOver();
            isLost = true;
        }
    }

    private void DeclareGameOver()
    {
		if(gameMenu.gameObject.activeSelf == true)
			gameMenu.gameObject.SetActive(false);
		
		SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
		spriteRenderer.sortingOrder = 1;
		
		StartCoroutine(UIAppear.ShowUI(gameOverMenu));
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
