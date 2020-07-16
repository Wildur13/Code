package de.tukl.programmierpraktikum2020.p1.a2;

import java.util.Set;

/**
 * Immutable weighted set
 * @param <E> Type of elements contained in WeightedSet
 */
public interface WeightedSet<E> {
    Set<E> getSet();
    double getWeight();
    // returns a WeightedSet without the elements contained in 'other' set
    WeightedSet<E> subtractWeightedSet(WeightedSet<E> other);
}
