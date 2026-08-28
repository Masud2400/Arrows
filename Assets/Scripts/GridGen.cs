using UnityEngine;
using System.Collections.Generic;

public class GridGen : MonoBehaviour
{
	private Data gameData;
	private Camera cam;
	
	private const float GRID_SIZE = 0.10f;
	
	void Start()
	{
		gameData = AssetManager.Instance.GameData;
		cam = AssetManager.Instance.Cam;
	}
	
	public void GenerateGrid()
	{	
		Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 10));

		Vector3 topRight = cam.ScreenToWorldPoint(
			new Vector3(Screen.width, Screen.height, 10)
		);

		float width = topRight.x - bottomLeft.x;
		float height = topRight.y - bottomLeft.y;
		
		int widthCount = Mathf.RoundToInt(width / GRID_SIZE);
		int heightCount = Mathf.RoundToInt(height / GRID_SIZE);
		
		float startX = (-width / 2f); // Starts at -10 and goes up to 10
		float startY = (height / 2f); // Starts at 5 and goes down to -5
		
		int gap = 1; // The count of lines between layers
		
		for(int i = 5; i < heightCount - 5; i++) // Starting at 5 and stopping at heightCount - 5 makes it narrower
		{	
			float currentY = startY - (i * GRID_SIZE); // Y position in Vector3
			
			for(int k = 10; k < widthCount - 7; k++)
			{	
				float currentX = startX + (k * GRID_SIZE); // X position in Vector3
				
				int distY = Mathf.Min(i, heightCount - 1 - i);
				int distX = Mathf.Min(k, widthCount - 1 - k);
				int minDist = Mathf.Min(distX, distY);
				
				int assignedLayer = (minDist / gap) + 1;
				
				Vector3 spawnPosition = new Vector3(currentX, currentY, 0);
				
				Vector2Int index = new Vector2Int(i, k); // I is the row and K is the column
				GridCell cell = new GridCell
				{
					position = spawnPosition,
					layer = assignedLayer
				};
				
				gameData.locations.Add(index, cell);
			}
		}
	}
}
