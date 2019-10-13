# Superwords Kata

Take a dictionary of words as input and list "super words" in the dictionary.

For every subset of letters in a word, and their permutations (anagrams)
count a point whenever one of these permutations matches another word
in the dictionary. The word(s) that yields the highest score are "super words"

## Approach

This program takes an approach of searching based on a couple observations.

* Anagrams have the same histogram of letter counts.
* An anagram of a subset of letters will contain a subset of the letters.

For example:
To simplify, lets use only a 5 letter alphabet: ABCDE 
```
                                  ABCDE
DEAD would have a "histogram" of "10021" - letters used "10011" 
DAD  would have a "histogram" of "10020" - letters used "10010"
```
In each position DAD has <= the same letter counts - so it is an
anagram of a subset of the larger word's letters.

Also note that DEAD "letters" used as binary number, and
DAD, would have  (DEAD.letters & DAD.letters) == DAD.letters only
if DAD has no letters *not* in DEAD.

This, and a few other tricks, allows a more efficient search of the
potential space compareds to a pure brute force.

Brute force usn the large dictionary in this project can take up to
4 hours. The more efficient search takes about 2 minutes on 
a 2.7GHZ I7 Laptop.


