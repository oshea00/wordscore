namespace wordscore {
    class WordHist {
        public static int[] HistKey(string word)
        {
            var counts = new int[26];
            foreach (var c in word)
                counts[char.ToUpper(c) - 'A']++;
            return counts;
        }
        
        public static bool WordInWord(string word, string subword) {
            var ahist = WordHist.HistKey(word);
            var bhist = WordHist.HistKey(subword);
            for (var i=0; i < 26; i++) {
                if (bhist[i]>ahist[i])
                    return false;
            }
            return true;
        }

        public static bool HistInHist(int[] ahist, int[] bhist)
        {
            for (var i = 0; i < 26; i++)
            {
                if (bhist[i] > ahist[i])
                    return false;
            }
            return true;
        }
    }

}
