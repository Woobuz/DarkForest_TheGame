using System;
using System.Collections.Generic;

namespace DF
{
    class Player
    {
        public static string name = "Player";
        public static int hp = 100,
                          maxHp = 100,
                          potions = 5,
                          damage = 9,
                          gold = 200;
    }
    class Enemy
    {
        private int hp,
                    maxHp,
                    minDamage,
                    maxDamage,
                    G;
        public string name;
        public int Gold;
        List<string> Communicates = new List<string>() { };

        public Enemy(string name, int hp, int maxHp, int minDamage, int maxDamage, int G)
        {
            this.hp = hp;
            this.name = name;
            this.maxHp = maxHp;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.G = G;
            Gold = this.G;
        }
        public void showCommunicates()
        {
            foreach (string x in Communicates)
            {
                Renders.RenderInfo(x);
                Console.WriteLine();
            }
        }
        public void fight()
        {
            Console.Clear();
            int x = 1;
            while (x == 1)
            {
                Console.Clear();

                Random random = new Random();
                int randomDamage;

                Renders.RenderInfo("Player's Health: " + Player.hp + "/" + Player.maxHp + "         "+name+"'s Health: " + hp + "/" + maxHp + "\t");
                Renders.RenderPotions("  Potions: " + Player.potions+" \n");
                Console.WriteLine("\nPress A to attack\t\t\tPress H to use potion (+25HP)\n");
                showCommunicates();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key) //WALKA
                {
                    case ConsoleKey.A:
                        randomDamage = random.Next(minDamage, maxDamage);
                        Communicates.Add("Player hit " + name + " with " + Player.damage + " damage. Received " + randomDamage + " damage from "+name+".");
                        hp -= Player.damage;
                        if (hp > 0)
                        Player.hp -= randomDamage;
                        break;
                    case ConsoleKey.H:
                        if (Player.potions > 0 && Player.hp < Player.maxHp)
                        {
                            Communicates.Add("Player used healing potion.");
                            Player.hp += 25;
                            if (Player.hp > Player.maxHp) Player.hp = Player.maxHp;
                            Player.potions--;
                        }
                        break;
                }
                if (hp <= 0 || Player.hp <= 0)
                {
                    Communicates.Clear();
                    x = 0;
                }
            }
        }
    }
}
