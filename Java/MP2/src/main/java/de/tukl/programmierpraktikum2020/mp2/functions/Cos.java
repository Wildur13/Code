package de.tukl.programmierpraktikum2020.mp2.functions;

public class Cos implements Function {
    //double x;
    Function g;

    public Cos(Function f) {
        this.g = f;
    }

    @Override
    public String toString() {
        return "cos(" + g.toString() + ")";
    }

    @Override
    public double apply(double x) {
       return Math.cos(g.apply(x));
    }

    @Override
    public Function derive() {
        return new Mult(new Mult(new Const(-1),g.derive()), new Sin(g));
    }

    @Override
    public Function simplify() {
        if (g instanceof Const){
            if (((Const) g).x== 0.0)
                return new Const(1.0);
        }
        return new Cos(g.simplify());
    }
}
