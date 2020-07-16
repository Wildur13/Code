package de.tukl.programmierpraktikum2020.mp2.functions;

public class Mult implements Function {
    Function f;
    Function g;

    public Mult(Function h, Function p) {
        this.f = h;
        this.g = p;
    }

    @Override
    public String toString() {
        if (f instanceof Const) {
            if (((Const) f).x == -1.0) {
                return "-" + g.toString();
            }
            if (g instanceof Const){
                return String.valueOf((((Const) g).x*((Const) f).x));
            }
        } else if (g instanceof Const) {
            if (((Const) g).x == -1.0) {
                return "-" + f.toString();
            }
        }
        return "("+f.toString() + "*" + g.toString()+")";
    }

    @Override
    public double apply(double x) {
        return f.apply(x) * g.apply(x);
    }

    @Override
    public Function derive() {

        return new Plus(new Mult(f, g.derive()), new Mult(f.derive(), g));

    }

    @Override
    public Function simplify() {
        if (f instanceof Const) {
            if (((Const) f).x == 1.0) {
                return g.simplify();
            } else if (((Const) f).x == 0.0) {
                return new Const(0.0);
            } else if (g instanceof Const) {
                return new Const(((Const) f).x * ((Const) g).x);
            }
        }

        return new Mult(f.simplify(), g.simplify());
    }
}
