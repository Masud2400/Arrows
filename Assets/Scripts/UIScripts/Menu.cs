using UnityEngine;

public class Menu : MonoBehaviour
{
	[SerializeField] private MoveArrow moveArrow;
	private CanvasGroup gameMenu;
	private CanvasGroup gameOverMenu;
	
    void Start()
    {
		gameMenu = AssetManager.Instance.GameMenu;
		gameOverMenu = AssetManager.Instance.GameOverMenu;
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
		if(gameOverMenu.gameObject.activeSelf == true)
			return;
		
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
