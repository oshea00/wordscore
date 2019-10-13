using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace wordscore
{
    public class SuperWordFinder
    {
        int maxScore = 0;
        public int MaxScore => maxScore;
        
        HashSet<string> maxSubs = new HashSet<string>();

        Dictionary<string, (string histStr, int letters)> dict;
        Dictionary<string, Histogram> histogramWords;
        Dictionary<int, SortedSet<(string histStr, int letters)>> histogramsByWordLength;
        List<string> maxSuperWords;

        public SuperWordFinder(string[] words)
        {
            dict = new Dictionary<string, (string histStr, int letters)>();
            histogramWords = new Dictionary<string, Histogram>();
            histogramsByWordLength = new Dictionary<int, SortedSet<(string histStr, int letters)>>();
            maxSuperWords = new List<string>();
            foreach (var w in words)
            {
                var key = Histogram.HistKey(w);
                dict.Add(w, key);
            }
            foreach (var word in dict.Keys)
            {
                var histStr = dict[word].histStr;
                if (!histogramWords.ContainsKey(histStr))
                    histogramWords.Add(histStr, new Histogram(dict[word]));
                histogramWords[histStr].Words.Add(word);
            }

            foreach (var histStr in histogramWords.Keys)
            {
                var wordLength = histogramWords[histStr].Words[0].Length;
                if (!histogramsByWordLength.ContainsKey(wordLength))
                    histogramsByWordLength.Add(wordLength,
                        new SortedSet<(string histStr, int letters)>(new DescOrder()));
                histogramsByWordLength[wordLength].Add((histStr, histogramWords[histStr].Letters));
            }

        }

        public List<string> FindSupers()
        {
            var maxLength = histogramsByWordLength.Keys.Max();
            var minLength = histogramsByWordLength.Keys.Min();

            for (int i = maxLength; i > minLength; i--)
            {
                var iterMax = 0;
                var superWords = new List<string>();
                var subs = new HashSet<string>();
                foreach (var top in histogramsByWordLength[i])
                {
                    var score = 0;
                    var pendSubs = new HashSet<string>();
                    for (int j = i - 1; j > minLength - 1; j--)
                    {
                        foreach (var sub in histogramsByWordLength[j])
                        {
                            if (Histogram.HistInHist(top, sub))
                            {
                                score += histogramWords[sub.histStr].Words.Count;
                                foreach (var w in histogramWords[sub.histStr].Words)
                                    pendSubs.Add(w);
                            }
                        }
                    }
                    if (score > iterMax)
                    {
                        superWords.Clear();
                        iterMax = score;
                        superWords.AddRange(histogramWords[top.histStr].Words);
                        subs = pendSubs;
                    }
                }

                if (iterMax < maxScore)
                {
                    i = minLength;
                    break;
                }
                else
                {
                    maxScore = iterMax;
                    maxSubs = subs;
                    maxSuperWords.Clear();
                    maxSuperWords.AddRange(superWords);
                }
            }
            return maxSuperWords;
        }

        public void ShowResults()
        {
            Console.WriteLine($"Max Score {maxScore}");
            Console.WriteLine($"Super Words:\n{string.Join(", ", maxSuperWords)}");
            Console.WriteLine("Subwords written to subwords.txt...");
            using (var sout = new StreamWriter($"subwords.txt", false))
            {
                sout.WriteLine($"Max Score {maxScore}");
                sout.WriteLine($"Super Words: {string.Join(", ", maxSuperWords)}\n\n");
                foreach (var word in maxSubs)
                {
                    sout.WriteLine(word);
                }
            }
        }


    }
}
