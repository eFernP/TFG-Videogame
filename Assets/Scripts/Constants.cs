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

    public static string[] SYLLABLES = { "Ei", "Ri", "Ha", "Go", "Kal" };

    public static string STONE_TYPE = "Stone";
    public static string ENEMY_TYPE = "Enemy";


    public static Dictionary<string, string> NAME_PREFIX = new Dictionary<string, string>()
     {
            {STONE_TYPE, SYLLABLES[3]},
            {ENEMY_TYPE, SYLLABLES[2]},
     };



    public static Dictionary<int, HeroDialogue>[] HeroDialogues = new Dictionary<int, HeroDialogue>[]
     {
            new Dictionary<int, HeroDialogue>()
            {
                {1, new HeroDialogue(1, "No me hagas da�o, �por favor!", 1, new int[] { 3 }, new int[]{}, 10, 10)},
                {2, new HeroDialogue(2, "Detente o lo lamentar�s.", 2, new int[] { 4 }, new int[]{ }, -10, 10)},
                {3, new HeroDialogue(3, "He venido a buscar a alguien.", 4, new int[]{6,7},  new int[]{ }, 5, 10)},
                {4, new HeroDialogue(4, "�Sabes acaso qui�n soy?", 3, new int[]{5,6},  new int[]{ }, -5, 10)},
                {5, new HeroDialogue(5, "�Qui�n eres t�?", 5, new int[]{ },  new int[]{ }, 0, 10)},
                {6, new HeroDialogue(6, "�Qu� es este lugar?", 6, new int[]{ },  new int[]{ }, 0, 10)},
                {7, new HeroDialogue(7, "La gente busca refugio. Hay una guerra terrible.", 7, new int[]{8},  new int[]{ }, 10, 10)},
                {8, new HeroDialogue(8, "Y te da igual?", 8, new int[]{9},  new int[]{ }, -5, 10)},
                {9, new HeroDialogue(9, "No es mi guerra", 9, new int[]{10,11},  new int[]{ }, 5, 10)},
                {10, new HeroDialogue(10, "Solo quer�a proteger a mi familia", 10, new int[]{12},  new int[]{11}, 5, 10)},
                {11, new HeroDialogue(11, "Era mi deber, pero yo no quer�a", 10, new int[]{12},  new int[]{10}, 5, 10)},
                {12, new HeroDialogue(12, "No, ahora solo quiero que acabe", 11, new int[]{},  new int[]{-1}, 10, 10)}, //TODO: Test. Has to remove all the other dialogue options
            },
            new Dictionary<int, HeroDialogue>()
            {
                {1, new HeroDialogue(1, "Ay�dame entonces.", 3, new int[] { 3,5,7 }, new int[]{}, 5, 10)},
                {2, new HeroDialogue(2, "�De qu� est�s hablando?", 2, new int[] { }, new int[]{ }, 0, 10)},
                {3, new HeroDialogue(3, "Por favor. No podemos soportarlo m�s.", 4, new int[]{4},  new int[]{5}, 5, 10)},
                {4, new HeroDialogue(4, "�Por favor!", 5, new int[]{6},  new int[]{ }, 5, 10)},
                {5, new HeroDialogue(5, "Que pese sobre tu conciencia tu decisi�n.", 5, new int[]{6},  new int[]{3}, -10, 10)},
                {6, new HeroDialogue(6, "Si no me das una respuesta, tendr� que sac�rtela por la fuerza.", 8, new int[]{},  new int[]{-1}, 5, 10)},//TODO: Test. Has to remove all the other dialogue options
                {7, new HeroDialogue(7, "�Qu� eres?", 7, new int[]{},  new int[]{ },0, 10)}
            }

    };

    public static Dictionary<int, BossDialogue>[] BossDialogues = new Dictionary<int, BossDialogue>[]
    {
        new Dictionary<int, BossDialogue>()
        {
            {1, new BossDialogue(1, new Vocals[]{ new Vocals("testClip", "No debiste entrar aqu�.") }) },
            {2, new BossDialogue(2, new Vocals[]{ new Vocals("testClip", "Eres d�bil."), new Vocals("testClip", "No eres ninguna amenaza para m�.") }) },
            {3, new BossDialogue(3, new Vocals[]{ new Vocals("testClip", "All� fuera eres Irik el Lucero, el Esplendoroso,"), new Vocals("testClip", "el hijo predilecto de un reino que se desmorona."), new Vocals("testClip", "Aqu� dentro no eres nadie.") }) },
            {4, new BossDialogue(4, new Vocals[]{ new Vocals("testClip", "Un intruso m�s.") }) },
            {5, new BossDialogue(5, new Vocals[]{ new Vocals("testClip", "El Guardi�n. El Vig�a.") }) },
            {6, new BossDialogue(6, new Vocals[]{ new Vocals("testClip", "Eso no te incumbe.") }) },
            {7, new BossDialogue(7, new Vocals[]{ new Vocals("testClip", "Lo s�."), new Vocals("testClip", "He o�do los gritos, he visto los monstruos que cosechan carne."), new Vocals("testClip", "He visto c�mo ensamblan esa carne y la convierten en nuevos soldados"), new Vocals("testClip", "que env�an a una absurda batalla contra la muerte, siempre vencedora.") }) },
            {8, new BossDialogue(8, new Vocals[]{ new Vocals("testClip", "Es tu guerra, no la m�a.") }) },
            {9, new BossDialogue(9, new Vocals[]{ new Vocals("testClip", "Mientes. Tambi�n te he visto a ti, Lucero,"), new Vocals("testClip", "blandiendo una lanza contra los que t� llamas traidores y sus m�quinas"), new Vocals("testClip", "Has derramado sangre.") }) },
            {10, new BossDialogue(10, new Vocals[]{ new Vocals("testClip", "�No vas a luchar m�s?") }) },
            {11, new BossDialogue(11, new Vocals[]{ new Vocals("testClip", "Ens��ame de qu� eres capaz.") }) },
        },
        new Dictionary<int, BossDialogue>()
        {
            {1, new BossDialogue(1, new Vocals[]{ new Vocals("testClip", "Eres osado, pero insignificante."), new Vocals("testClip", "La edad maldita ha llegado y est�is todos condenados.") }) },
            {2, new BossDialogue(2, new Vocals[]{ new Vocals("testClip", "Del final del tiempo, de un instante sin fin. "), new Vocals("testClip", "Los falsos dioses manipularon el hilo y terminaron enred�ndolo.") }) },
            {3, new BossDialogue(3, new Vocals[]{ new Vocals("testClip", "No voy a ayudarte, no debo intervenir.") }) },
            {4, new BossDialogue(4, new Vocals[]{ new Vocals("testClip", "...") }) },
            {5, new BossDialogue(5, new Vocals[]{ new Vocals("testClip", "�C�llate!") }) },
            {6, new BossDialogue(6, new Vocals[]{ new Vocals("testClip", "Insolente.") }) },
            {7, new BossDialogue(7, new Vocals[]{ new Vocals("testClip", "Algo que no podr�as comprender."), new Vocals("testClip", "Un ser que no se rige por las leyes de tu mundo.") }) },
            {8, new BossDialogue(8, new Vocals[]{ new Vocals("testClip", "Adelante, te espero.") }) },
        }
    };

    public static Vocals[][] LastBattleDialogues = new Vocals[][]
    {
        new Vocals[]{ new Vocals("testClip", "He tenido suficiente.") }
    };
}
