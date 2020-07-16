package de.tukl.programmierpraktikum2020.p1.a2;

import de.tukl.programmierpraktikum2020.p1.a1.PriorityQueue;
import de.tukl.programmierpraktikum2020.p1.a1.PriorityQueueTest;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.*;

import static org.junit.jupiter.api.Assertions.*;

public class WeightedSetCoveringTest {
    public static List<PriorityQueue<Integer>> getPriorityQueueInstances() {
        // Wir nutzen einfach die PriorityQueue Instanzen aus der anderen Testklasse.
        return PriorityQueueTest.getPriorityQueueInstances();
    }

    @ParameterizedTest
    @MethodSource("getPriorityQueueInstances")
    public void weightedSetBeispiel(PriorityQueue<WeightedSet<Integer>> queue) {
        System.out.println("Teste weightedSetBeispiel mit " + queue.getClass().getSimpleName());

        fail("Test wurde noch nicht implementiert");
    }
}
