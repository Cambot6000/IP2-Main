using UnityEngine;
using System;
using System.Collections;


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

    [Header("projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileLifeTime = 5f;
    public Transform firePoint; // Just fires from the turret but if we need to change it (from dino mouth for instance) this can be done with this

    private PlaceObject placeObject;
    private LineRenderer lineRenderer;
    private float fireTimer;
    private Enemies currentTarget;


    public GameObject tower;
    public Animator animator;
    [Header("Ring VIsibility Stuff")]
    public bool ringActive;
    private Coroutine activeFade;


    private void Awake()
    {
        
        tower = gameObject;
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

        // if no explicit fire point, use this transform
        if (firePoint == null)
            firePoint = transform;
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


       animator.SetTrigger("Attack");
       Vector3 targetPosition = new Vector3(target.transform.position.x, tower.transform.position.y,target.transform.position.z);
        tower.gameObject.transform.LookAt(targetPosition);
        
        if (projectilePrefab != null) // If there is a prefab assigned, spawn it and do the projectile script
        {
            GameObject go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile proj = go.GetComponent<Projectile>();
            
            if (proj != null)
            {
                proj.target = target;
                proj.speed = projectileSpeed;
                proj.damage = damage;
                proj.lifeTime = projectileLifeTime;
            }
            else
            {
                // Just in case, if its broken just deal the damage directly 
                Debug.LogWarning("projectile not working as intended.");
                Destroy(go);
                target.health -= damage;
            }

            return;
        }

        target.health -= damage; // Same again, just incase something is broken, deal direct damage
    }

    public void UpdateRangeRing() //added by callum
    {
        //so i can change the range ring atributes from diffrent scripts
        DrawRing();
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

            // tiny Y offset so it doesn’t tweak out
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

    private void OnTriggerEnter(Collider other) //Checks to see if player enters range of dino and then starts to make the range ring appear
    {
        if (other.gameObject.tag == "Player")
        {
            //print("Player touched dino");
            ringActive = true;
            if (activeFade != null) //If the wee player guy has already started a grow or shrink, end it and start the neweer one
            {
                StopCoroutine(activeFade);
            }
            activeFade = StartCoroutine(RingFade(1f, range));
        }
    }

    private void OnTriggerExit(Collider other) //Checks to see if player exits range of dino and then starts to make the range ring disappear
    {
        if (other.gameObject.tag == "Player")
        {
            //print("player exited dino");
            ringActive = false;
            if (activeFade != null) //If the wee player guy has already started a grow or shrink, end it and start the neweer one
            {
                StopCoroutine(activeFade);
            }
            activeFade = StartCoroutine(RingFade(0f, range * 0.5f));
        }
    }

    private IEnumerator RingFade(float targetTransparency, float targetRange) //Coroutine to make the ring fade in/out and grow/shrink
    {
        float duration = 0.5f; //How long both thingys last, like the fade in/out and the grow/shrink
        float currentTime = 0f;

        Color startLineColour = lineRenderer.startColor;
        float startTransparency = startLineColour.a;

        float startRange; //Starting range of the dino ring

        if (lineRenderer.enabled)
        {
            startRange = lineRenderer.GetPosition(0).magnitude; //Checks how big the line is the now
        }
        else
        {
            startRange = range * 0.5f;
        }

        lineRenderer.enabled = true;

        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime; //This bit because of the speed up stuff
            float timeVariable = currentTime / duration;
            float otherTimeVariable = Mathf.SmoothStep(0, 1, timeVariable); //Smooth step makes the sort of animation, I guess, start slowly, gradually ramp up and then softly end at the end, if you understand what I'm trying to say
            float newTransparency = Mathf.Lerp(startTransparency, targetTransparency, currentTime / duration); //Lerp gets the bit in the middle, I think

            Color newColour = new Color(ringColor.r, ringColor.g, ringColor.b, newTransparency);
            lineRenderer.startColor = newColour;
            lineRenderer.endColor = newColour;

            //^Sets the transparency of the line to the new one that is needed

            float brandNewRingDisplayThingy = Mathf.Lerp(startRange, targetRange, otherTimeVariable);
            DrawGrowingRing(brandNewRingDisplayThingy);

            //^Makes the ring grow or shrink

            yield return null;
        }

        if (targetTransparency <= 0f)
        {
            lineRenderer.enabled = false;
        }
    }

    private void DrawGrowingRing(float radius) //Similar to the OG draw ring method that was made. Old one still works too, alongside this one, this one hoever only comes into play omce you build the wee guys
    {
        float step = 2f * Mathf.PI / ringSegments;
        for (int i = 0; i <= ringSegments; i++)
        {
            float angle = i * step;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            //Small Y offset like the OG draw ring bit that was made
            lineRenderer.SetPosition(i, new Vector3(x, 0.1f, z));
        }
    }
}
