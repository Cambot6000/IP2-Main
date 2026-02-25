using UnityEngine;
using System;




[RequireComponent(typeof(PlaceObject))]
[RequireComponent(typeof(LineRenderer))]
public class TowerFire : MonoBehaviour
{
    [Header("combat")]
    public float range = 5f;
    public float fireRate = 1f;
    public int damage = 25;

    [Header("range Ring")]
    public int ringSegments = 64;
    public float ringWidth = 0.05f;
    public Color ringColor = new Color(1f, 0.9f, 0.2f, 0.6f);
    public bool showRingWhilePlacing = true;

    private PlaceObject placeObject;
    private LineRenderer lineRenderer;
    private float fireTimer;
    private Enemies currentTarget;

    private void Awake()
    {
        placeObject = GetComponent<PlaceObject>();
        lineRenderer = GetComponent<LineRenderer>();

        // just in case it's missing
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        // basic circle setup
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false; // easier to draw relative to tower
        lineRenderer.widthMultiplier = ringWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = ringColor;
        lineRenderer.endColor = ringColor;
        lineRenderer.positionCount = ringSegments + 1;

        DrawRing();
    }

    private void Update()
    {
        // toggle range ring visibility
        if (placeObject != null)
            lineRenderer.enabled = showRingWhilePlacing || placeObject.Placed;

        // don’t do anything until the tower is actually placed
        if (placeObject != null && !placeObject.Placed)
            return;

        // grab a new target if needed
        if (currentTarget == null || !IsTargetValid(currentTarget))
            currentTarget = FindClosestEnemyInRange();

        fireTimer += Time.deltaTime;

        float interval = fireRate > 0f ? 1f / fireRate : float.MaxValue;

        if (currentTarget != null && fireTimer >= interval)
        {
            FireAt(currentTarget);
            fireTimer = 0f;
        }
    }

    private bool IsTargetValid(Enemies e)
    {
        if (e == null) return false;
        if (e.health <= 0) return false;

        float sqrRange = range * range;
        return (transform.position - e.transform.position).sqrMagnitude <= sqrRange;
    }

    private Enemies FindClosestEnemyInRange()
    {
        Enemies[] all = FindObjectsOfType<Enemies>();

        Enemies best = null;
        float bestDist = float.MaxValue;
        float sqrRange = range * range;

        foreach (var en in all)
        {
            if (en == null || en.health <= 0)
                continue;

            float dist = (transform.position - en.transform.position).sqrMagnitude;

            if (dist <= sqrRange && dist < bestDist)
            {
                bestDist = dist;
                best = en;
            }
        }

        return best;
    }

    private void FireAt(Enemies target)
    {
        if (target == null)
            return;

        // simple direct damage for now
        // swap this out later if you add projectiles
        target.health -= damage;
    }

    private void DrawRing()
    {
        if (lineRenderer == null)
            return;

        float step = 2f * Mathf.PI / ringSegments;

        for (int i = 0; i <= ringSegments; i++)
        {
            float angle = i * step;
            float x = Mathf.Cos(angle) * range;
            float z = Mathf.Sin(angle) * range;

            // tiny Y offset so it doesn’t fight the ground
            lineRenderer.SetPosition(i, new Vector3(x, 0.05f, z));
        }
    }

    // update circle in editor when tweaking values
    private void OnValidate()
    {
        if (lineRenderer == null)
            return;

        ringWidth = Mathf.Max(0.001f, ringWidth);
        ringSegments = Mathf.Max(3, ringSegments);

        lineRenderer.widthMultiplier = ringWidth;
        lineRenderer.positionCount = ringSegments + 1;
        lineRenderer.startColor = ringColor;
        lineRenderer.endColor = ringColor;

        DrawRing();
    }
}
