using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainUIScript : MonoBehaviour
{
	[SerializeField] private Image blackScreen;
	
    void Start()
    {
        StartCoroutine(MakeTransparent(0));
    }
	
	private IEnumerator MakeTransparent(int value)
	{
		yield return StartCoroutine(FadeBlackScreen.FadeBlack(value, blackScreen));
		
		blackScreen.gameObject.SetActive(false);
	}
	
	private IEnumerator MakeBlack(int value)
	{
		blackScreen.gameObject.SetActive(true);
		
		yield return StartCoroutine(FadeBlackScreen.FadeBlack(value, blackScreen));
	}
	
	private IEnumerator TransitionToNewScene()
	{
		yield return StartCoroutine(MakeBlack(1));
		
		SceneManager.LoadScene("Game");
	}
	
	public void Play()
	{
		StartCoroutine(TransitionToNewScene());
	}
	
	public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
