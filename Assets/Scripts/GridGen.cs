using UnityEngine;
using System.Collections.Generic;

public class GridGen : MonoBehaviour
{
	private Data gameData;
	private Camera cam;
	
	private const float GRID_SIZE = 0.3f;
	private const float limit = 0.80f;
	
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

		float width = (topRight.x - bottomLeft.x) * limit; // For ex: 10 - (-10) = 20
		float height = (topRight.y - bottomLeft.y) * limit; // Limit gives an offset to the grid
		
		int widthCount = Mathf.CeilToInt(width / GRID_SIZE); // How many grid cells fit each width
		int heightCount = Mathf.CeilToInt(height / GRID_SIZE); // and height
		
		float startX = -width / 2; // For ex: -20 / 2 = -10
		float startY = height / 2;
		
		int gap = 2; // The count of lines between layers
		
		for(int i = 0; i < heightCount; i++) 
		{	
			float currentY = startY - (i * GRID_SIZE); // Y position in Vector3
			
			for(int k = 0; k < widthCount; k++)
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
				
				VectorPositions vector = new VectorPositions
				{
					isOccupied = false,
					vectorIndex = index
				};
				
				gameData.heatMap.Add(spawnPosition, vector);
			}
		}
	}
}
