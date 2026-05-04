#                                                 Лабораторная работа 7. Анализ и преобразование кода с использованием Clang и LLVM

                                                                                      
## Маст Денис АВТ - 313
## Постановка задачи.
Установка среды
Установить Clang, LLVM, opt и Graphviz (например, в Ubuntu 26.04).

Работа с AST
Сгенерировать абстрактное синтаксическое дерево для заданного C/C++‑файла.

Генерация LLVM IR
Получить промежуточное представление кода без оптимизаций (-O0) и с оптимизациями (-O2).

Оптимизация IR
Применить оптимизации с помощью opt и/или флагов Clang, сравнить изменения.

Построение CFG
Построить граф потока управления для одной или нескольких функций.

Индивидуальное задание (по варианту)
Выполнить анализ конкретной синтаксической конструкции в соответствии с вариантом. Сформулировать, как LLVM обрабатывает выбранную конструкцию, какие оптимизации применяются.

Выводы
Ответить на контрольные вопросы

## Общее задание: 
## работа с AST 
<img width="691" height="416" alt="image" src="https://github.com/user-attachments/assets/5916a750-06f7-4d4a-9951-2c2d76cbebbc" />
<img width="841" height="596" alt="image" src="https://github.com/user-attachments/assets/daa87305-3afa-4b0a-81ce-1e267901845a" />

## генерация LLVM IR
<img width="875" height="649" alt="image" src="https://github.com/user-attachments/assets/67e3ad6e-ea94-430e-838e-30798492abee" />

## Оптимизация IR 
<img width="880" height="662" alt="image" src="https://github.com/user-attachments/assets/ac63b658-d43f-4a31-a8b6-49d6c6b1434b" />
<img width="901" height="671" alt="image" src="https://github.com/user-attachments/assets/c07fa2d5-46ee-413b-aec0-abb19f096c74" />

### Сравнение 
<img width="820" height="562" alt="image" src="https://github.com/user-attachments/assets/a61d4fc8-f3e2-4eba-9e08-a6bf177b929c" />


## Построение CFG
<img width="696" height="74" alt="image" src="https://github.com/user-attachments/assets/fe047f22-7865-4d54-a625-64ed2ddf38e1" />

<img width="814" height="617" alt="image" src="https://github.com/user-attachments/assets/34b25894-f869-492f-9015-e9387f89809f" />
<img width="464" height="338" alt="image" src="https://github.com/user-attachments/assets/3cf67c01-2fe3-4f85-bec4-8c3af0b2f173" />

## Индивидуальное задание
<img width="977" height="613" alt="image" src="https://github.com/user-attachments/assets/b9225cc1-01fc-4c53-9153-c8b67c04a117" />
<img width="853" height="358" alt="image" src="https://github.com/user-attachments/assets/179ad9b9-a93b-4265-8de1-748559daf5d6" />
 
### Построенеи IR для O0
<img width="871" height="675" alt="image" src="https://github.com/user-attachments/assets/4d2aa705-fd8f-43b1-83ae-1c9a8f7a918d" />
<img width="858" height="676" alt="image" src="https://github.com/user-attachments/assets/1af4759d-d25f-4e91-9318-c6764491cc81" />

### Применение unroll
<img width="867" height="672" alt="image" src="https://github.com/user-attachments/assets/b7b41bcc-bea5-478a-b564-156111c86cb5" />

### indvars
<img width="862" height="674" alt="image" src="https://github.com/user-attachments/assets/19ac8b11-49d1-4b63-bf98-b12da56e7aba" />

### licm
<img width="862" height="666" alt="image" src="https://github.com/user-attachments/assets/5e1ea4ae-74ff-4f73-ad58-5cd9222f23a3" />

### Построение CFG
<img width="798" height="739" alt="image" src="https://github.com/user-attachments/assets/dc79f6f6-ad91-4571-b14e-f095d89e1bf2" />


### Результаты
#### loop-unroll 
 по итогу дублировал тело цикла несколько раз, уменьшая кол-во проверок условия и переходов. Счетчик цикла проверялся не каждую итерацию, а за несколько. Код стал больше, но уменьшилось кол-во ветвлений
 #### indvars
 Изменений никаких не было, так как условие было довольно легкое и не было смысла в упрощении 
 #### licm 
 Переместил вычисления, которые не зависили от итерации цикла за его пределы. Все load и store вынесены из цикла, переменные i и sum находятся в регистрах все время выполнения цикла.

