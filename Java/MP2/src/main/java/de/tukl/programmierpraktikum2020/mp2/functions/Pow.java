package de.tukl.programmierpraktikum2020.mp2.functions;

public class Pow implements Function {
    Function f;
    Function g;


    public Pow(Function h, Function g) {
        this.f = h;
        this.g = g;
    }

    @Override
    public String toString() {
        return "pow(" + f.toString() + ", " + g.toString() + ")";
    }

    @Override
    public double apply(double x) {
        return Math.pow(f.apply(x), g.apply(x));
    }

    @Override
    public Function derive() {
        return (new Mult(new Mult(g, f.derive()), new Pow(f, (new Sub(g, new Const(1))))));
    }

    @Override
    public Function simplify() {
        return new Pow(f.simplify(),g.simplify());
    }
}
