using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityApp
{
    // Клас для управління колекцією університетів та виконання операцій з ними
    public class UniversityManager
    {
        // Колекція університетів
        public List<University> Universities { get; set; } = new List<University>();

        // Додає новий університет до колекції
        public bool AddUniversity(University university)
        {
            try
            {
                if (university == null)
                {
                    Console.WriteLine("Помилка: Неможливо додати пустий університет.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(university.Name))
                {
                    Console.WriteLine("Помилка: Назва університету не може бути пустою.");
                    return false;
                }

                // Перевіряємо, чи не існує вже університет з такою назвою
                if (Universities.Any(u => u.Name.Equals(university.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Помилка: Університет з назвою '{university.Name}' вже існує.");
                    return false;
                }

                Universities.Add(university);
                Console.WriteLine($"Університет '{university.Name}' успішно додано!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при додаванні університету: {ex.Message}");
                return false;
            }
        }


        // Видаляє університет за номером зі списку (0-based індекс)
        public bool RemoveUniversityByIndex(int index)
        {
            try
            {
                if (index < 0 || index >= Universities.Count)
                {
                    Console.WriteLine("Помилка: Невірний номер університету.");
                    return false;
                }

                string removedName = Universities[index].Name;
                Universities.RemoveAt(index);
                Console.WriteLine($"Університет '{removedName}' успішно видалено!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при видаленні університету: {ex.Message}");
                return false;
            }
        }


        // Редагує інформацію про університет за індексом у списку
        public bool EditUniversityByIndex(int index, University updatedUniversity)
        {
            try
            {
                if (index < 0 || index >= Universities.Count)
                {
                    Console.WriteLine("Помилка: Невірний номер університету.");
                    return false;
                }

                if (updatedUniversity == null || string.IsNullOrWhiteSpace(updatedUniversity.Name))
                {
                    Console.WriteLine("Помилка: Дані для оновлення некоректні.");
                    return false;
                }

                var oldUniversity = Universities[index];

                // Якщо назва змінюється, перевіряємо на унікальність
                if (!oldUniversity.Name.Equals(updatedUniversity.Name, StringComparison.OrdinalIgnoreCase) &&
                    Universities.Any(u => u.Name.Equals(updatedUniversity.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Помилка: Університет з назвою '{updatedUniversity.Name}' вже існує.");
                    return false;
                }

                // Оновлюємо дані
                Universities[index] = updatedUniversity;

                Console.WriteLine($"Інформацію про університет '{oldUniversity.Name}' успішно оновлено.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при редагуванні університету: {ex.Message}");
                return false;
            }
        }


        // Відображає всі університети
        public void DisplayAll()
        {
            try
            {
                if (!Universities.Any())
                {
                    Console.WriteLine("Список університетів порожній.");
                    return;
                }

                Console.WriteLine("\n=== Список всіх університетів ===");
                Console.WriteLine(new string('=', 80));

                for (int i = 0; i < Universities.Count; i++)
                {
                    var university = Universities[i];
                    Console.WriteLine($"{i + 1}. {university.Name}");
                    Console.WriteLine($"   Адреса: {university.Address}");
                    Console.WriteLine($"   Місто: {university.City}");
                    Console.WriteLine($"   Рейтинг: {university.Rating}");
                    Console.WriteLine($"   Кількість спеціальностей: {university.GetSpecialtyCount()}");
                    Console.WriteLine(new string('-', 40));
                }

                Console.WriteLine($"\nЗагалом університетів: {Universities.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відображенні списку: {ex.Message}");
            }
        }

        // Пошук університету за назвою
        public University? SearchByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                return Universities.FirstOrDefault(u =>
                    u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при пошуку: {ex.Message}");
                return null;
            }
        }

        // Відображає детальну інформацію про університет
        public void DisplayUniversity(University university)
        {
            try
            {
                if (university == null)
                {
                    Console.WriteLine("Університет не знайдено.");
                    return;
                }

                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine($"ІНФОРМАЦІЯ ПРО УНІВЕРСИТЕТ: {university.Name.ToUpper()}");
                Console.WriteLine(new string('=', 60));
                Console.WriteLine($"Назва: {university.Name}");
                Console.WriteLine($"Адреса: {university.Address}");
                Console.WriteLine($"Місто: {university.City}");
                Console.WriteLine($"Рейтинг: {university.Rating}");
                Console.WriteLine($"Середній конкурс: {university.GetAverageCompetition():F2}");
                Console.WriteLine($"Кількість спеціальностей: {university.GetSpecialtyCount()}");

                Console.WriteLine("\nСПЕЦІАЛЬНОСТІ:");
                Console.WriteLine(new string('-', 60));

                if (!university.Specialties.Any())
                {
                    Console.WriteLine("Спеціальності не додано.");
                }
                else
                {
                    for (int i = 0; i < university.Specialties.Count; i++)
                    {
                        var spec = university.Specialties[i];
                        Console.WriteLine($"\n{i + 1}. {spec.Name}");

                        Console.WriteLine("   Конкурс за формами навчання:");
                        foreach (var comp in spec.Competition)
                        {
                            if (comp.Value > 0)
                                Console.WriteLine($"     {comp.Key}: {comp.Value:F2} осіб/місце");
                        }

                        Console.WriteLine("   Вартість навчання:");
                        foreach (var cost in spec.Cost)
                        {
                            if (cost.Value > 0)
                                Console.WriteLine($"     {cost.Key}: {cost.Value:F2} грн/рік");
                        }
                    }
                }
                Console.WriteLine(new string('=', 60));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відображенні університету: {ex.Message}");
            }
        }

        // Сортує університети за рейтингом (від найкращого до найгіршого)
        public void SortByRating()
        {
            try
            {
                Universities = Universities.OrderBy(u => u.Rating).ToList();
                Console.WriteLine("Університети відсортовано за рейтингом (від найкращого).\n");

                DisplaySortedList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при сортуванні: {ex.Message}");
            }
        }

        // Сортує університети за назвою
        public void SortByName()
        {
            try
            {
                Universities = Universities.OrderBy(u => u.Name).ToList();
                Console.WriteLine("Університети відсортовано за назвою.\n");

                DisplaySortedList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при сортуванні: {ex.Message}");
            }
        }

        // Відображає відсортований список університетів
        private void DisplaySortedList()
        {
            foreach (var university in Universities)
            {
                Console.WriteLine($"Назва: {university.Name}");
                Console.WriteLine($"Адреса: {university.Address}");
                Console.WriteLine($"Місто: {university.City}");
                Console.WriteLine($"Рейтинг: {university.Rating}");
                Console.WriteLine(new string('-', 40));
            }
        }

        // Відображає ТОП-3 університети за рейтингом
        public void DisplayTop3()
        {
            try
            {
                if (!Universities.Any())
                {
                    Console.WriteLine("Список університетів порожній.");
                    return;
                }

                var top3 = Universities.OrderBy(u => u.Rating).Take(3).ToList();

                Console.WriteLine("\n=== ТОП-3 УНІВЕРСИТЕТИ ЗА РЕЙТИНГОМ ===");
                Console.WriteLine(new string('=', 50));

                for (int i = 0; i < top3.Count; i++)
                {
                    var university = top3[i];
                    Console.WriteLine($"{i + 1}. {university.Name}");
                    Console.WriteLine($"   Рейтинг: {university.Rating}");
                    Console.WriteLine($"   Місто: {university.City}");
                    Console.WriteLine($"   Спеціальностей: {university.GetSpecialtyCount()}");
                    Console.WriteLine(new string('-', 30));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відображенні ТОП-3: {ex.Message}");
            }
        }

        // Фільтрує університети за формою навчання
        public void FilterByForm(string form)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(form))
                {
                    Console.WriteLine("Форма навчання не може бути пустою.");
                    return;
                }

                bool found = false;
                form = form.ToLower();

                Console.WriteLine($"\n=== УНІВЕРСИТЕТИ З ФОРМОЮ НАВЧАННЯ: {form.ToUpper()} ===");
                Console.WriteLine(new string('=', 60));

                foreach (var university in Universities)
                {
                    var matchingSpecialties = university.Specialties
                        .Where(s => s.Competition.ContainsKey(form) && s.Competition[form] > 0)
                        .ToList();

                    if (matchingSpecialties.Any())
                    {
                        found = true;
                        Console.WriteLine($"\nУніверситет: {university.Name}");
                        Console.WriteLine($"Місто: {university.City}");
                        Console.WriteLine($"Рейтинг: {university.Rating}");
                        Console.WriteLine("Спеціальності:");

                        foreach (var specialty in matchingSpecialties)
                        {
                            Console.WriteLine($"  • {specialty.Name}");
                            Console.WriteLine($"    Конкурс ({form}): {specialty.Competition[form]:F2}");

                            if (specialty.Cost.ContainsKey(form) && specialty.Cost[form] > 0)
                                Console.WriteLine($"    Вартість ({form}): {specialty.Cost[form]:F2} грн");
                        }
                        Console.WriteLine(new string('-', 40));
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"Нічого не знайдено для форми навчання '{form}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при фільтрації: {ex.Message}");
            }
        }

        // Обчислює середній конкурс за спеціальністю
        public double CalculateAverageCompetition(string specialtyName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(specialtyName))
                    return 0;

                var competitions = Universities
                    .SelectMany(u => u.Specialties)
                    .Where(s => s.Name.Equals(specialtyName, StringComparison.OrdinalIgnoreCase))
                    .SelectMany(s => s.Competition.Values)
                    .Where(c => c > 0)
                    .ToList();

                return competitions.Any() ? competitions.Average() : 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при обчисленні середнього конкурсу: {ex.Message}");
                return 0;
            }
        }

        // Повертає статистику за кількістю спеціальностей у кожному університеті
        public Dictionary<string, int> GetSpecialtyStatistics()
        {
            try
            {
                return Universities.ToDictionary(
                    u => u.Name,
                    u => u.GetSpecialtyCount()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні статистики: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }
    }
}       