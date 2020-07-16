package de.tukl.programmierpraktikum2020.p2.a1;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Set;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.*;

public class GraphTest <T> {
    @Test
    public void example() throws InvalidNodeException, DuplicateEdgeException, InvalidEdgeException {
        GraphImpl<String, Integer> graph = new GraphImpl<>();
        assertEquals(graph.size,0);
        assertThrows(InvalidNodeException.class, () -> {graph.getData(3);});
        graph.addNode("Brot");
        graph.addNode("mit");
        graph.addNode("Leberwurst");

        HashMap<Integer, String> hmap = new HashMap<>();
        hmap.put(0, "Brot");
        hmap.put(1, "mit");
        hmap.put(2, "Leberwurst");
        assertEquals(graph.knoten,hmap);

        Integer [][] intarr = new Integer[3][3];
        assertArrayEquals(intarr, graph.kanten);

        assertEquals(graph.size,3);
        assertThrows(InvalidNodeException.class, () -> {graph.getData(3);});
        assertEquals(graph.getData(0),"Brot");
        assertEquals(graph.getData(1),"mit");
        assertEquals(graph.getData(2),"Leberwurst");

        graph.addEdge(0,1,7);
        graph.addEdge(1,2,5);
        graph.addEdge(2,1,2);
        graph.addEdge(1,1,3);

        intarr[0][1] = 7;
        intarr[1][2] = 5;
        intarr[2][1] = 2;
        intarr[1][1] = 3;

        assertArrayEquals(intarr, graph.kanten);

        hmap.put(3,".");
        hmap.put(2, "Käse");
        graph.addNode(".");
        graph.setData(2, "Käse");
        graph.addEdge(2,3,70);
        graph.addEdge(3,1,5);

        Integer [][] intarrtwo = new Integer[4][4];
        intarrtwo[0][1] = 7;
        intarrtwo[1][2] = 5;
        intarrtwo[2][1] = 2;
        intarrtwo[1][1] = 3;
        intarrtwo[2][3] = 70;
        intarrtwo[3][1] = 5;

        assertArrayEquals(intarrtwo, graph.kanten);
        assertEquals(graph.getData(2),"Käse");
        assertEquals(graph.knoten,hmap);
        assertThrows(InvalidNodeException.class, () -> {graph.setData(4, "Milch");});
        assertThrows(DuplicateEdgeException.class, () -> {graph.addEdge(0,1, 8);});
        assertThrows(InvalidNodeException.class, () -> {graph.addEdge(1,6, 8);});
        assertEquals(graph.getWeight(0,1), 7);
        assertEquals(graph.getWeight(1,1), 3);
        assertEquals(graph.getWeight(2,3), 70);
        assertEquals(graph.size,4);

        assertThrows(InvalidEdgeException.class, () -> {graph.getWeight(2, 2);});
        assertThrows(InvalidEdgeException.class, () -> {graph.getWeight(3, 2);});


        graph.setWeight(0,1,22);
        graph.setWeight(1,2,5);
        graph.setWeight(1,2,345);
        assertEquals(graph.getWeight(0,1), 22);
        assertEquals(graph.getWeight(1,2), 345);

        intarrtwo[0][1] = 22;
        intarrtwo[1][2] = 345;
        assertArrayEquals(intarrtwo, graph.kanten);

        Set<Integer> so = new HashSet<>();
        so.add(0);
        so.add(1);
        so.add(2);
        so.add(3);

        assertEquals(graph.getNodeIds(),so);

        Set<Integer> setinone = new HashSet<>();
        setinone.add(3);
        setinone.add(2);
        setinone.add(1);
        setinone.add(0);

        Set<Integer> setintwo = new HashSet<>();
        setintwo.add(1);

        Set<Integer> setinnull = new HashSet<>();

        assertEquals(graph.getIncomingNeighbors(0), setinnull);
        assertEquals(graph.getIncomingNeighbors(1), setinone);
        assertEquals(graph.getIncomingNeighbors(2), setintwo);

        Set<Integer> setoutone = new HashSet<>();
        setoutone.add(2);
        setoutone.add(1);

        Set<Integer> setouttwo = new HashSet<>();
        setouttwo.add(1);
        setouttwo.add(3);

        Set<Integer> setoutnull = new HashSet<>();
        setoutnull.add(1);

        Set<Integer> setoutthree = new HashSet<>();
        setoutthree.add(1);

        assertEquals(graph.getOutgoingNeighbors(0), setoutnull);
        assertEquals(graph.getOutgoingNeighbors(1), setoutone);
        assertEquals(graph.getOutgoingNeighbors(2), setouttwo);
        assertEquals(graph.getOutgoingNeighbors(3), setoutthree);

        assertThrows(InvalidNodeException.class, () -> {graph.getIncomingNeighbors(7);});
        assertThrows(InvalidNodeException.class, () -> {graph.getOutgoingNeighbors(7);});
        assertThrows(InvalidNodeException.class, () -> {graph.addEdge(7,5, 99);});
        assertThrows(InvalidEdgeException.class, () -> {graph.setWeight(0,0, 1);});






    }
}