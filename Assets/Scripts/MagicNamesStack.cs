using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MagicNamesStack : MonoBehaviour
{


    private static string[] MagicSyllables = Constants.SYLLABLES;
    //private static int TOTAL_MAGIC_NAMES_NUMBER = MagicSyllables.Length + (int)System.Math.Pow(MagicSyllables.Length, 2) + (int)System.Math.Pow(MagicSyllables.Length, 3);
    private static int MAGIC_NAME_LENGTH_WITHOUT_PREFIX = 2;
    
    private static string[] StoneNames = new string[(int)System.Math.Pow(MagicSyllables.Length, MAGIC_NAME_LENGTH_WITHOUT_PREFIX)];
    private Stack<string> UsedStoneNames = new Stack<string>((int)System.Math.Pow(MagicSyllables.Length, MAGIC_NAME_LENGTH_WITHOUT_PREFIX));

    string[] getRandomMagicNameArray(int length, string prefix)
    {
        string[] MappedArray = new string[(int)System.Math.Pow(MagicSyllables.Length, length)];

        for (int i = 0; i < (int)System.Math.Pow(MagicSyllables.Length, length); i++)
        {
            for (int j = 0; j < length; j++)
            {

                MappedArray[i] = "_" + MagicSyllables[(i / (int)System.Math.Pow(MagicSyllables.Length, j)) % MagicSyllables.Length] + MappedArray[i];

                if (j == length-1)
                {
                    MappedArray[i] = prefix + MappedArray[i];
                    UsedStoneNames.Push(MappedArray[i]);
                }
            }
        }

        System.Random rnd = new System.Random();
        return MappedArray.OrderBy(x => rnd.Next()).ToArray();
    }


    void refillStoneStack()
    {
        foreach(string name in StoneNames)
        {
            //TODO: Check if name is not used by any current scene object
            UsedStoneNames.Push(name);
        }
    }

    public string GetStoneName()
    {
        if (UsedStoneNames.Count == 0)
        {
            refillStoneStack();
        }
        return UsedStoneNames.Pop();
    }


    void Awake()
    {

        StoneNames = getRandomMagicNameArray(MAGIC_NAME_LENGTH_WITHOUT_PREFIX, Constants.NAME_PREFIX[Constants.STONE_TYPE]);
        refillStoneStack();

    }

}
