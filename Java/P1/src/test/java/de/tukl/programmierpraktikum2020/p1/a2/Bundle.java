package de.tukl.programmierpraktikum2020.p1.a2;

import java.util.HashSet;
import java.util.Objects;
import java.util.Set;

public class Bundle implements WeightedSet<Integer> {
    public final String name;
    public final double price;
    private final Set<Integer> currentFeatures;

    public Bundle(String name, double price, Set<Integer> features) {
        this.name = name;
        this.price = price;
        this.currentFeatures = features;
    }

    @Override
    public String toString() {
        return "Bundle " + name;
    }


    /**
     * Weight function of the bundle
     *
     * @return price per feature
     */
    @Override
    public double getWeight() {
        return price / currentFeatures.size();
    }

    @Override
    public Set<Integer> getSet() {
        return currentFeatures;
    }

    /**
     * Returns a bundle, which has the features of this.getSet() without other.getSet()
     *
     * @param other Subtract other bundle's features from this bundle.
     * @return A bundle which does contain this.currentFeatures without the features of the other bundle. If intersection is empty, just return this
     */
    @Override
    public WeightedSet<Integer> subtractWeightedSet(WeightedSet<Integer> other) {
        Set<Integer> featureSet = new HashSet<>(currentFeatures);
        boolean change = featureSet.removeAll(other.getSet());
        if (change) {
            return new Bundle(name, price, featureSet);
        } else {
            return this;
        }
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        Bundle bundle = (Bundle) o;
        return Double.compare(bundle.price, price) == 0 &&
                name.equals(bundle.name) &&
                Objects.equals(currentFeatures, bundle.currentFeatures);
    }

    @Override
    public int hashCode() {
        return Objects.hash(name, price, currentFeatures);
    }
}
