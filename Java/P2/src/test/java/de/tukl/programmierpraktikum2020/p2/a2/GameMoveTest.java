package de.tukl.programmierpraktikum2020.p2.a2;

import de.tukl.programmierpraktikum2020.p2.a1.*;
import org.junit.jupiter.api.Test;

import java.util.HashSet;
import java.util.Set;

import static org.junit.jupiter.api.Assertions.*;

public class GameMoveTest {

    @Test
    public void Beispiel1() throws Exception {
        Graph<Color, Integer> graph = new GraphImpl<>();
        int a = graph.addNode(Color.WHITE);
        int b = graph.addNode(Color.WHITE);
        int c = graph.addNode(Color.WHITE);
        graph.addEdge(c, a, 1);
        graph.addEdge(c, b, 2);
        graph.addEdge(b, a, 3);
        graph.addEdge(a, b, 2);

        GameMove gm = new GameMoveImpl(graph);
        // Spielzug 1
        gm.setColor(c, Color.RED);
        assertEquals(Color.RED, graph.getData(c));
        assertEquals(Color.WHITE, graph.getData(a));
        assertEquals(Color.WHITE, graph.getData(b));

        // Spielzug 2
        gm.increaseWeight(c, b);
        assertEquals(3, graph.getWeight(c, b));
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.RED, graph.getData(c));

