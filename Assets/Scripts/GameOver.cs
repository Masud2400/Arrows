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
	
    private bool isLost = false;

    void Start()
    {
        gameData = AssetManager.Instance.GameData;
		heartOne = AssetManager.Instance.HeartOne;
		heartTwo = AssetManager.Instance.HeartTwo;
		heartThree = AssetManager.Instance.HeartThree;
		gameOverMenu = AssetManager.Instance.GameOverMenu;
		
		gameData.OnHealthChanged += UpdateHealthNum;
    }

    void Update()
    {
        if (!isLost && gameData.attempts < 0)
        {
            StartCoroutine(DeclareGameOver());
            isLost = true;
        }
    }

    private IEnumerator DeclareGameOver()
    {
		gameOverMenu.gameObject.SetActive(true);
		gameOverMenu.alpha = 0f;

		float duration = 0.5f;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			gameOverMenu.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
			yield return null;
		}

		gameOverMenu.alpha = 1f;
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
