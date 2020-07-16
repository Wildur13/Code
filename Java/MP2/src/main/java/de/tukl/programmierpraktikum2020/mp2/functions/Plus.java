package de.tukl.programmierpraktikum2020.mp2.functions;

public class Plus implements Function {
    Function h;
    Function p;

    public Plus(Function f, Function g) {
        this.h = f;
        this.p = g;
    }

    @Override
    public String toString() {
        if (h instanceof Const) {
            if (((Const) h).x == 0.0)
                return p.toString();
        }
        if (p instanceof Const) {
            if (((Const) p).x == 0.0)
                return h.toString();
        }
        return h.toString() + "+" + p.toString();
    }

    @Override
    public double apply(double x) {
        return h.apply(x) + p.apply(x);
    }

    @Override
    public Function derive() {
        return new Plus(h.derive(), p.derive());
    }

    @Override
    public Function simplify() {
        if (h instanceof Const) {
            if (((Const) h).x == 0.0) {
                return p;
            }
        } else if (p instanceof Const) {
            if (((Const) p).x == 0.0) {
                return h;
            }
        } else if (p instanceof Const && h instanceof Const) {
            double c = ((Const) h).x + ((Const) p).x;
            return new Const(c);
        }
            return new Plus(h.simplify(), p.simplify());
    }

}
