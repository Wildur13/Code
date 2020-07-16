package de.tukl.programmierpraktikum2020.mp2.functions;

public class Div implements Function {
    Function f;
    Function g;

    public Div(Function f, Function g) {
        this.f = f;
        this.g = g;
    }

    @Override
    public String toString() {
        if (f instanceof Const) {
            if (((Const) f).x == 0.0) {
                return String.valueOf(0.0);
            }
        }

        return f.toString() + "/" + g.toString();
    }

    @Override
    public double apply(double x) {
        return f.apply(x) / g.apply(x);
    }

    @Override
    public Function derive() {
    //       return new Div(new Sub(new Mult(f.derive(), g), new Mult(g.derive(), f)), new Mult(g, g));
    //    Function res = new Div(new Sub(new Mult(f.derive(),g),new Mult(g.derive(),f)),new Mult(g,g));
    //    return res;
    return new Div(new Sub(new Mult(g, f.derive()), new Mult(f, g.derive())), new Mult(g, g));

    }

    @Override
    public Function simplify() {
        if (f instanceof Const && ((Const) f).x == 0) {
            return new Const(0.0);
        } else if (g instanceof Const && ((Const) g).x == 0.0) {
            throw new IllegalArgumentException();
        }
        return (new Div(f.simplify(), g.simplify()));
    }
}
