using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Vocals
{
    public string ClipPath { get; set; }
    public string Text { get; set; }

    public Vocals(string clipPath, string text)
    {
        ClipPath = clipPath;
        Text = text;
    }
}

public class BossDialogue
{

    public int Id { get; set; }
    public Vocals[] Messages { get; set; }

    public BossDialogue(int id, Vocals[] messages)
    {
        Id = id;
        Messages = messages;

    }
}

public class HeroDialogue
{

    public int Id { get; set; }
    public string Text { get; set; }
    public int BossAnswer { get; set; }
    public int[] NextHeroDialogues { get; set; }
    public int[] IncompatibleDialogues { get; set; }
    public int Friendship { get; set; }
    public float Breath { get; set; }

    public HeroDialogue(int id, string text, int bossAnswer, int[] next, int[] incompatibleDialogues, int friendship, float breath)
    {
        Id = id;
        Text = text;
        BossAnswer = bossAnswer;
        NextHeroDialogues = next;
        IncompatibleDialogues = incompatibleDialogues;
        Friendship = friendship;
        Breath = breath;

    }
    
}


public class MagicSyllable
{
    public string Name { get; set; }
    public AudioClip Clip { get; set; }
    public Sprite Grapheme { get; set; }

    public MagicSyllable(string name, AudioClip audio, Sprite grapheme)
    {
        Name = name;
        Clip = audio;
        Grapheme = grapheme;
    }
}







