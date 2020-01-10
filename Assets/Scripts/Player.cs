using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTree;

public enum StatType { neutral, body, mind, soul, karma }

public class Player : Singleton<Player>
{
    const string NEGATIVE_KARMA_MESSAGE = "A negative karma message";
    const string POSITIVE_KARMA_MESSAGE = "A positive karma message";
    const int DICE_MIN_VALUE = 1;
    const int DICE_MAX_VALUE = 12;

    public Dictionary<StatType, int> stats;

    void Awake() {
        BuildStats();
    }

    private void BuildStats() {
        stats = new Dictionary<StatType, int>();
        foreach (StatType stat in System.Enum.GetValues(typeof(StatType))) {
            // Load stats or assign default value of 0
            stats[stat] = PlayerPrefs.GetInt(stat.ToString(), 0);
        }
    }

    // Returns the roll of a D12, modified by the given stat type.
    public int StatRoll(StatType statType) {
        // body or soul rolls influence karma
        string karmaMessage = ModifyKarma(statType);

        // Todo(matt) - show the results of the karma change in the dialogue somehow?
        // DialogueManager.DispalyMessage(karmaMessage);

        int roll = Random.Range(DICE_MIN_VALUE,DICE_MAX_VALUE);
        return stats[statType] + roll;
    }

    private string ModifyKarma(StatType statType) {
        switch (statType) {
            case StatType.body:
                stats[StatType.karma]--;
                return NEGATIVE_KARMA_MESSAGE;
            case StatType.soul:
                stats[StatType.karma]++;
                return POSITIVE_KARMA_MESSAGE;
        }
        return null;
    }
    public void ModifyStat(StatType statType) {
        // Karma can't be modified directly, it's affected only by choice statrolls
        switch (statType) {
            case StatType.karma:
                // do nothing, karma should only be modified by ModifyKarma()
                break;
            default:
                stats[statType]++;
                break;
        }
    }
}
