using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class MoveArrow : MonoBehaviour
{
	private Data gameData;
	private Dictionary<GameObject, GameObject> gameObjectReference;
	
	private Camera cam;
	
	private Vector3 startPosition;
    private Vector3 endPosition = Vector3.zero;
    [SerializeField] private float duration = 2f;
    
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
					GetEndPos(kvp.Value);
					StartCoroutine(MoveLine(kvp.Key, kvp.Value));
				}
			}
        }
    }
	
	private void GetEndPos(GameObject head)
	{
		Vector3 topRight = cam.ScreenToWorldPoint(
			new Vector3(Screen.width, Screen.height, 10)
		);
		
		float width = topRight.x;
		float height = topRight.y;
		
		float headPosX = head.transform.position.x;
		float headPosY = head.transform.position.y;
		float headRotZ = head.transform.eulerAngles.z;
		
		float targetY = 0;
		float targetX = 0;
		
		switch(headRotZ)
		{
			case 270:
				targetY = headPosY + height;
				break;
			case 90:
				targetY = headPosY - height;
				break;
			case 180:
				targetX = headPosX + width;
				break;
			case 0:
				targetX = headPosX - width;
				break;
		}
		
		if(headRotZ == 270 || headRotZ == 90)
			endPosition = new Vector3(headPosX, targetY, 0);
		if(headRotZ == 0 || headRotZ == 180)
			endPosition = new Vector3(targetX, headPosY, 0);
	}
	
	private IEnumerator MoveLine(GameObject line, GameObject head)
	{
		LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
		startPosition = lineRenderer.GetPosition(0);
		int posCount = lineRenderer.positionCount;
		
		List<Vector3> posList = new List<Vector3>();
		
		for(int i = 0; i < posCount; i++)
		{
			Vector3 pos = lineRenderer.GetPosition(i);
			posList.Add(pos);
		}
		
		float timeElapsed = 0;

        while (timeElapsed < duration)
        {	
            float t = timeElapsed / duration; 
            
			Vector3 middlePosition = Vector3.Lerp(posList[0], endPosition, t);
			posList[0] = middlePosition;
			
			for(int i = 1; i < posCount; i++)
			{
				middlePosition = Vector3.Lerp(posList[i], posList[i - 1], t);
				posList[i] = middlePosition;
			}
			
			lineRenderer.positionCount = posList.Count;
			lineRenderer.SetPositions(posList.ToArray());
			
			head.transform.position = posList[0];
            
            timeElapsed += Time.deltaTime;
			
			yield return null;
        }
		
		lineRenderer.SetPosition(0, endPosition);
	}
}
