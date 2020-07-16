package de.tukl.programmierpraktikum2020.mp1;

import java.util.Comparator;

public class BibleAnalyzer {
    public static void countWords(Map<String, Integer> counts) {
        for (String word : Util.getBibleWords()) {
            if (counts.get(word) == null) {
                // Das Wort existiert noch nicht
                counts.put(word, 1);
            } else {
                // Das Wort existiert schon, Vorkommen wird um +1 erhöht
                counts.put(word, counts.get(word) + 1);
            }
        }
    }

    public static void main(String[] args) {
        // 1
        Map<String, Integer> map = new TreeMap<>(Comparator.<String>naturalOrder());
        countWords(map); // Map wird instanziiert und Wörter mit dem jeweiligen Vorkommen eingetragen

        // 2
        String[] keywords = new String[map.size()]; // Array wird erstellt mit der Anzahl der verschiedenen Wörtern (ohne Duplikate) -> Array.length = Anzahl Wörter
        map.keys(keywords); // Wörter werden eingetragen

        // 3
        sort(keywords, map); //sortiert nach Häufigkeit

        // 4
        for (String word : keywords) {
            System.out.println(map.get(word) + " " + word); // Ausgabe des sortierten Arrays 
        }
    }

    public static void sort(String[] words, Map<String, Integer> counts) {
        // Selection Sort
        for (int i = 0; i < words.length - 1; i++) {
            for (int j = i + 1; j < words.length; j++) {
                if (counts.get(words[i]) >= counts.get(words[j])) {
                    String temporalWord = words[i];
                    words[i] = words[j];
                    words[j] = temporalWord;
                }
            }
        }
    }
}
