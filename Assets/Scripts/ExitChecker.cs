using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ExitChecker : MonoBehaviour
{
	private Data gameData;

	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<string, List<VectorData>> arrowDict;
	private HashSet<Vector3> occupiedPositions;
	private Dictionary<string, HashSet<string>> arrowConnections;
	private Dictionary<Vector3, VectorPositions> heatMap;
	
    void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		locations = gameData.locations;
		arrowDict = gameData.arrowDict;
		occupiedPositions = gameData.occupiedPositions;
		arrowConnections = gameData.arrowConnections;
		heatMap = gameData.heatMap;
	}
	
	private bool isLookingAtMe(VectorData targetCell, Vector2Int currentIndex, int currentAngle)
	{
		if(currentIndex.x < targetCell.index.x)
		{
			if(currentAngle == 90)
				return true;
		}
		
		if(currentIndex.x > targetCell.index.x)
		{
			if(currentAngle == 270)
				return true;
		}
		
		if(currentIndex.y < targetCell.index.y)
		{
			if(currentAngle == 180)
				return true;
		}
		
		if(currentIndex.y > targetCell.index.y)
		{
			if(currentAngle == 0)
				return true;
		}
		
		return false;
	}
	
	private void GetAlignedArrows(VectorData arrowData, out HashSet<string> targetArrows) // Arrows in its own direction
	{	
		targetArrows = new HashSet<string>();
		
		Vector2Int startIndex = arrowData.index;
		Vector2Int directionStep = Directions.GetDirectionStep(arrowData.angle);
		Vector2Int current = startIndex + directionStep;
		
		while (locations.ContainsKey(current))
		{
			if (locations.TryGetValue(current, out var locInfo))
			{
				string arrow = locInfo.arrowName;
				if(arrow != null)
					targetArrows.Add(arrow);
			}
			current += directionStep;
		}
	}
	
	private void GetArrowsInTwoOrientations(VectorData arrowData, out List<string> targetArrowHeads)
	{ // Gets arrow heads in both orientations
		
		targetArrowHeads = new List<string>();

		Vector2Int[] startIndices = { new Vector2Int(arrowData.index.x, 0), new Vector2Int(0, arrowData.index.y) };
		Vector2Int[] directions = { new Vector2Int(0, 1), new Vector2Int(1, 0) };

		for (int i = 0; i < 2; i++)
		{
			Vector2Int current = startIndices[i];

			while (locations.TryGetValue(current, out var locInfo))
			{	
				if(!locInfo.head || !isLookingAtMe(arrowData, current, locInfo.angle))
				{	
					current += directions[i];
					continue;
				}
				
				if (locInfo.arrowName != null)
					targetArrowHeads.Add(locInfo.arrowName);

				current += directions[i];
			}
		}
	}
	
	private void SaveAllConnections(string currentArrow)
	{
		HashSet<string> targetArrows = new HashSet<string>();
		List<string> targetArrowHeads = new List<string>();
		
		VectorData head = arrowDict[currentArrow][0];
		
		GetAlignedArrows(head, out targetArrows);
		
		if (!arrowConnections.ContainsKey(currentArrow))
		{
			arrowConnections[currentArrow] = targetArrows;
		}
		
		for (int i = 0; i < arrowDict[currentArrow].Count; i++)
		{	
			GetArrowsInTwoOrientations(arrowDict[currentArrow][i], out targetArrowHeads);
			
			foreach(var arrow in targetArrowHeads)
			{
				if(arrow == locations[head.index].arrowName)
					return;
				arrowConnections[arrow].Add(currentArrow);
			}
		}
	}
	
	private bool DetectCycleBFS(string startNode)
	{	
		Queue<string> toVisit = new Queue<string>();
		HashSet<string> visited = new HashSet<string>(); // Prevents infinite loop

		toVisit.Enqueue(startNode);
		visited.Add(startNode);

		while (toVisit.Count > 0)
		{	
			string current = toVisit.Dequeue();

			if (arrowConnections.TryGetValue(current, out var neighbors))
			{
				foreach (string neighbor in neighbors)
				{
					// Found the cycle
					if (neighbor == startNode)
					{
						return true;
					}

					if (!visited.Contains(neighbor))
					{
						visited.Add(neighbor);
						toVisit.Enqueue(neighbor);
					}
				}
			}
		}

		return false;
	}
	
	private void RemoveArrow(string currentArrow)
	{
		foreach (VectorData data in arrowDict[currentArrow])
		{
			occupiedPositions.Remove(data.position);
			
			locations[data.index].arrowName = null;
			locations[data.index].head = false;
			locations[data.index].angle = 0;
			
			heatMap[data.position].isOccupied = false;
		}
		
		arrowDict.Remove(currentArrow);
		
		arrowConnections.Remove(currentArrow);
		foreach (var kvp in arrowConnections)
		{
			kvp.Value.Remove(currentArrow);
		}
	}
	
	public void CheckExit()
	{
		string currentArrow = gameData.currentArrow;
		
		SaveAllConnections(currentArrow);
		
		bool detectCycle = DetectCycleBFS(currentArrow);
		
		if (detectCycle)
		{	
			RemoveArrow(currentArrow);
		}
	}
}
