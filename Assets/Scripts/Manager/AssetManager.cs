using UnityEngine;
using UnityEngine.UI;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get; private set; }
	
	[SerializeField] private Data gameData;
	[SerializeField] private GameObject line;
	[SerializeField] private Camera cam;
	
	[SerializeField] private GameObject prefabToSpawn;        
    [SerializeField] private Transform spawnParent;
	
	public Data GameData => gameData;
	public GameObject Line => line;
	public Camera Cam => cam;
	
	public GameObject PrefabToSpawn => prefabToSpawn;
	public Transform SpawnParent => spawnParent;

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
