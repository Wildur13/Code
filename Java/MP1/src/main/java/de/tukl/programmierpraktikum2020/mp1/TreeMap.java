package de.tukl.programmierpraktikum2020.mp1;

import java.util.Comparator;


public class TreeMap<K, V> implements Map<K, V> {
    Node<K, V> root;
    Comparator<K> comparator;
    int size;

    public TreeMap(Comparator<K> comparator) {
        this.comparator = comparator;
        this.root = null;
        this.size = 0;
    }

    @Override
    public V get(K key) {
        if (root != null) {
            Node<K, V> aktuelNode = root;
            while (aktuelNode != null) {
                int comp = comparator.compare(key, aktuelNode.getKey());
                if (comp < 0) aktuelNode = aktuelNode.getLeft();
                else if (comp > 0) aktuelNode = aktuelNode.getRight();
                else return aktuelNode.getValue();
            }
        }
        return null;
    }

    @Override
    public void put(K key, V value) {
        Node<K, V> newNode = new Node<>(key, value);
        if (root == null) {
            root = newNode;
            size++;
            return;
        }
        Node<K, V> currNode = root;
        Node<K, V> lastNode = null;
        boolean lastNodeLeft = false;
        while (currNode != null) {
            lastNode = currNode;
            int comp = comparator.compare(key, currNode.getKey());
            if (comp < 0) {
                currNode = currNode.getLeft();
                lastNodeLeft = true;
            } else if (comp > 0) {
                currNode = currNode.getRight();
                lastNodeLeft = false;
            } else {
                currNode.setValue(value);
                return;
            }
        }
        size++;
        if (lastNodeLeft) {
            lastNode.setLeft(newNode);
        } else {
            lastNode.setRight(newNode);
        }
    }

    @Override
    public void remove(K key) {
        throw new UnsupportedOperationException();
    }

    @Override
    public int size() {
        return this.size;
    }

    @Override
    public void keys(K[] array) {
        if (array == null || array.length < size) {
            throw new IllegalArgumentException("Array ist leer oder Länge klainer als Size of TreeMap");
        } else {
            inorder(root, array);
        }
    }

    private void inorder(Node<K, V> node, K[] array) {
        if (node != null) {
            inorder(node.getLeft(), array);
            insert(node.getKey(), array);
            inorder(node.getRight(), array);
        }
    }

    private void insert(K key, K[] array) {
        int i = 0;
        while (i < array.length) {
            if (array[i] == null) {
                array[i] = key;
                return;
            }
            i++;
        }
    }
}
