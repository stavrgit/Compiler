#                                                 Лабораторная работа 5.Построение AST и проверка контекстно-зависимых условий

                                                                                      
## Маст Денис АВТ - 313


## Вариант задания:
<img width="494" height="83" alt="image" src="https://github.com/user-attachments/assets/3b508c78-70c5-4e05-a225-e1d0c4288cbb" />

while x < 10:
x += 1;

##  Контекстно-зависимые условия:
Правило 3 (допустимые значения): Проверить, что значение находится в допустимых пределах (для числовых типов).

Корректный

<img width="1203" height="391" alt="image" src="https://github.com/user-attachments/assets/41f4cc78-dec4-41bf-9afe-57e851f721f0" />

Некорректный 

<img width="1453" height="403" alt="image" src="https://github.com/user-attachments/assets/3574e18f-1f1f-46be-a292-265ef9876830" />


##  a) Структура AST:

### WhileNode
Атрибуты:

Modifiers — список модификаторов (например, ключевое слово while)

Condition — условие цикла (выражение сравнения или логическое выражение)

Body — список операторов, выполняемых внутри цикла

Дочерние узлы:

ModifiersNode — контейнер для ключевого слова while

ConditionNode — контейнер для выражения условия (например, CompareNode)

StmtListNode (Body) — контейнер для списка операторов внутри цикла


### ConditionNode
Атрибуты:

Parts — список узлов, составляющих условие (например, идентификатор, оператор сравнения, литерал, двоеточие)

Дочерние узлы:

IdentifierNode — переменная, участвующая в условии

CompareNode — оператор сравнения 

IntLiteralNode — числовое значение для сравнения

ColonNode — двоеточие :



### AssignNode
Атрибуты:

Name — имя переменной

Op — оператор присваивания

Value — выражение или литерал, присваиваемый переменной

Дочерние узлы:

IdentifierNode — переменная, которой присваивается значение

Assign (TerminalNode) — сам оператор

IntLiteralNode — значение

SemicolonNode — точка с запятой ;

## b) Рисунок AST для верной строки:
<img width="789" height="784" alt="image" src="https://github.com/user-attachments/assets/2f786a7a-2f07-4f0f-8f71-44068a80bc59" />
<img width="1703" height="582" alt="image" src="https://github.com/user-attachments/assets/b71a7657-fc46-4370-816b-a093f75e5031" />


## c) Формат вывода AST в программе:
<img width="376" height="705" alt="image" src="https://github.com/user-attachments/assets/14ee82de-fe5a-4f6a-8a63-9d50fcb282a8" />

## Инструкция по запуску:
Путь к исполняемому файлу: "..\bin\Debug\net9.0-windows\compiiler.exe"

Открыть проект в Visual Studio 2022 (или новее).
Убедиться, что установлен .NET 9.0 SDK.
В меню выбрать: Build → Build Solution
Запустить проект: Debug → Start Debugging

## Дополнительное задание: 

### Использованные графические средства:
стандартные средства Windows Forms и встроенные классы из пространства имён System.Drawing

<img width="1877" height="601" alt="image" src="https://github.com/user-attachments/assets/c22c1fcd-4f4c-443e-a065-6263ad2cffa7" />
