using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace очередь_в_магазин
{
    class log
    {
        string name;
        StreamWriter fw;
        public log(string name)
        {
            this.name = name;
            File.Delete(name);
            //fw = new StreamWriter(name);
        }
        public void save(string s)
        {
            s += Environment.NewLine;
            File.AppendAllText(name, s);
            //fw.WriteLine(s);
        }
    }
    class product
    {
        public string name;
        public int price;
        public product(string name, int price)
        {
            this.name=name;
            this.price=price;
        }
    }
    class buyer
    {
        public string name;
        private int money;
        public List <product> l;
        log w_l;
        public buyer(string name, int money, log w_l)
        {
            this.name = name;
            this.money = money;
            l= new List<product>();
            this.w_l = w_l;
            string s = "Новый покупатель " + name;
            w_l.save(s);
        }
        public void stay_to_queue(seller s)
        {
            s.queue.Enqueue(this);
            string str = "Покупатель " + name + " стал в очередь к продавцу "+ s.name;
            w_l.save(str);
        }
        public bool pay(int price)
        {
            if (money >= price)
            {
                money -= price;
                return true;
            }
            else
                return false;
        }
    }
    class seller
    {
        public string name;
        int profit;
        public Queue<buyer> queue;
        log w_l;
        public seller(string name, log w_l)
        {
            this.name = name;
            profit = 0;
            queue = new Queue<buyer>();
            this.w_l = w_l;
        }
        public int punch_goods(buyer b)
        {
            int summ=0;
            foreach (product p in b.l)
                summ += p.price;
            return summ;
        }
        public void work()
        {
            while (queue.Count > 0)
            {
                buyer b=queue.Peek(); 
                int summ = punch_goods(b);
                string str = "Продавец пробил товары "+ b.name+ " на сумму " + summ.ToString();
                w_l.save(str);
                if (b.pay(summ))
                {
                    profit += summ;
                    str = "Покупка совершена. Сумма в кассе =  " + profit.ToString();
                    w_l.save(str);
                }
                else
                {
                    str = "У покупателя недостаточно средств";
                    w_l.save(str);
                }
                queue.Dequeue();
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            log log1 = new log("log.txt");

            seller a = new seller("Даша",log1);

            product [] p = new product[5];
            p[0] = new product("хлеб", 20);
            p[1] = new product("масло", 60);
            p[2] = new product("молоко", 40);
            p[3] = new product("каша", 35);
            p[4] = new product("чай", 100);

            buyer b1 = new buyer("Петя", 100,log1);
            b1.l.Add(p[0]);
            b1.l.Add(p[3]);
            b1.stay_to_queue(a);

            buyer b2 = new buyer("Маша", 100,log1);
            b2.l.Add(p[2]);
            b2.l.Add(p[4]);
            b2.stay_to_queue(a);

            a.work();
        }
    }
}
