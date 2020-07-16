package de.tukl.programmierpraktikum2020.mp2.functions;


public class Exp implements Function {

    Function g;

    public Exp(Function f) {
        this.g = f;
    }

    @Override
    public String toString() {
        return "exp(" + g.toString() + ")";
    }

    @Override
    public double apply(double x) {
        return Math.exp(g.apply(x));
    }

    @Override
    public Function simplify() {
        return new Exp(g.simplify());
    }

    @Override
    public Function derive() {
        //Function f = new X();
        return new Mult(g.derive(), new Exp(g));
    }
}
