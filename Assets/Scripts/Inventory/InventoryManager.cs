using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Inventory {
    public class InventoryManager : Singleton<InventoryManager>
    {
        // Set these to the actual sizes of the UI elements
        const int NUM_ITEMS_HORIZONTAL = 4;
        const int ITEM_BOX_WIDTH_PIXELS = 200;
        const int ITEM_BOX_HEIGHT_PIXELS = 200;
        const int ITEM_BOX_PADDING_PIXELS = 20;

        public GameObject _itemBox;
        public GameObject itemPanel;
        public List<GameObject> itemBoxes;

        // Moves the item from it's place in the world into the inventory UI
        public void AddItem(Item newItem){
            itemBoxes.Add(BuildItemBox(newItem));
        }
        public void AddItems(List<Item> items) {
            foreach (Item item in items) {
                Debug.Log("Adding item: " + item.name);
                AddItem(item);
            }
        }

        // Prepare a spot for the item in the UI, and display the item.
        private GameObject BuildItemBox(Item item) {
            // Create an itembox and add it to the UI
            // Todo (matt) - calculate the vertical or horizontal position based ont he current number of items
            GameObject itemBox = Instantiate(_itemBox);
            itemBox.transform.parent = itemPanel.transform;
            itemBox.transform.localPosition = Vector2.zero;
            itemBox.transform.localScale = Vector2.one;


            itemBox.transform.localPosition = NextItemBoxPosition();

            // Move the item to the itembox
            Transform t = item.transform;
            t.parent = itemBox.transform;
            t.localPosition = Vector2.zero;
            t.localScale = Vector2.one;

            // Now we want to strip out all the dialogue stuff. This item is now just an item with a description.
            foreach (Transform child in item.transform) {
                GameObject.Destroy(child.gameObject);
            }
            Destroy(item.GetComponent<DialogueTrigger>());

            return itemBox;
        }

        // Destroys the item's parent. This gets rid of the UI element as well as the item.
        public void RemoveItem(Item removeItem) {
            GameObject.Destroy(removeItem.transform.parent.gameObject);
            CleanUpPanel();
        }

        public bool HasItem(Item hasItem) {
            return Items().Contains(hasItem);
        }

        public List<Item> Items() {
            return itemPanel.GetComponentsInChildren<Item>().ToList<Item>();
        }

        // Called whenever we need to reorganize the inventory panel, specifically after removing an item.
        private void CleanUpPanel() {
            // Orphan items from their container so they dont get destroyed with the containers
            foreach (Item item in Items()) item.transform.SetParent(null);

            // Destroy the containers
            foreach (GameObject itemBox in itemBoxes) GameObject.Destroy(itemBox);

            // Re-add the items, creating fresh containers
            foreach (Item item in Items()) AddItem(item);
        }

        private Vector2 NextItemBoxPosition() {
            float yPosition = (itemBoxes.Count * ITEM_BOX_HEIGHT_PIXELS) - (itemPanel.GetComponent<RectTransform>().rect.height / 4);
            Debug.Log(yPosition);
            return new Vector2(0, yPosition);
        }
    }
}