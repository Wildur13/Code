package de.tukl.programmierpraktikum2020.mp1;

import java.util.Comparator;

public class Node<K, V> implements Comparator<K> {
    private Node<K, V> left;
    private Node<K, V> right;
    private K key;
    private V value;

    public Node(K key, V value) {
        this.key = key;
        this.value = value;
        this.left = null;
        this.right = null;
    }

    public K getKey() {
        return key;
    }

    public V getValue() {
        return value;
    }

    public void setValue(V value) {
        this.value = value;
    }

    public Node<K, V> getLeft() {
        return left;
    }

    public void setLeft(Node<K, V> left) {
        this.left = left;
    }

    public Node<K, V> getRight() {
        return right;
    }

    public void setRight(Node<K, V> right) {
        this.right = right;
    }

    @Override
    public int compare(K o1, K o2) {
        return compare(o1, o2);
    }
}
