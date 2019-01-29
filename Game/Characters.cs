using System;

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
                    damage,
                    G;
        public string name;
        public int Gold;
        public Enemy(string name, int hp, int maxHp, int damage, int G)
        {
            this.hp = hp;
            this.name = name;
            this.maxHp = maxHp;
            this.damage = damage;
            this.G = G;
            Gold = this.G;
        }
        public void fight()
        {
            Console.Clear();
            int x = 1;
            while (x == 1)
            {
                Console.Clear();
                Console.WriteLine("Player's Health: " + Player.hp + "/" + Player.maxHp + "         Enemy's Health: " + hp + "/" + maxHp + "\nPotions: " + Player.potions);
                Console.WriteLine("\nPress A to attack / Press H to use potion (+25HP)");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key) //WALKA
                {
                    case ConsoleKey.A:
                        hp -= Player.damage;
                        Player.hp -= damage;
                        break;
                    case ConsoleKey.H:
                        if (Player.potions > 0 && Player.hp < Player.maxHp)
                        {
                            Player.hp += 25;
                            if (Player.hp > Player.maxHp) Player.hp = Player.maxHp;
                            Player.potions--;
                        }
                        break;
                }
                if (hp <= 0 || Player.hp <= 0)
                {
                    x = 0;
                }
            }
        }
    }
}
