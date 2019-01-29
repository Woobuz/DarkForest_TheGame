using System;

namespace DF
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                System.Initialize();
            }
        }

    }
    class System
    {
        const string field = " - ",
                     player = " P ",
                     obstacle = " T ",
                     enemy = " ? ",
                     trader = " H ";
        const int size = 20,     //rozmiar mapy
                     density = 10;  //gęstość obiektów (3 drzewa : 1 przeciwnik)
        static int positionX = size / 2 - 1, //pozycja gracza
                     positionY = size / 2 - 1, //
                     trees = 0,
                     enemies = 0,
                     traders = 0,
                     axe = 0;

        static string[,] map = new string[size, size]; //mapa gry

        public static void Initialize()
        {
            buildMap();
            while (Player.hp > 0 && enemies != 0)
            {
                updateMap();
                Console.SetCursorPosition(0, 0);
                if (Player.hp <= 0)
                {
                    Console.Clear();
                    Renders.RenderInfo("You are DEAD!\n");
                    Console.WriteLine("Press enter to restart the game");
                    Console.ReadLine();
                    Console.Clear();
                    resetGame();
                    buildMap();
                }
                if (enemies == 0)
                {
                    Console.Clear();
                    Renders.RenderInfo("You've completed your objective! Congratulations!!!\n");
                    Console.WriteLine("Press enter to restart the game");
                    Console.ReadLine();
                    Console.Clear();
                    resetGame();
                    buildMap();
                }
            }
        } //Główny rdzeń programu
        static void buildMap()
        {
            Random randomSp = new Random();
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    int randomSpawn = randomSp.Next(0, density);
                    if (randomSpawn == 1 || randomSpawn == 2 || randomSpawn == 2 && map[x, y] != player)
                    {
                        map[x, y] = obstacle;
                        trees += 1;
                    }
                    else if (randomSpawn == 0 && map[x, y] != player)
                    {
                        map[x, y] = enemy;
                        enemies += 1;
                    }
                    else
                    {
                        map[x, y] = field;
                    }
                }
            }
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    int randomSpawn = randomSp.Next(0, 100);
                    if (traders < 2 && (randomSpawn == 5 && map[x, y] == field))
                    {
                        map[x, y] = trader;
                        traders++;
                    }
                }
            }
        } //Budowanie Mapy
        static void updateMap()
        {
            Console.WriteLine("WELCOME TO DARK FOREST!\n");
            Renders.RenderInfo("Move with Arrow Keys, interact by going into an object.\nObjective: DISCOVER THE WHOLE FOREST.\n");
            map[positionX, positionY] = player;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    //Console.Write(map[x,y]);
                    switch (map[x, y])
                    {
                        case field:
                            Renders.RenderTerrain(map[x, y]);
                            break;
                        case player:
                            Renders.RenderPlayer(map[x, y]);
                            break;
                        case obstacle:
                            Renders.RenderTree(map[x, y]);
                            break;
                        case enemy:
                            Renders.RenderUnknown(map[x, y]);
                            break;
                        case trader:
                            Renders.RenderTrader(map[x, y]);
                            break;
                    }
                }
                Console.WriteLine();
            }
            showInfo();

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (positionY == 0 || map[positionX, positionY - 1] == obstacle && axe == 0) break;
                    if (positionY == 0 || map[positionX, positionY - 1] == obstacle && axe == 1) { map[positionX, positionY - 1] = field; trees -= 1; break; }
                    if (map[positionX, positionY - 1] == enemy) { enemyEvent(); map[positionX, positionY - 1] = field; enemies--; Console.Clear(); break; }
                    if (map[positionX, positionY - 1] == trader) { tradeWith(); break; }
                    map[positionX, positionY] = field;
                    positionY--;
                    break;
                case ConsoleKey.RightArrow:
                    if (positionY == map.GetLength(1) - 1 || map[positionX, positionY + 1] == obstacle && axe == 0) break;
                    if (positionY == map.GetLength(1) - 1 || map[positionX, positionY + 1] == obstacle && axe == 1) { map[positionX, positionY + 1] = field; trees -= 1; break; }
                    if (map[positionX, positionY + 1] == enemy) { enemyEvent(); map[positionX, positionY + 1] = field; enemies--; Console.Clear(); break; }
                    if (map[positionX, positionY + 1] == trader) { tradeWith(); break; }
                    map[positionX, positionY] = field;
                    positionY++;
                    break;
                case ConsoleKey.UpArrow:
                    if (positionX == 0 || map[positionX - 1, positionY] == obstacle && axe == 0) break;
                    if (positionX == 0 || map[positionX - 1, positionY] == obstacle && axe == 1) { map[positionX - 1, positionY] = field; trees -= 1; break; }
                    if (map[positionX - 1, positionY] == enemy) { enemyEvent(); map[positionX - 1, positionY] = field; enemies--; Console.Clear(); break; }
                    if (map[positionX - 1, positionY] == trader) { tradeWith(); break; }
                    map[positionX, positionY] = field;
                    positionX--;
                    break;
                case ConsoleKey.DownArrow:
                    if (positionX == map.GetLength(0) - 1 || map[positionX + 1, positionY] == obstacle && axe == 0) break;
                    if (positionX == map.GetLength(0) - 1 || map[positionX + 1, positionY] == obstacle && axe == 1) { map[positionX + 1, positionY] = field; trees -= 1; break; }
                    if (map[positionX + 1, positionY] == enemy) { enemyEvent(); map[positionX + 1, positionY] = field; enemies--; Console.Clear(); break; }
                    if (map[positionX + 1, positionY] == trader) { tradeWith(); break; }
                    map[positionX, positionY] = field;
                    positionX++;
                    break;
            }
        } //Poruszanie Graczem po świecie
        static void enemyEvent()
        {
            Enemy enemy = new Enemy("", 0, 0, 0, 0);
            bool isEnemy = false;

            Random rnd = new Random();
            int randomEnemy = rnd.Next(1, 10); //o 1 wiecej
            switch (randomEnemy)
            {
                case 1:
                    enemy = new Enemy("Goblin", 15, 15, 8, 12);
                    isEnemy = true;
                    break;
                case 2:
                    enemy = new Enemy("Thug", 30, 30, 24, 43);
                    isEnemy = true;
                    break;
                case 3:
                    enemy = new Enemy("Dragon", 120, 120, 49, 265);
                    isEnemy = true;
                    break;
                case 4:
                    Random rndG = new Random();
                    int randomGold = rndG.Next(50, 156);
                    Renders.RenderInfo("You have found a huge bag of gold! It contained " + randomGold + "G\n");
                    Player.gold += randomGold;
                    break;
                case 5:
                    enemy = new Enemy("Lihzard", 25, 25, 12, 23);
                    isEnemy = true;
                    break;
                case 6:
                    enemy = new Enemy("Orc", 43, 43, 32, 86);
                    isEnemy = true;
                    break;
                case 7:
                    enemy = new Enemy("Troll", 78, 78, 40, 156);
                    isEnemy = true;
                    break;
                case 8:
                    Renders.RenderInfo("You met a blacksmith. He wanted to help you, so he upgraded you weapon +5.");
                    Player.damage += 5;
                    break;
                case 9:
                    Renders.RenderInfo("Nothing interesting here.");
                    break;
            }
            if (isEnemy)
            {
                Renders.RenderInfo(enemy.name + " has attacked you!");
                Console.ReadLine();
                enemy.fight();
                Console.Clear();
                if (Player.hp <= 0) return;
                Console.WriteLine("You defeated " + enemy.name + ". Earned " + enemy.Gold + "G.");
                Player.gold += enemy.Gold;
            }
            Console.ReadLine();
        } //Walka z Przeciwnikiem
        static void tradeWith()
        {
            int x = 0;

            Renders.RenderInfo("Welcome, Traveller! Let's Trade!");
            Console.ReadLine();
            Console.Clear();
            while (x == 0)
            {
                Renders.RenderInfo("DARK FOREST'S TRADER\t\tAvailable Gold: " + Player.gold);
                Console.WriteLine();
                Console.WriteLine("[P]otion 1/" + Player.potions + "" +
                    "              Price: 20G\n[U]pgrade Weapon 5/" + Player.damage + "" +
                    "      Price: 50G\n[H]ealth+ 10/" + Player.maxHp + "" +
                    "          Price: 80G\n[A]xe " + axe + "/1" +
                    "                 Price: 200G\n[ESC] Go Back");
                ConsoleKeyInfo choose = Console.ReadKey();
                switch (choose.Key)
                {
                    case ConsoleKey.P:
                        if (Player.gold >= 20)
                        {
                            Player.gold -= 20;
                            Player.potions++;
                        }
                        break;
                    case ConsoleKey.U:
                        if (Player.gold >= 50)
                        {
                            Player.damage += 5;
                            Player.gold -= 50;
                        }
                        break;
                    case ConsoleKey.H:
                        if (Player.gold >= 80)
                        {
                            Player.maxHp += 10;
                            Player.gold -= 80;
                        }
                        break;
                    case ConsoleKey.A:
                        if (Player.gold >= 200 && axe == 0)
                        {
                            axe = 1;
                            Player.gold -= 200;
                        }
                        break;
                    case ConsoleKey.Escape:
                        x = 1;
                        break;
                }
                Console.Clear();
            }
            Console.Clear();
        } //Handel
        static void showInfo()
        {
            Console.WriteLine("\t     Player's Health Points: " + Player.hp + "/" + Player.maxHp);
            Console.WriteLine("Trees: " + trees + "           Unknown Places: " + enemies + "           Traders: " + traders);
            Console.WriteLine("              Weapon Power: " + Player.damage + "      Gold: " + Player.gold + "\n");
        } //Wyświetlenie informacji o postaci i świecie
        static void resetGame()
        {
            Player.hp = 100;
            Player.maxHp = Player.hp;
            Player.gold = 50;
            Player.potions = 5;
            Player.damage = 9;
            positionX = size / 2;
            positionY = size / 2;
            trees = 0;
            enemies = 0;
            traders = 0;
            axe = 0;
        }
    }
    class Renders
    {
        public static void RenderTerrain(string value)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(value);
            Console.ResetColor();
        }
        public static void RenderPlayer(string value)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(value);
            Console.ResetColor();
        }
        public static void RenderUnknown(string value)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(value);
            Console.ResetColor();
        }
        public static void RenderTree(string value)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(value);
            Console.ResetColor();
        }
        public static void RenderTrader(string value)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(value);
            Console.ResetColor();
        }
        public static void RenderInfo(string value)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(value);
            Console.ResetColor();
        }
    }
}
