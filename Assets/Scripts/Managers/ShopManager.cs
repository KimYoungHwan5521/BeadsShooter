using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager
{
    public enum MerchandiseType { ETC, Material, Blueprint, Consumable, Accessory }

    public class MerchandiseInfo
    {
        public string name;
        public MerchandiseType type;
        public Sprite sprite;
        public int materialType;
        public RewardFormat reward;
        public Blueprint blueprint;
        public int price;

        public MerchandiseInfo(string name, MerchandiseType type, int price)
        {
            this.name = name;
            this.type = type;
            this.price = price;
        }

        public MerchandiseInfo(string name, MerchandiseType type, RewardFormat reward, int price)
        {
            this.name = name;
            this.type = type;
            this.reward = reward;
            this.price = price;
        }

        public MerchandiseInfo(string name, MerchandiseType type, int materialType, RewardFormat reward, int price)
        {
            this.name = name;
            this.type = type;
            this.materialType = materialType;
            this.reward = reward;
            this.price = price;
        }

        public MerchandiseInfo(string name, MerchandiseType type, Blueprint blueprint, int price)
        {
            this.name = name;
            this.type = type;
            this.blueprint = blueprint;
            this.price = price;
        }
    }

    public List<MerchandiseInfo> merchandises;
    public List<MerchandiseInfo> rareMerchandises;

    public IEnumerator Initiate()
    {
        merchandises = new()
        {
            new("Material1", MerchandiseType.Material, 1, new RewardFormat(RewardType.AttackDamage, 0.5f), 10),
            new("Material2", MerchandiseType.Material, 2, new RewardFormat(RewardType.AttackDamage, 0.5f), 10),
            new("Material3", MerchandiseType.Material, 3, new RewardFormat(RewardType.AttackDamage, 0.5f), 10),
            new("Material4", MerchandiseType.Material, 4, new RewardFormat(RewardType.AttackDamage, 0.5f), 10),
            new("Blueprint1", MerchandiseType.Blueprint, new Blueprint(new int[,] { { 3, 4, 3 } }, new(RewardType.AttackDamage, 0.5f)), 20),
            new("Reroll", MerchandiseType.Consumable, new RewardFormat(RewardType.Reroll, 1), 10),
            new("Life", MerchandiseType.Consumable, new RewardFormat(RewardType.Life, 1), 10),
        };
        rareMerchandises = new()
        {
            new("Rare Item", MerchandiseType.Accessory,  new RewardFormat(RewardType.AttackDamage, 1), 100),
        };
        yield return null;
    }
}