## Ответы на контрольные вопросы
1. Что такое Clang, и какова его роль?
Clang — компилятора для C/C++. Выполняет лексический и синтаксический анализ, строит AST и генерирует LLVM IR.

2. Что представляет собой LLVM?
LLVM — промежуточное представление (IR), набор оптимизаций и генератор машинного кода для разных архитектур.

3. Отличие AST от LLVM IR?
AST — дерево синтаксиса, близкое к исходному коду. LLVM IR — линейная последовательность низкоуровневых инструкций в SSA-форме.

4. Для чего нужно промежуточное представление?
IR разделяет фронтенд и бэкенд, позволяет переиспользовать оптимизации для всех языков и не зависит от архитектуры процессора.

5. Что делает alloca?
alloca выделяет память на стеке функции. Используется для хранения локальных переменных, при оптимизации заменяется на регистры.

6. Зачем нужна оптимизация кода?
Увеличить скорость выполнения, уменьшить размер кода, эффективнее использовать регистры и кэш процессора.

7. Что такое SSA-форма?
Форма кода, где каждая переменная присваивается только один раз. Упрощает анализ потоков данных и оптимизации.

8. Что такое CFG и зачем он нужен?
Граф потока управления — граф базовых блоков и переходов между ними. Показывает все пути выполнения, помогает находить циклы и мертвый код.

9. Как устроены арифметические операции в LLVM IR?
В виде трёхадресного кода: %result = add i32 %a, %b. Явно указывается тип и оба операнда.

10. Почему функции — отдельные единицы оптимизации?
У функции свой CFG и локальный поток управления. Это позволяет оптимизировать функции независимо и параллельно.

12. Что происходит с короткой функцией при одном вызове?
Она встраивается (inline) в место вызова, тело копируется, сама функция удаляется, устраняются накладные расходы на call.

13. Преимущества IR и CFG перед анализом исходного кода?
IR унифицирован для всех языков, имеет явный поток управления и данных (SSA), проще для автоматического анализа и оптимизаций.

## Доп задание

## Сгенерировать промежуточное представление (IR) для данной конструкции в виде трехадресного кода (TAC)
t1 = i < 10 <br> if t1 goto L1 <br> goto L2 <br> L1: t2 = i + 1 <br> i = t2 <br> goto L0 <br> L2: <br> t3 = 2 + 3 


### Оптимизация сверстки констант 
вычисляет константные выражения на этапе компиляции.t3 = 2 + 3 заменяется на t3 = 5. Это уменьшает количество операций во время выполнения 

<img width="552" height="665" alt="image" src="https://github.com/user-attachments/assets/6852de1d-1dc3-46b1-a1df-1d72af602818" />
<img width="547" height="770" alt="image" src="https://github.com/user-attachments/assets/8e157dca-a1fa-4d7d-a9d7-d5fb7612f9fb" />




t1 = i < 10 <br> if t1 goto L1 <br> goto L2 <br> L1: i = i + 1 <br> goto L0 <br> L2: <br> t3 = 2 + 3 

### Оптимизация устранения лишних копий переменных
устраняет лишние временные переменные, которые используются только для копирования значения. Вместо t2 = i + 1; i = t2 будет i = i + 1. Упрощает код и делает его более читаемым.
<img width="648" height="701" alt="image" src="https://github.com/user-attachments/assets/0736866b-d53f-4401-a8c1-db50057cc040" />
<img width="547" height="770" alt="image" src="https://github.com/user-attachments/assets/ef2989ed-cc3f-43b4-9b09-a9ce4ac1ad39" />




t1 = i < 10 <br> if t1 goto L1 <br> goto L2 <br> L1: i = i + 1 <br> goto L0 <br> L2: <br> t3 = 5


### Примеры:
<img width="517" height="730" alt="image" src="https://github.com/user-attachments/assets/733f0021-d2ad-4cbd-b895-54d3f91834f2" />
<img width="530" height="775" alt="image" src="https://github.com/user-attachments/assets/3299f8cc-c2a2-4be9-a4eb-dfe234c92871" />
<img width="530" height="775" alt="image" src="https://github.com/user-attachments/assets/7b9c9ecf-1b92-4ba2-b48d-100d005f0b6e" />




