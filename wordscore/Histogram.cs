using System;
using System.Collections.Generic;
using System.Text;

namespace wordscore
{
    public class Histogram
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
}
