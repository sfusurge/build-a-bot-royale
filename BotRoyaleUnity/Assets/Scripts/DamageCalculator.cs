using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class for determining how much damage to apply to robot parts when they hit
/// </summary>
public static class DamageCalculator
{
    private static readonly Vector2 RANDOM_PERCENT_RANGE = new Vector2(0.9f, 1.1f);

    private static readonly float SUPER_EFFECTIVE = 15f;
    private static readonly float MODERATE_EFFECTIVE = 10f;
    private static readonly float SOMEWHAT_EFFECTIVE = 5f;
    private static readonly float NOT_VERY_EFFECTIVE = 3f;
    private static readonly float BARELY_EFFECTIVE = 1f;

    private static Dictionary<string, float> MatchupDamageLookup = new Dictionary<string, float>()
    {
        // when "type1" hits "type2", how much DAMAGE happens to "type2"
      //{ MatchupKey("type1", "type2"),   DAMAGE },

        { MatchupKey("Block", "Block"),   SOMEWHAT_EFFECTIVE },
        { MatchupKey("Block", "Center"),  SOMEWHAT_EFFECTIVE },
        { MatchupKey("Block", "Spike"),   BARELY_EFFECTIVE },
        { MatchupKey("Block", "Shield"),  MODERATE_EFFECTIVE },

        { MatchupKey("Center", "Block"),  SOMEWHAT_EFFECTIVE },
        { MatchupKey("Center", "Center"), SOMEWHAT_EFFECTIVE },
        { MatchupKey("Center", "Spike"),  BARELY_EFFECTIVE },
        { MatchupKey("Center", "Shield"), MODERATE_EFFECTIVE },

        { MatchupKey("Spike", "Block"),   SUPER_EFFECTIVE },
        { MatchupKey("Spike", "Center"),  SOMEWHAT_EFFECTIVE },
        { MatchupKey("Spike", "Spike"),   SOMEWHAT_EFFECTIVE },
        { MatchupKey("Spike", "Shield"),  BARELY_EFFECTIVE },

        { MatchupKey("Shield", "Block"),  NOT_VERY_EFFECTIVE },
        { MatchupKey("Shield", "Center"), NOT_VERY_EFFECTIVE },
        { MatchupKey("Shield", "Spike"),  SUPER_EFFECTIVE },
        { MatchupKey("Shield", "Shield"), BARELY_EFFECTIVE }
    };

    private static string MatchupKey(string attacker, string defender)
    {
        return attacker.ToLower() + "->" + defender.ToLower();
    }

    public static float DamageToInflictOnCollision(string thisPartType, string otherPartType)
    {
        // get base damage from type matchup
        string matchupKey = MatchupKey(otherPartType, thisPartType);
        if (MatchupDamageLookup.ContainsKey(matchupKey) == false)
        {
            Debug.LogWarning("No matchup damage available for " + matchupKey);
            return 0f;
        }
        float damage = MatchupDamageLookup[matchupKey];

        // add some randomness
        damage *= UnityEngine.Random.Range(RANDOM_PERCENT_RANGE.x, RANDOM_PERCENT_RANGE.y);

        return damage;
    }
}
