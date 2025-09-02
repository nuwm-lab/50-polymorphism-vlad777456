using System;
using System.Linq;

namespace PraktikantAndPratsivnik
{
    /// <summary>
    /// Базовий клас "Практикант"
    /// </summary>
    class Praktikant
    {
        private string Prizvyshche { get; set; }
        private string Imya { get; set; }
        private string Vuz { get; set; }

        /// <summary>
        /// Загальний метод для зчитування введених даних з консолі
        /// </summary>
        protected string ZapytatyDani(string zapytannya)
        {
            Console.WriteLine(zapytannya);
            return Console.ReadLine();
        }

        /// <summary>
        /// Віртуальний метод для введення даних про практиканта
        /// </summary>
        public virtual void ZadatyDani()
        {
            Prizvyshche = ZapytatyDani("Введіть прізвище практиканта:");
            Imya = ZapytatyDani("Введіть ім'я практиканта:");
            Vuz = ZapytatyDani("Введіть назву навчального закладу:");
        }

        /// <summary>
        /// Перевірка, чи є прізвище симетричним (паліндромом)
        /// </summary>
        public virtual bool ChiSimetrichnePrizvyshche()
        {
            if (string.IsNullOrWhiteSpace(Prizvyshche))
                return false;

            string reversed = new string(Prizvyshche.Reverse().ToArray());
            return Prizvyshche.Equals(reversed, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Метод для демонстрації відсутності поліморфізму
        /// </summary>
        public void NevyrtualnyMethod()
        {
            Console.WriteLine("Це невіртуальний метод класу Praktikant.");
        }
    }

    /// <summary>
    /// Похідний клас "Працівник фірми"
    /// </summary>
    class PratsivnikFirmy : Praktikant
    {
        public DateTime DataPryjomu { get; private set; }
        public string Posada { get; private set; }
        public string ZakincheneVuz { get; private set; }

        /// <summary>
        /// Перевизначення методу для введення даних про працівника
        /// </summary>
        public override void ZadatyDani()
        {
            base.ZadatyDani();

            bool validDate = false;
            while (!validDate)
            {
                try
                {
                    string input = ZapytatyDani("Введіть дату прийому на роботу (в форматі рік-місяць-день):");
                    DataPryjomu = DateTime.Parse(input);

                    if (DataPryjomu > DateTime.Now)
                    {
                        Console.WriteLine("Дата прийому не може бути у майбутньому. Спробуйте ще раз.");
                    }
                    else
                    {
                        validDate = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Невірний формат дати! Спробуйте ще раз.");
                }
            }

            Posada = ZapytatyDani("Введіть посаду працівника:");
            ZakincheneVuz = ZapytatyDani("Введіть навчальний заклад, який закінчив працівник:");
        }

        /// <summary>
        /// Метод для визначення стажу роботи
        /// </summary>
        public int StazhRoboti()
        {
            int stazh = DateTime.Now.Year - DataPryjomu.Year;
            if (DateTime.Now < DataPryjomu.AddYears(stazh))
                stazh--;

            return stazh < 0 ? 0 : stazh;
        }

        /// <summary>
        /// Метод для демонстрації (затінює метод батьківського класу)
        /// </summary>
        public new void NevyrtualnyMethod()
        {
            Console.WriteLine("Це невіртуальний метод класу PratsivnikFirmy.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Praktikant praktikant;
            Console.WriteLine("Виберіть тип об'єкта для створення (1 - Praktikant, 2 - PratsivnikFirmy):");
            string choice = Console.ReadLine();

            if (choice == "2")
                praktikant = new PratsivnikFirmy();
            else
                praktikant = new Praktikant();

            praktikant.ZadatyDani();
            Console.WriteLine($"Прізвище симетричне? {praktikant.ChiSimetrichnePrizvyshche()}");

            praktikant.NevyrtualnyMethod();

            if (praktikant is PratsivnikFirmy pratsivnik)
            {
                Console.WriteLine($"Стаж роботи: {pratsivnik.StazhRoboti()} років");
                pratsivnik.NevyrtualnyMethod();
            }
        }
    }
}
