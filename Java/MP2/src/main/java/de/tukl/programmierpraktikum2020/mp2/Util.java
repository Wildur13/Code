package de.tukl.programmierpraktikum2020.mp2;

import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionVisitor;
import de.tukl.programmierpraktikum2020.mp2.functions.Function;
import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionLexer;
import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionParser;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.ParseCancellationException;
import org.antlr.v4.runtime.tree.ParseTree;
import org.knowm.xchart.SwingWrapper;
import org.knowm.xchart.XYChart;
import org.knowm.xchart.XYChartBuilder;
import org.knowm.xchart.XYSeries;
import org.knowm.xchart.style.Styler;
import org.knowm.xchart.style.markers.SeriesMarkers;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class Util {
    public static Function promptFunction(FunctionVisitor<Function> visitor) throws IOException {
        System.out.println("\nf(x) =");
        String input = readLine();
        return parseFunction(input, visitor);
    }

    public static double promptXMin() throws IOException {
        return promptDouble("\nxmin =");
    }

    public static double promptXMax() throws IOException {
        return promptDouble("\nxmax =");
    }

    public static void plotFunction(Function f, double xmin, double xmax) {
        XYChart chart = getChart(f, xmin, xmax);
        new SwingWrapper<>(chart).displayChart();
    }

    private static double promptDouble(String prompt) throws IOException {
        while (true) {
            System.out.println(prompt);
            try {
                return Double.parseDouble(readLine());
            } catch (NumberFormatException e) {
                System.out.println("Not a number!");
            }
        }
    }

    private static String readLine() throws IOException {
        return new BufferedReader(new InputStreamReader(System.in)).readLine();
    }

    private static Function parseFunction(String input, FunctionVisitor<Function> visitor) {
        FunctionLexer lexer = new FunctionLexer(CharStreams.fromString(input));
        lexer.removeErrorListeners();
        lexer.addErrorListener(ThrowingErrorListener.INSTANCE);
        FunctionParser parser = new FunctionParser(new CommonTokenStream(lexer));
        parser.removeErrorListeners();
        parser.addErrorListener(ThrowingErrorListener.INSTANCE);
        ParseTree tree = parser.expr();
        return visitor.visit(tree);
    }

    private static class ThrowingErrorListener extends BaseErrorListener {
        public static final ThrowingErrorListener INSTANCE = new ThrowingErrorListener();

        @Override
        public void syntaxError(Recognizer<?, ?> recognizer, Object offendingSymbol, int line, int charPositionInLine, String msg, RecognitionException e)
                throws ParseCancellationException {
            throw new ParseCancellationException("line " + line + ":" + charPositionInLine + " " + msg);
        }
    }

    private static XYChart getChart(Function f, double xmin, double xmax) {
        int samples = 200;
        double[] xdata = new double[samples];
        double[] ydata = new double[samples];
        double step = (xmax - xmin) / (samples - 1);
        for (int i = 0; i < samples; i++) {
            xdata[i] = xmin + step * i;
            try {
                ydata[i] = f.apply(xdata[i]);
            } catch (RuntimeException e) {
                e.printStackTrace();
            }
        }

        XYChart chart = new XYChartBuilder().width(800).height(600).title("Mini CAS").xAxisTitle("x").yAxisTitle("y").build();

        chart.getStyler().setLegendPosition(Styler.LegendPosition.OutsideS);

        XYSeries ys = chart.addSeries("f(x) = " + f, xdata, ydata);
        ys.setMarker(SeriesMarkers.NONE);

        Function derivative = f.derive();
        if (derivative != null) {
            Function derivativeSimplified = derivative.simplify();
            if (derivativeSimplified == null) {
                derivativeSimplified = derivative;
            }
            double[] dydata = new double[samples];
            for (int i = 0; i < samples; i++) {
                try {
                    dydata[i] = derivativeSimplified.apply(xdata[i]);
                } catch (RuntimeException e) {
                    e.printStackTrace();
                }
            }
            XYSeries dys = chart.addSeries("f'(x) = " + derivativeSimplified, xdata, dydata);
            dys.setMarker(SeriesMarkers.NONE);
        } else {
            System.out.println("Fehler: Ableitung von " + f + " ergibt null und kann daher nicht geplottet werden!");
        }

        return chart;
    }
}
