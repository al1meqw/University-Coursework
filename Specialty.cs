using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityApp
{

    // Клас, що представляє спеціальність з інформацією про конкурс та вартість навчання
    public class Specialty
    {

        // Назва спеціальності
        public string Name { get; set; } = string.Empty;


        // Словник конкурсів за формами навчання (денна, вечірня, заочна)
        // Ключ - форма навчання, значення - конкурс (кількість осіб на місце)
        public Dictionary<string, double> Competition { get; set; } = new Dictionary<string, double>();


        // Словник вартості навчання за формами навчання
        // Ключ - форма навчання, значення - вартість у гривнях
        public Dictionary<string, double> Cost { get; set; } = new Dictionary<string, double>();

        // Застарілі поля для зворотної сумісності
        public double PaymentAmount { get; set; }
        public double TuitionFee { get; set; }


        // Конструктор за замовчуванням
        public Specialty()
        {
            InitializeDefaults();
        }


        // Конструктор з параметрами
        public Specialty(string name)
        {
            Name = name;
            InitializeDefaults();
        }


        // Ініціалізує значення за замовчуванням для форм навчання
        private void InitializeDefaults()
        {
            var forms = new[] { "денна", "вечірня", "заочна" };
            
            foreach (var form in forms)
            {
                if (!Competition.ContainsKey(form))
                    Competition[form] = 0;
                    
                if (!Cost.ContainsKey(form))
                    Cost[form] = 0;
            }
        }

        // Встановлює конкурс для вказаної форми навчання
        public void SetCompetition(string form, double competition)
        {
            if (string.IsNullOrWhiteSpace(form))
                throw new ArgumentException("Форма навчання не може бути пустою");
                
            Competition[form.ToLower()] = Math.Max(0, competition);
        }


        // Встановлює вартість для вказаної форми навчання
        public void SetCost(string form, double cost)
        {
            if (string.IsNullOrWhiteSpace(form))
                throw new ArgumentException("Форма навчання не може бути пустою");
                
            Cost[form.ToLower()] = Math.Max(0, cost);
        }


        // Отримує конкурс для вказаної форми навчання
        public double GetCompetition(string form)
        {
            return Competition.TryGetValue(form?.ToLower() ?? "", out double value) ? value : 0;
        }


        // Отримує вартість для вказаної форми навчання
        public double GetCost(string form)
        {
            return Cost.TryGetValue(form?.ToLower() ?? "", out double value) ? value : 0;
        }


        // Обчислює середній конкурс по всіх формах навчання
        public double GetAverageCompetition()
        {
            var validCompetitions = Competition.Values.Where(c => c > 0).ToList();
            return validCompetitions.Any() ? validCompetitions.Average() : 0;
        }


        // Знаходить мінімальний конкурс серед усіх форм навчання
        public double GetMinCompetition()
        {
            var validCompetitions = Competition.Values.Where(c => c > 0).ToList();
            return validCompetitions.Any() ? validCompetitions.Min() : 0;
        }

    
        // Знаходить максимальний конкурс серед усіх форм навчання
        public double GetMaxCompetition()
        {
            var validCompetitions = Competition.Values.Where(c => c > 0).ToList();
            return validCompetitions.Any() ? validCompetitions.Max() : 0;
        }


        // Перевіряє, чи доступна спеціальність для вказаної форми навчання
        public bool IsAvailableForForm(string form)
        {
            return GetCompetition(form) > 0 || GetCost(form) > 0;
        }


        // Повертає рядкове представлення спеціальності
        public override string ToString()
        {
            return $"{Name} (Середній конкурс: {GetAverageCompetition():F2})";
        }
    }
}