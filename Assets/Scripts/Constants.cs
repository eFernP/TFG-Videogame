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
                {1, new HeroDialogue(1, "No me hagas daño, ¡por favor!", 1, new int[] { 3 }, new int[]{}, 10, 10)},
                {2, new HeroDialogue(2, "Detente o lo lamentarás.", 2, new int[] { 4 }, new int[]{ }, -10, 10)},
                {3, new HeroDialogue(3, "He venido a buscar a alguien.", 4, new int[]{6,7},  new int[]{ }, 5, 10)},
                {4, new HeroDialogue(4, "¿Sabes acaso quién soy?", 3, new int[]{5,6},  new int[]{ }, -5, 10)},
                {5, new HeroDialogue(5, "¿Quién eres tú?", 5, new int[]{ },  new int[]{ }, 0, 10)},
                {6, new HeroDialogue(6, "¿Qué es este lugar?", 6, new int[]{ },  new int[]{ }, 0, 10)},
                {7, new HeroDialogue(7, "La gente busca refugio. Hay una guerra terrible.", 7, new int[]{8},  new int[]{ }, 10, 10)},
                {8, new HeroDialogue(8, "Y te da igual?", 8, new int[]{9},  new int[]{ }, -5, 10)},
                {9, new HeroDialogue(9, "No es mi guerra", 9, new int[]{10,11},  new int[]{ }, 5, 10)},
                {10, new HeroDialogue(10, "Solo quería proteger a mi familia", 10, new int[]{12},  new int[]{11}, 5, 10)},
                {11, new HeroDialogue(11, "Era mi deber, pero yo no quería", 10, new int[]{12},  new int[]{10}, 5, 10)},
                {12, new HeroDialogue(12, "No, ahora solo quiero que acabe", 11, new int[]{},  new int[]{-1}, 10, 10)}, //TODO: Test. Has to remove all the other dialogue options
            },
            new Dictionary<int, HeroDialogue>()
            {
                {1, new HeroDialogue(1, "Ayúdame entonces.", 3, new int[] { 3,5,7 }, new int[]{}, 5, 10)},
                {2, new HeroDialogue(2, "¿De qué estás hablando?", 2, new int[] { }, new int[]{ }, 0, 10)},
                {3, new HeroDialogue(3, "Por favor. No podemos soportarlo más.", 4, new int[]{4},  new int[]{5}, 5, 10)},
                {4, new HeroDialogue(4, "¡Por favor!", 5, new int[]{6},  new int[]{ }, 5, 10)},
                {5, new HeroDialogue(5, "Que pese sobre tu conciencia tu decisión.", 5, new int[]{6},  new int[]{3}, -10, 10)},
                {6, new HeroDialogue(6, "Si no me das una respuesta, tendré que sacártela por la fuerza.", 8, new int[]{},  new int[]{-1}, 5, 10)},//TODO: Test. Has to remove all the other dialogue options
                {7, new HeroDialogue(7, "¿Qué eres?", 7, new int[]{},  new int[]{ },0, 10)}
            }

    };

    public static Dictionary<int, BossDialogue>[] BossDialogues = new Dictionary<int, BossDialogue>[]
    {
        new Dictionary<int, BossDialogue>()
        {
            {1, new BossDialogue(1, new Vocals[]{ new Vocals("testClip", "No debiste entrar aquí.") }) },
            {2, new BossDialogue(2, new Vocals[]{ new Vocals("testClip", "Eres débil."), new Vocals("testClip", "No eres ninguna amenaza para mí.") }) },
            {3, new BossDialogue(3, new Vocals[]{ new Vocals("testClip", "Allí fuera eres Irik el Lucero, el Esplendoroso,"), new Vocals("testClip", "el hijo predilecto de un reino que se desmorona."), new Vocals("testClip", "Aquí dentro no eres nadie.") }) },
            {4, new BossDialogue(4, new Vocals[]{ new Vocals("testClip", "Un intruso más.") }) },
            {5, new BossDialogue(5, new Vocals[]{ new Vocals("testClip", "El Guardián. El Vigía.") }) },
            {6, new BossDialogue(6, new Vocals[]{ new Vocals("testClip", "Eso no te incumbe.") }) },
            {7, new BossDialogue(7, new Vocals[]{ new Vocals("testClip", "Lo sé."), new Vocals("testClip", "He oído los gritos, he visto los monstruos que cosechan carne."), new Vocals("testClip", "He visto cómo ensamblan esa carne y la convierten en nuevos soldados"), new Vocals("testClip", "que envían a una absurda batalla contra la muerte, siempre vencedora.") }) },
            {8, new BossDialogue(8, new Vocals[]{ new Vocals("testClip", "Es tu guerra, no la mía.") }) },
            {9, new BossDialogue(9, new Vocals[]{ new Vocals("testClip", "Mientes. También te he visto a ti, Lucero,"), new Vocals("testClip", "blandiendo una lanza contra los que tú llamas traidores y sus máquinas"), new Vocals("testClip", "Has derramado sangre.") }) },
            {10, new BossDialogue(10, new Vocals[]{ new Vocals("testClip", "¿No vas a luchar más?") }) },
            {11, new BossDialogue(11, new Vocals[]{ new Vocals("testClip", "Enséñame de qué eres capaz.") }) },
        },
        new Dictionary<int, BossDialogue>()
        {
            {1, new BossDialogue(1, new Vocals[]{ new Vocals("testClip", "Eres osado, pero insignificante."), new Vocals("testClip", "La edad maldita ha llegado y estáis todos condenados.") }) },
            {2, new BossDialogue(2, new Vocals[]{ new Vocals("testClip", "Del final del tiempo, de un instante sin fin. "), new Vocals("testClip", "Los falsos dioses manipularon el hilo y terminaron enredándolo.") }) },
            {3, new BossDialogue(3, new Vocals[]{ new Vocals("testClip", "No voy a ayudarte, no debo intervenir.") }) },
            {4, new BossDialogue(4, new Vocals[]{ new Vocals("testClip", "...") }) },
            {5, new BossDialogue(5, new Vocals[]{ new Vocals("testClip", "¡Cállate!") }) },
            {6, new BossDialogue(6, new Vocals[]{ new Vocals("testClip", "Insolente.") }) },
            {7, new BossDialogue(7, new Vocals[]{ new Vocals("testClip", "Algo que no podrías comprender."), new Vocals("testClip", "Un ser que no se rige por las leyes de tu mundo.") }) },
            {8, new BossDialogue(8, new Vocals[]{ new Vocals("testClip", "Adelante, te espero.") }) },
        }
    };

    public static Vocals[][] LastBattleDialogues = new Vocals[][]
    {
        new Vocals[]{ new Vocals("testClip", "He tenido suficiente.") }
    };
}
