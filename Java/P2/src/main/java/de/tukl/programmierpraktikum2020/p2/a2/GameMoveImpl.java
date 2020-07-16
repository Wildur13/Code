package de.tukl.programmierpraktikum2020.p2.a2;

import de.tukl.programmierpraktikum2020.p2.a1.Graph;
import de.tukl.programmierpraktikum2020.p2.a1.GraphException;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class GameMoveImpl implements GameMove {

    private final Graph<Color, Integer> spielGraph;

    public GameMoveImpl(Graph<Color, Integer> graph) {
        this.spielGraph = graph;
    }

    @Override
    public void setColor(int nodeId, Color color) throws GraphException, ForcedColorException {
        Color currentColor = spielGraph.getData(nodeId);
        Set<Integer> incomingNeighbors = spielGraph.getIncomingNeighbors(nodeId);
        if (incomingNeighbors.size() == 1 && !currentColor.equals(Color.WHITE)) {
            int fromId = (int) incomingNeighbors.toArray()[0];
            if (spielGraph.getWeight(fromId, nodeId) > 0) throw new ForcedColorException(nodeId, color);
        }
        spielGraph.setData(nodeId, color);
        recoloring(nodeId);
    }

    @Override
    public void increaseWeight(int fromId, int toId) throws GraphException {
        int weight = spielGraph.getWeight(fromId, toId) + 1;
        spielGraph.setWeight(fromId, toId, weight);
        recoloring(fromId);
    }

    @Override
    public void decreaseWeight(int fromId, int toId) throws GraphException, NegativeWeightException {
        int weight = spielGraph.getWeight(fromId, toId) - 1;
        if (weight < 0) throw new NegativeWeightException(fromId, toId);
        spielGraph.setWeight(fromId, toId, weight);
        recoloring(fromId);
    }

    private void recoloring(int fromId) throws GraphException {
        Set<Integer> newColoredNodeIds = new HashSet<>();
        for (int toId : spielGraph.getOutgoingNeighbors(fromId)) {
            for (Color color : Color.values()) {
                if (!color.equals(Color.WHITE)) {
                    Set<Integer> incomingNodeIds = spielGraph.getIncomingNeighbors(toId);
                    Set<Integer> sameColorNodeIds = new HashSet<>();
                    List<Integer> incomingNodeList = new ArrayList<>(incomingNodeIds);
                    int j = 0;
                    while (j < incomingNodeList.size()) {
                        if (spielGraph.getData(incomingNodeList.get(j)).equals(color)) {
                            sameColorNodeIds.add(incomingNodeList.get(j));
                        }
                        j++;
                    }
                    double weightC = calculateNeighborsWeight(sameColorNodeIds, toId);
                    double weightTotal = calculateNeighborsWeight(incomingNodeIds, toId);
                    if (weightC > weightTotal / 2) {
                        spielGraph.setData(toId, color);
                        if (fromId != toId)
                            newColoredNodeIds.add(toId);
                        if (spielGraph.getOutgoingNeighbors(fromId).contains(toId) && spielGraph.getIncomingNeighbors(fromId).contains(toId))
                            newColoredNodeIds.remove(toId);
                        /*if (fromId != toId && !(graph.getOutgoingNeighbors(fromId).contains(toId) && graph.getIncomingNeighbors(fromId).contains(toId)))
                            newColoredNodeIds.add(toId);*/
                    }
                }
            }
        }
        recoloring(newColoredNodeIds);
    }

    private void recoloring(Set<Integer> nodeIds) throws GraphException {
        List<Integer> targetList1 = new ArrayList<>(nodeIds);
        for (int i = targetList1.size() - 1; i >= 0; i--) {
            recoloring(targetList1.get(i));
        }
    }

    private double calculateNeighborsWeight(Set<Integer> nodeIds, int toId) throws GraphException {
        double totalGewicht = 0;
        List<Integer> targetList = new ArrayList<>(nodeIds);
        int e = 0;
        while (e < targetList.size()) {
            totalGewicht += spielGraph.getWeight(targetList.get(e), toId);
            e++;
        }
        return totalGewicht;
    }
}
