using System.Collections.Generic;

public static class UserManager {
    public static string userID = "16";
    public static string username;
    public static int currentDeckID = -1;
    public static string currentDeckName = null;
    public static DeckDropdownData[] allDeckData;
    public static CustomGameData[] allCustomGameData;
    public static List<string> deckNames = new List<string>();
    public static List<string> customGameNames = new List<string>();
    public static string selectedCustomGameName;

    public static string spellPointsPerTurnValue = null;
    public static string startingSpellPointsValue = null;
    public static string handSizeValue = null;
    public static string maxSizeValue = null;
    public static string cardsPerTurnValue = null;
    public static string maxSummonsValue = null;
    public static string elementFactorValue = null;
    public static string timeLimitValue = null;
    public static string turnLimitValue = null;
    public static string startingLifeValue = null;
}