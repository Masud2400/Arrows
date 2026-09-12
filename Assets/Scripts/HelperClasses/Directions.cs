using UnityEngine;

public static class Directions
{
    public static readonly Vector2Int[] directions = new Vector2Int[]
	{
		new Vector2Int(0, -1), // left
		new Vector2Int(0, 1),  // right
		new Vector2Int(1, 0),  // down
		new Vector2Int(-1, 0)  // up
	};
	
	public static int GetHeadAngle(Vector2Int index)
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
	
	public static Vector2Int GetDirectionStep(int angle)
	{
		return angle switch
		{
			0   => directions[0],
			180 => directions[1],
			90  => directions[2],
			270 => directions[3],
			_   => directions[0] // Default fallback
		};
	}
}
