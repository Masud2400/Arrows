using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingUIScript : MonoBehaviour
{
    [SerializeField] private float loadingSpeed = 0.5f;
    [SerializeField] private float minPause = 0.1f;
    [SerializeField] private float maxPause = 0.5f;

    private Image loadingBar;
	private Image blackScreen;

    void Start()
    {
        loadingBar = AssetManager.Instance.LoadingBar;
		blackScreen = AssetManager.Instance.BlackScreen;
		
        loadingBar.fillAmount = 0f;
    }

    private IEnumerator LoadBar()
    {
		loadingBar.fillAmount = 0f; // Remove it later
		
		StartCoroutine(FadeBlackIn());
		
        while (loadingBar.fillAmount < 1f)
        {
            loadingBar.fillAmount += loadingSpeed * Time.deltaTime;

            // Randomly pause while loading
            if (Random.value < 0.01f)
            {
                yield return new WaitForSeconds(
                    Random.Range(minPause, maxPause)
                );
            }

            yield return null;
        }

        loadingBar.fillAmount = 1f;
		
		StartCoroutine(FadeBlackOut());
    }
	
	private IEnumerator FadeBlackIn()
	{
		float duration = 1f;
		float elapsed = 0f;
		Color color = blackScreen.color;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
			blackScreen.color = color;
			yield return null;
		}

		color.a = 1f;
		blackScreen.color = color;
	}

	private IEnumerator FadeBlackOut()
	{
		float duration = 1f;
		float elapsed = 0f;
		Color color = blackScreen.color;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			color.a = Mathf.Lerp(1f, 0f, elapsed / duration);
			blackScreen.color = color;
			yield return null;
		}

		color.a = 0f;
		blackScreen.color = color;
	}
	
	public void RenderLoadingScreen()
	{
		StartCoroutine(LoadBar());
	}
}
