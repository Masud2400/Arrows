using UnityEngine;
using System.Collections.Generic;

public class MoveArrow : MonoBehaviour
{
	private Data gameData;
	private Dictionary<GameObject, GameObject> gameObjectReference;
	
	private Camera cam;
    
    void Start()
    {
		gameData = AssetManager.Instance.GameData;
		gameObjectReference = gameData.gameObjectReference;
		
		cam = AssetManager.Instance.Cam;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            
			foreach(var kvp in gameObjectReference)
			{
				if(hit.collider.gameObject == kvp.Value)
				{
					MoveLine(kvp.Key);
				}
			}
        }
    }
	
	private void MoveLine(GameObject line)
	{
		
	}
}
