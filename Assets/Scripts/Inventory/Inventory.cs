using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory {

    public class Inventory : Singleton<Inventory>
    {
        public List<GameObject> items;
    }

}
