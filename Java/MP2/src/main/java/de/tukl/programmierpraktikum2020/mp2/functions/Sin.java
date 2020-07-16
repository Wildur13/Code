package de.tukl.programmierpraktikum2020.mp2.functions;

public class Sin implements Function {

    Function g;

    public Sin(Function f) {
        this.g = f;
    }

    @Override
    public String toString() {
        if (g instanceof Const) {
            if (((Const) g).x == -1.0)
                return "-sin" + g.toString();
        }
        return "sin(" + g.toString() + ")";
    }

    @Override
    public double apply(double x) {
        return Math.sin(g.apply(x));
    }

    @Override
    public Function derive() {
        //Function f = new Cos(new X());
        return new Mult(g.derive(), new Cos(g));
    }

    @Override
    public Function simplify() {
        return new Sin(g.simplify());
    }
}
