using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Waterfall : MonoBehaviour
{
    [Header("Shape")]
    [Tooltip("Width of the waterfall in world units.")]
    public float width = 1f;

    [Tooltip("Number of vertical slices. More = smoother cut on curved surfaces.")]
    [Range(2, 64)]
    public int rayCount = 8;

    [Tooltip("Maximum fall distance if nothing is hit.")]
    public float maxLength = 10f;

    public LayerMask collisionMask;

    [Header("Visuals")]
    [Tooltip("How fast the texture scrolls downward to simulate flow.")]
    public float scrollSpeed = 1f;

    [Header("Splash")]
    [Tooltip("Particle System child to show at the lowest hit point.")]
    public ParticleSystem splashEffect;

    public int layoutOrder;

    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private Vector3[] _vertices;
    private Vector2[] _uvs;
    private int[]     _triangles;

    private bool _isSplashing;

    private readonly HashSet<WaterfallDetector> _currentDetectors = new();
    private readonly HashSet<WaterfallDetector> _trackedDetectors = new();

    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();

        _mesh = new Mesh { name = "WaterfallMesh" };
        _meshFilter.mesh = _mesh;

        int vertCount = rayCount * 2;
        _vertices  = new Vector3[vertCount];
        _uvs       = new Vector2[vertCount];
        _triangles = new int[(rayCount - 1) * 6];

        for (int i = 0; i < rayCount - 1; i++)
        {
            int ti = i * 6;
            int tl = i * 2;
            int tr = tl + 2;
            int bl = tl + 1;
            int br = tr + 1;

            _triangles[ti + 0] = tl;
            _triangles[ti + 1] = tr;
            _triangles[ti + 2] = bl;
            _triangles[ti + 3] = tr;
            _triangles[ti + 4] = br;
            _triangles[ti + 5] = bl;
        }

        _mesh.vertices  = _vertices;
        _mesh.triangles = _triangles;

        if (splashEffect != null)
            splashEffect.Stop();
    }

    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "Default";
        meshRenderer.sortingOrder = layoutOrder;
    }

    void Update()
    {
        float halfWidth = width * 0.5f;
        float lowestHit = float.MaxValue;
        bool anyHit = false;

        _currentDetectors.Clear();

        for (int i = 0; i < rayCount; i++)
        {
            float t = (rayCount == 1) ? 0.5f : (float)i / (rayCount - 1);
            float localX = Mathf.Lerp(-halfWidth, halfWidth, t);

            Vector2 worldOrigin = transform.TransformPoint(new Vector3(localX, 0f, 0f));
            RaycastHit2D hit = Physics2D.Raycast(worldOrigin, Vector2.down, maxLength, collisionMask);

            float bottomY;
            if (hit.collider != null)
            {
                Vector3 localHit = transform.InverseTransformPoint(hit.point);
                bottomY = localHit.y;
                anyHit = true;

                if (hit.point.y < lowestHit)
                    lowestHit = hit.point.y;

                if (hit.collider.TryGetComponent(out WaterfallDetector detector))
                    _currentDetectors.Add(detector);
            }
            else
            {
                bottomY = -maxLength;
            }

            int vi = i * 2;
            _vertices[vi]     = new Vector3(localX, 0f,      0f);
            _vertices[vi + 1] = new Vector3(localX, bottomY, 0f);

            float columnLength = -bottomY;
            float vSpan = columnLength / maxLength;
            float scrollOffset = Time.time * scrollSpeed;
            _uvs[vi]     = new Vector2(t, scrollOffset);
            _uvs[vi + 1] = new Vector2(t, scrollOffset + vSpan);
        }

        _mesh.vertices = _vertices;
        _mesh.uv       = _uvs;
        _mesh.RecalculateBounds();

        if (anyHit)
            SetSplash(true, new Vector3(transform.position.x, lowestHit, 0f));
        else
            SetSplash(false, Vector3.zero);

        // Enter: newly hit detectors.
        foreach (WaterfallDetector d in _currentDetectors)
        {
            if (_trackedDetectors.Add(d))
                d.NotifyEnter();
        }

        // Exit: tracked detectors no longer hit by any ray AND whose collider
        // has genuinely left the waterfall's world-space bounds.
        float waterfallLeft  = transform.position.x - width * 0.5f;
        float waterfallRight = transform.position.x + width * 0.5f;
        float waterfallTop   = transform.position.y;
        float waterfallBot   = transform.position.y - maxLength;

        var toRemove = new List<WaterfallDetector>();
        foreach (WaterfallDetector d in _trackedDetectors)
        {
            if (_currentDetectors.Contains(d)) continue;

            // Still inside waterfall bounds — raycast just missed it (e.g. gap
            // between columns). Don't fire exit yet.
            Bounds b = d.GetComponent<Collider2D>().bounds;
            bool stillOverlaps = b.max.x > waterfallLeft  &&
                                 b.min.x < waterfallRight &&
                                 b.max.y > waterfallBot   &&
                                 b.min.y < waterfallTop;
            if (stillOverlaps) continue;

            d.NotifyExit();
            toRemove.Add(d);
        }
        foreach (WaterfallDetector d in toRemove)
            _trackedDetectors.Remove(d);
    }

    void OnDisable()
    {
        foreach (WaterfallDetector d in _trackedDetectors)
            d.NotifyExit();
        _trackedDetectors.Clear();
        _currentDetectors.Clear();
    }

    void SetSplash(bool active, Vector3 position)
    {
        if (splashEffect == null) return;

        if (active && !_isSplashing)
        {
            splashEffect.transform.position = position;
            splashEffect.Play();
            _isSplashing = true;
        }
        else if (active && _isSplashing)
        {
            splashEffect.transform.position = position;
        }
        else if (!active && _isSplashing)
        {
            splashEffect.Stop();
            _isSplashing = false;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.cyan;
        float halfWidth = width * 0.5f;
        for (int i = 0; i < rayCount; i++)
        {
            float t = (rayCount == 1) ? 0.5f : (float)i / (rayCount - 1);
            float localX = Mathf.Lerp(-halfWidth, halfWidth, t);
            Vector2 origin = transform.TransformPoint(new Vector3(localX, 0f, 0f));
            Gizmos.DrawLine(origin, origin + Vector2.down * maxLength);
        }
    }
#endif
}
