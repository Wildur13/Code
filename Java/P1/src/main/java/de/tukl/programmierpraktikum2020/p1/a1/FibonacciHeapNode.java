package de.tukl.programmierpraktikum2020.p1.a1;

public class FibonacciHeapNode<E> {
    FibonacciHeapNode<E> left, right, child, parent;
    int degree; // always 0 at the beginning
    boolean mark = false;
    E priority;

    public FibonacciHeapNode(E priority) {
        this.left = this; // a node itself
        this.right = this; // a node itself
        this.parent = null;
        this.degree = 0;
        this.priority = priority;
    }
}
