using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Inventory {
    public class Item : MonoBehaviour
    {
        public Sprite itemSprite;
        public string description;
        public AudioClip pickupNarrationClip;
        public AudioClip glowNarrationClip;
        public bool pickupNarrationReady = false;
        public bool glowNarrationReady = false;

        // Check if when we can narrate abou this
        void Start ()
        {
            if (pickupNarrationClip != null) pickupNarrationReady = true;
            if (glowNarrationClip != null) glowNarrationReady = true;
        }

    }
}