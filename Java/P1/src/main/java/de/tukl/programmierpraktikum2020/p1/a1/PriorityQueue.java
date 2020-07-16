package de.tukl.programmierpraktikum2020.p1.a1;

import java.util.function.UnaryOperator;

public interface PriorityQueue<E> {
    void insert(E elem);
    void merge(PriorityQueue<E> otherQueue);
    E deleteMax();
    E max();
    boolean isEmpty();
    boolean update(E elem, E updatedElem);
    void map(UnaryOperator<E> f);
}
