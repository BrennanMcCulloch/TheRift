using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTree;

namespace Inventory {

    public class ItemPrerequisite : Prerequisite
    {
        public Item requisiteItem;

        public override bool Met() {
            return InventoryManager.Instance.itemBoxes.Contains(requisiteItem);
        }
    }
}