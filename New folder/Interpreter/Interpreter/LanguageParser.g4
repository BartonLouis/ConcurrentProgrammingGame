grammar LanguageParser;

program
	: expr=expression							# SingleExpr
	| expr=expression NEWLINE prog=program		# MultipleExpr
	;

// Visit Immediately
expression
	: instruction								
	| selection									 
	| iteration										
	;

// Visit Immediately
instruction
	: operation									
	| assignment								
	;

// Queue evaluation
selection
	: IF expr=boolExpr '{' NEWLINE prog=program NEWLINE '}'					# SingleIf
	| IF expr=boolExpr '{' NEWLINE prog=program NEWLINE '}' else=elseBlock	# ExtendedIf
	;

// Immediately enter program expression
elseBlock
	: ELSE '{' NEWLINE prog=program NEWLINE '}'								# Else
	| ELSE select=selection													# ElseIf
	;

// Queue evaluation
iteration
	: WHILE expr=boolExpr '{' NEWLINE prog=program NEWLINE '}'				# While 
	;

// Queue operation
operation
	: ATTACK a=atom							# Attack
	| HEALSELF								# HealSelf
	| DEFENDSELF							# DefendSelf
	| HEAL a=atom							# Heal
	| BOOST a=atom							# Boost
	| DEFEND a=atom							# Defend
	| BLOCK a=atom							# Block
	| LOCK a=atom							# Lock
	| CHARGEUP								# ChargeUp
	| SENDMESSAGETO a1=atom a2=atom			# SendMessageTo
	| SENDMESSAGETOALL a=atom				# SendMessageToAll
	| YIELD									# Yield
	;

// Queue evalution
assignment
	: v=var '<-' a=atom						# AtomAssignment
	| v=var '<-' expr=mathExpr				# MathExprAssignment
	| v=var '<-' expr=boolExpr				# BoolExprAssignment
	| v=var '<-' f=function					# FunctionAssignment
	| v=var '<-' LISTEN						# ListenAssignment
	;

// Visit immediately
function
	: GETENEMYOFTYPE a=atom					# GetEnemyOfType
	| GETTEAMMATEOFTYPE a=atom				# GetTeammateOfType
	| GETHEALTH								# GetHealth
	| GETMAXHEALTH							# GetMaxHealth
	| ISFULLHEALTH							# IsFullHealth
	| ISCHARGED a=atom						# IsCharged
	| ISNONE a=atom							# IsNone
	| ISNOTNONE a=atom						# IsNotNone
	| GETPLAYERCOMPONENT a=atom				# GetPlayerComponent
	| GETTEXTCOMPONENT a=atom				# GetTextComponent
	| GETCLASS a=atom						# GetClass
	| GETTIMELEFT							# GetTimeLeft
	;

// Visit immediately
mathExpr
	: '(' expr=mathExpr ')'						# MathParen
	| e1=mathExpr op=('*'|'/') e2=mathExpr		# MathMulDiv
	| e1=mathExpr op=('+'|'-') e2=mathExpr		# MathAddSub
	| e1=mathExpr op='^' e2=mathExpr			# MathPow
	| e1=mathExpr op='%' e2=mathExpr			# MathMod
	| a=atom									# MathAtom
	;

// Visit immediately
boolExpr
	: '(' expr=boolExpr ')'										# BoolParen
	| e1=boolExpr AND e2=boolExpr								# BoolAnd
	| e1=boolExpr OR e2=boolExpr								# BoolOr
	| NOT expr=boolExpr											# BoolNot
	| a=atom													# BoolAtom
	| e1=boolExpr op=('=='|'!=') e2=boolExpr					# BoolEq
	| e1=mathExpr op=('<'|'>'|'<='|'>='|'=='|'!=') e2=mathExpr	# MathEq
	;

// Visit Immediately
atom
	: var		
	| literal	
	;

var : ID		
	;

literal
	: message	
	| side		
	| boolean		
	| class		
	| INT		
	| STRING
	| SELF
	;

message : '{' a1=atom ',' a2=atom '}';
side	: LEFT | RIGHT ;
boolean	: TRUE | FALSE ;
class	: DAMAGE | SUPPORT | TANK | ANY ;

compileUnit
	:	EOF
	;


// LEXER RULES 
// Fragments
COMMENT	: '#' .*? [\n\r]+ -> skip;
// BLOCKCOMMENT: '/*' .*? '*/' [\n\r]+ -> skip;
fragment LOWERCASE	: [a-z];
fragment UPPERCASE	: [A-Z];
// Code Blocks
ASSIGN  : '<-' ;
COMMA	: ',';
IF		: 'if';
ELSE	: 'else';
WHILE	: 'while';
LBRACK	: '{';
RBRACK	: '}';
OPENPAREN: '(';
CLOSEPAREN: ')';
// Bool expressions
AND		: 'and';
OR		: 'or';
NOT		: 'not';
GT		: '>';
LT		: '<';
GTE		: '>=';
LTE		: '<=';
EQ		: '==';
NOTEQ	: '!=';
// Math expressions
POW		: '^';
MUL		: '*';
DIV		: '/';
ADD		: '+';
SUB		: '-';
MOD		: '%';
// Operations
ATTACK				: 'Attack';
HEALSELF			: 'HealSelf';
DEFENDSELF			: 'DefendSelf';
HEAL				: 'Heal';
BOOST				: 'Boost';
DEFEND				: 'Defend';
BLOCK				: 'Block';
LOCK				: 'Lock';
CHARGEUP			: 'ChargeUp';
SENDMESSAGETO		: 'SendMessageTo';
SENDMESSAGETOALL	: 'SendMessageToAll';
YIELD				: 'Yield';
// Functions
GETENEMYOFTYPE		: 'GetEnemyOfType';
GETTEAMMATEOFTYPE	: 'GetTeammateOfType';
GETHEALTH			: 'GetHealth';
GETMAXHEALTH		: 'GetMaxHealth';
ISFULLHEALTH		: 'IsFullHealth';
ISCHARGED			: 'IsCharged';
ISNONE				: 'IsNone';
ISNOTNONE			: 'IsNotNone';
GETPLAYERCOMPONENT	: 'GetPlayerComponent';
GETTEXTCOMPONENT	: 'GetTextComponent';
GETCLASS			: 'GetClass';
GETTIMELEFT			: 'GetTimeLeft';
LISTEN				: 'Listen';
// Literals
LEFT	: 'Left';	
RIGHT	: 'Right';
TRUE	: 'True';
FALSE	: 'False';
DAMAGE	: 'Damage';
SUPPORT	: 'Support';
TANK	: 'Tank';
ANY		: 'Any';
SELF	: 'Self';
INT		: ('-')? [0-9]+;
STRING	: '"' [ a-zA-Z0-9]*? '"';
ID		: (LOWERCASE | UPPERCASE | '_')+;

NEWLINE : [\r\n]+ ;

WS :	[ \t]+ -> channel(HIDDEN);
