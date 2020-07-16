package de.tukl.programmierpraktikum2020.p1.a1;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.Comparator;
import java.util.LinkedList;
import java.util.List;
import java.util.Random;
import java.util.function.UnaryOperator;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.*;

public class PriorityQueueTest <T> {
    /**
     * Diese Methode wird verwendet, um Instanzen von PriorityQueue Implementierungen an Testmethoden zu übergeben.
     */
    public static List<PriorityQueue<Integer>> getPriorityQueueInstances() {
        List<PriorityQueue<Integer>> implementations = new LinkedList<>();
        // Um Compilefehler zu verhindern, sind die Instanziierungen der PriorityQueue Implementierungen auskommentiert.
        // Kommentieren Sie die Zeilen ein, sobald Sie die entsprechenden Klassen implementiert haben.
   //     implementations.add(new ListQueue<>(Comparator.<Integer>naturalOrder()));
    //    implementations.add(new SkewHeap<>(Comparator.<Integer>naturalOrder()));
    //    implementations.add(new FibonacciHeap<>(Comparator.<Integer>naturalOrder()));
        return implementations;
    }

    @ParameterizedTest
    @MethodSource("getPriorityQueueInstances")
    public void priorityQueueBeispiel(PriorityQueue<Integer> queue) {
        System.out.println("Teste priorityQueueBeispiel mit " + queue.getClass().getSimpleName());

        // Test: eine frisch initialisierte Queue ist leer
        assertTrue(queue.isEmpty());

        // Fügen Sie hier weitere Tests ein.
        // Sie dürfen auch gerne weitere Test-Methoden erstellen, z.B. priorityQueueBeispiel2 usw.
    }

    @ParameterizedTest
    @MethodSource("isEmptyTestData")
    public void isEmpty(PriorityQueue<Integer> queue, boolean empty) {
        assertEquals(empty, queue.isEmpty());


    }

    private static Stream<Arguments> isEmptyTestData() throws Exception {
        ListQueue<Integer> qlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        qlist.insert(8);
        ListQueue<Integer> plist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(8);
        plist.deleteMax();
        plist.insert(10);
        ListQueue<Integer> xlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        xlist.insert(8);
        xlist.deleteMax();
        xlist.insert(10);
        xlist.deleteMax();


        return Stream.of(
                Arguments.of(qlist, false),
                Arguments.of(xlist, true),
                Arguments.of(plist, false));
    }


    @ParameterizedTest
    @MethodSource("insertData")
    public void insertTest(Integer listmax, Integer max) throws Exception {


        assertEquals(listmax, max);


    }

    private static Stream<Arguments> insertData() throws Exception {
        ListQueue<Integer> plist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(45);
        plist.insert(5);
        plist.insert(450);
        return Stream.of(
                Arguments.of(plist.max(), 450));
    }
    @ParameterizedTest
    @MethodSource("mergeData")
    public void mergeTest(PriorityQueue<Integer> queue1, PriorityQueue<Integer> queue2, Integer max) throws Exception {

        // assertEquals(queue1, queue2);
        assertEquals(max, queue2.max());


    }

    private static Stream<Arguments> mergeData() throws Exception {
        ListQueue<Integer> qlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        qlist.insert(8);
        ListQueue<Integer> plist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(8);
        plist.deleteMax();
        plist.insert(10);
        ListQueue<Integer> xlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        xlist.insert(99);
        xlist.deleteMax();
        xlist.insert(100);
        xlist.deleteMax();
        xlist.merge(qlist);
        xlist.merge(plist);
        ListQueue<Integer> zlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        zlist.insert(8);
        zlist.insert(10);

        return Stream.of(
                Arguments.of(xlist, zlist, 10));
    }

    @ParameterizedTest
    @MethodSource("deletemaxData")
    public void dmaxTest(PriorityQueue<Integer> queue, T max, boolean empty) throws Exception {

        assertEquals(max, queue.max());
        assertEquals(empty, queue.isEmpty());


    }

    private static Stream<Arguments> deletemaxData() throws Exception {
        ListQueue<Integer> plist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(8);
        plist.insert(18);
        plist.deleteMax();
        ListQueue<Integer> qlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(8);
        plist.deleteMax();
        return Stream.of(
                Arguments.of(plist, 8, false),
                Arguments.of(qlist, null, true));
    }

    @ParameterizedTest
    @MethodSource("maxData")
    public void maxTest(PriorityQueue<Integer> queue, T max) throws Exception {

        assertEquals(max, queue.max());


    }

    private static Stream<Arguments> maxData() throws Exception {
        ListQueue<Integer> plist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(8);
        plist.insert(18);
        plist.deleteMax();
        ListQueue<Integer> qlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        qlist.insert(8);
        qlist.insert(18);
        return Stream.of(
                Arguments.of(plist, 8),
                Arguments.of(qlist, 18)
                );
    }

    @ParameterizedTest
    @MethodSource("mapData")
    public void mapTest(PriorityQueue<Integer> queue1, PriorityQueue<Integer> queue2,T max) throws Exception {

        assertEquals(max, queue1.max());
        assertEquals(max, queue2.max());
       // assertEquals(queue1,queue2);


    }

