using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UniversityApp
{
    // Головний клас програми для управління базою університетів
    class Program
    {
        private static UniversityManager manager = new UniversityManager();
        private static bool isRunning = true;

        // Точка входу в програму
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Система управління базою університетів ===\n");
            
            // Завантажуємо дані при запуску
            LoadData();

            // Основний цикл програми
            while (isRunning)
            {
                try
                {
                    ShowMainMenu();
                    ProcessMainMenuChoice();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nНесподівана помилка: {ex.Message}");
                    Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                }
            }
        }

        // Відображає головне меню програми
        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              БАЗА УНІВЕРСИТЕТІВ - ГОЛОВНЕ МЕНЮ           ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1.  Додати новий університет                             ║");
            Console.WriteLine("║ 2.  Переглянути всі університети                         ║");
            Console.WriteLine("║ 3.  Знайти університет за назвою                         ║");
            Console.WriteLine("║ 4.  Редагувати університет                               ║");
            Console.WriteLine("║ 5.  Видалити університет                                 ║");
            Console.WriteLine("║ 6.  Пошук та фільтрація                                  ║");
            Console.WriteLine("║ 7.  Статистика та аналіз                                 ║");
            Console.WriteLine("║ 8.  Сортування                                           ║");
            Console.WriteLine("║ 9.  Робота з файлами                                     ║");
            Console.WriteLine("║ 10. Перевірка цілісності даних                           ║");
            Console.WriteLine("║ 0.  Вихід з програми                                     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.Write("\nОберіть пункт меню (0-10): ");
        }

  
        // Обробляє вибір користувача з головного меню
        static void ProcessMainMenuChoice()
        {
            string? input = Console.ReadLine();
            
            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Помилка: Введіть числове значення!");
                PauseAndContinue();
                return;
            }

            switch (choice)
            {
                case 1:
                    AddUniversityMenu();
                    break;
                case 2:
                    ViewAllUniversities();
                    break;
                case 3:
                    SearchUniversityByName();
                    break;
                case 4:
                    EditUniversityMenu();
                    break;
                case 5:
                    DeleteUniversityMenu();
                    break;
                case 6:
                    SearchAndFilterMenu();
                    break;
                case 7:
                    StatisticsMenu();
                    break;
                case 8:
                    SortingMenu();
                    break;
                case 9:
                    FileOperationsMenu();
                    break;
                case 10:
                    ValidateDataMenu();
                    break;
                case 0:
                    ExitProgram();
                    break;
                default:
                    Console.WriteLine("Помилка: Невірний вибір! Оберіть від 0 до 10.");
                    PauseAndContinue();
                    break;
            }
        }


        // Меню додавання нового університету
        static void AddUniversityMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ДОДАВАННЯ НОВОГО УНІВЕРСИТЕТУ ===\n");

            try
            {
                var university = new University();

                // Введення основної інформації
                Console.Write("Введіть назву університету: ");
                university.Name = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Введіть адресу: ");
                university.Address = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Введіть місто: ");
                university.City = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Введіть рейтинг (число): ");
                if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double rating))
                {
                    university.Rating = rating;
                }

                // Додавання спеціальностей
                AddSpecialtiesMenu(university);

                // Додавання університету
                if (manager.AddUniversity(university))
                {
                    Console.WriteLine("\nУніверситет успішно додано!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при додаванні університету: {ex.Message}");
            }

            PauseAndContinue();
        }


        // Меню додавання спеціальностей до університету
        static void AddSpecialtiesMenu(University university, bool askFirst = true)
        {
            Console.WriteLine("\n=== ДОДАВАННЯ СПЕЦІАЛЬНОСТЕЙ ===");

            if (askFirst)
            {
               Console.Write("Бажаете додати спеціальності? (y/n): ");
               if (Console.ReadLine()?.ToLower() != "y") return;
            }

            while (true)
            {
                Console.Write("\nВведіть назву спеціальності (або 'exit' для завершення): ");
                string? specialtyName = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(specialtyName) || specialtyName.ToLower() == "exit")
                    break;

                var specialty = new Specialty(specialtyName);

                // Введення конкурсів
                Console.WriteLine("Введіть конкурс для різних форм навчання (0 - якщо форма недоступна):");
                
                string[] forms = { "денна", "вечірня", "заочна" };
                foreach (string form in forms)
                {
                    Console.Write($"Конкурс ({form}): ");
                    if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double competition))
                    {
                        specialty.SetCompetition(form, competition);
                    }
                }

                // Введення вартості
                Console.WriteLine("Введіть вартість навчання (грн/рік):");
                foreach (string form in forms)
                {
                    Console.Write($"Вартість ({form}): ");
                    if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double cost))
                    {
                        specialty.SetCost(form, cost);
                    }
                }

                university.Specialties.Add(specialty);
                Console.WriteLine($"Спеціальність '{specialtyName}' додано!");
            }
        }

        // Перегляд всіх університетів
        static void ViewAllUniversities()
        {
            Console.Clear();

            if (!manager.Universities.Any())
            {
                Console.WriteLine("Список університетів порожній.");
                PauseAndContinue();
                return;
            }

            Console.WriteLine("Список університетів:\n");

            for (int i = 0; i < manager.Universities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {manager.Universities[i].Name}");
            }

            Console.Write("\nВведіть номер для перегляду деталей або Enter для виходу: ");
            string? input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out int index) &&
                index >= 1 && index <= manager.Universities.Count)
            {
                ViewUniversityDetails(manager.Universities[index - 1]);
            }
        }

        //детальний перегляд університету
        static void ViewUniversityDetails(University university)
        {
            Console.Clear();
            Console.WriteLine($"== {university.Name} ==");
            Console.WriteLine($"Адреса: {university.Address}");
            Console.WriteLine($"Місто: {university.City}");
            Console.WriteLine($"Рейтинг: {university.Rating}");
            Console.WriteLine($"Кількість спеціальностей: {university.Specialties.Count}");

            Console.WriteLine("\nСпеціальності:");
            for (int i = 0; i < university.Specialties.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {university.Specialties[i].Name}");
            }

            Console.Write("\nВведіть номер спеціальності для перегляду або Enter: ");
            string? input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out int index) &&
                index >= 1 && index <= university.Specialties.Count)
            {
                ViewSpecialtyDetails(university.Specialties[index - 1]);
            }

            PauseAndContinue();
        }

        //детальний перегляд спеціальностей
        static void ViewSpecialtyDetails(Specialty specialty)
        {
            Console.Clear();
            Console.WriteLine($"== Спеціальність: {specialty.Name} ==\n");

            string[] forms = { "денна", "вечірня", "заочна" };

            foreach (string form in forms)
            {
                double comp = specialty.GetCompetition(form);
                double cost = specialty.GetCost(form);

                Console.WriteLine($"• {form} — конкурс: {comp}, вартість: {cost} грн");
            }
        }

        // Пошук університету за назвою
        static void SearchUniversityByName()
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК УНІВЕРСИТЕТУ ===\n");

            Console.Write("Введіть назву або частину назви університету: ");
            string? name = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Назва не може бути пустою!");
                PauseAndContinue();
                return;
            }

            var university = manager.SearchByName(name);
            if (university != null)
            {
                manager.DisplayUniversity(university);
            }
            else
            {
                Console.WriteLine($"Університет з назвою '{name}' не знайдено.");
            }

            PauseAndContinue();
        }



        // Меню редагування університету за номером
        static void EditUniversityMenu()
        {
            Console.Clear();
            Console.WriteLine("=== РЕДАГУВАННЯ УНІВЕРСИТЕТУ ===\n");

            if (!manager.Universities.Any())
            {
                Console.WriteLine("Список університетів порожній.");
                PauseAndContinue();
                return;
            }

            // Показуємо нумерований список
            for (int i = 0; i < manager.Universities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {manager.Universities[i].Name}");
            }

            Console.Write("\nВведіть номер університету для редагування: ");
            if (!int.TryParse(Console.ReadLine(), out int number) || number < 1 || number > manager.Universities.Count)
            {
                Console.WriteLine("Некоректний номер.");
                PauseAndContinue();
                return;
            }

            int index = number - 1;
            var existingUniversity = manager.Universities[index];

            Console.WriteLine("\nПоточна інформація:");
            manager.DisplayUniversity(existingUniversity);

            // Створюємо нову версію
            var updatedUniversity = new University();

            Console.WriteLine("\nВведіть нові дані (натисніть Enter для збереження поточного значення):");

            Console.Write($"Назва [{existingUniversity.Name}]: ");
            string? newName = Console.ReadLine()?.Trim();
            updatedUniversity.Name = string.IsNullOrEmpty(newName) ? existingUniversity.Name : newName;

            Console.Write($"Адреса [{existingUniversity.Address}]: ");
            string? newAddress = Console.ReadLine()?.Trim();
            updatedUniversity.Address = string.IsNullOrEmpty(newAddress) ? existingUniversity.Address : newAddress;

            Console.Write($"Місто [{existingUniversity.City}]: ");
            string? newCity = Console.ReadLine()?.Trim();
            updatedUniversity.City = string.IsNullOrEmpty(newCity) ? existingUniversity.City : newCity;

            Console.Write($"Рейтинг [{existingUniversity.Rating}]: ");
            string? ratingInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(ratingInput))
            {
                updatedUniversity.Rating = existingUniversity.Rating;
            }
            else if (double.TryParse(ratingInput, NumberStyles.Float, CultureInfo.InvariantCulture, out double newRating))
            {
                updatedUniversity.Rating = newRating;
            }
            else
            {
                updatedUniversity.Rating = existingUniversity.Rating;
            }

            // Копіюємо спеціальності 
            updatedUniversity.Specialties = existingUniversity.Specialties;

            if (manager.EditUniversityByIndex(index, updatedUniversity))
            {
                Console.WriteLine("Університет успішно оновлено!");
            }


            PauseAndContinue();

            Console.Write("\nБажаєте редагувати спеціальності? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                updatedUniversity.Specialties = EditSpecialties(existingUniversity.Specialties);
            }
            else
            {
                updatedUniversity.Specialties = existingUniversity.Specialties;
            }

            Console.Write("Бажаете додати спеціальності? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                AddSpecialtiesMenu(updatedUniversity, false);
            }

        }

        // Редагування кожної спеціальності вручну
        static List<Specialty> EditSpecialties(List<Specialty> specialties)
        {
            var updated = specialties.ToList(); // копія

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== РЕДАГУВАННЯ СПЕЦІАЛЬНОСТЕЙ ===");

                for (int i = 0; i < updated.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {updated[i].Name}");
                }

                Console.Write("\nВведіть номер спеціальності для редагування (або 0 для виходу): ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int index) || index < 0 || index > updated.Count)
                {
                    Console.WriteLine("Невірне число. Спробуйте ще раз.");
                    PauseAndContinue();
                    continue;
                }

                if (index == 0)
                    break;

                var spec = updated[index - 1];

                Console.WriteLine($"\nРедагування спеціальності: {spec.Name}");

                Console.Write($"Нова назва [{spec.Name}]: ");
                string? newName = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(newName))
                    spec.Name = newName;

                string[] forms = { "денна", "вечірня", "заочна" };
                foreach (string form in forms)
                {
                    Console.Write($"Конкурс ({form}) [{spec.GetCompetition(form)}]: ");
                    input = Console.ReadLine();
                    if (double.TryParse(input, out double newComp))
                    {
                        spec.SetCompetition(form, newComp);
                    }

                    Console.Write($"Вартість ({form}) [{spec.GetCost(form)}]: ");
                    input = Console.ReadLine();
                    if (double.TryParse(input, out double newCost))
                    {
                        spec.SetCost(form, newCost);
                    }
                }

                Console.WriteLine("Редагування завершено.");
                PauseAndContinue();
            }

            return updated;
        }

        // Меню видалення університету за номером зі списку
        static void DeleteUniversityMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ВИДАЛЕННЯ УНІВЕРСИТЕТУ ===\n");

            if (!manager.Universities.Any())
            {
                Console.WriteLine("Список університетів порожній.");
                PauseAndContinue();
                return;
            }

            // Вивід нумерованого списку університетів
            for (int i = 0; i < manager.Universities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {manager.Universities[i].Name}");
            }

            Console.Write("\nВведіть номер університету для видалення: ");
            if (int.TryParse(Console.ReadLine(), out int index) &&
                index >= 1 && index <= manager.Universities.Count)
            {
                string name = manager.Universities[index - 1].Name;

                // Підтвердження
                Console.Write($"Ви впевнені, що хочете видалити '{name}'? (y/n): ");
                string? confirmation = Console.ReadLine()?.Trim().ToLower();

                if (confirmation == "y")
                {
                    manager.RemoveUniversityByIndex(index - 1);
                }
                else
                {
                    Console.WriteLine("Видалення скасовано.");
                }
            }
            else
            {
                Console.WriteLine("Некоректний номер.");
            }

            PauseAndContinue();
        }



        // Меню пошуку та фільтрації
        static void SearchAndFilterMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ПОШУК ТА ФІЛЬТРАЦІЯ ===\n");
            Console.WriteLine("1. Пошук за спеціальністю");
            Console.WriteLine("2. Пошук за містом");
            Console.WriteLine("3. Фільтрація за формою навчання");
            Console.WriteLine("4. Знайти мінімальний конкурс");
            Console.WriteLine("5. Знайти максимальний конкурс");
            Console.WriteLine("0. Повернутися до головного меню");

            Console.Write("\nОберіть опцію: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Невірний вибір!");
                PauseAndContinue();
                return;
            }

            switch (choice)
            {
                case 1:
                    SearchBySpecialty();
                    break;
                case 2:
                    SearchByCity();
                    break;
                case 3:
                    FilterByForm();
                    break;
                case 4:
                    FindMinCompetition();
                    break;
                case 5:
                    FindMaxCompetition();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Невірний вибір!");
                    PauseAndContinue();
                    break;
            }
        }

        // Пошук за спеціальністю
        static void SearchBySpecialty()
        {
            Console.Write("Введіть назву спеціальності: ");
            string? specialty = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(specialty))
            {
                Console.WriteLine("Назва спеціальності не може бути пустою!");
                PauseAndContinue();
                return;
            }

            var universities = manager.FindBySpecialty(specialty);
            
            if (universities.Any())
            {
                Console.WriteLine($"\nЗнайдено {universities.Count} університетів зі спеціальністю '{specialty}':");
                Console.WriteLine(new string('=', 60));
                
                foreach (var uni in universities)
                {
                    manager.DisplayUniversity(uni);
                }
            }
            else
            {
                Console.WriteLine($"Університетів зі спеціальністю '{specialty}' не знайдено.");
            }

            PauseAndContinue();
        }

        // Пошук за містом
        static void SearchByCity()
        {
            Console.Write("Введіть назву міста: ");
            string? city = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(city))
            {
                Console.WriteLine("Назва міста не може бути пустою!");
                PauseAndContinue();
                return;
            }

            var universities = manager.FindByCity(city);
            
            if (universities.Any())
            {
                Console.WriteLine($"\nЗнайдено {universities.Count} університетів у місті '{city}':");
                Console.WriteLine(new string('=', 60));
                
                foreach (var uni in universities)
                {
                    Console.WriteLine($"• {uni.Name} - {uni.Address}");
                    Console.WriteLine($"  Рейтинг: {uni.Rating}, Спеціальностей: {uni.GetSpecialtyCount()}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Університетів у місті '{city}' не знайдено.");
            }

            PauseAndContinue();
        }

        // Фільтрація за формою навчання
        static void FilterByForm()
        {
            Console.WriteLine("Доступні форми навчання:");
            Console.WriteLine("1. денна");
            Console.WriteLine("2. вечірня");
            Console.WriteLine("3. заочна");
            
            Console.Write("Оберіть форму навчання (1-3): ");
            
            string[] forms = { "денна", "вечірня", "заочна" };
            
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
            {
                manager.FilterByForm(forms[choice - 1]);
            }
            else
            {
                Console.WriteLine("Невірний вибір!");
            }

            PauseAndContinue();
        }

        // Пошук мінімального конкурсу
        static void FindMinCompetition()
        {
            Console.Write("Введіть назву спеціальності (або залиште пустим для пошуку серед усіх): ");
            string? specialty = Console.ReadLine()?.Trim();
            
            var result = string.IsNullOrEmpty(specialty) ? 
                manager.FindMinCompetition() : 
                manager.FindMinCompetition(specialty);

            if (result.HasValue)
            {
                Console.WriteLine($"\nМінімальний конкурс:");
                Console.WriteLine($"Університет: {result.Value.UniversityName}");
                Console.WriteLine($"Спеціальність: {result.Value.SpecialtyName}");
                Console.WriteLine($"Форма навчання: {result.Value.Form}");
                Console.WriteLine($"Конкурс: {result.Value.Competition:F2} осіб/місце");
            }
            else
            {
                Console.WriteLine("Дані не знайдено.");
            }

            PauseAndContinue();
        }

        // Пошук максимального конкурсу
        static void FindMaxCompetition()
        {
            Console.Write("Введіть назву спеціальності (або залиште пустим для пошуку серед усіх): ");
            string? specialty = Console.ReadLine()?.Trim();
            
            var result = string.IsNullOrEmpty(specialty) ? 
                manager.FindMaxCompetition() : 
                manager.FindMaxCompetition(specialty);

            if (result.HasValue)
            {
                Console.WriteLine($"\nМаксимальний конкурс:");
                Console.WriteLine($"Університет: {result.Value.UniversityName}");
                Console.WriteLine($"Спеціальність: {result.Value.SpecialtyName}");
                Console.WriteLine($"Форма навчання: {result.Value.Form}");
                Console.WriteLine($"Конкурс: {result.Value.Competition:F2} осіб/місце");
            }
            else
            {
                Console.WriteLine("Дані не знайдено.");
            }

            PauseAndContinue();
        }

        // Меню статистики
        static void StatisticsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СТАТИСТИКА ТА АНАЛІЗ ===\n");
            Console.WriteLine("1. Загальна статистика");
            Console.WriteLine("2. ТОП-3 університети");
            Console.WriteLine("3. Статистика за спеціальностями");
            Console.WriteLine("4. Середній конкурс за спеціальністю");
            Console.WriteLine("0. Повернутися до головного меню");
            
            Console.Write("\nОберіть опцію: ");
            
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Невірний вибір!");
                PauseAndContinue();
                return;
            }

            switch (choice)
            {
                case 1:
                    manager.DisplayGeneralStatistics();
                    break;
                case 2:
                    manager.DisplayTop3();
                    break;
                case 3:
                    ShowSpecialtyStatistics();
                    break;
                case 4:
                    CalculateAverageCompetition();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }

            PauseAndContinue();
        }

        // Відображення статистики за спеціальностями
        static void ShowSpecialtyStatistics()
        {
            var stats = manager.GetSpecialtyStatistics();
            
            if (stats.Any())
            {
                Console.WriteLine("\n=== СТАТИСТИКА ЗА СПЕЦІАЛЬНОСТЯМИ ===");
                Console.WriteLine(new string('=', 50));
                
                foreach (var stat in stats.OrderByDescending(s => s.Value))
                {
                    Console.WriteLine($"{stat.Key}: {stat.Value} спеціальностей");
                }
            }
            else
            {
                Console.WriteLine("Дані відсутні.");
            }
        }

        // Обчислення середнього конкурсу за спеціальністю
        static void CalculateAverageCompetition()
        {
            Console.Write("Введіть назву спеціальності: ");
            string? specialty = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(specialty))
            {
                Console.WriteLine("Назва спеціальності не може бути пустою!");
                return;
            }

            double avgCompetition = manager.CalculateAverageCompetition(specialty);
            
            if (avgCompetition > 0)
            {
                Console.WriteLine($"Середній конкурс за спеціальністю '{specialty}': {avgCompetition:F2} осіб/місце");
            }
            else
            {
                Console.WriteLine($"Дані для спеціальності '{specialty}' не знайдено.");
            }
        }

        // Меню сортування
        static void SortingMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СОРТУВАННЯ ===\n");
            Console.WriteLine("1. Сортувати за рейтингом");
            Console.WriteLine("2. Сортувати за назвою");
            Console.WriteLine("0. Повернутися до головного меню");
            
            Console.Write("\nОберіть опцію: ");
            
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Невірний вибір!");
                PauseAndContinue();
                return;
            }

            switch (choice)
            {
                case 1:
                    manager.SortByRating();
                    break;
                case 2:
                    manager.SortByName();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }

            PauseAndContinue();
        }

        // Меню роботи з файлами
        static void FileOperationsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== РОБОТА З ФАЙЛАМИ ===\n");
            Console.WriteLine("1. Зберегти дані");
            Console.WriteLine("2. Завантажити дані");
            Console.WriteLine("3. Зберегти у новий файл");
            Console.WriteLine("4. Завантажити з іншого файлу");
            Console.WriteLine("0. Повернутися до головного меню");
            
            Console.Write("\nОберіть опцію: ");
            
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Невірний вибір!");
                PauseAndContinue();
                return;
            }

            switch (choice)
            {
                case 1:
                    SaveData();
                    break;
                case 2:
                    LoadData();
                    break;
                case 3:
                    SaveToCustomFile();
                    break;
                case 4:
                    LoadFromCustomFile();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Невірний вибір!");
                    break;
            }

            PauseAndContinue();
        }

        // Збереження даних у файл
        static void SaveData()
        {
            if (FileManager.SaveToFile(manager.Universities))
            {
                Console.WriteLine("Дані успішно збережено!");
            }
            else
            {
                Console.WriteLine("Помилка при збереженні даних!");
            }
        }

        // Завантаження даних з файлу
        static void LoadData()
        {
            manager.Universities = FileManager.LoadFromFile();
        }

        // Збереження у користувацький файл
        static void SaveToCustomFile()
        {
            Console.Write("Введіть шлях до файлу: ");
            string? filePath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Шлях до файлу не може бути пустим!");
                return;
            }

            if (FileManager.Save(filePath, manager.Universities))
            {
                Console.WriteLine($"Дані збережено у файл: {filePath}");
            }
            else
            {
                Console.WriteLine("Помилка при збереженні!");
            }
        }

        // Завантаження з користувацького файлу
        static void LoadFromCustomFile()
        {
            Console.Write("Введіть шлях до файлу: ");
            string? filePath = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Шлях до файлу не може бути пустим!");
                return;
            }

            var universities = FileManager.Load(filePath);
            
            Console.Write("Замінити поточні дані? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                manager.Universities = universities;
                Console.WriteLine("Дані завантажено!");
            }
            else
            {
                Console.WriteLine("Завантаження скасовано.");
            }
        }

        // Меню перевірки цілісності даних
        static void ValidateDataMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ПЕРЕВІРКА ЦІЛІСНОСТІ ДАНИХ ===\n");
            
            bool isValid = manager.ValidateData();
            
            if (isValid)
            {
                Console.WriteLine("✓ Всі дані коректні!");
            }
            else
            {
                Console.WriteLine("✗ Знайдено проблеми з даними. Перегляньте повідомлення вище.");
            }

            PauseAndContinue();
        }

        // Вихід з програми
        static void ExitProgram()
        {
            Console.Write("Зберегти дані перед виходом? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                SaveData();
                Console.WriteLine("\nДякуємо за використання програми!");
            }
            
            isRunning = false;
        }

        // Пауза для читання результатів
        static void PauseAndContinue()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }
    }
}
