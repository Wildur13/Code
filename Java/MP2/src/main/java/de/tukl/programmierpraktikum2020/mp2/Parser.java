package de.tukl.programmierpraktikum2020.mp2;

import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionBaseVisitor;
import de.tukl.programmierpraktikum2020.mp2.antlr.FunctionParser;
import de.tukl.programmierpraktikum2020.mp2.functions.*;

public class Parser extends FunctionBaseVisitor<Function> {

    @Override
    public Function visitAddExpr(FunctionParser.AddExprContext ctx) {
        Function f = visit(ctx.lexpr);
        Function g = visit(ctx.rexpr);
        if (ctx.op.getType() == FunctionParser.PLUS) {
            return new Plus(f, g);
        } else {
            return new Sub(f, g);
        }
    }

    @Override
    public Function visitConstVar(FunctionParser.ConstVarContext ctx) {
        return new Const(Double.parseDouble(ctx.CONST().getText()));
    }

    @Override
    public Function visitExpExpr(FunctionParser.ExpExprContext ctx) {
        Function f = visit(ctx.expr());
        if (ctx.op.getType() == FunctionParser.LOG) {
            return new Log(f);
        } else
            return new Exp(f);
    }

    @Override
    public Function visitIdVar(FunctionParser.IdVarContext ctx) {
        return new X();
    }

    @Override
    public Function visitMultExpr(FunctionParser.MultExprContext ctx) {
        Function f = visit(ctx.lexpr);
        Function g = visit(ctx.rexpr);
        if (ctx.op.getType() == FunctionParser.MULT) {
            return new Mult(f, g);
        } else {
            return new Div(f, g);
        }
    }

    @Override
    public Function visitParExpr(FunctionParser.ParExprContext ctx) {
        return visit(ctx.expr());
    }

    @Override
    public Function visitTrigExp(FunctionParser.TrigExpContext ctx) {
        Function f = visit(ctx.expr());
        if (ctx.op.getType() == FunctionParser.SIN) {
            return new Sin(f);
        } else
            return new Cos(f);
    }

    @Override
    public Function visitSgnValExpr(FunctionParser.SgnValExprContext ctx) {
        if (ctx.sgn != null && ctx.sgn.getType() == FunctionParser.MINUS) {
            return new Sub(new Const(0.0), super.visitSgnValExpr(ctx));
        }
        return super.visitSgnValExpr(ctx);

    }

    @Override
    public Function visitPowExpr(FunctionParser.PowExprContext ctx) {
        Function f = visit(ctx.lexpr);
        Function g = visit(ctx.rexpr);
        return new Pow(f,g);
    }
}