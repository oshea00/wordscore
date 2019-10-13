using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace wordscore
{
    class Program
    {
        static string ROOTDIR = @"c:\users\mike\repos\wordscore";
        static int maxScore = 0;
        static List<string> maxSuperWords = new List<string>();
        static HashSet<string> maxSubs = new HashSet<string>();

        static void Main(string[] args)
        {
            var dict = new Dictionary<string, (string histStr, int letters)>();
	        var words = File.ReadAllLines($"{ROOTDIR}\\words.txt");
            foreach (var w in words)
            {
                var key = Histogram.HistKey(w);
                dict.Add(w, key);
            }
	
            // Search
            FindSupers(dict);
        }

        static void FindSupers(Dictionary<string, (string histStr, int letters)> dict) {

            var histToWords = new Dictionary<string,Histogram>();
            var lenToHist = new Dictionary<int,SortedSet<(string histStr, int letters)>>();
            
            foreach (var word in dict.Keys) {
                var histStr = dict[word].histStr;
                if (!histToWords.ContainsKey(histStr))
                    histToWords.Add(histStr,new Histogram(dict[word]));
                histToWords[histStr].Words.Add(word);
            }
            
            foreach (var histStr in histToWords.Keys)
            {
                var wordLength = histToWords[histStr].Words[0].Length;
                if (!lenToHist.ContainsKey(wordLength))
                    lenToHist.Add(wordLength, 
                        new SortedSet<(string histStr, int letters)>(new DescOrder()));

                lenToHist[wordLength].Add((histStr,histToWords[histStr].Letters));
            }

            var maxLength = lenToHist.Keys.Max();
            var minLength = lenToHist.Keys.Min();

            for (int i = maxLength; i > minLength; i--)
            {
                var iterMax = 0;
                var superWords = new List<string>();
                var subs = new HashSet<string>();
                foreach (var top in lenToHist[i])
                {
                    var score = 0;
                    var pendSubs = new HashSet<string>();
                    for (int j = i - 1; j > minLength - 1; j--)
                    {
                        foreach (var sub in lenToHist[j])
                        {
                            if (Histogram.HistInHist(top,sub))
                            {
                                score += histToWords[sub.histStr].Words.Count;
                                foreach (var w in histToWords[sub.histStr].Words)
                                    pendSubs.Add(w);
                            }
                        }
                    }
                    if (score > iterMax)
                    {
                        superWords.Clear();
                        iterMax = score;
                        superWords.AddRange(histToWords[top.histStr].Words);
                        subs = pendSubs;
                    }
                }

                if (iterMax < maxScore) {
                    i = minLength;
                    break;
                } else {
                    maxScore = iterMax;
                    maxSubs = subs;
                    maxSuperWords.Clear();
                    maxSuperWords.AddRange(superWords);
                }			
            }

            ShowResults();
        }


        static void ShowResults() {
            Console.WriteLine($"Max Score {maxScore}");
            Console.WriteLine($"Super Words:\n{string.Join(", ",maxSuperWords)}");
            using (var sout = new StreamWriter($"{ROOTDIR}\\subwords.txt",false)) {
                sout.WriteLine($"Max Score {maxScore}");
                sout.WriteLine($"Super Words: {string.Join(", ",maxSuperWords)}\n\n");
                foreach (var word in maxSubs) {
                    sout.WriteLine(word);
                }
            }
        }
    }

    class Histogram
    {
        public string HistStr { get; set; }
        public int Letters { get; set; }
        public List<string> Words { get; set; }
        public Histogram((string histStr, int letters) hist)
        {
            HistStr = hist.histStr;
            Letters = hist.letters;
            Words = new List<String>();
        }

        public static (string histStr, int letters) HistKey(string word)
        {
            var counts = new int[26];
            int letters = 0;
            foreach (var c in word)
            {
                int idx = char.ToUpper(c) - 'A';
                letters |= (1 << idx);
                counts[idx]++;
            }
            return (string.Join("", counts), letters);
        }

        public static bool HistInHist((string histStr, int letters) keya,
                                    (string histStr, int letters) keyb)
        {
            if (keyb.letters != (keya.letters & keyb.letters))   // Improves compare by
                return false;                                    // approx 33%

            for (var i = 0; i < 26; i++)
            {
                if (keyb.histStr[i] > keya.histStr[i])
                    return false;
            }
            return true;
        }
    }

    class DescOrder : IComparer<(string histStr, int letters)>
    {
        public int Compare((string histStr, int letters) x, 
                        (string histStr, int letters) y) => y.histStr.CompareTo(x.histStr);
    }

}
