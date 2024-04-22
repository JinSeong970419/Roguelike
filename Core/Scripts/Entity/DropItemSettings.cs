using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    [CreateAssetMenu(fileName = "DropItemSettings", menuName = "TheSalt/Settings/DropItemSettings")]
    public class DropItemSettings : ScriptableObject
    {
        [ArrayElementTitle("monsterKind")]
        [SerializeField] private DropItemInfo[] dropItemInfos = new DropItemInfo[(int)ActorKind.End];
        
        public DropItemInfo[] DropItemInfos { get { return dropItemInfos; } }

        private void OnValidate()
        {
            int count = (int)ActorKind.End;
            DropItemInfo[] temp = new DropItemInfo[count];
            if (dropItemInfos.Length != count)
            {
                for (int i = 0; i < count; i++)
                {
                    temp[i] = new DropItemInfo();
                    temp[i].MonsterKind = (ActorKind)i;
                    if(i < dropItemInfos.Length)
                    {
                        temp[i].DropItems = dropItemInfos[i].DropItems;
                    }
                }
            }

        }
    }

    [System.Serializable]
    public class DropItemInfo
    {
        [SerializeField] private ActorKind monsterKind;
        [SerializeField] private List<DropItemElement> dropItems;

        public ActorKind MonsterKind { get { return monsterKind; } set { monsterKind = value; } }
        public List<DropItemElement> DropItems { get { return dropItems; } set { dropItems = value; } }
    }

    [System.Serializable]
    public class DropItemElement
    {
        [SerializeField] private ItemKind itemKind;
        [SerializeField] private int quantity;
        [SerializeField] private float probability;

        public ItemKind ItemKind { get { return itemKind; } }
        public int Quantity { get { return quantity; } }
        public float Probability { get { return probability; } }
    }
}
