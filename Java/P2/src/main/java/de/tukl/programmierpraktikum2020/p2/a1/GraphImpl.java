package de.tukl.programmierpraktikum2020.p2.a1;

import java.util.HashMap;
import java.util.HashSet;
import java.util.Set;

public class GraphImpl<D, W> implements Graph<D, W> {

   HashMap<Integer, D> knoten; // = new HashMap<>();

    Object[][] kanten; //= new Object[1][1];

    Integer size;

    //separate Darstellung

    public GraphImpl() {
    this.knoten = new HashMap<>();
    this.size = 0;
    }

    @Override
    public int addNode(D data) {
        Integer key = this.size;
        knoten.put(key, data);
        this.size ++;
        //kanten = new Object[size][size];
        arrtoarr();
        return key;
    }

    private void arrtoarr (){
        if(size - 1 == 0){kanten = new Object[size][size];}
        else {
            Object[][] arrnew = new Object[size][size];

            for (int posi = 0; posi < size - 1; posi++) {
                for (int posj = 0; posj < size - 1; posj++) {
                    arrnew[posi][posj] = kanten[posi][posj];
                }
            }
            kanten = arrnew;
        }
    }

    @Override
    public D getData(int nodeId) throws InvalidNodeException {
        if(knoten.get(nodeId) == null){
                 throw new InvalidNodeException(nodeId);

            }
        else{ return knoten.get(nodeId);}
    }

    @Override
    public void setData(int nodeId, D data) throws InvalidNodeException {
        if(knoten.get(nodeId) == null){
            throw new InvalidNodeException(nodeId);

        }
        else{ knoten.put(nodeId, data);}
    }

    @Override
    public void addEdge(int fromId, int toId, W weight) throws InvalidNodeException, DuplicateEdgeException {
        if(fromId >= this.size){throw new InvalidNodeException(fromId);}
        if(toId >= this.size){throw new InvalidNodeException(toId);}
        if(kanten[fromId][toId] == null){
            kanten[fromId][toId] = weight;
        }
        else {
            throw new DuplicateEdgeException(fromId, toId);
        }
    }

    @Override
    public W getWeight(int fromId, int toId) throws InvalidEdgeException {
        //if(fromId >= this.size){throw new InvalidNodeException(fromId);}
        //if(toId >= this.size){throw new InvalidNodeException(toId);}
        if(kanten[fromId][toId] == null){throw new InvalidEdgeException(fromId, toId);}
        else{return (W)kanten[fromId][toId];}
    }

    @Override
    public void setWeight(int fromId, int toId, W weight) throws InvalidEdgeException {
        if(kanten[fromId][toId] == null){throw new InvalidEdgeException(fromId, toId);}
        else{kanten[fromId][toId] = weight;}
    }

    @Override
    public Set<Integer> getNodeIds() {
        Set<Integer> si = new HashSet<>();
        for(int i = 0; i < this.size; i++){
            si.add(i);
        }
        return si;
    }

    @Override
    public Set<Integer> getIncomingNeighbors(int nodeId) throws InvalidNodeException {
        if(nodeId >= this.size){throw new InvalidNodeException(nodeId);}
        else {
            Set<Integer> setin = new HashSet<>();
            for (int in = 0; in < this.size; in++) {
                if (kanten[in][nodeId] != null) {
                    setin.add(in);
                }
            }
            return setin;
        }
    }

    @Override
    public Set<Integer> getOutgoingNeighbors(int nodeId) throws InvalidNodeException {
        if(nodeId >= this.size){throw new InvalidNodeException(nodeId);}
        else {
            Set<Integer> setout = new HashSet<>();
            for (int out = 0; out < this.size; out++) {
                if (kanten[nodeId][out] != null) {
                    setout.add(out);
                }
            }
            return setout;
        }
    }
}
