
using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace Serialization
{
    class Program
    {
        private static string StaticDirectory = @"T:\Practical 5";
        //Parallel Arrays:
        private static string[] names = { "John Doe", "Jane Smith", "Alice Johnson" };
        private static string[] addresses = { "123 Elm St", "456 Oak St", "789 Pine St" };

        static void Main(string[] args)
        {
            int choice = 0;
            do
            {
                try
                {
                    Console.Clear();
                    // Get the directory to work in when the program starts
                    Console.Write($"Default directory is {StaticDirectory}\nEnter the directory path you would like to use or press enter to stick with default: ");
                    string sDir = Console.ReadLine();
                    if (sDir != "") StaticDirectory = Regex.Unescape(sDir);

                    Console.Clear();
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Serialize to JSON");
                    Console.WriteLine("2. Deserialise from JSON");
                    Console.WriteLine("3. Print Arrays");
                    Console.WriteLine("4. Exit");
                    Console.Write("Enter your choice: ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1://Serialize the arrays to their respective JSON files
                                Console.Clear();
                                Console.WriteLine("Serializing names array.");
                                SerializeToJson(names);
                                Console.WriteLine("Serializing addresses array.");
                                SerializeToJson(addresses);
                                break;
                            case 2://Deserialize the JSON files back into the arrays
                                Console.Clear();
                                Console.WriteLine("Deserialize names array.");
                                names = DeserializeFromJson(Path.Combine(StaticDirectory, "names.json"));
                                Console.WriteLine("Deserialize addresses array.");
                                addresses = DeserializeFromJson(Path.Combine(StaticDirectory, "addresses.json"));
                                break;
                            case 3://Print the arrays to verify correctness
                                PrintArrays();
                                break;
                            case 4://Exit
                                choice = -1;
                                break;
                            default:
                                Console.WriteLine("Invalid choice.Press any key to try again...");
                                Console.ReadKey();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number. \nPress any key to try again...");
                        Console.ReadKey();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.Write("Press any key to try again...");
                    Console.ReadKey();
                }

            } while (choice != -1);
        }//Main

        static void SerializeToJson(string[] data)
        {
            try
            {
                //2. SerializeToJson
                Console.Write("Enter the file name (including the .json file extenstion): ");
                string fileName = Console.ReadLine();
                string filePath = Path.Combine(StaticDirectory, fileName);
                string jsonString = JsonSerializer.Serialize(data);
                File.WriteAllText(filePath, jsonString);
                Console.WriteLine("File serialized successfully! Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while serializing the file: {ex.Message}");
                Console.Write("Press any key to try again...");
            }
            Console.ReadKey();
        }

        static string[] DeserializeFromJson(string filePath)
        {
            try
            {
                //3. DeserializeFromJson
                Console.WriteLine($"Current target file is {filePath}");
                Console.Write("Enter the file name (including the .json file extenstion) or press Enter to deserialize the default target file: ");
                string fileName2 = Console.ReadLine();
                if (!string.IsNullOrEmpty(fileName2))
                {
                    filePath = Path.Combine(StaticDirectory, fileName2);
                }
                string jsonString = File.ReadAllText(filePath);
                string[] deserializedData = JsonSerializer.Deserialize<string[]>(jsonString);
                
                Console.Write("File deserialized successfully! Press any key to continue...");
                return deserializedData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deserialising the file: {ex.Message}");
                Console.Write("Press any key to try again...");
                Console.ReadKey();
                return new string[0];
            }
        }

        private static void PrintArrays()
        {
            Console.Clear();
            Console.WriteLine("Printing arrays: ");

            //4. PrintArrays Loop
            for(int i=0; i<names.Length; i++)
            {
                Console.WriteLine("Names: " + names[i]+ " Address: " + addresses[i]);
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
