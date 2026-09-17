using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SetBlocks : MonoBehaviour
{	
	private Data gameData;

	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<string, List<VectorData>> arrowDict;
	private HashSet<Vector3> occupiedPositions; 
	private Dictionary<Vector3, VectorPositions> heatMap;
	
	private List<Vector3> availableVectors;
	
	private int counter = 0;
	private int currentLayer;
	private bool layerInitialized = false;
	
	private Vector3 randomVector;
	private Vector2Int randomVectorIndex;
	private int angle;
	private Vector2Int? headIndex = null;
	private Vector3 headPos;
	
	private Vector2Int[] directions;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		locations = gameData.locations;
		arrowDict = gameData.arrowDict;
		occupiedPositions = gameData.occupiedPositions;
		heatMap = gameData.heatMap;
		
		directions = Directions.directions;
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
		if (availableVectors.Count == 0)
			return;
		
		int index = Random.Range(0, availableVectors.Count);
		int angleIndex = Random.Range(0, 4);
		
		var randomizedDirections = GetRandomAngle();
		
		randomVector = availableVectors[index];
		angle = Directions.GetHeadAngle(randomizedDirections[angleIndex]);
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
		randomVectorIndex = heatMap[randomVector].vectorIndex;
	}
	
	private List<Vector2Int> GetRandomAngle()
	{
		return directions.OrderBy(d => Random.value).ToList();
	}
	
	private void GetHeadPosition()
	{
		Vector2Int newIndex;
		var randomizedDirections = GetRandomAngle();
		
		foreach(var i in randomizedDirections)
		{
			newIndex = randomVectorIndex + i;
			
			if(!locations.ContainsKey(newIndex))
			{
				headIndex = null;
				return;
			}
			
			if(!occupiedPositions.Contains(locations[newIndex].position))
			{
				angle = Directions.GetHeadAngle(i);
				
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
			
			locations[headIndex.Value].arrowName = arrowName;
			locations[headIndex.Value].head = first;
			locations[headIndex.Value].angle = angle;
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
		
		locations[randomVectorIndex].arrowName = arrowName;
		locations[randomVectorIndex].head = second;
		locations[randomVectorIndex].angle = angle;
		
		gameData.currentArrow = arrowName;
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
