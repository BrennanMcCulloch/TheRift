using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Inventory {
    public class Item : MonoBehaviour
    {
        public Sprite itemSprite;
        public string description;
        public AudioClip pickupNarrationClip;
        
        private bool pickupNarrationReady = false;

        // Check if when we can narrate abou this
        void Start ()
        {
            if (pickupNarrationClip != null) pickupNarrationReady = true;
        }

        // Plays the pickup narration if possible
        public void PlayPickupNarration()
        {
            if (pickupNarrationReady == true)
            {
                Narration.Narrate(pickupNarrationClip);
                pickupNarrationReady = false;
            }
        }

    }
}