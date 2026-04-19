# Оглавление

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


##  Структура AST:
