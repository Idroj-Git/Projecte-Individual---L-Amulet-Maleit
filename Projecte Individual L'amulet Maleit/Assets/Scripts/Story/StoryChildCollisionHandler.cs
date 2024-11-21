using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryChildCollisionHandler : MonoBehaviour
{
    private MainWorldStoryEvents parentStoryEvents;

    private void Start()
    {
        parentStoryEvents = GetComponentInParent<MainWorldStoryEvents>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parentStoryEvents != null)
        {
            parentStoryEvents.HandleChildCollision(collision, gameObject);
        }
    }
}
