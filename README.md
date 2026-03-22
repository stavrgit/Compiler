#                                                 Лабораторная работа 2. Разработка лексического анализатора (сканера)

                                                                                      
##                                                                                   Цель работы.
Изучить назначение и принципы работы лексического анализатора в структуре компилятора. Спроектировать алгоритм (диаграмму состояний) и выполнить программную реализацию сканера для выделения лексем из входного текста. Интегрировать разработанный модуль в ранее созданный графический интерфейс языкового процессора.

## Постановка задачи.
Разработать лексический анализатор (сканер) в соответствии с индивидуальным вариантом задания, интегрировать его в приложение из лабораторной работы №1 и обеспечить наглядный вывод результатов..

## Вариант задания:
<img width="494" height="83" alt="image" src="https://github.com/user-attachments/assets/3b508c78-70c5-4e05-a225-e1d0c4288cbb" />

while x < 10: y = y + 1;

while counter < 100:
value = 10;
result = value;

## Перечень доступных лексем:
ключевое слово while and or not

идентификаторы (i, counter, value)

целое без знака (10, 1)

операторы сравнения <, >, <=, >=, 
операторы равенства: ==, !=

оператор присваивания =, +. +=, -=, /=, *=

арифметические операторы +, -, /, *
логическиие операторы &&, ||, !

разделители ()

начало блока :

конец конструкции ;

## Диаграмма состояний:
<img width="503" height="752" alt="image" src="https://github.com/user-attachments/assets/8678af60-52df-4b26-a9f4-8b8b4594c33e" />

## Тестовые примеры:
while x < 10: y = y + 1;
<img width="1894" height="1000" alt="image" src="https://github.com/user-attachments/assets/07d8da3b-51c2-475e-ad68-472229c57be7" />

while x @ 5: y = 1; (с ошибкой)
<img width="1886" height="805" alt="image" src="https://github.com/user-attachments/assets/c8f3b0c3-235d-4c57-a49c-01ff27e38123" />

while counter < 100:
    value = 10;
    result = value;   (многострочный)
<img width="1876" height="867" alt="image" src="https://github.com/user-attachments/assets/b8b742d2-bfda-4483-af46-7951941ec8cb" />



## моя грамматика 
Vt = {
    'a'..'z', 'A'..'Z',
    '0'..'9',
    '+', '-', '*', '/',
    '<', '>', '=', '!', 
    '(', ')',
    ';', ':',
    'w','h','i','l','e',
    'a','n','d','o','r','t',
    ' ',
    '_'
}

Vn = {
    <Программа>, <ОператорWhile>,

    <ЛогическоеВыражение>, <OrExpr>, <OrTail>,
    <AndExpr>, <AndTail>,
    <RelExpr>, <RelTail>, <ОператорСравнения>,

    <AddExpr>, <AddTail>,
    <MulExpr>, <MulTail>,
    <Factor>,

    <СписокОператоров>, <Оператор>,
    <ОператорПрисваивания>, <AssignOp>,
    <ВыражениеОператор>,

    <Идентификатор>, <Число>, <Буква>, <Цифра>
}

S = <Программа>

<Программа> → <ОператорWhile>

<ОператорWhile> → 'w''h''i''l''e' ' ' <ЛогическоеВыражение> ':' <СписокОператоров>

<ЛогическоеВыражение> → <OrExpr>
<OrExpr> → <AndExpr> <OrTail>
<OrTail> → 'o''r' <AndExpr> <OrTail> | ε
<RelExpr> → <AddExpr> <RelTail>
<RelTail> → <ОператорСравнения> <AddExpr> | ε
<ОператорСравнения> → '<' | '>' | '<=' | '>=' | '==' | '!='
<AddExpr> → <MulExpr> <AddTail>
<AddTail> → '+' <MulExpr> <AddTail> | '-' <MulExpr> <AddTail> | ε
<MulExpr> → <Factor> <MulTail>
<MulTail> → '*' <Factor> <MulTail> | '/' <Factor> <MulTail> | ε
<Factor> → <Идентификатор>| <Число>| '(' <ЛогическоеВыражение> ')'| 'n''o''t' <Factor>| '!' <Factor>э
<СписокОператоров> → <Оператор> <СписокОператоров> | ε
<Оператор> → <ОператорПрисваивания> | <ВыражениеОператор>
<ОператорПрисваивания> → <Идентификатор> <AssignOp> <ЛогическоеВыражение> ';'
<AssignOp> → '=' | '+=' | '-=' | '*=' | '/='
<ВыражениеОператор> → <ЛогическоеВыражение> ';'
<Идентификатор> → <Буква> | '_'| <Идентификатор> <Буква> | <Идентификатор> <Цифра> | <Идентификатор> '_'
<Число> → <Цифра>| <Число> <Цифра>
<Буква> → 'a' | ... | 'z' | 'A' | ... | 'Z'
<Цифра> → '0' | ... | '9'
## Классификация 
Тип по Хомскому
Грамматика относится к контекстно‑свободным (тип 2) т.к. слева всегда один нетерминал, справа — произвольная последовательность терминалов и нетерминалов,
правила не зависят от контекста.


## Грамматика FLEX
%{
    #include "parser.tab.h"
%}

WS      [ \t\r\n]+
ID      [a-zA-Z][a-zA-Z0-9]*
NUM     [0-9]+

%%

"while"         { return WHILE; }

"<="            { return LE; }
">="            { return GE; }
"=="            { return EQ; }
"!="            { return NE; }
"<"             { return '<'; }
">"             { return '>'; }

":"             { return ':'; }
";"             { return ';'; }
"="             { return '='; }

{NUM}           { yylval.ival = atoi(yytext); return NUMBER; }
{ID}            { return IDENT; }

{WS}            { /* skip */ }

.               { printf("Lexical error: %s\n", yytext); exit(1); }

%%
## Классификация
FLEX использует регулярные выражения и это регулярная грамматика (тип 3).

## грамматика BISON
%{
#include <stdio.h>
#include <stdlib.h>

void yyerror(const char *s);
int yylex(void);
%}

%union {
    int ival;
}

%token WHILE
%token IDENT
%token NUMBER
%token LE GE EQ NE

%type <ival> NUMBER

%start Program

%%

Program
    : WhileStmt
    ;

WhileStmt
    : WHILE Condition ':' StmtList
    ;

Condition
    : IDENT RelOp NUMBER
    ;

RelOp
    : '<'
    | '>'
    | LE
    | GE
    | EQ
    | NE
    ;

StmtList
    : StmtList Stmt
    | Stmt
    ;

Stmt
    : IDENT '=' Expr ';'
    ;

Expr
    : IDENT
    | NUMBER
    ;

%%

void yyerror(const char *s)
{
    printf("Синтаксическая ошибка: %s\n", s);
}
## Классификация
Bison работает с контекстно‑свободными грамматиками (тип 2) и строит LALR(1)‑парсер.

## Тестовые примеры:
1) правильный
 <img width="1023" height="371" alt="image" src="https://github.com/user-attachments/assets/28d4b34c-b176-49e9-bfdb-54a2da87b6c4" />
2) неправильный
   <img width="1002" height="363" alt="image" src="https://github.com/user-attachments/assets/0e70b36f-f27a-48a7-882c-6b1da0ab8df6" />
3) многострочный
   <img width="1014" height="499" alt="image" src="https://github.com/user-attachments/assets/4252d9ec-883e-42a0-9840-a0c65f40643b" />
