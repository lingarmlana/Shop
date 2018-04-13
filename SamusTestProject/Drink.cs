using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamusTestProject
{
    class Drink
    {
        private string type;
        private string name;
        private double purchase_price;
        private string group;
        private double volume;
        private double strength;
        private string composition;
        private int quantity;

        private int purchased;
        private int sales;

        public Drink(string data)
        {
            string[] items = data.Split(';');

            this.type = items[0];
            this.name = items[1];
            this.purchase_price = Convert.ToDouble(items[2]);
            this.group = items[3];
            this.volume = Convert.ToDouble(items[4]);
            this.strength = Convert.ToDouble(items[5]);
            this.composition = items[6];
            this.quantity = Convert.ToInt32(items[7]);

            this.purchased = 0;
            this.sales = 0;
        }

        public string ToString()
        {
            string str = this.type + ";"
                + this.name + ";"
                + this.purchase_price + ";"
                + this.group + ";"
                + this.volume + ";"
                + this.strength + ";"
                + this.composition + ";"
                + this.quantity;
            return str;
        }

        public string report()
        {
            string str = this.name + " "
                + this.volume + " л"
                + ". Продано единиц: "
                + this.sales
                + ". Дозакуплено единиц: "
                + this.purchased;
            return str;
        }
        
        public int get_quantity()
        {
            return this.quantity;
        }

        public void inc_quantity()
        {
            this.quantity++;
        }

        public void inc_quantity(int quantity)
        {
            this.quantity += quantity;
        }

        public void dec_quantity()
        {
            this.quantity--;
        }

        public double get_purchase_price()
        {
            return this.purchase_price;
        }

        public int get_purchased()
        {
            return this.purchased;
        }

        public void inc_purchased(int count)
        {
            this.purchased += count;
        }

        public int get_sales()
        {
            return this.sales;
        }

        public void inc_sales()
        {
            this.sales++;
        }
    }
}