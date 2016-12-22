# RSA_WPF
Implementacja algorytmu RSA z wbudowaną funkcją weryfikacji sygnatury wygenerowanej przez program - napisany w C# i frameworku WPF

Zaimplementowany program posiada następujące karty
- Generowanie kluczy
- Szyfrowanie
- Deszyfrowanie
- Podpisywanie wiadomości sygnaturą
- Weryfikacja podpisu

Poniżej zrzut ekranu demonstrujący program

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/1.JPG)

#Przykładowa generacja kluczy: prywatnego (tajnego) i publicznego

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/2.JPG)

•	Przy pomocy przycisku „Wygeneruj klucze” można wygenerować klucze prywatny i publiczny przy pomocy algorytmu RSA, który tworzy je i wyświetla wszystkie parametry użyte przy generacji w polu tekstowym jak powyżej
•	Zapis klucza prywatnego odbywa się do pliku tekstowego jako ciąg znaków 
private;<wartość e>;<wartość n>
•	Zapis klucza publicznego odbywa się do pliku tekstowego jako ciąg znaków
public;<wartość e>;<wartość n>
Przykład: Zapis po wybraniu dowolnego katalogu na dysku

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/3.JPG)

Zawartość pliku private_key.txt

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/4.JPG)

Podobnie sytuacja wygląda w przypadku zapisu kluczy publicznych


#Szyfrowanie danych 

- Przykładowe szyfrowanie – plik tekst1.txt, zawierający słynne „Lorem ipsum…”

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/5.JPG)

W powyższym przykładzie wykorzystałem wcześniej wygenerowany 
klucz publiczny w zakładce „Generowanie kluczy”  zaznaczając opcję 
„Wykorzystaj wcześniej wygenerowane klucze”

Dane można zapisać do pliku tekstowego

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/6.JPG)


#Deszyfrowanie danych 

- Wykorzystam zapisane w pliku zaszyfrowane.txt dane oraz wygenerowany klucz prywatny

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/7.JPG)

Na ekranie pojawił się tekst „Lorem ipsum …”

#Podpisywanie wiadomości (tworzenie sygnatury)

Aby podpisać wiadomość należy w pierwszej kolejności:
1.	Wczytać dane, tekst z którego będzie wyliczona sygnatura
2.	Wczytać klucz prywatny dzięki któremu będzie możliwe utworzenie sygnatury.
	
Dla przykładu wykorzystam wcześniej wygenerowany już klucz prywatny.

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/8.JPG)

Jak można zauważyć została wygenerowana sygnatura na podstawie zawartości pliku zgodnie ze wzorem 

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/9.JPG)

s – sygnatura 
m – znak wiadomości
d – składowa klucza prywatnego
n – składowa klucza prywatnego 

Sygnaturę zapisuję do pliku

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/10.JPG)


#Weryfikacja podpisu

Aby podpisać wiadomość należy w pierwszej kolejności:
1.	Wczytać wcześniej utworzoną sygnaturę 
2.	Wczytać dane na podstawie których była wygenerowana sygnatura 
3.	Wczytać klucz publiczny wygenerowanego wcześniej

Dla przykładu wykorzystam wcześniej wygenerowany już klucz publiczny.

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/11.JPG)

Widać że po wczytaniu odpowiednich plików i kliknięciu przycisku „Weryfikuj sygnaturę” 
Otrzymaliśmy informację dołączoną do pola tekstowego, że dokument jest poprawny.

Sprawdzenie podpisu odbyło się z użyciem poniższej zależności

[Kliknij aby zobaczyć](https://raw.githubusercontent.com/krzysiekj94/RSA_WPF/master/images/12.JPG)

v – wynik sprawdzenia (wiadomość) 
s – sygnatura
e – składowa klucza publicznego
n – składowa klucza publicznego 

jeśli v = m , to dokument jest poprawny, gdzie 





