using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingUIScript : MonoBehaviour
{
    [SerializeField] private float loadingSpeed = 0.5f;
    [SerializeField] private float minPause = 0.1f;
    [SerializeField] private float maxPause = 0.5f;
	
	private Image loadingImage;
    private Image loadingBar;
	private Image blackScreen;
	private TextMeshProUGUI loadingText;

    void Start()
    {
		loadingImage = AssetManager.Instance.LoadingImage;
        loadingBar = AssetManager.Instance.LoadingBar;
		blackScreen = AssetManager.Instance.BlackScreen;
		loadingText = AssetManager.Instance.LoadingText;
		
        loadingBar.fillAmount = 0f;
    }

    private IEnumerator LoadBar()
    {
		loadingBar.fillAmount = 0f; // Remove it later
		
		StartCoroutine(FadeBlackScreen.FadeBlack(0, blackScreen));
		
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
		
		StartCoroutine(FadeBlackScreen.FadeBlack(1, blackScreen));
    }
	
	public void RenderLoadingScreen()
	{
		StartCoroutine(LoadingSequence());
	}

	private IEnumerator LoadingSequence()
	{
		yield return StartCoroutine(LoadBar());
		yield return StartCoroutine(FadeBlackScreen.FadeBlack(1, loadingText));

		loadingImage.gameObject.SetActive(false);
		loadingBar.gameObject.SetActive(false);
		loadingText.gameObject.SetActive(false);

		yield return StartCoroutine(FadeBlackScreen.FadeBlack(0, blackScreen));
		blackScreen.gameObject.SetActive(false);
	}
}
