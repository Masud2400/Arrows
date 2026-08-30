using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private GameObject line;
    private Transform parent;
    private LineRenderer activeLineRenderer;

    [SerializeField] private float stepDelay = 0.05f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private int lineLength = 4;

    private List<Vector3> positions = new List<Vector3>
    {
        new Vector3(-6.215419769287109f, 1.6999999284744263f, 0.0f),
        new Vector3(-6.115419864654541f, 1.6999999284744263f, 0.0f),
        new Vector3(-6.0154194831848148f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.915419578552246f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.815419673919678f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.715419769287109f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.615419864654541f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.5154194831848148f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.415419578552246f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.315419673919678f, 1.6999999284744263f, 0.0f),
        new Vector3(-5.215419769287109f, 1.6999999284744263f, 0.0f)
    };

    void Start()
    {
        line = AssetManager.Instance.Line;
        parent = AssetManager.Instance.SpawnParent;
    }

    public void Draw()
    {
        GameObject spawnedObj = Instantiate(line, parent);
        activeLineRenderer = spawnedObj.GetComponent<LineRenderer>();

        StartCoroutine(AnimateLine(activeLineRenderer));
    }

    public void MoveToMinusOneX()
    {
        if (activeLineRenderer != null)
        {
            StartCoroutine(MoveLineToX(activeLineRenderer.transform, -1.0f));
        }
    }

    private IEnumerator AnimateLine(LineRenderer lineRenderer)
    {
        int headIndex = 0;

        while (headIndex < positions.Count)
        {
            int tailIndex = Mathf.Max(0, headIndex - lineLength + 1);
            int currentSegmentCount = headIndex - tailIndex + 1;

            lineRenderer.positionCount = currentSegmentCount;

            List<Vector3> activeSlice = positions.GetRange(tailIndex, currentSegmentCount);
            lineRenderer.SetPositions(activeSlice.ToArray());

            headIndex++;
            yield return new WaitForSeconds(stepDelay);
        }
    }

    private IEnumerator MoveLineToX(Transform lineTransform, float targetX)
    {
        while (Mathf.Abs(lineTransform.position.x - targetX) > 0.001f)
        {
            Vector3 currentPos = lineTransform.position;
            currentPos.x = Mathf.MoveTowards(currentPos.x, targetX, moveSpeed * Time.deltaTime);
            lineTransform.position = currentPos;
            yield return null;
        }

        Vector3 finalPos = lineTransform.position;
        finalPos.x = targetX;
        lineTransform.position = finalPos;
    }
}
