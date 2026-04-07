using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position; //Gets the camera starting position
    }
   
    public IEnumerator Shakermaker(float duration, float strength) //Coroutine that shakes the camera (duration is how long it lasts and strength is how strong the shake is)
    {
        float greenockMorton = 0f; //An elapsed timer variable

        while (greenockMorton < duration) //Essentially (time elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.position = new Vector3(startPosition.x + x, startPosition.y + y, startPosition.z); //Sets the new camera position to shake it about

            greenockMorton += Time.unscaledDeltaTime;

            yield return null;
        }
        transform.position = startPosition;
    }
}
