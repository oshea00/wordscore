# Superwords Kata

Take a dictionary of words as input and list "super words" in the dictionary.

For every subset of letters in a word, and their permutations (anagrams)
count a point whenever one of these permutations matches another word
in the dictionary. The word(s) that yields the highest score are "super words"

## Approach

This program takes an approach of searching based on a couple observations.

* Anagrams have the same histogram of letter counts.
* An anagram of a subset of letters will contain a subset of the letters and counts.

For example:
To simplify, lets use only a 5 letter alphabet: ABCDE 
```
                                  ABCDE                  ABCDE
DEAD would have a "histogram" of "10021" - letters used "10011" 
DAD  would have a "histogram" of "10020" - letters used "10010"
```
In each position DAD has <= the same letter counts - so it is an
anagram of a subset of the larger word's letters.

Also note that DEAD "letters" used as binary number, and
DAD, would have  (DEAD.letters & DAD.letters) == DAD.letters only
if DAD has no letters *not* in DEAD.

This, and a few other tricks, allows a more efficient search of the
potential space compared to a pure brute force search

Brute force using the large dictionary in this project can take up to
4 hours. The more efficient search takes about 2 minutes on 
a 2.7GHZ I7 Laptop.

## Full Run Results

Using the `words.txt` in the `wordscore` project folder the results are...
(see actual words `subwords.txt` in same folder)

```
Max Score 7924
Super Words: OVERSPECULATING

INOPERCULATES
INOPERCULATE
REINOCULATES
COUNTERPLEAS
COUNTERVAILS
...
```

## Comments

It's interesting to note the following:

The answer would tend to be the histogram with largest number of common buckets (letters) with other words in dictionary with largest sum of buckets (word length).
	
For english words - should contain as many vowels as possible and the most common consonants - should match a high number of other english words.

* OVERSPECULATING has all vowels (aeiou)
* Has some of most most common consonants: R T N S P

FYI most frequent english letters: E A R I O T N S L C U D P





