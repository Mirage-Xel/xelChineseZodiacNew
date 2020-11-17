using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using rnd = UnityEngine.Random;
public class ChineseZodiac : MonoBehaviour
{
    public KMSelectable year;
    public KMSelectable zodiac;
    public KMSelectable sumbit;
    int[] years = Enumerable.Range(1804, 2104).ToArray<int>();
    string[] zodiacs = new string[] { "木鼠", "木牛", "火虎", "火兔", "土龙", "土蛇", "金马", "金羊", "水猴", "水鸡", "木狗", "木猪", "火鼠", "火牛", "土虎", "土兔", "金龙", "金蛇", "水马", "水羊", "木猴", "木鸡", "火狗", "火猪", "土鼠", "土牛", "金虎", "金兔", "水龙", "水蛇", "木马", "木羊", "火猴", "火鸡", "土狗", "土猪", "金鼠", "金牛", "水虎", "水兔", "木龙", "木蛇", "火马", "火羊", "土猴", "土鸡", "金狗", "金猪", "水鼠", "水牛", "木虎", "木兔", "火龙", "火蛇", "土马", "土羊", "金猴", "金鸡", "水狗", "水猪", };
    int[] yearZodiacs = new int[5];
    string[] yearOptions = new string[5];
    string[] zodiacOptions = new string[] {"", "", "", "", ""};
    string[] forbiddenZodiacOptions = new string[4];
    int correctYear;
    int correctZodiac;
    int curYearIndex;
    int curZodiacIndex;
    public KMBombModule module;
    public KMAudio sound;
    int moduleID;
    static int moduleIDCounter = 1;
    bool moduleSolved;
    // Use this for initialization
    void Awake()
    {
        moduleID = moduleIDCounter++;
        year.OnInteract += delegate { cycleYearOptions(); return false; };
        zodiac.OnInteract += delegate { cycleZodiacOptions(); return false; };
        sumbit.OnInteract += delegate { handleSumbit(); return false; };
    }
    void Start()
    {
        pickYears();
        pickZodiacs();
        year.GetComponentInChildren<TextMesh>().text = yearOptions[0];
        zodiac.GetComponentInChildren<TextMesh>().text = zodiacOptions[0];
        Debug.LogFormat("[Chinese Zodiac #{0}] The year options are {1}, {2}, {3}, {4}, and {5}.", moduleID, yearOptions[0], yearOptions[1], yearOptions[2], yearOptions[3], yearOptions[4]);
        Debug.LogFormat("[Chinese Zodiac #{0}] The zodiac options are {1}, {2}, {3}, {4}, and {5}.", moduleID, logZodiac(zodiacOptions[0]), logZodiac(zodiacOptions[1]), logZodiac(zodiacOptions[2]), logZodiac(zodiacOptions[3]), logZodiac(zodiacOptions[4]));
        Debug.LogFormat("[Chinese Zodiac #{0}] The correct year is {1}", moduleID, yearOptions[correctYear]);
        Debug.LogFormat("[Chinese Zodiac #{0}] The correct zodiac is {1}", moduleID, logZodiac(zodiacOptions[correctZodiac]));
    }
    void pickYears()
    {
        for (int i = 0; i < 5; i++)
        {
            string year;
            int yearZodiac;
            do
            {
                year = years[rnd.Range(0, 300)].ToString();
                yearZodiac = (int.Parse(year) - 4) % 60;
            }
            while (yearOptions.Contains(year) || yearZodiacs.Contains(yearZodiac));
            yearOptions[i] = year;
            yearZodiacs[i] = yearZodiac;
        }
        correctYear = rnd.Range(0, 5);
    }
    void pickZodiacs()
    {
        correctZodiac = rnd.Range(0, 5);
        zodiacOptions[correctZodiac] = zodiacs[yearZodiacs[correctYear]];
        for (int i = 0; i < 5; i++)
        {
            int candidateZodiacIndex;
            if (zodiacOptions[i].Equals(""))
            {
                do
                {
                    candidateZodiacIndex = rnd.Range(0, 60);
                } while (yearZodiacs.Contains(candidateZodiacIndex));
                zodiacOptions[i] = zodiacs[candidateZodiacIndex];
            }
        }
    }
    void cycleYearOptions()
    {
        if (!moduleSolved)
        {
            year.AddInteractionPunch(.5f);
            curYearIndex++;
            if (curYearIndex == 5)
            {
                curYearIndex = 0;
            }
            year.GetComponentInChildren<TextMesh>().text = yearOptions[curYearIndex];
        }
    }
    void cycleZodiacOptions()
    {
        if (!moduleSolved)
        {
            zodiac.AddInteractionPunch(.5f);
            curZodiacIndex++;
            if (curZodiacIndex == 5)
            {
                curZodiacIndex = 0;
            }
            zodiac.GetComponentInChildren<TextMesh>().text = zodiacOptions[curZodiacIndex];
        }
    }
    void handleSumbit()
    {
        if (!moduleSolved)
        {
            Debug.LogFormat("[Chinese Zodiac #{0}] You submitted: {1},{2}", moduleID, year.GetComponentInChildren<TextMesh>().text, logZodiac(zodiac.GetComponentInChildren<TextMesh>().text));
            if (year.GetComponentInChildren<TextMesh>().text == yearOptions[correctYear] && zodiac.GetComponentInChildren<TextMesh>().text == zodiacOptions[correctZodiac])
            {
                module.HandlePass();
                moduleSolved = true;
                Debug.LogFormat("[Chinese Zodiac #{0}] That was correct.  Module solved.", moduleID);
                sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            }
            else
            {
                module.HandleStrike();
                Debug.LogFormat("[Chinese Zodiac #{0}] That was incorrect.  Strike!", moduleID);
                for (int i = 0; i < 5; i++) zodiacOptions[i] = "";
                Start();
            }
        }
    }
    string logZodiac(string zodiac)
    {
        string element = "";
        string animal = "";
        switch (zodiac[0])
        {
            case '木':
                element = "Wood";
                break;
            case '火':
                element = "Fire";
                break;
            case '土':
                element = "Earth";
                break;
            case '金':
                element = "Metal";
                break;
            case '水':
                element = "Water";
                break;
        }
        switch (zodiac[1])
        {
            case '鼠':
                animal = "Rat";
                break;
            case '牛':
                animal = "Ox";
                break;
            case '虎':
                animal = "Tiger";
                break;
            case '兔':
                animal = "Rabbit";
                break;
            case '龙':
                animal = "Dragon";
                break;
            case '蛇':
                animal = "Snake";
                break;
            case '马':
                animal = "Horse";
                break;
            case '羊':
                animal = "Sheep";
                break;
            case '猴':
                animal = "Monkey";
                break;
            case '鸡':
                animal = "Rooster";
                break;
            case '狗':
                animal = "Dog";
                break;
            case '猪':
                animal = "Pig";
                break;
        }
        return element + " " + animal;
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "Use '!{0} cycle year/zodiac' to cycle the year or or zodiac and e.g. '!{0} submit 1804 wood rat' to sumbit an answer.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        string[] commandArray = command.Split(' ');
        bool validZodiac = false;
        if (commandArray[0] == "cycle" && commandArray.Length == 2)
        {
            if (commandArray[1] == "year")
            {

                yield return null;
                yield return new WaitForSeconds(1.5f);
                year.OnInteract();
                yield return new WaitForSeconds(1.5f);
                year.OnInteract();
                yield return new WaitForSeconds(1.5f);
                year.OnInteract();
                yield return new WaitForSeconds(1.5f);
                year.OnInteract();
                yield return new WaitForSeconds(1.5f);
                year.OnInteract();
            }
            else if (commandArray[1] == "zodiac")
            {

                yield return null;
                yield return new WaitForSeconds(1.5f);
                zodiac.OnInteract();
                yield return new WaitForSeconds(1.5f);
                zodiac.OnInteract();
                yield return new WaitForSeconds(1.5f);
                zodiac.OnInteract();
                yield return new WaitForSeconds(1.5f);
                zodiac.OnInteract();
                yield return new WaitForSeconds(1.5f);
                zodiac.OnInteract();
            }
        }
        else if (commandArray[0] == "submit" && commandArray.Length == 4)
        {
            foreach (string zodiac in zodiacOptions)
            {
                if (logZodiac(zodiac).ToLowerInvariant() == commandArray[2] + " " + commandArray[3])
                {
                    validZodiac = true;
                }
            }

            if (!yearOptions.Contains(commandArray[1]) || !validZodiac)
            {
                yield return "sendtochaterror Invalid command.";
                yield break;
            }
            yield return null;
            do
            {
                yield return new WaitForSeconds(1f);
                zodiac.OnInteract();
            }
            while (logZodiac(zodiac.GetComponentInChildren<TextMesh>().text).ToLowerInvariant() != commandArray[2] + " " + commandArray[3]);
            do
            {
                yield return new WaitForSeconds(1f);
                year.OnInteract();
            }
            while (year.GetComponentInChildren<TextMesh>().text != commandArray[1]);
            sumbit.OnInteract();
        }
        else
        {
            yield return "sendtochaterror Invalid command.";
            yield break;
        }
    }
    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
        do
        {
            zodiac.OnInteract();
        }
        while (zodiac.GetComponentInChildren<TextMesh>().text != zodiacOptions[correctZodiac]);
        do
        {
            year.OnInteract();
        }
        while (year.GetComponentInChildren<TextMesh>().text != yearOptions[correctYear]);
        sumbit.OnInteract();
    }
}
