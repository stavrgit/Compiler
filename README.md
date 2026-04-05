<img width="811" height="96" alt="image" src="https://github.com/user-attachments/assets/c7b090c1-bc93-474f-969e-5a33b675a835" />#                                     Лабораторная работа 4. Реализация алгоритма поиска подстрок с помощью регулярных выражений

## Цель работы
Изучить теоретические основы регулярных выражений и их применение для поиска и извлечения подстрок из текста. Освоить практические навыки использования библиотечных средств работы с регулярными выражениями, а также интеграцию алгоритмов поиска в графический интерфейс приложения.
## Маст Денис АВТ - 313
## Постановка задачи
Разработать модуль поиска подстрок с использованием регулярных выражений, интегрировать его в существующее приложение (текстовый редактор) и обеспечить наглядный вывод результатов.

Студент получает индивидуальный вариант, содержащий 3 задачи на поиск подстрок определенных форматов.
<img width="823" height="171" alt="image" src="https://github.com/user-attachments/assets/cef52be4-0f92-4816-99ff-ba640d9d6871" />

### ^[a-zA-Z$_][0-9]*$  ^ начало строки   [a-zA-Z$_] первый символ    [0-9]* 0 или боле раз    $ конец строки   
<img width="1304" height="341" alt="image" src="https://github.com/user-attachments/assets/1a1b4770-0f3d-408d-8e0c-c7bd634c32af" />

## x123 подходит



<img width="1186" height="342" alt="image" src="https://github.com/user-attachments/assets/7a3725f7-473c-4ad2-8ec9-1bc0b4876738" />

## 111 не подходит




<img width="828" height="123" alt="image" src="https://github.com/user-attachments/assets/72e04641-311b-4704-9106-7ddfe9a6ad9c" />

### ^[a-z0-9_-]{8,16}$.    [a-z0-9_-] разрешенные символы.   {8,16} длина.   

<img width="1339" height="342" alt="image" src="https://github.com/user-attachments/assets/7a2ea5ad-2dd9-47f7-bb51-2c03267ca0de" />

## dawdadwadda подходит   

<img width="1183" height="294" alt="image" src="https://github.com/user-attachments/assets/b5d3cc4a-d546-440e-bf1f-75de05573abd" />


## dee не подходит
<img width="811" height="96" alt="image" src="https://github.com/user-attachments/assets/54efe662-3a84-4d38-9577-948ebdd45841" />

### ^[A-HJ-NPR-Z0-9]{17}$  [A-HJ-NPR-Z0-9] разрешённые символы  (A - Z но пропущеныв I Q O так как похожи на цифры)   {17} длина 

<img width="1292" height="337" alt="image" src="https://github.com/user-attachments/assets/cb4a7676-c7c4-427f-be80-a148b5babb6b" />


## 1HGCM82633A004352 подходит 

<img width="1187" height="307" alt="image" src="https://github.com/user-attachments/assets/1d6c6830-8990-4b35-aec9-2bef1fbed8b4" />


## 1HGCM82633A0O4352 не подходит
 
