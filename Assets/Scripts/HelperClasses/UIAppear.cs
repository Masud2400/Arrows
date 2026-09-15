using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public static class UIAppear
{
    public static IEnumerator ShowUI(CanvasGroup gameOverMenu)
	{
		gameOverMenu.gameObject.SetActive(true);
		gameOverMenu.alpha = 0f;

		float duration = 0.5f;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.unscaledDeltaTime;
			gameOverMenu.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
			yield return null;
		}

		gameOverMenu.alpha = 1f;
	}
}
