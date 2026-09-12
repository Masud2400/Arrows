using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using TMPro;

[System.Serializable]
public class ArrowData
{
	public Vector3 position;
	public Vector2Int index;
    public string arrowName;
	public int angle;
	public bool head;
}

[System.Serializable]
public class DataWrapper
{
    public List<ArrowData> arrows = new List<ArrowData>();
}

public class DebugInterface : MonoBehaviour
{
	private Data gameData;        
    private Transform spawnParent;
	private Camera cam;
	[SerializeField] private GameObject part;
	[SerializeField] private Toggle headOrNot;
	[SerializeField] private TMP_InputField angleInput;
	[SerializeField] private TMP_InputField arrowNameInput;
	
	private Dictionary<Vector3, VectorPositions> heatMap;
	private Dictionary<Vector2Int, GridCell> locations;
	
	private string filePath;
	private int arrowCounter = 0;
	private bool head = false;
	private int angle = 0;
	
	void Awake()
	{
		string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		filePath = Path.Combine(documentsPath, "data.json");
	}
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		spawnParent = AssetManager.Instance.SpawnParent;
		cam = AssetManager.Instance.Cam;
		heatMap = gameData.heatMap;
		locations = gameData.locations;
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

			if(hit.collider != null && hit.collider.gameObject != null)
			{	
				var key = hit.transform.position;
				VectorPositions pos = heatMap[key];
				
				SaveArrowToJson(key, pos.vectorIndex.x, pos.vectorIndex.y);
				
				SpriteRenderer img = hit.collider.gameObject.GetComponent<SpriteRenderer>();
				img.color = Color.red;
			}
        }
	}
	
	public void MakeInterface()
	{
		foreach(var pair in locations)
		{
			Vector2Int index = pair.Key;
			GridCell cell = pair.Value;
			
			Vector3 spawnPosition = cell.position;
			
			GameObject spawnedObj = Instantiate(part, spawnParent);
			spawnedObj.transform.localPosition = spawnPosition;
			
			SpriteRenderer img = spawnedObj.GetComponent<SpriteRenderer>();
			
			float hue = ((cell.layer - 1) * 0.61803398875f) % 1.0f;
			img.color = Color.HSVToRGB(hue, 0.5f, 1.0f);
		}
	}
	
	public DataWrapper LoadData()
    {
        if (!File.Exists(filePath))
        {
            return new DataWrapper();
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<DataWrapper>(json) ?? new DataWrapper();
    }
	
	public void SaveArrowToJson(Vector3 position, int row, int col)
	{
		DataWrapper wrapper = LoadData();
		
        wrapper.arrows.Add(new ArrowData
		{
			arrowName = $"Arrow {arrowCounter}",
			position = position,
			index = new Vector2Int(row, col),
			angle = angle,
			head = head
		});

        string json = JsonUtility.ToJson(wrapper, prettyPrint: true);
        File.WriteAllText(filePath, json);
	}
	
	public void SetArrow()
	{
		string input = arrowNameInput.text;
		
		Debug.Log(input);
		
		if(int.TryParse(input, out int result))
		{
			arrowCounter = result;
		} 
		
		gameData.currentArrow = $"Arrow {arrowCounter}";
	}
	
	public void ReadInputField()
    {
        string input = angleInput.text;
		
		Debug.Log(input);
		
		if(int.TryParse(input, out int result))
		{
			angle = result;
		}
    }
	
	public void ChangeHeadValue()
	{
		head = headOrNot.isOn;
		Debug.Log(head);
	}
}
