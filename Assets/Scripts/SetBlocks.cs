using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SetBlocks : MonoBehaviour
{	
	private Data gameData;

	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<string, List<VectorData>> arrowDict;
	private HashSet<Vector3> occupiedPositions; // * 
	private Dictionary<Vector3, VectorPositions> heatMap;
	
	private List<Vector3> availableVectors;
	
	private int counter = 0;
	private int currentLayer;
	private bool layerInitialized = false;
	
	private readonly Vector2Int[] directions = new Vector2Int[]
	{
		new Vector2Int(0, -1), // left
		new Vector2Int(0, 1),  // right
		new Vector2Int(1, 0),  // down
		new Vector2Int(-1, 0)  // up
	};
	
	private Vector3 randomVector;
	private Vector2Int randomVectorIndex;
	private int angle;
	private Vector2Int? headIndex = null;
	private Vector3 headPos;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		locations = gameData.locations;
		arrowDict = gameData.arrowDict;
		occupiedPositions = gameData.occupiedPositions; // *
		heatMap = gameData.heatMap;
	}
	
	private void GetCurrentLayer()
	{
		if(!layerInitialized)
		{
			currentLayer = locations.Values.Max(c => c.layer);
			layerInitialized = true;
		}
		
		bool isLayerFull = locations.Values
			.Where(c => c.layer == currentLayer)
			.All(c => occupiedPositions.Contains(c.position));

		if (isLayerFull)
		{
			currentLayer--;
		}
	}
	
	private void GetAvailableVectors()
	{
		availableVectors = locations.Values
			.Where(c => c.layer == currentLayer && !occupiedPositions.Contains(c.position))
			.Select(c => c.position)
			.ToList();
		
		if (availableVectors.Count == 0)
		{
			return; 
		}
	}

    private void SetRandomLocation()
    {			
		int index = Random.Range(0, availableVectors.Count);	
		randomVector = availableVectors[index];
    }
	
	private void SpawnParent(out string arrowName)
	{
		counter += 1;
		
		arrowName = "Arrow" + counter;
		
		if (!arrowDict.ContainsKey(arrowName))
		{
			arrowDict[arrowName] = new List<VectorData>();
		}
	}
	
	private void SetRandVecIndex()
	{
		var match = locations.FirstOrDefault(pair => pair.Value.position == randomVector);
		randomVectorIndex = match.Key;
	}
	
	private int GetHeadAngle(Vector2Int index)
	{		
		return index switch
		{
			var v when v == directions[0] => 0,
			var v when v == directions[1] => 180,
			var v when v == directions[2] => 90,
			var v when v == directions[3] => 270,
			_ => 0 // Default fallback
		};
	}
	
	private void GetHeadPosition()
	{
		Vector2Int newIndex;
		var randomizedDirections = directions.OrderBy(d => Random.value).ToList();
		
		foreach(var i in randomizedDirections)
		{
			newIndex = randomVectorIndex + i;
			
			if(!occupiedPositions.Contains(locations[newIndex].position))
			{
				angle = GetHeadAngle(i);
				
				headIndex = newIndex;
				return;
			}
			newIndex = new Vector2Int(0, 0);
		}
		
		headIndex = null;
	}
	
	private void SaveToArrowDict()
	{
		SpawnParent(out string arrowName);
		
		Quaternion rotation = Quaternion.Euler(0, 0, angle);
		
		bool first = headIndex != null ? true : false;
		bool second = headIndex != null ? false : true;
		
		if(headIndex != null)
		{
			arrowDict[arrowName].Add(
				new VectorData { 
					position = headPos, 
					rotation = rotation, 
					head = first,
					index = headIndex.Value,
					angle = angle
				}
			);
		}
		
		arrowDict[arrowName].Add(
			new VectorData { 
				position = randomVector, 
				rotation = rotation, 
				head = second,
				index = randomVectorIndex,
				angle = angle
			}
		);
	}
	
	private void SaveToOccupiedPositions()
	{
		occupiedPositions.Add(randomVector);
		heatMap[randomVector].isOccupied = true;
		
		if(headIndex == null) return;
		
		occupiedPositions.Add(headPos);
		heatMap[headPos].isOccupied = true;
	}
	
	private void SaveFirstBlockData()
	{	
		SetRandVecIndex();
		GetHeadPosition();
		
		if(headIndex != null)
			headPos = locations[headIndex.Value].position;
		
		SaveToArrowDict();
		SaveToOccupiedPositions();	
	}
	
	public void SpawnBlock()
	{
		GetCurrentLayer();
		
		GetAvailableVectors();
		
		SetRandomLocation();
		
		SaveFirstBlockData();
		
		gameData.currentLayer = currentLayer;
	}
}
