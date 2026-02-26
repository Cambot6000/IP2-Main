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
    public Transform firePoint; // Just fires from the turret but if we need to change it (from dino mouth for instance) this can be done with this

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
        Debug.Log($"Placed = {(placeObject == null ? "NULL" : placeObject.Placed)}");
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
            if (EnemiesInRange())
            {
                FireAoe(); 
                fireTimer = 0f;
            }
        }
    }
        private void FireAoe()
   {
       if (aoeParticleEffect != null)
       {
         aoeParticleEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
         aoeParticleEffect.Play();
       }

       Enemies[] all = FindObjectsOfType<Enemies>();
       float sqrRange = range * range;
        foreach (var en in all)
        {
         if (en == null || en.health <= 0)
         continue;

         float dist = (transform.position - en.transform.position).sqrMagnitude;
         if (dist <= sqrRange)
         en.health -= damage;
        }
    }

    private bool EnemiesInRange()
   {
      Enemies[] all = FindObjectsOfType<Enemies>();
       float sqrRange = range * range;
       foreach (var en in all)
       {
         if (en == null || en.health <= 0)
            continue;

         float dist = (transform.position - en.transform.position).sqrMagnitude;
         if (dist <= sqrRange)
            return true;
       }

     return false;
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

            // tiny Y offset so it doesn�t tweak out
            lineRenderer.SetPosition(i, new Vector3(x, 0.05f, z));
        }
    }

    // update circle in editor when changing values
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