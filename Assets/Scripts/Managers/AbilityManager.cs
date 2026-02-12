using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public enum AbilityName { Ice, FastFreezeLV1, FastFreezeLV2, FastFreezeLV3, IcicleBurstLV1, IcicleBurstLV2, IcicleBurstLV3,
        ChilingAuraLV1, ChilingAuraLV2, ChilingAuraLV3, MultiLayerLV1, MultiLayerLV2, MultiLayerLV3, FrostWideLV1, FrostWideLV2, FrostWideLV3, 
        Fire, QuickDrawLV1, QuickDrawLV2, QuickDrawLV3, HotterBallLV1, HotterBallLV2, HotterBallLV3, ExplosionLV1, ExplosionLV2, ExplosionLV3,
        BurningLV1, BurningLV2, BurningLV3, Laser, FrequentShotLV1, FrequentShotLV2, FrequentShotLV3, MultipleShotLV1, MultipleShotLV2,
        MultipleShotLV3, MultipleShotLV4, MultipleShotLV5, MultipleShotLV6, Telekinesis, FasterBarLV1, FasterBarLV2, FasterBarLV3,
        FasterBallLV1, FasterBallLV2, FasterBallLV3, WideBarLV1, WideBarLV2, WideBarLV3, MultipleBallLV1, MultipleBallLV2, MultipleBallLV3,
        BiggerBallLV1, BiggerBallLV2, BiggerBallLV3, Electric, HigherVoltageLV1, HigherVoltageLV2, HigherVoltageLV3, MoreChainsLV1, MoreChainsLV2,
        MoreChainsLV3, MultipleDischargeLV1, MultipleDischargeLV2, MultipleDischargeLV3, ElectrostaticInductionLV1, ElectrostaticInductionLV2,
        ElectrostaticInductionLV3, FrequentTelekinesisLV1, FrequentTelekinesisLV2, FrequentTelekinesisLV3, StrongerTelekinesisLV1,
        StrongerTelekinesisLV2, StrongerTelekinesisLV3, GuidedBallsLV1, GuidedBallsLV2, GuidedBallsLV3, GuidedFireBall, GuidedIcicles,
    }
    
    public class Ability
    {
        public AbilityName name;
        public bool isPassive = false;
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
            explain = "Ice cubes that bounce the ball appear on either side of the bar."
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FastFreezeLV1,
            explain = "Reduced ice regeneration cooldown (10 ¡æ 8 seconds)"
        };
        Ability root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FastFreezeLV2,
            explain = "Reduced ice regeneration cooldown (8 ¡æ 6 seconds)"
        };
        root = abilities.Find(x => x.name == AbilityName.FastFreezeLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FastFreezeLV3,
            explain = "Reduced ice regeneration cooldown (6 ¡æ 4 seconds)"
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
            explain = "Fire more icicles (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.IcicleBurstLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.IcicleBurstLV3,
            explain = "Fire more icicles (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.IcicleBurstLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ChilingAuraLV1,
            explain = "Slows down the ball speed around the bar."
        };
        root = abilities.Find(x => x.name == AbilityName.Ice);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ChilingAuraLV2,
            explain = "Increased deceleration rate (20% ¡æ 40%)"
        };
        root = abilities.Find(x => x.name == AbilityName.ChilingAuraLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ChilingAuraLV3,
            explain = "Increased deceleration rate (40% ¡æ 60%)"
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
        #region Electric
        ability = new()
        {
            name = AbilityName.Electric,
            explain = "Charges electricity into the ball and discharges it during the next attack."
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.HigherVoltageLV1,
            explain = "Increase discharge damage (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.Electric);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.HigherVoltageLV2,
            explain = "Increase discharge damage (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.HigherVoltageLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.HigherVoltageLV3,
            explain = "Increase discharge damage (3 ¡æ 4)"
        };
        root = abilities.Find(x => x.name == AbilityName.HigherVoltageLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MoreChainsLV1,
            explain = "Increase maximum chains (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.Electric);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MoreChainsLV2,
            explain = "Increase maximum chains (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.MoreChainsLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MoreChainsLV3,
            explain = "Increase maximum chains (3 ¡æ 4)"
        };
        root = abilities.Find(x => x.name == AbilityName.MoreChainsLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleDischargeLV1,
            explain = "Multiple discharge on each charge (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.Electric);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleDischargeLV2,
            explain = "Multiple discharge on each charge (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleDischargeLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleDischargeLV3,
            explain = "Multiple discharge on each charge (3 ¡æ 4)"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleDischargeLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ElectrostaticInductionLV1,
            explain = "Enemies that have been damaged by discharge will re-discharge after a short while."
        };
        root = abilities.Find(x => x.name == AbilityName.Electric);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ElectrostaticInductionLV2,
            explain = "Increase re-discharge damage (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.ElectrostaticInductionLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        root = abilities.Find(x => x.name == AbilityName.HigherVoltageLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);

        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.ElectrostaticInductionLV3,
            explain = "Increase re-discharge damage (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.ElectrostaticInductionLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        root = abilities.Find(x => x.name == AbilityName.HigherVoltageLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        #endregion
        #region Telekinesis
        ability = new()
        {
            name = AbilityName.Telekinesis,
            explain = "Throw the ball towards the enemy"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrequentTelekinesisLV1,
            explain = "Telekinesis cool down (15s ¡æ 13s)"
        };
        root = abilities.Find(x => x.name == AbilityName.Telekinesis);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrequentTelekinesisLV2,
            explain = "Telekinesis cool down (13s ¡æ 11s)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrequentTelekinesisLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FrequentTelekinesisLV3,
            explain = "Telekinesis cool down (11s ¡æ 9s)"
        };
        root = abilities.Find(x => x.name == AbilityName.FrequentTelekinesisLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.StrongerTelekinesisLV1,
            explain = "Guided balls give additional damage"
        };
        root = abilities.Find(x => x.name == AbilityName.Telekinesis);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.StrongerTelekinesisLV2,
            explain = "Increase guided balls' additional damage (3 ¡æ 6)"
        };
        root = abilities.Find(x => x.name == AbilityName.StrongerTelekinesisLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.StrongerTelekinesisLV3,
            explain = "Increase guided balls' additional damage (6 ¡æ 9)"
        };
        root = abilities.Find(x => x.name == AbilityName.StrongerTelekinesisLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.GuidedBallsLV1,
            explain = "Control more balls at the same time. (1 ¡æ 2)"
        };
        root = abilities.Find(x => x.name == AbilityName.Telekinesis);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.GuidedBallsLV2,
            explain = "Control more balls at the same time. (2 ¡æ 3)"
        };
        root = abilities.Find(x => x.name == AbilityName.GuidedBallsLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.GuidedBallsLV3,
            explain = "Control all balls at the same time."
        };
        root = abilities.Find(x => x.name == AbilityName.GuidedBallsLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.GuidedFireBall,
            explain = "Throw the fire ball toward the enemy"
        };
        root = abilities.Find(x => x.name == AbilityName.Telekinesis);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        root = abilities.Find(x => x.name == AbilityName.Fire);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.GuidedIcicles,
            explain = "Throw the icicles toward the enemy"
        };
        root = abilities.Find(x => x.name == AbilityName.Telekinesis);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        root = abilities.Find(x => x.name == AbilityName.IcicleBurstLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);
        #endregion
        #region Passives
        ability = new()
        {
            name = AbilityName.FasterBarLV1,
            isPassive = true,
            explain = "Increase bar moving speed"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FasterBarLV2,
            isPassive = true,
            explain = "Increase bar moving speed"
        };
        root = abilities.Find(x => x.name == AbilityName.FasterBarLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FasterBarLV3,
            isPassive = true,
            explain = "Increase bar moving speed"
        };
        root = abilities.Find(x => x.name == AbilityName.FasterBarLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FasterBallLV1,
            isPassive = true,
            explain = "Increase balls moving speed"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FasterBallLV2,
            isPassive = true,
            explain = "Increase balls moving speed"
        };
        root = abilities.Find(x => x.name == AbilityName.FasterBallLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.FasterBallLV3,
            isPassive = true,
            explain = "Increase balls moving speed"
        };
        root = abilities.Find(x => x.name == AbilityName.FasterBallLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.WideBarLV1,
            isPassive = true,
            explain = "Increase bar length"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.WideBarLV2,
            isPassive = true,
            explain = "Increase bar length"
        };
        root = abilities.Find(x => x.name == AbilityName.WideBarLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.WideBarLV3,
            isPassive = true,
            explain = "Increase bar length"
        };
        root = abilities.Find(x => x.name == AbilityName.WideBarLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleBallLV1,
            isPassive = true,
            explain = "Throw more balls at one shot"
        };
        Ability leaf = abilities.Find(x => x.name == AbilityName.GuidedBallsLV1);
        ability.lowerAbilities.Add(leaf);
        leaf.upperAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleBallLV2,
            isPassive = true,
            explain = "Throw more balls at one shot"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleBallLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        leaf = abilities.Find(x => x.name == AbilityName.GuidedBallsLV2);
        ability.lowerAbilities.Add(leaf);
        leaf.upperAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.MultipleBallLV3,
            isPassive = true,
            explain = "Throw more balls at one shot"
        };
        root = abilities.Find(x => x.name == AbilityName.MultipleBallLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        leaf = abilities.Find(x => x.name == AbilityName.GuidedBallsLV2);
        ability.lowerAbilities.Add(leaf);
        leaf.upperAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BiggerBallLV1,
            isPassive = true,
            explain = "Ball size +, Ball damage +"
        };
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BiggerBallLV2,
            isPassive = true,
            explain = "Ball size +, Ball damage +"
        };
        root = abilities.Find(x => x.name == AbilityName.BiggerBallLV1);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);

        ability = new()
        {
            name = AbilityName.BiggerBallLV3,
            isPassive = true,
            explain = "Ball size +, Ball damage +"
        };
        root = abilities.Find(x => x.name == AbilityName.BiggerBallLV2);
        ability.upperAbilities.Add(root);
        root.lowerAbilities.Add(ability);
        abilities.Add(ability);
        #endregion

        yield return null;
    }
}
