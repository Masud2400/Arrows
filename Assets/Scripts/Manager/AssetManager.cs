using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get; private set; }
	
	[SerializeField] private Data gameData;
	[SerializeField] private GameObject line;
	[SerializeField] private Camera cam;
	[SerializeField] private Transform spawnParent;
	[SerializeField] private GameObject head;
	[SerializeField] private Image heartOne;
	[SerializeField] private Image heartTwo;
	[SerializeField] private Image heartThree;
	[SerializeField] private CanvasGroup gameOverMenu;
	[SerializeField] private CanvasGroup gameMenu;
	[SerializeField] private GameObject background;
	[SerializeField] private CanvasGroup settingsMenu;
	
	public Data GameData => gameData;
	public GameObject Line => line;
	public Camera Cam => cam;
	public Transform SpawnParent => spawnParent;
	public GameObject Head => head;
	public Image HeartOne => heartOne;
	public Image HeartTwo => heartTwo;
	public Image HeartThree => heartThree;
	public CanvasGroup GameOverMenu => gameOverMenu;
	public CanvasGroup GameMenu => gameMenu;
	public GameObject Background => background;
	public CanvasGroup SettingsMenu => settingsMenu;

    private void Awake()
    {	
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
