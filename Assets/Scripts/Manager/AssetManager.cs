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
	[SerializeField] private CanvasGroup gameMenu;
	[SerializeField] private GameObject background;
	
	[Header("Attempts and Game Finish")]
	[SerializeField] private Image heartOne;
	[SerializeField] private Image heartTwo;
	[SerializeField] private Image heartThree;
	[SerializeField] private CanvasGroup winOrGameOver;
	
	[Header("Settings")]
	[SerializeField] private CanvasGroup settingsMenu;
	[SerializeField] private Scrollbar musicBar;
	[SerializeField] private Scrollbar soundEffectBar;
	
	[Header("Loading UI")]
	[SerializeField] private Image loadingBar;
	[SerializeField] private Image blackScreen;
	[SerializeField] private TextMeshProUGUI loadingText;
	[SerializeField] private Image loadingImage;
	
	public Data GameData => gameData;
	public GameObject Line => line;
	public Camera Cam => cam;
	public Transform SpawnParent => spawnParent;
	public GameObject Head => head;
	public Image HeartOne => heartOne;
	public Image HeartTwo => heartTwo;
	public Image HeartThree => heartThree;
	public CanvasGroup WinOrGameOver => winOrGameOver;
	public CanvasGroup GameMenu => gameMenu;
	public GameObject Background => background;
	public CanvasGroup SettingsMenu => settingsMenu;
	public Scrollbar MusicBar => musicBar;
	public Scrollbar SoundEffectBar => soundEffectBar;
	public Image LoadingBar => loadingBar;
	public Image BlackScreen => blackScreen;
	public TextMeshProUGUI LoadingText => loadingText;
	public Image LoadingImage => loadingImage;

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
