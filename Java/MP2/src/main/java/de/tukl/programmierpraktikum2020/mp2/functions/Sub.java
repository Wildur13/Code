package de.tukl.programmierpraktikum2020.mp2.functions;

public class Sub implements Function {
    Function f;
    Function g;

    public Sub(Function f, Function g) {
        this.f = f;
        this.g = g;
    }

    @Override
    public String toString() {
        if (g instanceof Const) {
            if (((Const) g).x == -0.0 || ((Const) g).x == 0.0)
                return "-" + f.toString();
        }
        if (f instanceof Const) {
            if (((Const) f).x == -0.0 || ((Const) f).x == 0.0)
                return "-" + g.toString();
        }
        return '(' + f.toString() + "-" + g.toString() + ')';
    }

    @Override
    public double apply(double x) {
        return (f.apply(x) - g.apply(x));
    }

    @Override
    public Function derive() {
        return new Sub(f.derive(), g.derive());
    }

    @Override
    public Function simplify() {
        /*if (f instanceof Const) {
            if (g instanceof Const) {
                double p = ((Const) f).x - ((Const) g).x;
                return (new Const(p));
            }
        }if (f instanceof Const && ((Const) f).x == 0)
            return g.simplify();
        if (g instanceof  Const && ((Const)g).x == 0)
            return f.simplify();*/
        return new Sub(f.simplify(), g.simplify());
    }
}
