package de.tukl.programmierpraktikum2020.p1.a1;

import java.util.Comparator;
import java.util.LinkedList;
import java.util.function.UnaryOperator;

public class ListQueue<E> implements PriorityQueue<E> {
    Comparator<E> comparator;

    LinkedList<E> queueList = new LinkedList<>();

    public ListQueue(Comparator<E> comparator) {
        this.comparator = comparator;
    }

    @Override
    public void insert(E elem) {
        queueList.addLast(elem);
        queueList.sort(comparator);
    }

    @Override
    public void merge(PriorityQueue<E> otherQueue) {
        // Methode 1
        while (!otherQueue.isEmpty()) {
            this.insert(otherQueue.deleteMax());
        }

        // Methode 2
        /*LinkedList<E> otherQueueList = ((ListQueue<E>) otherQueue).queueList;
        for (E elem : otherQueueList) {
            this.insert(elem);
        }*/
    }

    /*public void printElement() {
        int i = 0;
        while (i < queueList.size()) {
            System.out.print(queueList.get(i) + "->");
            i++;
        }
    }*/

    @Override
    public E deleteMax() {
        if (this.isEmpty()) {
            return null;
        }
        return queueList.removeLast();
    }

    @Override
    public E max() {
        if (this.isEmpty()) {
            return null;
        }
        return queueList.getLast();
    }

    @Override
    public boolean isEmpty() {
        return queueList.isEmpty();
    }

    @Override
    public boolean update(E elem, E updatedElem) {
        // Methode 1
        /*if (queueList.remove(elem)) {
            this.insert(updatedElem);
            return true;
        }*/

        // Methode 2 ohne for-each Schleife
        for (int i = 0; i < queueList.size(); i++) {
            if (comparator.compare(queueList.get(i), elem) == 0) {
                queueList.remove(i); // oder elem beide sind gleich
                this.insert(updatedElem);
                return true;
            }
        }

        // Methode 2
        /*for (E element : queueList) {
            if (comparator.compare(element, elem) == 0) {
                queueList.remove(element); // oder elem beide sind gleich
                this.insert(updatedElem);
                return true;
            }
        }*/
        return false;
    }

    @Override
    public void map(UnaryOperator<E> f) {
        LinkedList<E> safeQueueList = queueList;
        queueList = new LinkedList<>();
        for (E elem : safeQueueList) {
            this.insert(f.apply(elem));
        }
    }
}
