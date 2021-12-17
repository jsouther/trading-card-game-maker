// Name:    CardCollectors
// Date:    11/12/2021
// Description: Static class for path variables and variable names
// Makes it easy to change parameters if we want to reorganize our code/folders
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Definitions 
{
    // Card type defintions; summons vs. spells
    public const string typePath = "CardType/";
    public const string typeSpellName = "Spells";
    public const string typeSummonName = "Summons";

    // Element defintions
    public const string elementPath = "Elements/";
    public const string elementAir = "Air";
    public const string elementAirBorder = "BorderAir";
    public const string elementFire = "Fire";
    public const string elementFireBorder = "BorderFire";
    public const string elementWater = "Water";
    public const string elementWaterBorder = "BorderWater";
    public const string elementEarth = "Earth";
    public const string elementEarthBorder = "BorderEarth";
    
    // Images used in the card displayer
    public enum ElementSet { Air, Earth, Fire, Water };
    public static Sprite spellImage = Resources.Load<Sprite>(Definitions.typePath + Definitions.typeSpellName);
    public static Sprite summonImage = Resources.Load<Sprite>(Definitions.typePath + Definitions.typeSummonName);
    public static Sprite airLogo = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementAir);
    public static Sprite airBorder = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementAirBorder);
    public static Sprite fireLogo = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementFire);
    public static Sprite fireBorder = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementFireBorder);
    public static Sprite earthLogo = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementEarth);
    public static Sprite earthBorder = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementEarthBorder);
    public static Sprite waterLogo = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementWater);
    public static Sprite waterBorder = Resources.Load<Sprite>(Definitions.elementPath + Definitions.elementWaterBorder);

}
