using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
	private GameObject line;
	private GameObject head;
	
	public IObjectPool<GameObject> poolLine;
	public IObjectPool<GameObject> poolHead;
	
    void Start()
    {
		line = AssetManager.Instance.Line;
		head = AssetManager.Instance.Head;
		
        poolLine = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(line),            
            actionOnGet: (obj) => obj.SetActive(true),        
            actionOnRelease: (obj) => obj.SetActive(false),     
            actionOnDestroy: (obj) => Destroy(obj),           
            collectionCheck: true,                             
            defaultCapacity: 1000
        );
		
		poolHead = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(head),            
            actionOnGet: (obj) => obj.SetActive(true),        
            actionOnRelease: (obj) => obj.SetActive(false),     
            actionOnDestroy: (obj) => Destroy(obj),           
            collectionCheck: true,                             
            defaultCapacity: 1000
        );
    }
}
