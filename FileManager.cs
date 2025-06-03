using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace UniversityApp
{

    // Клас для роботи з файлами - збереження та завантаження даних університетів
    public class FileManager
    {
        private const string DefaultFilePath = "universities.json";
        private const string BackupExtension = ".backup";


        // Зберігає список університетів у JSON файл
        public static bool Save(string filePath, List<University> universities)
        {
            try
            {
                if (universities == null)
                {
                    throw new ArgumentNullException(nameof(universities));
                }

                // Створюємо резервну копію існуючого файлу
                CreateBackup(filePath);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    PropertyNamingPolicy = null
                };

                string json = JsonSerializer.Serialize(universities, options);
                File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);

                Console.WriteLine($"Дані успішно збережено у файл: {filePath}");
                Console.WriteLine($"Збережено {universities.Count} університетів");
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Помилка: Недостатньо прав для запису у файл.");
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Помилка: Папка не знайдена.");
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Помилка вводу/виводу: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Несподівана помилка при збереженні: {ex.Message}");
                return false;
            }
        }


        // Завантажує список університетів з JSON файлу
        public static List<University> Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Файл {filePath} не знайдено. Створюється новий список.");
                    return new List<University>();
                }

                var json = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
                
                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("Файл порожній. Створюється новий список.");
                    return new List<University>();
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var universities = JsonSerializer.Deserialize<List<University>>(json, options);
                
                // Валідація завантажених даних
                universities = ValidateAndFixData(universities);
                
                Console.WriteLine($"Дані успішно завантажено з файлу: {filePath}");
                Console.WriteLine($"Завантажено {universities.Count} університетів");
                
                return universities;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Помилка формату JSON: {ex.Message}");
                
                // Спробуємо відновити з резервної копії
                if (TryRestoreFromBackup(filePath, out List<University>? backupData))
                {
                    return backupData;
                }
                
                return new List<University>();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл {filePath} не знайдено.");
                return new List<University>();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Помилка: Недостатньо прав для читання файлу.");
                return new List<University>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Несподівана помилка при завантаженні: {ex.Message}");
                return new List<University>();
            }
        }


        // Зберігає дані з використанням стандартного шляху
        public static bool SaveToFile(List<University> universities)
        {
            return Save(DefaultFilePath, universities);
        }


        // Завантажує дані з використанням стандартного шляху
        public static List<University> LoadFromFile()
        {
            return Load(DefaultFilePath);
        }


        // Створює резервну копію файлу
        private static void CreateBackup(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string backupPath = filePath + BackupExtension;
                    File.Copy(filePath, backupPath, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося створити резервну копію: {ex.Message}");
            }
        }


        // Намагається відновити дані з резервної копії
        private static bool TryRestoreFromBackup(string filePath, out List<University> backupData)
        {
            backupData = new List<University>();
            
            try
            {
                string backupPath = filePath + BackupExtension;
                if (File.Exists(backupPath))
                {
                    Console.WriteLine("Спроба відновлення з резервної копії...");
                    backupData = Load(backupPath);
                    Console.WriteLine("Дані успішно відновлено з резервної копії.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося відновити з резервної копії: {ex.Message}");
            }
            
            return false;
        }


        // Валідує та виправляє завантажені дані
        private static List<University> ValidateAndFixData(List<University>? universities)
        {
            if (universities == null)
                return new List<University>();

            var validUniversities = new List<University>();

            foreach (var university in universities)
            {
                try
                {
                    // Перевіряємо обов'язкові поля
                    if (string.IsNullOrWhiteSpace(university.Name))
                        university.Name = "Невідомий університет";
                    
                    if (string.IsNullOrWhiteSpace(university.Address))
                        university.Address = "Адреса не вказана";
                    
                    if (string.IsNullOrWhiteSpace(university.City))
                        university.City = "Місто не вказане";

                    // Перевіряємо спеціальності
                    if (university.Specialties == null)
                        university.Specialties = new List<Specialty>();

                    // Валідуємо кожну спеціальність
                    var validSpecialties = new List<Specialty>();
                    foreach (var specialty in university.Specialties)
                    {
                        if (specialty != null && !string.IsNullOrWhiteSpace(specialty.Name))
                        {
                            // Ініціалізуємо словники, якщо вони null
                            specialty.Competition ??= new Dictionary<string, double>();
                            specialty.Cost ??= new Dictionary<string, double>();
                            
                            validSpecialties.Add(specialty);
                        }
                    }
                    university.Specialties = validSpecialties;

                    validUniversities.Add(university);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при обробці університету {university?.Name}: {ex.Message}");
                }
            }

            return validUniversities;
        }


        // Перевіряє, чи існує файл
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }


        // Видаляє файл
        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"Файл {filePath} успішно видалено.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при видаленні файлу: {ex.Message}");
                return false;
            }
        }
    }
}
