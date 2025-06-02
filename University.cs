using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityApp
{
    // Клас, що представляє університет з його основними характеристиками
    public class University
    {
        // Назва університету
        public string Name { get; set; } = string.Empty;

        // Адреса університету
        public string Address { get; set; } = string.Empty;

        // Місто, в якому розташований університет
        public string City { get; set; } = string.Empty;

        // Список спеціальностей університету
        public List<Specialty> Specialties { get; set; } = new List<Specialty>();

        // Рейтинг університету (1 - найкращий)
        public double Rating { get; set; }

        // Обчислює середній конкурс по всіх спеціальностях університету
        public double GetAverageCompetition()
        {
            if (Specialties == null || !Specialties.Any())
                return 0;

            var allCompetitions = Specialties
                .SelectMany(s => s.Competition.Values)
                .Where(c => c > 0)
                .ToList();

            return allCompetitions.Any() ? allCompetitions.Average() : 0;
        }

        // Повертає кількість спеціальностей в університеті
        public int GetSpecialtyCount()
        {
            return Specialties?.Count ?? 0;
        }

        // Знаходить мінімальний конкурс серед усіх спеціальностей університету
        public double GetMinCompetition()
        {
            if (Specialties == null || !Specialties.Any())
                return 0;

            var allCompetitions = Specialties
                .SelectMany(s => s.Competition.Values)
                .Where(c => c > 0)
                .ToList();

            return allCompetitions.Any() ? allCompetitions.Min() : 0;
        }

        // Знаходить максимальний конкурс серед усіх спеціальностей університету
        public double GetMaxCompetition()
        {
            if (Specialties == null || !Specialties.Any())
                return 0;

            var allCompetitions = Specialties
                .SelectMany(s => s.Competition.Values)
                .Where(c => c > 0)
                .ToList();

            return allCompetitions.Any() ? allCompetitions.Max() : 0;
        }

        // Перевіряє, чи має університет спеціальність з вказаною назвою
        public bool HasSpecialty(string specialtyName)
        {
            return Specialties?.Any(s => 
                s.Name.Equals(specialtyName, StringComparison.OrdinalIgnoreCase)) ?? false;
        }

        // Повертає рядкове представлення університету
        public override string ToString()
        {
            return $"{Name} ({City}) - Рейтинг: {Rating}, Спеціальностей: {GetSpecialtyCount()}";
        }
    }
}