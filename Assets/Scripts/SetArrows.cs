using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class SetArrows : MonoBehaviour
{
    private Data gameData;

	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<string, List<VectorData>> arrowDict;
	private HashSet<Vector3> occupiedPositions;
	private Dictionary<Vector3, VectorPositions> heatMap;
	
	private List<Vector2Int> indices = new List<Vector2Int>();
	private Vector2Int lastIndex;
	
	private KeyValuePair<string, List<VectorData>> arrow;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		locations = gameData.locations;
		arrowDict = gameData.arrowDict;
		occupiedPositions = gameData.occupiedPositions;
		heatMap = gameData.heatMap;
	}
	
	private KeyValuePair<string, List<VectorData>> GetLastArrow()
	{
		return arrowDict.Last();
	}
	
	private GridCell GetCell(Vector2Int index)
	{
		return locations[index];
	}
	
	private int GetLength()
	{
		return Random.Range(10, 20);
	}
	
	private bool CheckIsOccupied(Vector2Int index)
	{	
		if(!locations.ContainsKey(index))
		{
			return true;
		}
		
		int layer = locations[index].layer;
		Vector3 position = locations[index].position;
		
		if(gameData.currentLayer != layer)
			return true;
		
		if(occupiedPositions.Contains(position))
			return true;
		
		return false;
	}
	
	private int[] TryGetNextDirection(int value)
	{
		int[] angles = { 270, 90, 0, 180 };
		
		return angles.Where(angle => angle != value).ToArray();
	}
	
	private void GetBlocks(Vector2Int block, int angle)
	{	
		int arrowLength = GetLength();
		
		Vector2Int index;
		
		for(int i = 1; i <= arrowLength; i++)
		{	
			switch(angle)
			{
				case 270:
					index = new Vector2Int(block.x + i, block.y);
					if(CheckIsOccupied(index)) return;
					indices.Add(index);
					break;
				case 90:
					index = new Vector2Int(block.x - i, block.y);
					if(CheckIsOccupied(index)) return;
					indices.Add(index);
					break;
				case 0:
					index = new Vector2Int(block.x, block.y + i);
					if(CheckIsOccupied(index)) return;
					indices.Add(index);
					break;
				case 180:
					index = new Vector2Int(block.x, block.y - i);
					if(CheckIsOccupied(index)) return;
					indices.Add(index);
					break;
			}
		}
	}
	
	private void AddToArrowDict(Vector2Int block, int angle)
	{
		GetBlocks(block, angle);
		
		foreach(Vector2Int currentIndex in indices)
		{
			GridCell cell = GetCell(currentIndex);
			
			arrow.Value.Add(new VectorData {
				position = cell.position,
				rotation = Quaternion.Euler( 0, 0, angle ),
				head = false,
				index = currentIndex,
				angle = angle
			});
			
			occupiedPositions.Add(cell.position);
			heatMap[cell.position].isOccupied = true;
		}
		
		if (indices.Count > 0) 
		{
			lastIndex = indices[^1];
		}

		indices.Clear();
	}
	
	public void LayArrows()
	{
		arrow = GetLastArrow();
		
		Vector2Int initialIndex = arrow.Value[^1].index;
		int angle = arrow.Value[^1].angle;
		
		lastIndex = initialIndex;
		
		AddToArrowDict(initialIndex, angle);
		
		int[] result = TryGetNextDirection(angle);
		
		for(int i = 0; i < result.Length; i++)
		{	
			angle = result[i];
			AddToArrowDict(lastIndex, angle);
		}
	}
}
