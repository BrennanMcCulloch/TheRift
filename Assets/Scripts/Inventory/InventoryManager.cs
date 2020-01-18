using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Inventory {
    public class InventoryManager : Singleton<InventoryManager>
    {
        // A prefab button to hold an item in the ui
        const int NUM_ITEMS_HORIZONTAL = 4;
        const int ITEM_BOX_WIDTH_PIXELS = 200;
        const int ITEM_BOX_HEIGHT_PIXELS = 200;
        const int ITEM_BOX_PADDING_PIXELS = 20;

        public GameObject _itemBox;
        public GameObject itemPanel;

        public List<GameObject> itemBoxes;

        void Awake() { BuildInventoryScreen(); }

        public void AddItemToInventory(Item newItem){}

        public void RemoveItemFromInventory(Item removeItem) {}

        public void BuildInventoryScreen() {
            for (int i = 0; i < 10; i++) {
                itemBoxes.Add(BuildItemBox(null));
                PlaceItemBox(itemBoxes.Last().gameObject);
            }
            // foreach (Item i in Inventory.items) {
                //itemBoxes.Add(BuildItemBox(item));
                //
            //}
        }

        public GameObject BuildItemBox(Item buildItem) {
            GameObject newBox = Instantiate(_itemBox);
            return newBox;
            //Item i = buildItem.GetComponent<Item();
            //buildItem.transform.parent = newBox.transform;
            //buildItem.transform.position = Vector3.zero;
        }

        public void PlaceItemBox(GameObject itemBox) {
            itemBox.transform.parent = itemPanel.transform;
            itemBox.transform.localScale = Vector3.one;
            float xPosition = ((itemBoxes.Count - 1) % NUM_ITEMS_HORIZONTAL) * ITEM_BOX_WIDTH_PIXELS + ITEM_BOX_PADDING_PIXELS;
            float yPosition = ((itemBoxes.Count - 1) / NUM_ITEMS_HORIZONTAL) * ITEM_BOX_HEIGHT_PIXELS + ITEM_BOX_PADDING_PIXELS;
            xPosition -= itemPanel.GetComponent<RectTransform>().rect.width * 2;
            Debug.Log(xPosition);
            Debug.Log(itemPanel.GetComponent<RectTransform>().rect.width / 2);
            yPosition -= itemPanel.GetComponent<RectTransform>().rect.height / 2;

            Vector2 itemBoxPosition = new Vector2(xPosition,-yPosition);
            itemBox.transform.localPosition = itemBoxPosition;
        }

        public void ToggleInventoryWindow() {
            gameObject.SetActive(!gameObject.active);
        }

    }
}