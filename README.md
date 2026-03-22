#                                                 Лабораторная работа 3. Разработка синтаксического анализатора (парсера)

                                                                                      
##                                                                                 Цель работы.
Изучить назначение и принципы работы синтаксического анализатора в структуре компилятора. Спроектировать грамматику, построить соответствующую схему метода анализа грамматики и выполнить программную реализацию парсера с нейтрализацией синтаксических ошибок методом Айронса. Интегрировать разработанный модуль в ранее созданный графический интерфейс языкового процессора.

## Маст Денис АВТ - 313
## Постановка задачи.
Разработать синтаксический анализатор (парсер) в соответствии с индивидуальным вариантом курсовой (расчетно-графической) работы, интегрировать его в приложение из лабораторной работы №1 и обеспечить наглядный вывод результатов анализа.

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

## Метод анализа
## Рекурсвиный спуск
<img width="1024" height="486" alt="image" src="https://github.com/user-attachments/assets/40626d0f-0c4b-4bec-9af2-edd7bb69bbb6" />

## Диагностика и нейтрализация синтаксических ошибок.

Разрабатываемый синтаксический анализатор основан на контекстно‑свободной грамматике (тип‑2 по классификации Хомского) и реализован методом рекурсивного спуска.
Это определяет специфику поведения алгоритма Айронса при обработке ошибок.
В КС‑грамматике, построенной для конструкции:
while <логическое выражение> : <список операторов>
каждый нетерминал соответствует отдельной рекурсивной функции.
Поэтому в процессе разбора в каждый момент времени активно только одно правило грамматики, которое и формирует текущую ветвь дерева разбора.



## грамматика ANTLR
grammar antler;

options { language = CSharp; }

ID      : LETTER (LETTER | DIGIT | '_')* ;
INT     : DIGIT+ ;

fragment LETTER : [a-zA-Z] ;
fragment DIGIT  : [0-9] ;

WS      : [ \t\r\n]+ -> skip ;

AND : 'and' | '&&';
OR  : 'or'  | '||';

program
    : operatorWhile EOF
    ;

operatorWhile
    : 'while' logicalExpr ':' stmtList?
    ;

stmtList
    : statement stmtList
    |
    ;

statement
    : assignStmt
    | logicalExpr ';'
    ;

assignStmt
    : ID assignOp logicalExpr ';'
    ;

assignOp
    : '=' | '+=' | '-=' | '*=' | '/='
    ;

logicalExpr
    : orExpr
    ;

orExpr
    : andExpr orTail
    ;

orTail
    : OR andExpr orTail
    |
    ;

andExpr
    : relExpr andTail
    ;

andTail
    : AND relExpr andTail
    |
    ;

relExpr
    : addExpr relTail
    ;

relTail
    : relOp addExpr
    |
    ;

relOp
    : '<' | '>' | '<=' | '>=' | '==' | '!='
    ;

addExpr
    : mulExpr addTail
    ;

addTail
    : '+' mulExpr addTail
    | '-' mulExpr addTail
    |
    ;

mulExpr
    : factor mulTail
    ;

mulTail
    : '*' factor mulTail
    | '/' factor mulTail
    |
    ;

factor
    : ID
    | INT
    | '(' logicalExpr ')'
    | 'not' factor
    | '!' factor
    ;


## Тестовые примеры:
1) правильный
 <img width="1023" height="371" alt="image" src="https://github.com/user-attachments/assets/28d4b34c-b176-49e9-bfdb-54a2da87b6c4" />
2) неправильный
   <img width="1002" height="363" alt="image" src="https://github.com/user-attachments/assets/0e70b36f-f27a-48a7-882c-6b1da0ab8df6" />
3) многострочный
   <img width="1014" height="499" alt="image" src="https://github.com/user-attachments/assets/4252d9ec-883e-42a0-9840-a0c65f40643b" />
