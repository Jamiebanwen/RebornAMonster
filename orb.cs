using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orb : MonoBehaviour
{
    [SerializeField] AudioClip orbPickupSFX;
    [SerializeField] int pointsPerOrb = 10;
    // Was collected entered to ensure coin can only be picked up once
    bool wasCollected = false;
void OnTriggerEnter2D(Collider2D other) 
{
    if(other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointsPerOrb);
            AudioSource.PlayClipAtPoint(orbPickupSFX,Camera.main.transform.position);
            Destroy(gameObject);
        }
}
}
