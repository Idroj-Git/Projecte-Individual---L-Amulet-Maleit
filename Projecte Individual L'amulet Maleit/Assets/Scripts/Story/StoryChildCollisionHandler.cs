using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryChildCollisionHandler : MonoBehaviour
{
    private MainWorldStoryEvents mainWorldStoryEvents;
    private ForestStoryEvents forestStoryEvents;

    private void Start()
    {
        mainWorldStoryEvents = GetComponentInParent<MainWorldStoryEvents>();
        forestStoryEvents = GetComponentInParent<ForestStoryEvents>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mainWorldStoryEvents != null)
        {
            mainWorldStoryEvents.HandleChildCollision(collision, gameObject);
        }
        else if (forestStoryEvents != null)
        {
            forestStoryEvents.HandleChildCollision(collision, gameObject);
        }
    }
}
