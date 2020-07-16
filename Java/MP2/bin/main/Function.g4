grammar Function;
@header {package de.tukl.programmierpraktikum2020.mp2.antlr;}

expr:
      op=(EXP | LOG) expr                      #expExpr
    | op=(SIN | COS) expr                      #trigExp
    | lexpr=expr POW rexpr=expr                #powExpr
    | lexpr=expr op=(MULT | DIV) rexpr=expr    #multExpr
    | lexpr=expr op=(PLUS | MINUS) rexpr=expr  #addExpr
    | LPAREN expr RPAREN                       #parExpr
    | sgn=(PLUS | MINUS)? var                  #sgnValExpr
    ;

var: CONST  #constVar
   | ID     #idVar
   ;

CONST   : [0-9]+ ('.' [0-9]+)?;
ID      : 'x';
POW     : '^';
PLUS    : '+';
MINUS   : '-';
MULT    : '*';
DIV     : '/';
EXP     : 'exp';
LOG     : 'log';
SIN     : 'sin';
COS     : 'cos';
LPAREN  : '(';
RPAREN  : ')';
