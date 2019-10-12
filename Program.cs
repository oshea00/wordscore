using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace wordscore
{
    class Program
    {
        static void Main(string[] args)
        {
            var maxScore = 0;
            var superwords = new List<string>();
            var dict = new Dictionary<string, int[]>();
            var allwords = new List<string> {
                "ALLY","SD","SOUND","NOUS","YA","LA","AL","AY","ALL","LAY"
            };
            var words = allwords
                        .Select(w => string.Join("", w.OrderBy(c => c)))
                        //.OrderBy(a => a);
                        .OrderByDescending(a => a.Length);
            foreach (var w in words)
            {
                
            }
        //	var words = File.ReadAllLines(@"c:\repos\wordscore\words.txt")
        //		.OrderByDescending(f => f.Length)
        //		.ThenByDescending(f => f);
        //	foreach (var w in words)
        //		dict.Add(w, WordHist.HistKey(w));
        //	var keys = dict.Keys.ToArray();
        //	for (int i = 0; i < keys.Length; i++) {
        //		var score=0;
        //		for (int j = i + 1; j < keys.Length; j++) {
        //			if (WordHist.HistInHist(dict[keys[i]],dict[keys[j]])) {
        //				score++;
        //			}
        //		}
        //		if (score > maxScore) {
        //			superwords.Clear();
        //			maxScore = score;
        //			superwords.Add(keys[i]);
        //		} else if (score == maxScore) {
        //			superwords.Add(keys[i]);
        //		}
        //	}
            
            Console.WriteLine(string.Join("\n",words));        }
    }
}
