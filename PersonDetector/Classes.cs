using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
    }






}
