using System;
using System.Linq;

class Yatzy
{
    static Random random = new Random();
    static int[] dice = new int[5];   // En array som används för att hålla koll på vilket värde tärningarna har. 
    static bool[] keep = new bool[5]; // en bool-array som ska hålla koll på vilka tärningar som sparas. 
    static int[] scores = new int[13];  // 
    static bool[] categoriesUsed = new bool[13]; // En bool-array som kollar vilka kategorier som används. 
    
                                                            // skapar variabler för varje komponent av yatzy spelet. 
    static void Main()
    {
        Console.WriteLine("Välkommen till Yatzy!");         
        bool playAgain = true;                                    // Welcome message

        while (playAgain)                                         // Loop to allow replaying the game
        {
            ResetGame();                                          // Reset scores and used categories
            for (int round = 0; round < 13; round++)              // 13 rounds in a Yatzy game
            {   
                PlayRound();
            }
            ShowFinalScore();                                    // Show total score after 13 rounds
            Console.Write("Vill du spela igen? (ja/nej): ");
            playAgain = Console.ReadLine().ToLower() == "ja";
        }
    }

    static void ResetGame()
    {
        Array.Clear(scores, 0, scores.Length);                          // Reset scores array to 0
        Array.Clear(categoriesUsed, 0, categoriesUsed.Length);          // Reset used categories
    }

    static void PlayRound()
    {
        Array.Clear(keep, 0, keep.Length);                              // Clear saved dice
        int rolls = 0;

        while (rolls < 3)                           // Player can roll up to 3 times
        {
            RollDice();
            DisplayDice();
            if (rolls < 2) ChooseKeep();                        // Let player choose dice to keep (not after last roll)
            rolls++;
        }

        int category = SuggestCategory();
        Console.WriteLine($"Föreslagen kategori: {CategoryName(category)}");
        Console.Write("Vill du använda denna kategori? (ja/nej): ");
        if (Console.ReadLine().ToLower() != "ja")                           // föreslår en kategori till användaren.
        {
            category = ChooseCategory();
        }

        ScoreCategory(category);
    }

    static void RollDice()
    {
        for (int i = 0; i < 5; i++)                     // Loop through all 5 dice
        {
            if (!keep[i]) dice[i] = random.Next(1, 7);          // Roll dicee if not marked as "keep"
        }
    }

    static void DisplayDice()
    {
        Console.WriteLine("Tärningar: " + string.Join(", ", dice));             // Print all dice values
    }

    static void ChooseKeep()
    {
        Console.Write("Vilka tärningar vill du spara? (ex: 1 3 5 för att spara första, tredje och femte tärningen): ");
        string[] input = Console.ReadLine().Split();                // Read and split user input by spaces
        Array.Clear(keep, 0, keep.Length);                            // Clear previous keep values



        foreach (var num in input)          // Loop through entered numbers
        {
            if (int.TryParse(num, out int index) && index >= 1 && index <= 5)
                keep[index - 1] = true;
        }
    }

    static int SuggestCategory()
    {
        int[] counts = dice.GroupBy(x => x).Select(g => g.Count()).ToArray();
        if (counts.Contains(5)) return 11; // Yatzy
        if (counts.Contains(4)) return 10; // Fyrtal
        if (counts.Contains(3) && counts.Contains(2)) return 9; // Kåk
        if (counts.Contains(3)) return 8; // Triss
        if (dice.OrderBy(x => x).SequenceEqual(new[] { 1, 2, 3, 4, 5 })) return 7; // Liten stege
        if (dice.OrderBy(x => x).SequenceEqual(new[] { 2, 3, 4, 5, 6 })) return 6; // Stor stege
        return 12; // Chans
    }

    static int ChooseCategory()
    {
        Console.WriteLine("Välj kategori:");
        for (int i = 0; i < 13; i++)
        {
            if (!categoriesUsed[i])
                Console.WriteLine($"{i + 1}. {CategoryName(i)}");
        }
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 13 || categoriesUsed[choice - 1])
        {
            Console.Write("Ogiltigt val. Välj igen: ");
        }
        return choice - 1;
    }                                                               // Visar tillgängliga kategorier och låter spelaren välja en som inte redan har använts.


    static void ScoreCategory(int category)
    {
        scores[category] = CalculateScore(category);
        categoriesUsed[category] = true;
    }

    static int CalculateScore(int category)
    {
        switch (category)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                return dice.Where(d => d == category + 1).Sum();
            case 6: return 40;
            case 7: return 30;
            case 8: return dice.Sum();
            case 9: return 25;
            case 10: return dice.Sum();
            case 11: return 50;
            case 12: return dice.Sum();
            default: return 0;
        }
    }                       // Returnerar poängen för vald Yatzy-kategori baserat på nuvarande tärningskast.

    static void ShowFinalScore()
    {
        Console.WriteLine("\nSlutpoäng:");  
        int total = 0;
        for (int i = 0; i < 13; i++)
        {
            Console.WriteLine($"{CategoryName(i)}: {scores[i]} poäng");
            total += scores[i];
        }
        Console.WriteLine("Totalpoäng: " + total);
    }                                                                   // // Skriver ut poäng per kategori och totalpoäng

    static string CategoryName(int index)
    {
        string[] categories = { "Ettor", "Tvåor", "Treor", "Fyror", "Femmor", "Sexor", "Stor stege", "Liten stege", "Triss", "Kåk", "Fyrtal", "Yatzy", "Chans" };
        return categories[index];
    }
}
