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
    public GameObject aoeParticlePrefab; 

    private PlaceObject placeObject;
    private LineRenderer lineRenderer;
    private float fireTimer;

    private void Awake()
    {
        placeObject = GetComponent<PlaceObject>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

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
        float interval = fireRate;

        if (fireTimer >= interval)
        {
            if (TryFireAoe())
            {
                fireTimer = 0f;
            }
        }
    }

    private bool TryFireAoe()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        bool hitAny = false;

        foreach (var hitCollider in hitColliders)
        {
            Enemies en = hitCollider.GetComponent<Enemies>();
            if (en != null && en.health > 0)
            {
                if (!hitAny) 
                {
                    // Instantiate a new particle effect at the tower's position
                    if (aoeParticlePrefab != null)
                    {
                        GameObject effect = Instantiate(aoeParticlePrefab, transform.position, Quaternion.identity);
                        Destroy(effect, 2f);
                    }
                    hitAny = true;
                }

                en.health -= damage;

                if (en.health <= 0)
                {
                    if (MoneyManager.instance != null)
                    {
                        MoneyManager.instance.AddGold(50);
                    }
                    
                    Destroy(hitCollider.gameObject);
                }
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

            lineRenderer.SetPosition(i, new Vector3(x, 0.5f, z));
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
