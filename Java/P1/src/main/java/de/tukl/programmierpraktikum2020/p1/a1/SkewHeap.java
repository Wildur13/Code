package de.tukl.programmierpraktikum2020.p1.a1;

        import java.util.Comparator;
        import java.util.NoSuchElementException;
        import java.util.function.UnaryOperator;

public class SkewHeap<E> implements PriorityQueue {
    Object [] heap;
    int heapsize;
    int maxHeapsize;
    Object [] ersatzheap;
    private UnaryOperator<E> f;
    Comparator<E> comparator;


    public int size() {
        return this.heapsize;
    }

    public void printHeap() {
        System.out.print("Heap = ");
        for (int i = 0; i < heapsize; i++)
            System.out.print(heap[i] + " ");
        System.out.println();

    }

    public SkewHeap(int x) {
        this.heap = new Object [x];
        this.maxHeapsize = x;
        this.heapsize = 0;

    }

    public boolean isEmpty() {
        return this.heapsize == 0;
    }

    @Override
    public boolean update(Object elem, Object updatedElem) {
        if (this.heapsize == 0) {
            return false;
        } else {
            boolean exists = false;
            for (int i = 0; i < heapsize; i++) {
                if (heap[i] == elem) {
                    heap[i] = updatedElem;
                    exists = true;
                    break;
                }
            }
            return exists;
        }
    }

    @Override
    public void map(UnaryOperator f){
        this.f = f;
        for (int i = 0; i < heapsize; i++) {
            heap[i] = f.apply((E) heap[i]);
        }

    }

    @Override
    public void insert(Object elem) {
        {
            if (this.heapsize == 0) {
                heap[0] = elem;
                heapsize++;
            } else {
                if (this.maxHeapsize == this.heapsize + 1) {
                    this.ersatzheap = new Integer[this.heapsize + 10];
                    for (int i = 0; i < this.heapsize; i++) {
                        this.ersatzheap[i] = this.heap[i];
                    }
                    this.heap = this.ersatzheap;
                }

                int pos = 0;
                while (this.heap[pos] != null) {
                    pos++;
                }
                this.heap[pos] = elem;
                heapsize++;
                heapSort();
            }
        }

    }

    public SkewHeap<E> transform(PriorityQueue otherQueue){
        SkewHeap<E> sHeap = new SkewHeap<>(10);
        if(otherQueue.isEmpty()){return sHeap;}
        else {
            while (!otherQueue.isEmpty()) {
                try {
                    sHeap.insert((E) otherQueue.deleteMax());
                } catch (Exception e) {
                    break;
                }
            }
            return sHeap;
        }
    }

    @Override
    public void merge(PriorityQueue otherQueue) {
        System.out.println("merge Start");
        SkewHeap<E> h1 = transform(otherQueue);
        h1.printHeap();
        if (!h1.isEmpty()) {
            for (int i = 0; i < h1.heapsize; i++) {
                insert(h1.heap[i]);
                heapSort();
            }
            System.out.println("Größe" + h1.heap.length);
            System.out.println("Größe" + h1.heapsize);
            h1.printHeap();
            printHeap();
            System.out.println("merge Ende");
        }

    }

    @Override
    public Object deleteMax() throws Exception {
        Object max;
        if (this.heapsize == 0) {
            throw new Exception("Heap hat keinen Inhalt");
        } else {
            max = heap[0];
            Object newnode = heap[this.heapsize - 1];
            heap[0] = newnode;
            heap[this.heapsize - 1] = null;
            this.heapsize--;
            heapSort();

        }
        return (E) max;
    }
    @Override
    public Object max() {
        if (isEmpty()) {
            throw new NoSuchElementException("kein Element im Heap");
        } else {
            return (E) this.heap[0];
        }
    }


    public void heapverifizieren(Object[] heap, int node, int heapsize) {
        int lheap = 2 * node + 1;
        int rheap = 2 * node + 2;
        int ersatznode = node;
        if (lheap <= heapsize && (comparator.compare((E)heap[lheap], (E)heap[node]) > 0)){
            ersatznode = lheap;
        }
        if (rheap <= heapsize && (comparator.compare((E)heap[rheap], (E)heap[node]) > 0)) {
            ersatznode = rheap;
        }

        if (ersatznode != node) {
            change(heap, node, ersatznode);
            heapverifizieren(heap, ersatznode, heapsize);
        }
    }

    public static void change(Object[] heap, int node, int ersatznode) {
        Object zwischenSpeicher = heap[node];
        heap[node] = heap[ersatznode];
        heap[ersatznode] = zwischenSpeicher;
    }

    public void heapSort() {
        for (int spoint = (this.heapsize / 2); spoint >= 0; spoint--) {
            heapverifizieren(this.heap, spoint, this.heapsize - 1);
        }
        for (int f = this.heapsize - 2; f > 0; f--) {
            change(this.heap, 0, f);
            heapverifizieren(this.heap, 0, f);


        }
    }

}