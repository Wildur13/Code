package de.tukl.programmierpraktikum2020.p1.a1;

public class SkewHeapNode<E> {
    E priority;
    SkewHeapNode<E> leftSkewHeapNode;
    SkewHeapNode<E> rightSkewHeapNode;

    public SkewHeapNode(E priority) {
        this.priority = priority;
        this.leftSkewHeapNode = null;
        this.rightSkewHeapNode = null;
    }
}
