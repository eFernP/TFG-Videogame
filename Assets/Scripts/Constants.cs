using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    //public const string SYLLABLE_EI = "Ei";
    //public const string SYLLABLE_RI = "Ri";
    //public const string SYLLABLE_HA = "Ha";
    //public const string SYLLABLE_GO = "Go";
    //public const string SYLLABLE_TER = "Ter";
    //public const string SYLLABLE_KAL = "Kal";

    public static string[] SYLLABLES = { "Ei", "Ri", "Ha", "Go", "Ter", "Kal" };

    public static string STONE_TYPE = "Stone";
    public static string ENEMY_TYPE = "Enemy";


    public static Dictionary<string, string> NAME_PREFIX = new Dictionary<string, string>()
     {
            {STONE_TYPE, SYLLABLES[3]},
            {ENEMY_TYPE, SYLLABLES[5]},
     };
}
