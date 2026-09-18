using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] private MoveArrow moveArrow;
	private CanvasGroup gameMenu;
	private CanvasGroup winOrGameOver;
	
    void Start()
    {
		gameMenu = AssetManager.Instance.GameMenu;
		winOrGameOver = AssetManager.Instance.WinOrGameOver;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowGameMenu();
        }
    }
	
	public void ShowGameMenu()
	{	
		if(gameMenu.gameObject.activeSelf == true)
		{
			gameMenu.gameObject.SetActive(false);
			ResumeGame();
			return;
		}
		
		StartCoroutine(UIAppear.ShowUI(gameMenu));
		
		PauseGame();
	}
	
	private void PauseGame()
    {
        Time.timeScale = 0f;
		moveArrow.enabled = false;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
		moveArrow.enabled = true;		
    }
}
