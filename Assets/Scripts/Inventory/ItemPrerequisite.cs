using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTree;

namespace Inventory {

    public class ItemPrerequisite : Prerequisite
    {
        public Item requisiteItem;
        public bool destroyItem;

        public override bool Met() {
            return (requisiteItem == null) || InventoryManager.instance.HasItem(requisiteItem);
        }
    }
}