using UnityEngine;

public class ResizeBackground : MonoBehaviour
{
	private GameObject background;
	private Camera cam;
	
	void Start()
	{
		background = AssetManager.Instance.Background;
		cam = AssetManager.Instance.Cam;
		
		ResizeBG();
	}
	
    private void ResizeBG()
	{
		Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, 10));
		Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));

		float worldWidth = topRight.x - bottomLeft.x;
		float worldHeight = topRight.y - bottomLeft.y;
		
		background.transform.localScale = new Vector3(worldWidth, worldHeight, 0);
	}
}
