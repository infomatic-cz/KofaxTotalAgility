## Šablona pro C# scripty
### Logování
- Na vstupu i výstupu je proměnná ProcessingLog, do které se přidávají záznamy o průběhu zpracování (zápisy je nutné vytvořit pomocí log.Append(string))
- Processing log se vypíše do event logu v případě chyby
- Do logu se automaticky přidá výpis vstupních a výstupních proměnných
- U každého záznamu je čas a metoda (vše nemusí být jeden blok)
- Namísto krátkých popisných komentářů je lepší zapsat do logu, aby byl přehled nejen v kódu, ale i v runtime

### Exception
- Místo pro vložení kódu je zabalené v Try/Catch, který zachytí chybu, zapíše do event logu a chybu přepošle dál
- V případě nutnosti použít Finaly nebo změnit základní fungování je samozřejmě možné chování upravit
