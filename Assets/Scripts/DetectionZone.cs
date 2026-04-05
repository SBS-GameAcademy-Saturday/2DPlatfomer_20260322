using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class DetectionZone : MonoBehaviour
{
    private List<Collider2D> detectedColiders = new List<Collider2D>();

    public int DetectedColidersCount
    {
        get { return detectedColiders.Count; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColiders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColiders.Remove(collision);
    }
}
