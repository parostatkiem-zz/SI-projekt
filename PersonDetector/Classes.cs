using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

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
        public static UserData userData = new UserData();
        public static int SPEECH_AMOUNT = 4;
        public static int DEBUG_REFRESH_INTERVAL = 200;
        // public Directory saveFilePath;
    }

    public static class WritingAnalytics
    {
        public static bool isWordTypedNow = false;
        private static Stopwatch singleWordTime = new Stopwatch();
        private static int textLenAtWordStart = 0;
        private static int totalWordsCount = 0;
        public static void AnalizeReadyText( SingleInput input, string text)
        {
            if(input==null) return;
            var textLength = Convert.ToDouble(text.Length);
            input.newLinesPerText = Convert.ToDouble( text.Count(f => f == '\n'))/ textLength ;

            if (!input.polishChars &&( text.Contains('ą') || text.Contains('ć') || text.Contains('ż') || text.Contains('ź') || text.Contains('ó') || text.Contains('ł') || text.Contains('ś')))
                input.polishChars = true;

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


        }
    }






}
