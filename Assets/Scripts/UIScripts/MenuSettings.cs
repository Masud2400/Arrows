using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    private CanvasGroup settingsMenu;
	
    void Start()
    {
		settingsMenu = AssetManager.Instance.SettingsMenu;
    }
	
	public void ShowSettingsMenu()
	{
		if(settingsMenu.gameObject.activeSelf == true)
		{
			settingsMenu.gameObject.SetActive(false);
			return;
		}
		
		StartCoroutine(UIAppear.ShowUI(settingsMenu));
	}
}