    private static Stream<Arguments> mapData() {
        ListQueue<Integer> plist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(45);
        plist.insert(5);
        plist.insert(450);
        UnaryOperator<Integer> f = new UnaryOperator<Integer>() {
            @Override
            public Integer apply(Integer integer) {
                return integer * integer;
            }
        };
        plist.map(f);
        ListQueue<Integer> qlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        qlist.insert(2025);
        qlist.insert(25);
        qlist.insert(202500);
        return Stream.of(
                Arguments.of(plist, qlist, 202500));
    }

    @ParameterizedTest
    @MethodSource("updateData")
    public void updateTest(PriorityQueue<Integer> queue1, PriorityQueue<Integer> queue2,T max) throws Exception {

        assertEquals(max, queue1.max());
        assertEquals(max, queue2.max());
        assertEquals(queue1.deleteMax(), queue2.deleteMax());
        //assertEquals(queue1, queue2); -> false obwohl q1 = 90 8 0 und q2 = 90 8 0


    }

    private static Stream<Arguments> updateData() throws Exception {
        ListQueue<Integer> plist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        plist.insert(8);
        plist.insert(18);
        plist.deleteMax();
        plist.insert(0);
        plist.insert(77);
        plist.update(77, 90);
        ListQueue<Integer> qlist = new ListQueue<>(Comparator.<Integer>naturalOrder());
        qlist.insert(8);
        qlist.insert(0);
        qlist.insert(90);
        return Stream.of(
                Arguments.of(plist, qlist, 90));
    }

/*
    @ParameterizedTest
    @MethodSource("getPriorityQueueInstances")
    public void priorityQueueBeispielFirmin(PriorityQueue<Integer> queue) {
        System.out.println("Teste priorityQueueBeispiel mit " + queue.getClass().getSimpleName());

        // Test: eine frisch initialisierte Queue ist leer
        assertTrue(queue.isEmpty());
        queue.insert(1);
        assertFalse(queue.isEmpty());
        assertEquals(1, queue.max());
        queue.insert(2);
        queue.insert(3);
        queue.insert(4);
        assertFalse(queue.isEmpty());
        UnaryOperator<Integer> f = x -> x * x;
        queue.map(f);
        assertEquals(16, queue.max());
        assertEquals(16, queue.deleteMax());
        queue.update(queue.max(), 3);
        assertEquals(4, queue.max()); //1, 3, 4
        assertFalse(queue.isEmpty());

        PriorityQueue<Integer> otherQueue;

        if (queue instanceof ListQueue) otherQueue = new ListQueue<>(Comparator.<Integer>naturalOrder());
        else if (queue instanceof SkewHeap) otherQueue = new SkewHeap<>(Comparator.<Integer>naturalOrder());
        else otherQueue = new FibonacciHeap<>(Comparator.<Integer>naturalOrder());

        assertTrue(otherQueue.isEmpty());
        otherQueue.insert(8);
        assertFalse(otherQueue.isEmpty());
        queue.merge(otherQueue);

        assertEquals(8, queue.max());
        assertEquals(8, queue.deleteMax());
        assertEquals(4, queue.max());
        assertEquals(4, queue.deleteMax());
        assertEquals(3, queue.max());
        assertEquals(3, queue.deleteMax());
        assertEquals(1, queue.max());
        assertEquals(1, queue.deleteMax());
        assertTrue(queue.isEmpty());

        for (int i = 0; i < 50; i++) {
            queue.insert(i);
            assertFalse(queue.isEmpty());
        }
        assertEquals(49, queue.max());
        assertEquals(49, queue.deleteMax());
        assertEquals(48, queue.max());
        assertTrue(queue.update(queue.max(), 50));
        assertEquals(50, queue.max());
        assertFalse(queue.update(-1, new Random().nextInt()));
        queue.map(i -> 2 * i);
        assertEquals(100, queue.max());
        assertEquals(100, queue.deleteMax());
        assertEquals(94, queue.max());

        for (int i = 0; i <= 47; i++) {
            assertEquals(2 * (47 - i), queue.deleteMax());
        }
        assertTrue(queue.isEmpty());
        assertNull(queue.max());
        assertNull(queue.deleteMax());

        queue.insert(20);
        assertFalse(queue.isEmpty());
        assertEquals(20, queue.max());
        queue.insert(10);
        assertEquals(20, queue.max());
        queue.map(t -> t % 10);
        assertEquals(0, queue.max());
        assertEquals(0, queue.deleteMax());
        assertFalse(queue.isEmpty()); // 2 times 0 in the queue
        assertEquals(0, queue.max());
        assertEquals(0, queue.deleteMax());
        assertTrue(queue.isEmpty());
        queue.map(t -> t + 1);
    } 
    */

    @ParameterizedTest
    @MethodSource("Test")
    public void test(LinkedList<Integer> k, boolean empty) {
        assertEquals(empty, k.isEmpty());


    }

    private static Stream<Arguments> Test() throws Exception {
        LinkedList<Integer> il = new LinkedList<>();
        il.add(4);



        return Stream.of(
                Arguments.of(il, true)
                );
    }
}