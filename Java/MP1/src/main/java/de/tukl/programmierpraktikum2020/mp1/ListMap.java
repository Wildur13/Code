package de.tukl.programmierpraktikum2020.mp1;

public class ListMap<K, V> implements Map<K, V> {
    ListenElement<K, V> head = null;

    @Override
    public V get(K key) {
        V ausgabe = null;

        for (ListenElement<K, V> kopf = head; kopf != null; kopf = kopf.next) {
            if (kopf.key.equals(key)) {
                ausgabe = kopf.value;
                break;
            }
        }
        return ausgabe;
    }

    @Override
    public void put(K key, V value) {
        ListenElement<K, V> n1 = new ListenElement<>(key, value);
        if (head == null) {
            head = n1;
            return;
        }
        for (ListenElement<K, V> kopf = head; kopf != null; kopf = kopf.next) {
            if (kopf.key.equals(key)) {
                kopf.value = value;
                return;
            }
        }
        n1.next = head;
        head = n1;
    }

    @Override
    public void remove(K key) {
        if (head != null) {
            ListenElement<K, V> kopf = this.head;
            if (kopf.key.equals(key)) {
                head = kopf.next;
            } else {
                ListenElement<K, V> pioneer = null;
                while (kopf.next != null) {
                    pioneer = kopf;
                    if (kopf.next.key.equals(key)) {
                        pioneer.next = kopf.next.next;
                        break;
                    }
                    kopf = kopf.next;
                }
            }
        }
    }

    @Override
    public int size() {
        int anzahl = 0;
        for (ListenElement<K, V> kopf = head; kopf != null; kopf = kopf.next) {
            anzahl++;
        }
        return anzahl;
    }

    @Override
    public void keys(K[] array) {
        if (array == null || array.length < size()) {
            throw new IllegalArgumentException("");
        }
        int anzahl = 0;
        for (ListenElement<K, V> kopf = head; kopf != null; kopf = kopf.next) {
            array[anzahl] = kopf.key;
            anzahl++;
        }
    }
}