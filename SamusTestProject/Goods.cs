using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamusTestProject
{
    class Goods
    {
        private int drink;
        private int markup;
        private double new_price;

        public Goods(int id)
        {
            this.drink = id;
        }

        public int get_drink()
        {
            return this.drink;
        }

        public int get_markup()
        {
            return this.markup;
        }

        public void set_markup(int markup)
        {
            this.markup = markup;
        }

        public double get_new_price()
        {
            return this.new_price;
        }

        public void set_new_price(double new_price)
        {
            this.new_price = new_price;
        }

        public void calculate(int markup, double purchase_price)
        {
            this.markup = markup;
            this.new_price = purchase_price / 100 * markup + purchase_price;
        }
    }
}