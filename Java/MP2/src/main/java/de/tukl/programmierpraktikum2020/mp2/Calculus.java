package de.tukl.programmierpraktikum2020.mp2;

import de.tukl.programmierpraktikum2020.mp2.functions.*;

import java.io.IOException;


public class Calculus {
    public static void main(String[] args) throws IOException {
        /*String s1 = "Sachin";
        String s2 = new String("Sachin");
        boolean b = s1 == s2;
        boolean b2 = s1.equals(s2);
        System.out.println(b);
        System.out.println(b2);*/
        //System.out.print("Enter your number: ");
        //Scanner sc = new Scanner(System.in);
        //double v = sc.nextDouble();
        //Function f = new Log(new Mult(new Const(2),new Pow(new X(),new Const(2))));//new Mult(new Const(42.0), new X());
        //Function f = new Mult(new Const(-1), new X());
        //Function f = new Exp(new Mult(new Mult(new Const(5), new X()), new X()));
        //Function f = new Plus(new Cos(new X()),new Sin(new X()));
        //Function f = new Div(new Const(0),new Sin(new X()));
        //Function f = new Pow(new X(),new Const(2));
        //System.out.println(Math.ceil(-13.90625));
        Function f = null;
        double xmin, xmax;

        // -------------------------------------------------------------------------------------------------------------
        // Aufgabe 1:
        // Funktionen direkt hier im Code instanziieren, z. B.:
        // f = new Exp(new X());

        // Bereich, der geplottet werden soll:
        xmin = -10;
        xmax = 10;

        // Geplottet wird f weiter unten.


        // -------------------------------------------------------------------------------------------------------------
        // Aufgabe 3:

        // Benutzer nach Funktion (als String) fragen und diesen String parsen:
        // (Sobald die Klasse Parser exisitiert, können Sie die Kommentarzeichen vor der nächsten Zeile entfernen)
        f = Util.promptFunction(new Parser());

        if (f != null) { // Gültige Funktion eingegeben
            // Benutzer nach xmin und xmax fragen
            // (Kommentarzeichen vor den folgenden beiden Zeilen entfernen!)
           xmin = Util.promptXMin();
           xmax = Util.promptXMax();

            // Eingaben anzeigen:
            System.out.println("\nf(x)  = " + f.toString());
            Function derivative = f.derive();
            if (derivative != null) {
                System.out.println("f'(x) = " + derivative.toString());
                Function simpleDerivative = derivative.simplify();
                if (simpleDerivative != null && simpleDerivative != derivative) {
                    System.out.println("f'(x) = " + simpleDerivative.toString() + "    // simplified");
                }
            }

            // Plot
            Util.plotFunction(f, xmin, xmax);
        } else {
            System.out.println("\nKeine gültige Funktion eingegeben!");
        }
    }
}
