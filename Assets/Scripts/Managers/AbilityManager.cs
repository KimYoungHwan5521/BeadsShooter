using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public enum AbilityName { Ice, FastFreezeLV1, FastFreezeLV2, FastFreezeLV3, IcicleBurstLV1, IcicleBurstLV2, IcicleBurstLV3,
        ChilingAuraLV1, ChilingAuraLV2, ChilingAuraLV3, MultiLayerLV1, MultiLayerLV2, MultiLayerLV3, FrostWideLV1, FrostWideLV2, FrostWideLV3, 
        Fire, QuickDrawLV1, QuickDrawLV2, QuickDrawLV3, HotterBallLV1, HotterBallLV2, HotterBallLV3, ExplosionLV1, ExplosionLV2, ExplosionLV3,
        BurningLV1, BurningLV2, BurningLV3, Laser, Telekinesis, Steel}
    
    public class Ability
    {
        public AbilityName name;
        public List<Ability> upperAbilities = new();
        public List<Ability> lowerAbilities = new();
        public string explain;
    }

    static List<Ability> abilities = new();
    public static List<Ability> Abilities => abilities;

    public IEnumerator Initiate()
    {
        GameManager.ClaimLoadInfo("Loading abilities");
        abilities = new();
        Ability ability = new()
        {
            name = AbilityName.Ice,
            explain = "Ice blocks appear on either side of the bar, allowing you to bounce the ball once. (Respawns after 10 seconds)"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FastFreezeLV1,
            explain = "Reduced ice regeneration cooldown (8 seconds)"
        };
        Ability root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FastFreezeLV2,
            explain = "Reduced ice regeneration cooldown (6 seconds)"
        };
        root = abilities.Find(x => x.name == AbilityName.FastFreezeLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FastFreezeLV3,
            explain = "Reduced ice regeneration cooldown (4 seconds)"
        };
        root = abilities.Find(x => x.name == AbilityName.FastFreezeLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.IcicleBurstLV1,
            explain = "When the ice breaks, it fires a single icicle"
        };
        root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.IcicleBurstLV2,
            explain = "When the ice breaks, it fire two icicles"
        };
        root = abilities.Find(x => x.name == AbilityName.IcicleBurstLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.IcicleBurstLV3,
            explain = "When the ice breaks, it fire three icicles"
        };
        root = abilities.Find(x => x.name == AbilityName.IcicleBurstLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ChilingAuraLV1,
            explain = "Ball speed near the bar reduced by 20%"
        };
        root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ChilingAuraLV2,
            explain = "Ball speed near the bar reduced by 40%"
        };
        root = abilities.Find(x => x.name == AbilityName.ChilingAuraLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ChilingAuraLV3,
            explain = "Ball speed near the bar reduced by 60%"
        };
        root = abilities.Find(x => x.name == AbilityName.ChilingAuraLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultiLayerLV1,
            explain = "Increased ice durability (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultiLayerLV2,
            explain = "Increased ice durability (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultiLayerLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultiLayerLV3,
            explain = "Increased ice durability (3 ¡æ 4)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultiLayerLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrostWideLV1,
            explain = "Increase the number of ice cubes (2 ¡æ 4)"
        };
        root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrostWideLV2,
            explain = "Increase the number of ice cubes (4 ¡æ 6)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrostWideLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrostWideLV3,
            explain = "Increase the number of ice cubes (6 ¡æ 8)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrostWideLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.Fire,
            explain = "Fires a fireball in a straight line"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.QuickDrawLV1,
            explain = "Reduced cooldown of fireball launch (9s ¡æ 7s)"
        };
        root = abilities.Find(x => x.name == AbilityName.Fire);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.QuickDrawLV2,
            explain = "Reduced cooldown of fireball launch (7s ¡æ 5s)"
        };
        root = abilities.Find(x => x.name == AbilityName.QuickDrawLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.QuickDrawLV3,
            explain = "Reduced cooldown of fireball launch (5s ¡æ 3s)"
        };
        root = abilities.Find(x => x.name == AbilityName.QuickDrawLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.HotterBallLV1,
            explain = "Increased fireball damage (2 ¡æ 4)"
        };
        root = abilities.Find(x => x.name == AbilityName.Fire);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.HotterBallLV2,
            explain = "Increased fireball damage (4 ¡æ 6)"
        };
        root = abilities.Find(x => x.name == AbilityName.HotterBallLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.HotterBallLV3,
            explain = "Increased fireball damage (6 ¡æ 8)"
        };
        root = abilities.Find(x => x.name == AbilityName.HotterBallLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ExplosionLV1,
            explain = "The fireball explodes when hit, dealing damage to the surrounding area."
        };
        root = abilities.Find(x => x.name == AbilityName.Fire);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ExplosionLV2,
            explain = "Increased fireball explosion range (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.ExplosionLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ExplosionLV3,
            explain = "Increased fireball explosion range (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.ExplosionLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BurningLV1,
            explain = "Blocks damaged by the fireball will burn, dealing 1 damage per second for 3 seconds."
        };
        root = abilities.Find(x => x.name == AbilityName.Fire);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BurningLV2,
            explain = "Increased burning damage (1 ¡æ 2 per second)"
        };
        root = abilities.Find(x => x.name == AbilityName.BurningLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BurningLV3,
            explain = "Increased burning damage (2 ¡æ 3 per second)"
        };
        root = abilities.Find(x => x.name == AbilityName.BurningLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.Telekinesis,
            explain = "Throw the ball towards the block"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.Steel,
            explain = "Increased number of balls"
        };
        abilities.Add(ability);

        yield return null;
    }
}
