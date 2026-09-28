using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class WallColliderStretch : MonoBehaviour
{
    private PolygonCollider2D polygon;
    private SpriteRenderer spriteRenderer;

    private Vector2[] originalPoints;
    private float originalHeight;

    void OnEnable()
    {
        polygon = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalPoints = polygon.points;
        originalHeight = spriteRenderer.size.y;

        UpdateCollider();
    }

    void Update()
    {
        UpdateCollider();
    }

    void UpdateCollider()
    {
        if (polygon == null || spriteRenderer == null)
            return;

        if (originalPoints == null || originalPoints.Length == 0)
            return;

        float difference = spriteRenderer.size.y - originalHeight;

        Vector2[] points = (Vector2[])originalPoints.Clone();

        for (int i = 0; i < points.Length; i++)
        {
            points[i].y = originalPoints[i].y + difference / 2f;
        }

        points[6].y = originalPoints[6].y - difference / 2f;
        points[7].y = originalPoints[7].y - difference / 2f;

        polygon.points = points;
    }
}