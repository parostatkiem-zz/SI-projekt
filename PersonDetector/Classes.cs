using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

using Newtonsoft.Json;

namespace PersonDetector
{


    public class SingleInput
    {
        public double newLinesPerText; //ilosc enterów na długość tekstu
        public double spacesAfterPunctuation; //spacje po znakach przestankowych
        public double spacesBeforePunctuation; //spacje przed znakami przestankowymi
        public bool polishChars; //uzycie polskich znakow
        public double avgLetterTime; //średni czas pisania jednej litery w słowie
        public double avgCapitalLetterTime; //średni czas wciśnięcia klawisza z literką po SHIFT
    }

    public class UserData
    {
        public List<SingleInput> inputs;
        public string userName;

        public UserData(string _userName = "")
        {
            userName = _userName;
            inputs = new List<SingleInput>();
        }
    }

    public static class Config
    {
        public static List<UserData> allUsersData = new List<UserData>();
        public static UserData currentUserData = new UserData();
        public static int SPEECH_AMOUNT = 2;
        public static int DEBUG_REFRESH_INTERVAL = 200;
        public static bool IS_DEBUG_ENABLED = false;
        public static DirectoryInfo saveFileDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\"); //pulpit
        public static string SavePath
        {
            get
            {
                return saveFileDir + currentUserData.userName + ".json";
            }

        }

        public static int parsedFiles = 0;
        public static int unParsedFiles = 0;
    }

    public static class WritingAnalytics
    {
        public static bool isWordTypedNow = false;
        private static Stopwatch singleWordTime = new Stopwatch();
        private static int textLenAtWordStart = 0;
        private static int totalWordsCount = 0;
        private static int totalShiftUsed = 0;
        private static Stopwatch shiftDownTime = new Stopwatch();
        public static void AnalizeReadyText( SingleInput input, string text)
        {
            if(input==null) return;
            var textLength = Convert.ToDouble(text.Length);

            if (textLength > 0)
                input.newLinesPerText = Convert.ToDouble(text.Count(f => f == '\n')) / textLength;
            else
                input.newLinesPerText = 0;

            input.polishChars = Regex.Match(text, "[ĄąĆćŻżŹźÓóŁłŚś]").Success;


            input.spacesBeforePunctuation = Regex.Matches(text, " [.,?!:;]").Count;
            input.spacesAfterPunctuation = Regex.Matches(text, "[.,?!:;] ").Count;
        }
        public static void AnalizeFreshInput(SingleInput input, string text)
        {
            if (text.Length <= 2) return;

            char lastChar = text[text.Length - 1];
            char secondLastChar = text[text.Length - 2];
            char[] lastChars = { secondLastChar, lastChar };
            string sLastChars = new string(lastChars);

            if(!isWordTypedNow && Regex.Matches(sLastChars, "[.,?!:;) ][A-z]").Count==1 )
            {
                //poczatek wyrazu
                isWordTypedNow = true;
                singleWordTime.Restart();
                textLenAtWordStart = text.Length;
            }
            if (isWordTypedNow && Regex.Matches(sLastChars, "[A-z][\r.,?!:;( ]").Count == 1)
            {
                //koniec wyrazu
                double output;
                isWordTypedNow = false;
                singleWordTime.Stop();
                if (text.Length - textLenAtWordStart <= 2) return; //pomijamy 1 oraz 2 literowe slowa
                
               
                if(input.avgLetterTime!=0)
                 output = ((singleWordTime.ElapsedMilliseconds / (text.Length - textLenAtWordStart))+input.avgLetterTime*totalWordsCount)/(totalWordsCount+2) ;
                else
                   output = singleWordTime.ElapsedMilliseconds / (text.Length - textLenAtWordStart);
                if(output>0) //ujemna wartosc = BŁĄD (np. przez usuniecie wyrazu backspace)
                {
                    input.avgLetterTime = output;
                    totalWordsCount++;
                }
            }

            if(shiftDownTime.IsRunning && Regex.Matches(lastChar.ToString(), "[A-Z!@#$%^&*()]").Count == 1)
            {
                shiftDownTime.Stop();
                double output;
                if (input.avgCapitalLetterTime != 0)
                    output = (shiftDownTime.ElapsedMilliseconds + input.avgCapitalLetterTime * totalShiftUsed) / (totalShiftUsed + 2);
                else
                    output = shiftDownTime.ElapsedMilliseconds;
                if (output > 0) //ujemna wartosc = BŁĄD (np. przez usuniecie wyrazu backspace)
                {
                    input.avgCapitalLetterTime = output;
                    totalShiftUsed++;
                }
            }
        }

        public static void ShiftPressed(bool isDown=true)
        {
            shiftDownTime.Restart();
        }
    }

    public static class DataAnalytics
    {
        public static bool ParseSingleFile(FileInfo file)
        {
            try
            {
                UserData data = JsonConvert.DeserializeObject<UserData>(File.ReadAllText(file.FullName));
                Config.allUsersData.Add(data);
                return true;
            }
            catch { return false; }

        }
    }

    public static class IOoperations
    {
        
        public static bool SerializeToJson()
        {
            try
            {
              using (StreamWriter sw = new StreamWriter(Config.SavePath))
                { 
                    sw.Write(JsonConvert.SerializeObject(Config.currentUserData));
                }

            }
            catch { return false; }
            return true;
        }
        public static bool ReadFilesFrom(string path)
        {
            if (!System.IO.Directory.Exists(path)) return false;
            
            DirectoryInfo d = new DirectoryInfo(path);

            foreach (var file in d.GetFiles("*.json"))
            {
                DataAnalytics.ParseSingleFile(file);
            }
            return true;
        }
    }






}
