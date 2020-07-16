package de.tukl.programmierpraktikum2020.mp2;

import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionLexer;
import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionParser;
import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionVisitor;
import de.tukl.programmierpraktikum2020.mp2.functions.Function;
import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.antlr.v4.runtime.tree.ParseTree;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.Random;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

public class CalculusTest {
    private static final String packageName = Function.class.getPackageName();
    private static final String calculusPackageName = Calculus.class.getPackageName();

    private static class FunctionWithRef {
        final String functionStr;
        final java.util.function.Function<Double, Double> ref;  // reference implementation for f
        final java.util.function.Function<Double, Double> dref; // reference implementation for f'

        FunctionWithRef(String functionStr, java.util.function.Function<Double, Double> ref, java.util.function.Function<Double, Double> dref) {
            this.functionStr = functionStr;
            this.ref = ref;
            this.dref = dref;
        }
    }

    private static Stream<FunctionWithRef> getFunctionInstances() {
        Stream.Builder<FunctionWithRef> sb = Stream.builder();

        sb.add(new FunctionWithRef("42.0", x -> 42.0, x -> 0.0));
        sb.add(new FunctionWithRef("x+3", x -> x + 3, x -> 1.0));
        sb.add(new FunctionWithRef("x*x", x -> x * x, x -> 2 * x));
        sb.add(new FunctionWithRef("sin(x)", Math::sin, Math::cos));
        sb.add(new FunctionWithRef("3/x", x -> 3 / x, x -> -3 / (x * x)));
        //sb.add(new FunctionWithRef("x^2", x -> x*x, x -> 2*x)); // Auswertung der Ableitung ergibt ggf. NaN für x<0
        sb.add(new FunctionWithRef("3.0*x+15*sin(x)", x -> 3.0 * x + 15.0 * Math.sin(x), x -> 3.0 + 15.0 * Math.cos(x)));
        sb.add(new FunctionWithRef("exp(-x*x)", x -> Math.exp(-x * x), x -> -2 * Math.exp(-x * x) * x));
        sb.add(new FunctionWithRef("sin(x*x)/(x*x)", x -> Math.sin(x * x) / (x * x), x -> (2 * x * x * Math.cos(x * x) - 2 * Math.sin(x * x)) / (x * x * x)));
        sb.add(new FunctionWithRef("sin(x)+1/3*sin(3*x)+1/5*sin(5*x)+1/7*sin(7*x)+1/9*sin(9*x)+1/11*sin(11*x)", x -> Math.sin(x) + 1 / 3.0 * Math.sin(3 * x) + 1 / 5.0 * Math.sin(5 * x) + 1 / 7.0 * Math.sin(7 * x) + 1 / 9.0 * Math.sin(9 * x) + 1 / 11.0 * Math.sin(11 * x), x -> Math.cos(x) + Math.cos(3 * x) + Math.cos(5 * x) + Math.cos(7 * x) + Math.cos(9 * x) + Math.cos(11 * x)));
        sb.add(new FunctionWithRef("1/x*1/(x-2)*1/(x-4)*1/(x-6)*1/(x+2)*1/(x+4)*1/(x+6)", x -> 1 / x * 1 / (x - 2) * 1 / (x - 4) * 1 / (x - 6) * 1 / (x + 2) * 1 / (x + 4) * 1 / (x + 6), x -> (2304 - 2352 * Math.pow(x, 2) + 280 * Math.pow(x, 4) - 7 * Math.pow(x, 6)) / (Math.pow(-6 + x, 2) * Math.pow(-4 + x, 2) * Math.pow(-2 + x, 2) * Math.pow(x, 2) * Math.pow(2 + x, 2) * Math.pow(4 + x, 2) * Math.pow(6 + x, 2))));
        sb.add(new FunctionWithRef("sin(x)*cos(20*x)", x -> Math.sin(x) * Math.cos(20 * x), x -> Math.cos(x) * Math.cos(20 * x) - Math.sin(x) * Math.sin(20 * x) * 20));

        // Optional parts
        try {
            Class.forName(packageName + "." + "Log");
            sb.add(new FunctionWithRef("log(x*x)", x -> Math.log(x * x), x -> 1 / (x * x) * 2 * x));
        } catch (ClassNotFoundException e) {
            System.out.println("Überspringe Test für optionalen Teil von Aufgabe 1 (Klasse Log).");
        }

        try {
            Class.forName(packageName + "." + "Pow");
            sb.add(new FunctionWithRef("(x*x)^3", x -> Math.pow(x * x, 3), x -> 6 * Math.pow(x, 5)));
        } catch (ClassNotFoundException e) {
            System.out.println("Überspringe Test für optionalen Teil von Aufgabe 1 (Klasse Pow).");
        }

        return sb.build();
    }

