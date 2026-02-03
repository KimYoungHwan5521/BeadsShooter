using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public enum AbilityName { Ice, FastFreezeLV1, FastFreezeLV2, FastFreezeLV3, IcicleBurstLV1, IcicleBurstLV2, IcicleBurstLV3,
        ChilingAuraLV1, ChilingAuraLV2, ChilingAuraLV3, MultiLayerLV1, MultiLayerLV2, MultiLayerLV3, FrostWideLV1, FrostWideLV2, FrostWideLV3, 
        Fire, QuickDrawLV1, QuickDrawLV2, QuickDrawLV3, HotterBallLV1, HotterBallLV2, HotterBallLV3, ExplosionLV1, ExplosionLV2, ExplosionLV3,
        BurningLV1, BurningLV2, BurningLV3, Laser, FrequentShotLV1, FrequentShotLV2, FrequentShotLV3, MultipleShotLV1, MultipleShotLV2,
        MultipleShotLV3, MultipleShotLV4, MultipleShotLV5, MultipleShotLV6, Telekinesis, }
    
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
        #region Ice
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
            explain = "Increase the number of ice cubes (4 ¡æ 6)"
        };
        root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrostWideLV2,
            explain = "Increase the number of ice cubes (6 ¡æ 8)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrostWideLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrostWideLV3,
            explain = "Increase the number of ice cubes (8 ¡æ 12)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrostWideLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);
        #endregion
        #region Fire
        ability = new()
        {
            name = AbilityName.Fire,
            explain = "Fires a fireball in a straight line"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.QuickDrawLV1,
            explain = "Reduced cooldown of fireball launch (6s ¡æ 5s)"
        };
        root = abilities.Find(x => x.name == AbilityName.Fire);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.QuickDrawLV2,
            explain = "Reduced cooldown of fireball launch (5s ¡æ 4s)"
        };
        root = abilities.Find(x => x.name == AbilityName.QuickDrawLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.QuickDrawLV3,
            explain = "Reduced cooldown of fireball launch (4s ¡æ 3s)"
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
            explain = "Blocks damaged by the fireball will burn, dealing 0.4 damage per second for 3 seconds."
        };
        root = abilities.Find(x => x.name == AbilityName.Fire);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BurningLV2,
            explain = "Increased burning damage (0.4 ¡æ 0.8 per second)"
        };
        root = abilities.Find(x => x.name == AbilityName.BurningLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BurningLV3,
            explain = "Increased burning damage (0.8 ¡æ 1.2 per second)"
        };
        root = abilities.Find(x => x.name == AbilityName.BurningLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);
        #endregion
        #region LASER
        ability = new()
        {
            name = AbilityName.Laser,
            explain = "Shot a laser once every 6 seconds."
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrequentShotLV1,
            explain = "Reduced cooldown of shot laser. (6s ¡æ 5.5s)"
        };
        root = abilities.Find(x => x.name == AbilityName.Laser);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrequentShotLV2,
            explain = "Reduced cooldown of shot laser. (5.5s ¡æ 5s)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrequentShotLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrequentShotLV3,
            explain = "Reduced cooldown of shot laser. (5s ¡æ 4.5s)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrequentShotLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleShotLV1,
            explain = "Shoot more lasers. (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.Laser);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleShotLV2,
            explain = "Shoot more lasers. (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleShotLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleShotLV3,
            explain = "Shoot more lasers. (3 ¡æ 4)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleShotLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleShotLV4,
            explain = "Shoot more lasers. (4 ¡æ 6)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleShotLV3);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleShotLV5,
            explain = "Shoot more lasers. (6 ¡æ 8)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleShotLV4);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleShotLV6,
            explain = "Shoot more lasers. (8 ¡æ 10)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleShotLV5);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);
        #endregion
        ability = new()
        {
            name = AbilityName.Telekinesis,
            explain = "Throw the ball towards the block"
        };
        abilities.Add(ability);


        yield return null;
    }
}
