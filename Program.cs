using System;
namespace Arkham
{
    class Program
    {
        static int room_number;
        static void Main(string[] args)
        {
            room_number = 0;
            StartGame();
        }
        static void cc(string color) // (Console Color)
        {
            switch (color)
            {
                case "a": // Action
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("> ");
                    break;
                case "i": // Information
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "n": // Notice
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "e": // Environment
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            if (color == null)
            {
                Console.Write("[!] Error while outputting message with console color. Color is null. (x34) [!]");
            }
        }
        static void GenerateRoom()
        {
            Console.Clear();
            room_number += 1;
            cc("e"); Console.WriteLine("You are now in room number " + room_number + ".");
        }
        static string GenerateEvent()
        {
            Random rnum = new Random();
            int RoomType = rnum.Next(1, 5);
            switch (RoomType)
            {
                case 1: // Easy Enemy
                    cc("e"); Console.WriteLine("An enemy appears!");
                    Console.ReadLine();
                    return "easy";

                case 2: // Hard Enemy
                    cc("e"); Console.WriteLine("An enemy appears!");
                    Console.ReadLine();
                    return "hard";

                case 3: // Trap
                    cc("e"); Console.WriteLine("A box lies at the center of the room.");
                    Console.ReadLine();
                    return "trap";

                case 4: // Treasure
                    cc("e"); Console.WriteLine("A box lies at the center of the room.");
                    Console.ReadLine();
                    return "treasure";
            }
            return null;
        }
        static void StartGame()
        {
            room_number = 0;
            double[] PlayerStats = new double[2];
            double[] EnemyStats = new double[2];
            PlayerStats[0] = 10;
            PlayerStats[1] = 10;
            EnemyStats[0] = 10;
            EnemyStats[1] = 10;
            string event_difficulty = null;
            int treasure_type = 0;
            // Commands
            bool motherlode = false;
            bool sans = false;
            bool konami = false;
            string turn;
            goto cheats;
            // "Secret" command intake
            cheats:
            string cheats = null;
            cheats = Console.ReadLine();
            if (cheats == "motherlode") // Only treasure chests
            {
                motherlode = true;
                Console.WriteLine("Enabled.");
                Console.ReadLine();
                Console.Clear();
                goto cheats;
            }
            if (cheats == "sans") // Only traps
            {
                sans = true;
                Console.WriteLine("Maybe it's the way you're dressed?");
                Console.ReadLine();
                Console.Clear();
                goto cheats;
            }
            if (cheats == "konami") // Only double_stat treasure
            {
                konami = true;
                Console.WriteLine("Up up down down down down left left right left right up left down right left up right down left right b a b a b a left down right select start b a b aa left right down select up do...");
                Console.ReadLine();
                Console.Clear();
                goto cheats;
            } 
            // Gameplay loop
            Start:
            if (motherlode || sans || konami)
            {
                if (motherlode)
                {
                    Console.WriteLine("Wow, already?!");
                    goto treasure;
                }
                if (sans)
                {
                    Console.WriteLine("Ah shoot, here we go again..");
                    event_difficulty = "hard";
                    goto fight;
                }
                if (konami)
                {
                    Console.WriteLine("Wow, already!?");
                    treasure_type = 100;
                    goto konami_treasure;
                }
            }
            GenerateRoom();
            string event_type = GenerateEvent();
            switch (event_type)
            {
                case "easy":
                    event_difficulty = "easy";
                    goto fight;
                case "hard":
                    event_difficulty = "hard";
                    goto fight;
                case "treasure":
                    goto treasure;
                case "trap":
                    goto trap;
            }
            // Fight //
            fight:
            Console.Clear();
            // Health //
            if (event_difficulty == "easy")
            {
                Random randhealth = new Random();

                double RandomStat = randhealth.NextDouble();
                int RandomStatMultiplier = randhealth.Next(0,1);

                EnemyStats[0] = PlayerStats[0] * RandomStat + RandomStatMultiplier;
                EnemyStats[0] = Math.Round(EnemyStats[1],0,MidpointRounding.AwayFromZero);
            }
            else
            {
                // Initalize
                Random randhealth = new Random();
                double RandomStat = randhealth.NextDouble();
                // Calculate
                EnemyStats[0] = PlayerStats[0] * RandomStat * 1.5;
                EnemyStats[0] = Math.Round(EnemyStats[0], 0, MidpointRounding.AwayFromZero);
                // Check if enemy's health is too low
                int enemyhealth = Convert.ToInt32(EnemyStats[0]);
                if (enemyhealth < 10){EnemyStats[0] = 10;}
                if (enemyhealth > 2 * PlayerStats[0]) { EnemyStats[0] = PlayerStats[0] / 2; }
            }
            // Output health
            cc("i");
            Console.WriteLine("It has " + EnemyStats[0] + " health.");
            // Power //
            if (event_difficulty == "easy")
            {
                Random randpower = new Random();
                double RandomStat = randpower.NextDouble();
                int RandomStatMultiplier = randpower.Next(0, 1);
                EnemyStats[1] = PlayerStats[1] * RandomStat + RandomStatMultiplier;
                EnemyStats[1] = Math.Round(EnemyStats[1], 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                Random randpower = new Random();
                double RandomStat = randpower.NextDouble();
                EnemyStats[1] = PlayerStats[1] * RandomStat * 1.5;
                EnemyStats[1] = Math.Round(EnemyStats[1], 0, MidpointRounding.AwayFromZero);
                // Check if enemy's power is too low
                int enemypower = Convert.ToInt32(EnemyStats[1]);
                if (enemypower < 10){EnemyStats[0] = 10;}
                if (enemypower > 2 * PlayerStats[1]) { EnemyStats[1] = PlayerStats[1] / 2; }
            }
            cc("i"); Console.WriteLine("It has " + EnemyStats[1] + " power.");
            turn = "p";
            fight_loop:
            if (turn == "p") // Player's Turn
            {
                cc("a"); Console.WriteLine("You attacked!");
                Console.ReadLine();
                // (Calculate Damage)
                Random randdamage = new Random();
                int player_pp = Convert.ToInt32(PlayerStats[1]);
                double playerdmg_send = randdamage.Next(0,player_pp);
                playerdmg_send = Math.Round(playerdmg_send, 0, MidpointRounding.AwayFromZero);
                // Check damage
                if (playerdmg_send == 0)
                {
                    cc("a"); Console.WriteLine("You missed!");
                }
                // Send Damage
                EnemyStats[0] -= playerdmg_send;
                cc("i"); Console.WriteLine("You did " + playerdmg_send + " damage.");
                Console.ReadLine();
                // Check enemy's health
                if (EnemyStats[0] < 1)
                {
                    Console.Clear();
                    PlayerStats[0] += 1;
                    cc("n");  Console.WriteLine("You won the fight and gained a little bit of health back! (You are now at " + PlayerStats[0] + "HP)");
                    Console.ReadLine();
                    goto Start;
                }
                cc("i");
                Console.WriteLine("~~~~~~~~~~~");
                Console.WriteLine("Enemy stats:");
                Console.WriteLine("HP:" + EnemyStats[0]);
                Console.WriteLine("PP:" + EnemyStats[1]);
                Console.WriteLine("~~~~~~~~~~~");
                turn = "e";
                goto fight_loop;
            }
            if (turn == "e") // Enemy's Turn
            {
                cc("a");  Console.WriteLine("It attacked!");
                Console.ReadLine();
                // (Calculate Damage)
                Random randdamage = new Random();
                int enemy_pp = Convert.ToInt32(EnemyStats[1]);
                double enemydmg_send = randdamage.Next(0, enemy_pp);
                enemydmg_send = Math.Round(enemydmg_send, 0, MidpointRounding.AwayFromZero);
                //
                // Check damage
                if (enemydmg_send == 0)
                {
                    cc("a"); Console.WriteLine("It missed!");
                }
                PlayerStats[0] -= enemydmg_send; // (Send Damage)
                cc("i");
                Console.WriteLine("It did " + enemydmg_send + " damage.");
                Console.ReadLine();
                // Check player's health
                if (PlayerStats[0] < 1)
                {
                    Console.Clear();
                    cc("n"); Console.WriteLine("Game over. You died after " + room_number + " rooms. Press enter to try again.");
                    Console.ReadLine();
                    StartGame();
                }
                Console.WriteLine("~~~~~~~~~~~");
                Console.WriteLine("Your stats:");
                Console.WriteLine("HP:" + PlayerStats[0]);
                Console.WriteLine("PP:" + PlayerStats[1]);
                Console.WriteLine("~~~~~~~~~~~");
                Console.ReadLine();
                turn = "p";
                goto fight_loop;
            }
            goto fight_loop;
            // Trap //
            trap:
            cc("a");
            Console.WriteLine("You opened it and found...");
            {
                Random randTense = new Random();
                int i;
                int rt = randTense.Next(3, 10);
                for (i = 0; i < rt; i++)
                {
                    Console.Beep(500 + (100 * i), 250);
                    Console.WriteLine("...");
                }
            }
            cc("n");  Console.WriteLine("An enemy jumps out of the box!");
            Console.ReadLine();
            event_difficulty = "hard";
            goto fight;
            // Treasure //
            treasure:
            cc("a");  Console.WriteLine("You opened it and found...");  
            {
                Random randTense = new Random();
                int i;
                int rt = randTense.Next(3, 10);
                for (i = 0; i < rt; i++)
                {
                    Console.Beep(500 + (100 * i), 250);
                    Console.WriteLine("...");
                }
            }
            // Generate treasure
            Random randtreasure = new Random();
            treasure_type = randtreasure.Next(1,100);
            string treasure = null;
            konami_treasure:
            if (treasure_type >= 90)
            {
                treasure = "double_stats";
            }
            else
            {
                if (treasure_type < 90 && treasure_type > 40)
                {
                    treasure = "+10hp";
                }
                else
                {
                    treasure = "+10pp";
                }
            }
            switch (treasure)
            {
                case "double_stats":
                    PlayerStats[0] *= 2;
                    PlayerStats[1] *= 2;
                    cc("n");  Console.WriteLine("AMAZING! Your stats have been DOUBLED!");
                    Console.ReadLine();
                    Console.WriteLine(" ~ Congratulations! ~");
                    cc("i");
                    Console.WriteLine("HP: " + PlayerStats[0]);
                    Console.WriteLine("PP: " + PlayerStats[1]);
                    cc("n");
                    Console.WriteLine(" ~~~~~~~~~~~~~~~~~~~~");
                    Console.ReadLine();
                    Console.Clear();
                    goto Start;
                case "+10hp":
                    PlayerStats[0] += 10;
                    cc("n"); Console.WriteLine("You gained 10 HP! You now have " + PlayerStats[0] + "HP");
                    Console.ReadLine();
                    goto Start;
                case "+10pp":
                    PlayerStats[1] += 10;
                    cc("n"); Console.WriteLine("You gained 10 PP! You now have " + PlayerStats[1] + "PP");
                    Console.ReadLine();
                    goto Start;
            }
            goto Start; 
        }
    }
}
