using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ExitChecker : MonoBehaviour
{
	private Data gameData;

	private Dictionary<Vector2Int, GridCell> locations;
	private Dictionary<string, List<VectorData>> arrowDict;
	private HashSet<Vector3> occupiedPositions;
	
	private Dictionary<string, HashSet<string>> arrowConnections = new Dictionary<string, HashSet<string>>();
	
    void Start()
	{
		gameData = AssetManager.Instance.GameData;
		
		locations = gameData.locations;
		arrowDict = gameData.arrowDict;
		occupiedPositions = gameData.occupiedPositions;
	}
	
	private void GetTargetPos(VectorData head, out HashSet<Vector3> targetPositions)
	{	
		targetPositions = new HashSet<Vector3>();
		
		int angle = head.angle;
		Vector2Int headIndex = head.index;
		int row = headIndex.x;
		int col = headIndex.y;
		
		int finalRow = locations.Last().Key.x;
		int finalCol = locations.Last().Key.y;
		
		Vector2Int index;
		Vector3 position;
		
		switch (angle)
		{
			case 270: // Up
				for (int i = row - 1; i >= 0; i--)
				{
					index = new Vector2Int(i, col);
					position = locations[index].position;
					if (occupiedPositions.Contains(position))
						targetPositions.Add(position);
				}
				break;

			case 90: // Down
				for (int i = row + 1; i <= finalRow; i++)
				{
					index = new Vector2Int(i, col);
					position = locations[index].position;
					if (occupiedPositions.Contains(position))
						targetPositions.Add(position);
				}
				break;

			case 0: // Left
				for (int i = col - 1; i >= 0; i--)
				{
					index = new Vector2Int(row, i);
					position = locations[index].position;
					if (occupiedPositions.Contains(position))
						targetPositions.Add(position);
				}
				break;

			case 180: // Right
				for (int i = col + 1; i <= finalCol; i++)
				{
					index = new Vector2Int(row, i);
					position = locations[index].position;
					if (occupiedPositions.Contains(position))
						targetPositions.Add(position);
				}
				break;
		}
	}
	
	private string GetKeyByPosition(Vector3 targetPos)
	{
		foreach (var pair in arrowDict)
		{
			foreach (VectorData obj in pair.Value)
			{
				if ((obj.position - targetPos).sqrMagnitude < 0.0001f) 
				{
					return pair.Key;
				}
			}
		}
		return null;
	}
	
	private void GetFirstBlock(
		string parentArrow, out VectorData head, out VectorData body
	)
	{
		List<VectorData> list = arrowDict[parentArrow];
		head = list[0];
		body = list.Count > 1 ? list[1] : null;
	}
	
	private void SaveAllConnections(string currentArrow)
	{
		foreach(var kvp in arrowDict)
		{
			var key = kvp.Key;
			
			GetFirstBlock(key, out VectorData head, out VectorData body);
			
			GetTargetPos(head, out HashSet<Vector3> targetPositions);
			
			if (!arrowConnections.ContainsKey(key))
			{
				arrowConnections[key] = new HashSet<string>();
			}
			
			foreach(var pos in targetPositions)
			{
				var target = GetKeyByPosition(pos);
				arrowConnections[key].Add(target);
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
		foreach(VectorData data in arrowDict[currentArrow])
		{
			occupiedPositions.Remove(data.position);
		}
		
		arrowDict.Remove(currentArrow);
		arrowConnections.Remove(currentArrow);
	}
	
	public void CheckExit()
	{
		string currentArrow = arrowDict.Last().Key;
		
		SaveAllConnections(currentArrow);
		
		bool detectCycle = DetectCycleBFS(currentArrow);
		
		if(detectCycle)
		{
			RemoveArrow(currentArrow);
		}
	}
}
