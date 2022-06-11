# Parser logov

<br>**RU:** Программа преднозначена для сбора информации с текстовых файлов и последующим добавление нужных данных в базу данных (MongoDB). 
Первая версия программы, обрабатывала информацию более **5-ти** часов, сейчас занимает **1:30** Минуты. Программа обрабатывает более 50.000.000 строк.
<br>**EN:** The program is designed to collect information from text files and then add the necessary data to the database (MongoDB).
The first version of the program processed information for more than **5** hours, now it takes **1:30** Minutes. The program processes more than 50.000.000 lines.


## Technologies used | Использованные технологии
- MongoDB.
- Regex.

## Principle of operation | Принцип работы
<br>**RU:** <br>
Изначально, мы загружаем в кэш(List) все никнеймы игроков, которые менялись. (Вызов функции ```SearchLastNickName.LoadChangedNicknames()```
Данные берем с файла ```LogName.txt``` входные данные ```[4/3/2017 6:19:30 PM] Diego_Johnson change name: Jeiron_Merton to Aaron_Salvatruche```.
С помощью Regex'a мы достаём два никнейма(Прошлый ник(Jeiron_Merton) и новый(Aaron_Salvatruche))
<br>**EN:** <br>
Initially, we load into the cache all the nicknames of the players that have changed.(Calling a function from the Program class, ```SearchLastNickName.LoadChangedNicknames()```
We take data from the file ```LogName.txt``` input data ```[4/3/2017 6:19:30 PM] Diego_Johnson change name: Jeiron_Merton to Aaron_Salvatruche```.
Using Regex, we get two nicknames (Previous nickname (Jeiron_Merton) and new one (Aaron_Salvatruche))

![image](https://user-images.githubusercontent.com/101990183/173193528-b9c082d0-2692-408a-8313-f3e2476f0e86.png)

<br>**RU:** <br>
Следующий шаг, мы загружаем в кэш(Dictionary) все никнеймы игроков. (Вызов функции ```LoadingNickNamesToList()```)
Данные берем с файла ```LogReg.txt``` входные данные ```[4/21/2017 1:08:02 PM] Danilevich_Panuskin [###.###.##.##] [HW: SystemSerialNumber // WD-WCC3F2ZJJP3F // ]```.
С помощью Regex'a мы достаём два параметра(Никнейм и HardWare). И сразу вызываем функцию ```SearchLastNickName.SearchLastChangedNick(nickName)``` что бы узнать, 
менял ли игрок никнейм и если менял, то получить новый никнейм. Также и обрабывает HardWare - вызывая функцию ```GetHardWareFromString.GetHardWareList(line)```,
что бы отсортировать плохие HardWare.

<br>**EN:** <br>
The next step, we load into the cache (Dictionary) all the nicknames of the players. (Calling the function ```LoadingNickNamesToList()```)
We take data from the file ```LogReg.txt``` input data ```[4/21/2017 1:08:02 PM] Danilevich_Panuskin [###.###.##.##] [HW: SystemSerialNumber // WD-WCC3F2ZJJP3F // ]```.
Using Regex, we get two parameters (Nickname and HardWare). And immediately call the function ```SearchLastNickName.SearchLastChangedNick(nickName)``` to find out
whether the player changed his nickname and if he did, then get a new nickname. It also handles HardWare - by calling the function ```GetHardWareFromString.GetHardWareList(line);```,
to sort out the bad HardWare.

![image](https://user-images.githubusercontent.com/101990183/173194138-9f94bcaf-c741-49fe-a1ed-ef5fd135bdd7.png)
![image](https://user-images.githubusercontent.com/101990183/173194141-77b70246-a4f4-4a53-835e-9c95920dfb34.png)

<br>**RU:** <br>
После того, как у нас есть список игроков с их последними никнеймами, нам осталось лишь добавить данные, сколько они имели денег на последний момент.
Для этого вызываем функцию ```AddMoneyToAccount()```. Из-за того, что данных с деньгами слишком много, они разделены на три файла
(Суммарно более 50млн строк. А вес файлов 1.89гб, 4.0гб, 0.521гб. Здесь мы читаем каждую строчку из файла и с помощью Regex'a мы получаем три параметра
(Деньги на руках, деньги в банке, никнейм). Возвращаем данные во функцию, и здесь система обрабатывает, если никнейм есть в кэше(Dictionary) и эта операция новее чем была до этого,
то записываются новые данные (Количество денег и номер строки с логов).

<br>**EN:** <br>
After we have a list of players with their last nicknames, we only need to add data on how much money they had at the last moment.
To do this, we call the ```AddMoneyToAccount()``` function. Due to the fact that there is too much data with money, they are divided into three files
(In total, more than 50 million lines. And the weight of the files is 1.89GB, 4.0GB, 0.521GB. Here we read each line from the file and using Regex we get three parameters
(Money on hand, money in the bank, nickname). We return the data to the function, and here the system processes if the nickname is in the cache (Dictionary) and this operation is newer than it was before,
then new data is written (Amount of money and line number from the logs).

![image](https://user-images.githubusercontent.com/101990183/173194519-c5cb5d33-9136-47bf-9575-3c9dd5185a69.png)
![image](https://user-images.githubusercontent.com/101990183/173194523-5e4cf989-ae9c-428f-908d-ce4ee55fa65f.png)

<br>**RU:** <br>
Ну и последний шаг, после того как все наши данные подготовленны, осталось добавить их только в базу данных(MongoDB). 
Вызываем функцию ```AddAccountsToMongoDateBase()```.Функция добавляет в базу данных по 1.000 аккаунтов.
(С целью оптимизации, это лучшее значение)

<br>**EN:** <br>
Well, the last step, after all our data is prepared, it remains to add them only to the database (MongoDB).
We call the function ```AddAccountsToMongoDateBase()``` The function adds 1,000 accounts to the database.
(For optimization purposes, this is the best value)

![image](https://user-images.githubusercontent.com/101990183/173194666-f99b647f-99e9-40b3-bd3e-864242c8435b.png)
![image](https://user-images.githubusercontent.com/101990183/173194764-3163a850-a477-4fc7-8327-296b22454c07.png)
![image](https://user-images.githubusercontent.com/101990183/173194785-695874dd-c69b-49cc-bb1f-9569774a718f.png)

Вес файлов: <br>
![image](https://user-images.githubusercontent.com/101990183/173194865-c87fe150-f110-4114-9238-ca02ee07780e.png)
![image](https://user-images.githubusercontent.com/101990183/173194872-5e369763-32ae-4755-b0da-5de23eea153e.png)
![image](https://user-images.githubusercontent.com/101990183/173194880-6a8406e1-3255-44f7-9f1f-847af5b3bfe9.png)

