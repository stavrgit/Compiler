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
<img width="875" height="656" alt="image" src="https://github.com/user-attachments/assets/24a31667-5d05-44cd-9fed-635e0b78e990" />
<img width="866" height="671" alt="image" src="https://github.com/user-attachments/assets/d799a950-1a4e-4751-8e69-3746f167191c" />

### O2
<img width="880" height="662" alt="image" src="https://github.com/user-attachments/assets/ac63b658-d43f-4a31-a8b6-49d6c6b1434b" />
<img width="901" height="671" alt="image" src="https://github.com/user-attachments/assets/c07fa2d5-46ee-413b-aec0-abb19f096c74" />

### Сравнение 
<img width="820" height="562" alt="image" src="https://github.com/user-attachments/assets/a61d4fc8-f3e2-4eba-9e08-a6bf177b929c" />
