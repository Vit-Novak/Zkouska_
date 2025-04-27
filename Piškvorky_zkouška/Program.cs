namespace Piškvorky_zkouška
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            bool zapnuto = true;
            // Smyčka pro zobrazení menu
            while (zapnuto)
            {
                Console.Clear();  // Vyčistíme obrazovku 
                Console.WriteLine("--- MENU ---");
                Console.WriteLine("1. Hrát novou hru");
                Console.WriteLine("2. Historie výherců");
                Console.WriteLine("3. Konec");
                Console.Write("Vyber možnost (1-3): ");
                string volba = Console.ReadLine();// Zpracování výběru uživatele
                                
                switch (volba)
                {
                    case "1":
                        Hra();  // Pokud uživatel zvolí 1, zahájí se nová hra
                        break;

                    case "2":
                        UkazHistorii();  // Pokud uživatel zvolí 2, zobrazí se historie výher
                        break;

                    case "3":
                        Console.WriteLine("Program ukončen.");  // Ukončení programu
                        zapnuto = false;
                        break;

                    default:
                        Console.WriteLine("Neplatná volba, zkuste to znovu.");  // Pokud uživatel zadá neplatnou volbu
                        break;
                }

                // Po zobrazení výsledku programu čekáme na stisk klávesy, než pokračujeme
                Console.WriteLine("\nStiskněte libovolnou klávesu pro pokračování...");
                Console.ReadKey();
            }
        }

        ///// FUNKCE PRO ZAHAJENI HRY /////
        public static void Hra()
        {
            string[,] Deska = new string[3, 3];
            int Radky = 3;  
            int Sloupce = 3;  
            string Hrac1 = "0";  
            string Hrac2 = "0";  

            // Inicializace herní desky a nastavení všech buněk na "."
            for (int i = 0; i < Radky; i++) 
            {
                for (int j = 0; j < Sloupce; j++)
                {
                    Deska[i, j] = ".";
                }
            }

            // Výběr symbolů pro hráče
            while (true)
            {
                Console.WriteLine("Player 1, vyber 'x' nebo 'o':");
                Hrac1 = Console.ReadLine().ToLower();  
                if (Hrac1 == "x" || Hrac1 == "o") break;
                Console.WriteLine("Neplatná volba, vyberte znovu.");
            }

            while (true)
            {
                Console.WriteLine("Player 2, vyber druhý symbol:");
                Hrac2 = Console.ReadLine().ToLower();
                if ((Hrac2 == "x" || Hrac2 == "o") && Hrac2 != Hrac1) break;
                Console.WriteLine("Neplatná volba, vyberte znovu.");
            }
            string aktualniHrac = Hrac1;  // Začíná hráč 1
            Program.AktualniDeska(Deska, Radky, Sloupce);
            // Běh hry
            while (true)
            {
                
                VyberMista(Deska, aktualniHrac);  // Vybírání místa
                Program.AktualniDeska(Deska, Radky, Sloupce);  

                if (Konec(Deska, ref aktualniHrac)) // Kontrola výherce
                {
                    Console.WriteLine($"Hráč '{aktualniHrac}' Vyhrál!");
                    UlozVysledek(aktualniHrac);  // Ukládáme výherce
                    break;
                }

                if (PlnaDeska(Deska)) // Kontrola remízi
                {
                    Console.WriteLine("Remíza!");
                    UlozVysledek("Remíza");  // Ukládáme remízu
                    break;
                }

                // Přepnutí hráče
                aktualniHrac = (aktualniHrac == Hrac1) ? Hrac2 : Hrac1;
            }
        }

        ///// Funkce pro vypsání herní desky /////
        public static void AktualniDeska(string[,] deska, int radky, int sloupce)
        {
            
            Console.Clear();  
            for (int i = 0; i < radky; i++)  
            {
                for (int j = 0; j < 3; j++)  
                {
                    Console.Write(deska[i, j]+ '|');  // Vytiskneme obsah buňky
                }
                Console.WriteLine();  // Po každém řádku přidáme nový řádek
                Console.WriteLine("------");
            }
        }

        ///// Funkce pro výběr pozice hráčem /////
        public static void VyberMista(string[,] deska, string hrac)
        {
            int hracRadek = 0;
            int hracSloupec = 0;

            while (true)
            {
                // Přečteme od hráče, kde chce umístit svůj symbol
                Console.WriteLine($"Hráč {hrac}, vyberte řádek (0-2):");
                hracRadek = Convert.ToInt32(Console.ReadLine()); // Prevedeni do int

                Console.WriteLine($"Hráč {hrac}, vyberte sloupec (0-2):");
                hracSloupec = Convert.ToInt32(Console.ReadLine());

                if (deska[hracRadek, hracSloupec] == ".") // kontrola jestli je pozice volná
                {
                    deska[hracRadek, hracSloupec] = hrac; // zapsani x nebo o na pozici
                    break;
                }
                else
                {
                    Console.WriteLine("Tato pozice je zabraná, vyberte znovu.");
                }
            }
        }

        ///// Funkce kontrolující konec hry (JE VÝHERCE) /////
        public static bool Konec(string[,] deska, ref string hrac)
        {
            string Radek0 = deska[0, 0] + deska[0, 1] + deska[0, 2]; //vsechny varianty vyherninch kombinaci
            string Radek1 = deska[1, 0] + deska[1, 1] + deska[1, 2];
            string Radek2 = deska[2, 0] + deska[2, 1] + deska[2, 2];
            string Sloupec0 = deska[0, 0] + deska[1, 0] + deska[2, 0];
            string Sloupec1 = deska[0, 1] + deska[1, 1] + deska[2, 1];
            string Sloupec2 = deska[0, 2] + deska[1, 2] + deska[2, 2];
            string Diagonala1 = deska[0, 0] + deska[1, 1] + deska[2, 2];
            string Diagonala2 = deska[0, 2] + deska[1, 1] + deska[2, 0];

            string Spojeno = hrac + hrac + hrac;  // Pokud má hráč 3 stejné symboly v řadě

            if (Radek0 == Spojeno || Radek1 == Spojeno || Radek2 == Spojeno // Kontrola splnění podmínky
                || Sloupec0 == Spojeno || Sloupec1 == Spojeno || Sloupec2 == Spojeno
                || Diagonala1 == Spojeno || Diagonala2 == Spojeno)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        ///// Funkce kontrolující pokud je deska plná (REMÍZA) /////
        public static bool PlnaDeska(string[,] deska)
        {
            foreach (string bunka in deska)
            {
                if (bunka == ".") return false;  // Pokud najdeme prázdnou buňku, hra pokračuje
            }
            return true;  // Pokud jsou všechny buňky obsazené, hra končí remízou
        }

        ///// Ukládání výsledků do souboru CSV /////
        //BYLA VYUŽITA POMOC OD AI
        public static void UlozVysledek(string Vyherce)
        {
            string filePath = "vysledky.csv";
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"{timeStamp},{Vyherce}";

            if (!File.Exists(filePath)) // Pokud soubor neexistuje vytvoříme
            {
                File.WriteAllText(filePath, "Datum a čas,Vítěz\n");  
            }

            File.AppendAllText(filePath, line + "\n");  // Přidání nového výsledeku
        }

        ///// Zobrazení historie výher /////
        //BYLA VYUŽITA POMOC OD AI
        public static void UkazHistorii()
        {
            string filePath = "vysledky.csv";
            Console.Clear();

            if (!File.Exists(filePath)) // Pokud neni
            {
                Console.WriteLine("Zatím nejsou žádné uložené výsledky.");
                return;
            }

            string[] lines = File.ReadAllLines(filePath); // Pokud je

            Console.WriteLine("\n--- Historie výher ---");
            foreach (string line in lines)
            {
                Console.WriteLine(line);  // Vytiskneme každý řádek z historie
            }
        }
    }
}
