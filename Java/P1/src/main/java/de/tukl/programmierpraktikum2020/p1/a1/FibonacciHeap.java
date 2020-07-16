package de.tukl.programmierpraktikum2020.p1.a1;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.function.UnaryOperator;

public class FibonacciHeap<E> implements PriorityQueue<E> {
    Comparator<E> comparator;

    private FibonacciHeapNode<E> maxNode;
    private int numberOfNodes;
    private List<E> priorityList = new ArrayList<>(); // save all priorities helpful for the methods update() and map()

    public FibonacciHeap(Comparator<E> comparator) {
        this.comparator = comparator;
    }

    @Override
    public void insert(E elem) {
        insertHelper(new FibonacciHeapNode<>(elem));
        priorityList.add(elem);
    }

    //Insert a new node in the heap
    public void insertHelper(FibonacciHeapNode<E> node) {

        //check if max node is not null
        if (maxNode != null) {

            //add to the right of max node
            node.left = maxNode;
            node.right = maxNode.right;
            maxNode.right = node;

            //check if node right is not null
            if (node.right != null) {
                node.right.left = node;
            }
            if (node.right == null) {
                node.right = maxNode;
                maxNode.left = node;
            }
            // Check if the new element greater then the max node
            if (comparator.compare(node.priority, maxNode.priority) > 0) {
                maxNode = node;
            }
        } else {
            maxNode = node;
        }

        numberOfNodes++;
    }

    @Override
<<<<<<< HEAD
    public void merge(PriorityQueue<E> otherQueue) {
=======
    public void merge(PriorityQueue<E> otherQueue) throws Exception {
>>>>>>> origin/master
        while (!otherQueue.isEmpty()) {
            this.insert(otherQueue.deleteMax());
        }
    }

    @Override
    public E deleteMax() {
        return deleteMaxHelper();
    }

    //Removes the maximum from the heap
    public E deleteMaxHelper() {
        FibonacciHeapNode<E> maxNode = this.maxNode;
        if (maxNode != null) {
            priorityList.remove(this.maxNode.priority);
            int numberOfChildren = maxNode.degree;
            FibonacciHeapNode<E> childOfMaxNode = maxNode.child;
            FibonacciHeapNode<E> tempRight;

            //while  there are children of max
            while (numberOfChildren > 0) {
                tempRight = childOfMaxNode.right;

                // remove childOfMaxNode from child list
                childOfMaxNode.left.right = childOfMaxNode.right;
                childOfMaxNode.right.left = childOfMaxNode.left;

                // add childOfMaxNode to root list of heap
                childOfMaxNode.left = this.maxNode;
                childOfMaxNode.right = this.maxNode.right;
                this.maxNode.right = childOfMaxNode;
                childOfMaxNode.right.left = childOfMaxNode;

                // set parent to null
                childOfMaxNode.parent = null;
                childOfMaxNode = tempRight;
                //decrease number of children of max
                numberOfChildren--;

            }

            // remove maxNode from root list of heap
            maxNode.left.right = maxNode.right;
            maxNode.right.left = maxNode.left;

            if (maxNode == maxNode.right) {
                this.maxNode = null;

            } else {
                this.maxNode = maxNode.right;
                degreewiseMerge();
            }
            numberOfNodes--;
            return maxNode.priority;
        }
        return null;
    }

    //performs degree wise merge(if 2 degrees are same then it merges it)
    public void degreewiseMerge() {
        //chosen at random, read on internet that 45 is most optimised,
        // else can be calculated using the formulae given in cormen
        int sizeofDegreeTable = numberOfNodes;

        List<FibonacciHeapNode<E>> degreeTable =
                new ArrayList<>(sizeofDegreeTable);

        // Initialize degree table
        for (int i = 0; i < sizeofDegreeTable; i++) {
            degreeTable.add(null);
        }

        // Find the number of root nodes.
        int numRoots = 0;
        FibonacciHeapNode<E> x = maxNode;


        if (x != null) {
            numRoots++;
            x = x.right;

            while (x != maxNode) {
                numRoots++;
                x = x.right;
            }
        }

        // For each node in root list
        while (numRoots > 0) {

            int degree = x.degree;
            FibonacciHeapNode<E> next = x.right;

            // check if the degree is there in degree table, if not add,if yes then combine and merge
            while (true) {
                FibonacciHeapNode<E> y = degreeTable.get(degree);
                if (y == null) {
                    break;
                }

                //Check whos key value is greater
                if (comparator.compare(x.priority, y.priority) < 0) {
                    FibonacciHeapNode<E> temp = y;
                    y = x;
                    x = temp;
                }

                //make y the child of x as x key value is greater
                makeChild(y, x);

                //set the degree to null as x and y are combined now
                degreeTable.set(degree, null);
                degree++;
            }

            //store the new x(x+y) in the respective degree table postion
            degreeTable.set(degree, x);

            // Move forward through list.
            x = next;
            numRoots--;
        }


        //Deleting the max node
        maxNode = null;

        // combine entries of the degree table
        for (int i = 0; i < sizeofDegreeTable; i++) {
            FibonacciHeapNode<E> y = degreeTable.get(i);
            if (y == null) {
                continue;
            }

            //till max node is not null
            if (maxNode != null) {

                // First remove node from root list.
                y.left.right = y.right;
                y.right.left = y.left;

                // Now add to root list, again.
                y.left = maxNode;
                y.right = maxNode.right;
                maxNode.right = y;
                y.right.left = y;

                // Check if this is a new maximum
                if (comparator.compare(y.priority, maxNode.priority) > 0) {
                    maxNode = y;
                }
            } else {
                maxNode = y;
            }
        }
    }

    //Makes y the child of node x
    public void makeChild(FibonacciHeapNode<E> y, FibonacciHeapNode<E> x) {
        // remove y from root list of heap
        y.left.right = y.right;
        y.right.left = y.left;

        // make y a child of x
        y.parent = x;

        if (x.child == null) {
            x.child = y;
            y.right = y;
            y.left = y;
        } else {
            y.left = x.child;
            y.right = x.child.right;
            x.child.right = y;
            y.right.left = y;
        }

        // increase degree of x by 1
        x.degree++;

        // make mark of y as false
        y.mark = false;
    }

    @Override
    public E max() {
        if (maxNode != null) return maxNode.priority;
        return null;
    }

    @Override
    public boolean isEmpty() {
        return maxNode == null;
    }

    @Override
    public boolean update(E elem, E updatedElem) {
        if (priorityList.contains(elem)) {
            List<E> oldPriorityList = priorityList;
            priorityList = new ArrayList<>();
            maxNode = null;
            for (E priority : oldPriorityList) {
                if (comparator.compare(priority, elem) == 0) {
                    this.insert(updatedElem);
                } else this.insert(priority);
            }
            return true;
        }
        return false;
    }

    @Override
    public void map(UnaryOperator<E> f) {
        priorityList.replaceAll(f);
        List<E> oldPriorityList = priorityList;
        priorityList = new ArrayList<>();
        maxNode = null;
        for (E elem : oldPriorityList) this.insert(elem);
    }
}
<<<<<<< HEAD
=======

>>>>>>> origin/master
