package de.tukl.programmierpraktikum2020.mp2.functions;

public class X implements Function{
    double x;

    public X() {

    }

    @Override
    public Function simplify() {
        return this;
    }

    @Override
    public String toString() {
        return "x";
    }

    @Override
    public double apply(double x) {
        return x;
    }

    @Override
    public Function derive() {
        return new Const(1.0);
    }

}
