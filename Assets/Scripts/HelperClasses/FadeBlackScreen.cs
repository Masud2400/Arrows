using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public static class FadeBlackScreen
{
	public static IEnumerator FadeBlack<T>(float targetAlpha, T targetObject) where T : Graphic
	{
		float startAlpha = targetObject.color.a;
		float elapsed = 0f;
		float duration = 1f;
		Color color = targetObject.color;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
			targetObject.color = color;
			yield return null;
		}

		color.a = targetAlpha;
		targetObject.color = color;
	}
}