        // Spielzug 3
        gm.setColor(c, Color.RED);
        assertEquals(Color.RED, graph.getData(c));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 4
        gm.setColor(c, Color.GREEN);
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.GREEN, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(a));

    }


    @Test
    public void Beispiel2() throws Exception {
        Graph<Color, Integer> graph = new GraphImpl<>();
        int a = graph.addNode(Color.WHITE);
        int b = graph.addNode(Color.WHITE);
        int c = graph.addNode(Color.WHITE);
        int d = graph.addNode(Color.WHITE);
        graph.addEdge(a, b, 1);
        graph.addEdge(a, c, 1);
        graph.addEdge(c, b, 3);
        graph.addEdge(c, c, 3);
        graph.addEdge(d, b, 1);
        graph.addEdge(d, c, 1);

        GameMove gm = new GameMoveImpl(graph);

        // Spielzug 1
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.WHITE, graph.getData(b));
        assertEquals(Color.WHITE, graph.getData(c));
        assertEquals(Color.WHITE, graph.getData(d));

        // Spielzug 2
        gm.setColor(c, Color.GREEN);
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.GREEN, graph.getData(b));
        assertEquals(Color.WHITE, graph.getData(d));

        // Spielzug 3
        gm.setColor(d, Color.RED);
        assertEquals(Color.RED, graph.getData(d));
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.GREEN, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));

        // Spielzug 4
        gm.decreaseWeight(c, b);
        assertEquals(2, graph.getWeight(c, b));
        assertEquals(Color.GREEN, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 5
        gm.decreaseWeight(c, b);
        assertEquals(1, graph.getWeight(c, b));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.RED, graph.getData(d));

    }


    @Test
    public void Beispiel03() throws Exception {
        Graph<Color, Integer> graph = new GraphImpl<>();
        int a = graph.addNode(Color.WHITE);
        int b = graph.addNode(Color.WHITE);
        int c = graph.addNode(Color.WHITE);
        int d = graph.addNode(Color.WHITE);
        graph.addEdge(a, b, 1);
        graph.addEdge(a, c, 1);
        graph.addEdge(c, b, 2);
        graph.addEdge(c, c, 2);
        graph.addEdge(d, b, 2);
        graph.addEdge(d, c, 1);

        GameMove gm = new GameMoveImpl(graph);

        // Spielzug 1
        gm.setColor(d, Color.RED);
        assertEquals(Color.RED, graph.getData(d));
        assertEquals(Color.WHITE, graph.getData(a));
        assertEquals(Color.WHITE, graph.getData(b));
        assertEquals(Color.WHITE, graph.getData(c));

        // Spielzug 2
        gm.setColor(c, Color.GREEN);
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.WHITE, graph.getData(a));
        assertEquals(Color.WHITE, graph.getData(b));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 3
        gm.setColor(b, Color.BLUE);
        assertEquals(Color.BLUE, graph.getData(b));
        assertEquals(Color.WHITE, graph.getData(a));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 4
        gm.setColor(b, Color.YELLOW);
        assertEquals(Color.YELLOW, graph.getData(b));
        assertEquals(Color.WHITE, graph.getData(a));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 5
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 6
        gm.increaseWeight(c, c);
        assertEquals(3, graph.getWeight(c, c));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 7
        gm.setColor(d, Color.BLUE);
        assertEquals(Color.BLUE, graph.getData(d));
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));

        // Spielzug 8
        gm.setColor(a, Color.YELLOW);
        assertEquals(Color.YELLOW, graph.getData(a));
        assertEquals(Color.BLUE, graph.getData(d));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(b));

        // Spielzug 9
        gm.setColor(d, Color.RED);
        assertEquals(Color.RED, graph.getData(d));
        assertEquals(Color.YELLOW, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));

        // Spielzug 10
        gm.increaseWeight(c, b);
        assertEquals(3, graph.getWeight(c, b));
        assertEquals(Color.RED, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.YELLOW, graph.getData(a));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 11
        gm.setColor(b, Color.BLUE);
        assertEquals(Color.BLUE, graph.getData(b));
        assertEquals(Color.YELLOW, graph.getData(a));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.RED, graph.getData(d));

        // Spielzug 12
        gm.setColor(d, Color.YELLOW);
        assertEquals(Color.YELLOW, graph.getData(d));
        assertEquals(Color.YELLOW, graph.getData(a));
        assertEquals(Color.BLUE, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));

        // Spielzug 13
        gm.setColor(d, Color.RED);
        assertEquals(Color.RED, graph.getData(d));
        assertEquals(Color.YELLOW, graph.getData(a));
        assertEquals(Color.BLUE, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));

        // Spielzug 14
        gm.increaseWeight(c, b);
        assertEquals(4, graph.getWeight(c, b));
        assertEquals(Color.GREEN, graph.getData(b));
        assertEquals(Color.GREEN, graph.getData(c));
        assertEquals(Color.YELLOW, graph.getData(a));
        assertEquals(Color.RED, graph.getData(d));

    }


    @Test
    public void Beispiel04() throws Exception {
        Graph<Color, Integer> graph = new GraphImpl<>();
        int a = graph.addNode(Color.WHITE);
        int b = graph.addNode(Color.WHITE);
        graph.addEdge(a, b, 1);

        GameMove gm = new GameMoveImpl(graph);

        // Spielzug 1
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));

        // Spielzug 2
        assertThrows(ForcedColorException.class, () ->
                gm.setColor(b, Color.GREEN), "Ausnahme ForcedColorException wurde nicht geworfen");
        assertEquals(Color.RED, graph.getData(a));
        assertEquals(Color.RED, graph.getData(b));

    }


    @Test
    public void Beispiel05() throws Exception {
        Graph<Color, Integer> graph = new GraphImpl<>();
        int a = graph.addNode(Color.WHITE);
        graph.addEdge(a, a, 1);

        GameMove gm = new GameMoveImpl(graph);

        // Spielzug 1
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 2
        assertThrows(ForcedColorException.class, () ->
                gm.setColor(a, Color.GREEN), "Ausnahme ForcedColorException wurde nicht geworfen");
        assertEquals(Color.RED, graph.getData(a));

    }


    @Test
    public void Beispiel06() throws Exception {
        Graph<Color, Integer> graph = new GraphImpl<>();
        int a = graph.addNode(Color.WHITE);
        graph.addEdge(a, a, 1);

        GameMove gm = new GameMoveImpl(graph);

        // Spielzug 1
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 2
        gm.decreaseWeight(a, a);
        assertEquals(0, graph.getWeight(a, a));
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 3
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 4
        gm.setColor(a, Color.GREEN);
        assertEquals(Color.GREEN, graph.getData(a));
    }


    @Test
    public void Beispiel07() throws Exception {
        Graph<Color, Integer> graph = new GraphImpl<>();
        int a = graph.addNode(Color.WHITE);
        graph.addEdge(a, a, 1);

        GameMove gameMove = new GameMoveImpl(graph);

        // Spielzug 1
        gameMove.decreaseWeight(a, a);
        assertEquals(0, graph.getWeight(a, a));
        assertEquals(Color.WHITE, graph.getData(a));

        // Spielzug 2
        assertThrows(NegativeWeightException.class, () ->
                gameMove.decreaseWeight(a, a), "Ausnahme NegativeWeightException wurde nicht geworfen");
        assertEquals(Color.WHITE, graph.getData(a));
    }

    @Test
    public void example4() throws Exception {
        Graph<Color, Integer> graph = new Graph<>() {
            // 0 -> 1
            Color colorA = Color.WHITE;
            Color colorB = Color.WHITE;
            int weight = 1;

            @Override
            public int addNode(Color data) {
                throw new UnsupportedOperationException();
            }

            @Override
            public Color getData(int nodeId) throws InvalidNodeException {
                if (nodeId == 0) return colorA;
                else if (nodeId == 1) return colorB;
                else throw new InvalidNodeException(nodeId);
            }

            @Override
            public void setData(int nodeId, Color data) throws InvalidNodeException {
                if (nodeId == 0) colorA = data;
                else if (nodeId == 1) colorB = data;
                else throw new InvalidNodeException(nodeId);
            }

            @Override
            public void addEdge(int fromId, int toId, Integer weight) throws InvalidNodeException, DuplicateEdgeException {
                throw new UnsupportedOperationException();
            }

            @Override
            public Integer getWeight(int fromId, int toId) throws InvalidEdgeException {
                if (fromId == 0 && toId == 1) return weight;
                else throw new InvalidEdgeException(fromId, toId);
            }

            @Override
            public void setWeight(int fromId, int toId, Integer weight) throws InvalidEdgeException {
                if (fromId == 0 && toId == 1) this.weight = weight;
                else throw new InvalidEdgeException(fromId, toId);
            }

            @Override
            public Set<Integer> getNodeIds() {
                Set<Integer> nodeIds = new HashSet<>();
                nodeIds.add(0);
                nodeIds.add(1);
                return nodeIds;
                //return Set.of(0, 1);
            }

            @Override
            public Set<Integer> getIncomingNeighbors(int nodeId) throws InvalidNodeException {
                if (nodeId == 0) return Set.of();
                else if (nodeId == 1) return Set.of(0);
                else throw new InvalidNodeException(nodeId);
            }

            @Override
            public Set<Integer> getOutgoingNeighbors(int nodeId) throws InvalidNodeException {
                if (nodeId == 0) return Set.of(1);
                else if (nodeId == 1) return Set.of();
                else throw new InvalidNodeException(nodeId);
            }
        };

        assertEquals(1, graph.getWeight(0, 1));
        assertEquals(Color.WHITE, graph.getData(0));
        assertEquals(Color.WHITE, graph.getData(1));

        GameMove gm = new GameMoveImpl(graph);

        // Spielzug 1
        gm.setColor(0, Color.RED);
        assertEquals(Color.RED, graph.getData(0));
        assertEquals(Color.RED, graph.getData(1));

        // Spielzug 2
        assertThrows(ForcedColorException.class, () ->
                gm.setColor(1, Color.GREEN), "Ausnahme ForcedColorException wurde nicht geworfen");
        assertEquals(Color.RED, graph.getData(0));
        assertEquals(Color.RED, graph.getData(1));
    }

    @Test
    public void example6() throws Exception {
        Graph<Color, Integer> graph = new Graph<>() {
            Color data = Color.WHITE;
            int weight = 1;

            @Override
            public int addNode(Color data) {
                throw new UnsupportedOperationException();
            }

            @Override
            public Color getData(int nodeId) throws InvalidNodeException {
                if (nodeId == 0) return data;
                else throw new InvalidNodeException(nodeId);
            }

            @Override
            public void setData(int nodeId, Color data) throws InvalidNodeException {
                if (nodeId == 0) this.data = data;
                else throw new InvalidNodeException(nodeId);
            }

            @Override
            public void addEdge(int fromId, int toId, Integer weight) throws InvalidNodeException, DuplicateEdgeException {
                throw new UnsupportedOperationException();
            }

            @Override
            public Integer getWeight(int fromId, int toId) throws InvalidEdgeException {
                if (fromId == 0 && toId == 0) return weight;
                else throw new InvalidEdgeException(fromId, toId);
            }

            @Override
            public void setWeight(int fromId, int toId, Integer weight) throws InvalidEdgeException {
                if (fromId == 0 && toId == 0) this.weight = weight;
                else throw new InvalidEdgeException(fromId, toId);
            }

            @Override
            public Set<Integer> getNodeIds() {
                return Set.of(0);
            }

            @Override
            public Set<Integer> getIncomingNeighbors(int nodeId) throws InvalidNodeException {
                if (nodeId == 0) return Set.of(0);
                else throw new InvalidNodeException(nodeId);
            }

            @Override
            public Set<Integer> getOutgoingNeighbors(int nodeId) throws InvalidNodeException {
                if (nodeId == 0) return Set.of(0);
                else throw new InvalidNodeException(nodeId);
            }
        };

        int a = 0;
        assertEquals(Color.WHITE, graph.getData(a));
        assertEquals(1, graph.getWeight(a, a));

        GameMove gm = new GameMoveImpl(graph);

        // Spielzug 1
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 2
        gm.decreaseWeight(a, a);
        assertEquals(0, graph.getWeight(a, a));
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 3
        gm.setColor(a, Color.RED);
        assertEquals(Color.RED, graph.getData(a));

        // Spielzug 4
        gm.setColor(a, Color.GREEN);
        assertEquals(Color.GREEN, graph.getData(a));
    }
}
