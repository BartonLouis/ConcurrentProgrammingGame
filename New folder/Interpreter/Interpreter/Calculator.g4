grammar Calculator;

/*
 * Parser Rules
 */

compileUnit
	:	mathExpr EOF
	;

mathExpr
	: left=mathExpr op=('*'|'/') right=mathExpr #MulDiv
	| left=mathExpr op=('+'|'-') right=mathExpr #AddSub
	| constant			#Const
	| '(' expr=mathExpr ')'	#Paren 
	;

constant
	: INT | DOUBLE;

/*
 * Lexer Rules
 */


MUL: '*';
DIV: '/';
ADD: '+';
SUB: '-';
LPAREN: '(';
RPAREN: ')';
INT	: ('-')? [0-9]+;
DOUBLE: ('-')? [0-9]+ '.' [0-9]+;
WS
	:	[ \n\r\t] -> skip
	;


