package de.tukl.programmierpraktikum2020.mp1;

public class ListenElement<K, V> {
    K key;
    V value;
    ListenElement<K, V> next;

    public ListenElement(K k, V v) {
        this.key = k;
        this.value = v;
        this.next = null;
    }
}
