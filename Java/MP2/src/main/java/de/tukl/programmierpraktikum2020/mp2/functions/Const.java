package de.tukl.programmierpraktikum2020.mp2.functions;

public class Const implements Function{
    double x;

    public Const(double x) {
        this.x = x;
    }

    @Override
    public String toString() {
        return String.valueOf(x);
    }

    @Override
    public double apply(double x) {
        return this.x;
    }

    @Override
    public Function derive() {
        return new Const(0.0);
    }

    @Override
    public Function simplify() {
        //return new Const(this.x);
        return this;
    }
}
