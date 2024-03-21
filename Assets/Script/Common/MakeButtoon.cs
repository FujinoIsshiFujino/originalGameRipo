using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeButtoon : MonoBehaviour
{
    public enum makeItemType
    {
        bridge,
        block,
    }

    [SerializeField] public makeItemType type;

    [System.Serializable]
    public class ItemData
    {
        public Item.ItemType itemType;
        public int consumeItemNumber;
    }

    public ItemData[] itemDataArray;
}
