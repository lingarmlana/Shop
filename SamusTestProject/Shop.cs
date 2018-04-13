using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SamusTestProject
{
    class Shop
    {
        List<Drink> drinks;
        private double proceed_sum; //выручка
        private double purchase_sum;//потрачено на дозакупку

        private const int hour_open = 8;//время открытия магазина
        private const int hour_close = 21;//время закрытия магазина

        private DateTime time;
        private Random rand;
        private string db_header;
        

        /*
         * Shop construct
         * */
        public Shop()
        {
            this.drinks = new List<Drink>();
            this.proceed_sum = 0;
            this.purchase_sum = 0;
            this.time = DateTime.Today;
            this.time += new TimeSpan(hour_open, 0, 0);//время открытия магазина
            this.rand = new Random();
            this.db_header = "";
            this.read_csv();//наполняем магазин товарами из CSV файла
        }

        /*
         * Загрузка БД из CSV файла
         * */
        private void read_csv()
        {
            StreamReader sr = new StreamReader(@"goods.csv");
            db_header = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                drinks.Add(new Drink(sr.ReadLine()));
            }
            sr.Close();
            Console.WriteLine("База данных магазина загружена в программу.");
        }

        /*
         * Сохранение БД в CSV файл
         */
        private void save_csv()
        {
            StreamWriter sw = new StreamWriter(@"goods.csv");
            sw.WriteLine(db_header);
            foreach (Drink d in drinks)
            {
                sw.WriteLine(d.ToString());
            }
            sw.Close();
            sw.Dispose();
            Console.WriteLine("Файл базы данных магазина обновлён.");
        }

        /*
         * Отчет в текстовый файл в конце месяца
         */
        private void report()
        {
            StreamWriter sw = new StreamWriter(@"report_" + DateTime.Today.ToShortDateString() + ".txt");
            foreach(Drink d in drinks)
            {
                sw.WriteLine(d.report());
            }
            sw.WriteLine("Прибыль магазина от продаж: " + Math.Round(this.proceed_sum, 2) + " грн.");
            sw.WriteLine("Затраченные средства на дозакупку товара: " + Math.Round(this.purchase_sum, 2) + " грн.");
            sw.Close();
            Console.WriteLine("Сгенерирован файл отчёта за 30 дней.");
        }

        /*
         * продажа товара
         */

        private void sale(int num_buyers)
        {
            List<Goods> order;
            int count;
            int goods_id;

            for (int i = 1; i <= num_buyers; i++)//одна итерация == один покупатель
            {
                count = rand.Next(0, 10);//количество товаров одного покупателя
                if (count > 0)
                {
                    Console.WriteLine("Заказ " + i + "-го покупателя. Товаров: " + count);
                    order = new List<Goods>();
                    while(count > 0)
                    {
                        goods_id = rand.Next(0, drinks.Count);
                        if (drinks[goods_id].get_quantity() <= 0)
                        {
                            continue;
                        }
                        order.Add(new Goods(goods_id));
                        count--;
                    }
                    order = this.markup(order);//расчёт наценки и стоимости каждого товара
                    foreach (Goods goods in order)
                    {
                        drinks[goods.get_drink()].dec_quantity();
                        drinks[goods.get_drink()].inc_sales();
                        this.proceed_sum += goods.get_new_price() - drinks[goods.get_drink()].get_purchase_price();
                        Console.WriteLine("Товар: " + goods.get_drink() + ". Наценка: " + goods.get_markup() + "% К оплате: " + goods.get_new_price() + " грн.");
                    }
                }
                else
                {
                    Console.WriteLine("Покупатель " + i + " не сделал заказ");
                }
                Console.WriteLine();
            }
        }

        /*
         * дозакупка товара
         */
        private void purchase()
        {
            int batch = 150;//количество товара в одной закупочной партии
            for(int i = 0; i < drinks.Count; i++)
            {
                if (drinks[i].get_quantity() < 10)
                {
                    drinks[i].inc_quantity(batch);
                    drinks[i].inc_purchased(batch);
                    this.purchase_sum += drinks[i].get_purchase_price() * batch;
                }
            }
        }

        /*
         * Расчёт наценки и стоимости товара
         */
        private List<Goods> markup(List<Goods> order)
        {
            order = order.OrderBy(obj => obj.get_drink()).ToList();
            int last_id = -1;
            int count = 0;
            for(int i = 0; i < order.Count; i++)
            {
                if (order[i].get_drink() == last_id)
                {
                    count++;
                }
                else
                {
                    last_id = order[i].get_drink();
                    count = 1;
                }

                if(count > 2)
                {
                    order[i].calculate(7, drinks[order[i].get_drink()].get_purchase_price());
                    continue;
                }
                if(time.Hour > 16 && time.Hour < 20)
                {
                    order[i].calculate(8, drinks[order[i].get_drink()].get_purchase_price());
                    continue;
                }
                if(time.DayOfWeek == DayOfWeek.Saturday && time.DayOfWeek == DayOfWeek.Sunday)
                {
                    order[i].calculate(15, drinks[order[i].get_drink()].get_purchase_price());
                    continue;
                }
                order[i].calculate(10, drinks[order[i].get_drink()].get_purchase_price());
            }
            return order;
        }

        /*
         * Эмуляция одного часа работы магазина
         */
        private void one_hour()
        {
            Console.WriteLine("  --------  TIME  --------  " + this.time.Hour.ToString() + ":00  --------  TIME  --------  ");
            this.sale(rand.Next(1, 10));
            this.time += new TimeSpan(1, 0, 0);
            purchase();
        }

        /*
         * Эмуляция одного дня работы магазина
         */
        private void one_day()
        {
            Console.WriteLine("  --------  DAY  --------  " + this.time.DayOfWeek + " " + this.time.ToLongDateString() + "  --------  DAY  --------  ");
            while (this.time.Hour < hour_close)
            {
                this.one_hour();
            }
            this.purchase();
            this.time = this.time.AddHours(24.00 - (hour_close - hour_open));
        }

        /*
         * Эмуляция тридцати дней работы магазина
         */
        public void one_month()
        {
            Console.WriteLine("one_month");
            for (int i = 0; i < 30; i++)
            {
                this.one_day();
            }
            report();
            save_csv();
        }
    }
}