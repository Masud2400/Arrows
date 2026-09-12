using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class ArrowWrapper
{
    public List<ArrowEntry> arrows;
}

[Serializable]
public class ArrowEntry
{
	public string arrowName;
	public Vector3 position;
	public Quaternion rotation = Quaternion.identity;
	public bool head;
	public Vector2Int index;
	public int angle;
}

[Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}

public class DebugArrowMaker : MonoBehaviour
{
	private Data gameData;
	
    private Dictionary<string, List<VectorData>> arrowDict;
	private Dictionary<Vector2Int, GridCell> locations;
	private HashSet<Vector3> occupiedPositions;
	
	private string filePath;
	
	void Awake()
	{
		string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		filePath = Path.Combine(documentsPath, "data.json");
	}
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		arrowDict = gameData.arrowDict;
		occupiedPositions = gameData.occupiedPositions;
		locations = gameData.locations;
	}
	
	private ArrowWrapper LoadData()
    {
        return JsonUtility.FromJson<ArrowWrapper>(File.ReadAllText(filePath));
    }

	private void FillArrowDict()
	{	
		ArrowWrapper data = LoadData();

		foreach (ArrowEntry entry in data.arrows)
		{	
			string arrow = entry.arrowName;

			if (!arrowDict.TryGetValue(arrow, out List<VectorData> arrowData))
			{
				arrowData = new List<VectorData>();
				arrowDict.Add(arrow, arrowData);
			}

			Vector3 vectorPos = Vector3.zero;

			vectorPos = new Vector3(
				entry.position.x,
				entry.position.y,
				entry.position.z);

			arrowData.Add(new VectorData
			{
				position = vectorPos,
				index = new Vector2Int(entry.index.x, entry.index.y),
				head = entry.head,
				angle = entry.angle,
				rotation = Quaternion.Euler(0, 0, entry.angle)
			});
			
			occupiedPositions.Add(vectorPos);
		}
	}
	
	private void FillLocations()
	{
		ArrowWrapper data = LoadData();

		foreach (ArrowEntry entry in data.arrows)
		{	
			Vector2Int index = entry.index;

			locations[index].arrowName = entry.arrowName;
			locations[index].head = entry.head;
			locations[index].angle = entry.angle;
		}
	}
	
	public void Fill()
	{
		FillArrowDict();
		FillLocations();
	}
}
