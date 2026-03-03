using UnityEngine;
using System;

public class AoeTowerScript : MonoBehaviour
{
    [Header("combat")]
    public float range = 1.5f;
    public float fireRate = 0.75f;
    public int damage = 15;

    [Header("range Ring")]
    public int ringSegments = 64;
    public float ringWidth = 0.05f;
    public Color ringColor = new Color(1f, 0.9f, 0.2f, 0.6f);
    public bool showRingWhilePlacing = true;

    [Header("Aoe Settings")]
    public ParticleSystem aoeParticleEffect;
    public LayerMask enemyLayer;

    [Header("projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileLifeTime = 5f;
    public Transform firePoint;

    private PlaceObject placeObject;
    private LineRenderer lineRenderer;
    private float fireTimer;

    private void Awake()
    {
        placeObject = GetComponent<PlaceObject>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        // basic circle setup
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.widthMultiplier = ringWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = ringColor;
        lineRenderer.endColor = ringColor;
        lineRenderer.positionCount = ringSegments + 1;

        DrawRing();
    }

    private void Update()
    {
        if (placeObject != null) 
            lineRenderer.enabled = showRingWhilePlacing || placeObject.Placed;

        if (placeObject != null && !placeObject.Placed) 
        {
            fireTimer = 0f; 
            return;
        }

        fireTimer += Time.deltaTime;
        float interval = fireRate > 0f ? 1f / fireRate : float.MaxValue;

        if (fireTimer >= interval)
        {
            // Combined detection and firing for efficiency
            if (TryFireAoe())
            {
                fireTimer = 0f;
            }
        }
    }

    private bool TryFireAoe()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, enemyLayer);
        bool hitAny = false;

        foreach (var hitCollider in hitColliders)
        {
            Enemies en = hitCollider.GetComponent<Enemies>();
            if (en != null && en.health > 0)
            {
                if (!hitAny) // First valid enemy found, trigger effects
                {
                    if (aoeParticleEffect != null)
                    {
                        aoeParticleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        aoeParticleEffect.Play();
                    }
                    hitAny = true;
                }
                en.health -= damage;
            }
        }

        return hitAny;
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

            lineRenderer.SetPosition(i, new Vector3(x, 0.05f, z));
        }
    }

    private void OnValidate()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        
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