    private static Stream<Arguments> getFunctionInstancesWithSeed() throws ReflectiveOperationException {
        try {
            FunctionVisitor<?> visitor = (FunctionVisitor<?>) Class.forName(calculusPackageName + "." + "Parser").getConstructor().newInstance();
            return Stream.of(4711, 123, 42).flatMap(seed ->
                    getFunctionInstances().map(fr -> Arguments.of(fr, seed, visitor))
            );
        } catch (ClassNotFoundException e) {
            System.out.println("FEHLER: Klasse Parser im Paket " + calculusPackageName + " nicht gefunden!");
            throw e;
        }
    }

    private static Function parseFunction(String input, FunctionVisitor<Function> visitor) {
        FunctionLexer lexer = new FunctionLexer(CharStreams.fromString(input));
        FunctionParser parser = new FunctionParser(new CommonTokenStream(lexer));
        ParseTree tree = parser.expr();
        return visitor.visit(tree);
    }

    @ParameterizedTest
    @MethodSource("getFunctionInstancesWithSeed")
    public void testFunctions(FunctionWithRef fr, long seed, FunctionVisitor<Function> visitor) {
        System.out.println(String.format("\nTeste die Funktion         f(x) = %s   mit seed %s", fr.functionStr, seed));
        Function f = parseFunction(fr.functionStr, visitor);
        assertNotNull(f, "Aufruf des Parsers gibt null zurück!");

        String lpad = "    ";
        System.out.println(lpad + "Ergebnis des Parsers:  f(x) = " + f + "   [bitte manuell mit Funktion darüber vergleichen, Darstellung ist nicht eindeutig]");

        Function fSimplified = f.simplify();
        assertNotNull(fSimplified, "f.simplify() gibt null zurück!");

        if (fSimplified != f) {
            System.out.println(lpad + "Ergebnis von simplify: f(x) = " + fSimplified + "   [bitte manuell mit Funktion darüber vergleichen, Darstellung ist nicht eindeutig]");
        } else {
            System.out.println(lpad + "f.simplify() gibt f zurück (optionale Aufgabe 2 nicht bearbeitet oder keine Vereinfachung gefunden)");
        }

        new Random(seed).doubles(-200, 200).limit(1000).forEach(s -> {
            assertEquals(fr.ref.apply(s), f.apply(s), 0.1, lpad + "Die Funktion liefert für x=" + s + " ein falsches Ergebnis!");
        });
        System.out.println(lpad + "Werte der Funktion stimmen mit den Vorgaben überein.");

        if (fSimplified != f) {
            new Random(seed).doubles(-200, 200).limit(1000).forEach(s -> {
                assertEquals(fr.ref.apply(s), fSimplified.apply(s), 0.1, lpad + "Die vereinfachte Funktion liefert für x=" + s + " ein falsches Ergebnis!");
            });
            System.out.println(lpad + "Werte der vereinfachten Funktion stimmen mit den Vorgaben überein.");
        }
    }

    @ParameterizedTest
    @MethodSource("getFunctionInstancesWithSeed")
    public void testDerivatives(FunctionWithRef fr, long seed, FunctionVisitor<Function> visitor) {
        System.out.println(String.format("\nTeste die Funktion         f(x)  = %s   mit seed %s", fr.functionStr, seed));
        Function f = parseFunction(fr.functionStr, visitor);
        assertNotNull(f, "Aufruf des Parsers gibt null zurück!");

        String lpad = "    ";
        System.out.println(lpad + "Ergebnis des Parsers:  f(x)  = " + f + "   [bitte manuell mit Funktion darüber vergleichen, Darstellung ist nicht eindeutig]");

        Function derivative = f.derive();
        assertNotNull(derivative, "f.derive() gibt null zurück!");
        System.out.println(lpad + "Ableitung:             f'(x) = " + derivative + "   [bitte manuell mit Funktion darüber vergleichen, Darstellung ist nicht eindeutig]");

        Function derivativeSimplified = derivative.simplify();
        assertNotNull(derivativeSimplified, "f.derive().simplify() gibt null zurück!");

        if (derivativeSimplified != derivative) {
            System.out.println(lpad + "Ergebnis von simplify: f'(x) = " + derivativeSimplified + "   [bitte manuell mit Funktion darüber vergleichen, Darstellung ist nicht eindeutig]");
        } else {
            System.out.println(lpad + "f.derive().simplify() gibt f.derive() zurück (optionale Aufgabe 2 nicht bearbeitet oder keine Vereinfachung gefunden)");
        }

        new Random(seed).doubles(-200, 200).limit(1000).forEach(s -> {
            assertEquals(fr.dref.apply(s), derivative.apply(s), 0.1, lpad + "Ableitung liefert für x=" + s + " ein falsches Ergebnis!"); // f(x)
        });
        System.out.println(lpad + "Werte der Ableitung stimmen mit den Vorgaben überein.");

        if (derivativeSimplified != derivative) {
            new Random(seed).doubles(-200, 200).limit(1000).forEach(s -> {
                assertEquals(fr.dref.apply(s), derivativeSimplified.apply(s), 0.1, lpad + "Vereinfachung der Ableitung liefert für x=" + s + " ein falsches Ergebnis!"); // f(x)
            });
            System.out.println(lpad + "Werte der vereinfachten Ableitung stimmen mit den Vorgaben überein.");
        }
    }
}
