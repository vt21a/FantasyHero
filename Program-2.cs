using System;
using System.Reflection;

public class FantasyCharacter
{
    public string Name { get; set; }
    public string Class { get; set; }
    public int Level { get; set; }
    public double HP { get; set; }
    private string secretAbility = "Invisibility";

    public FantasyCharacter(string name, string playerClass, int level, double hp)
    {
        Name = name;
        Class = playerClass;
        Level = level;
        HP = hp;
    }

    public void Attack()
    {
        Console.WriteLine($"{Name} performs a powerful attack!");
    }

    public void Heal()
    {
        HP += 20;
        Console.WriteLine($"{Name} heals for 20 HP. New HP: {HP}");
    }
}

public class Program
{
    public static void Main()
    {
        FantasyCharacter character = new FantasyCharacter("Prilep", "Reptilian", 11, 170.0);
        Type charType = typeof(FantasyCharacter);

        DisplayAttributes(character, charType);

        while (true)
        {
            Console.WriteLine("\nSelect an action:");
            Console.WriteLine("1 - Change an attribute");
            Console.WriteLine("2 - Execute a method");
            Console.WriteLine("3 - Reveal secret ability");
            Console.WriteLine("4 - Exit");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ChangeAttribute(character, charType);
                    break;
                case "2":
                    ExecuteMethod(character, charType);
                    break;
                case "3":
                    UncoverSecretAbility(character, charType);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }

            DisplayAttributes(character, charType);
        }
    }

    public static void DisplayAttributes(object obj, Type type)
    {
        PropertyInfo[] properties = type.GetProperties();
        Console.WriteLine("Attributes:");
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(obj);
            Console.WriteLine($"- {property.Name}: {value}");
        }
    }

    public static void ChangeAttribute(object obj, Type type)
    {
        Console.WriteLine("Enter the name of the property to modify:");
        string propertyName = Console.ReadLine();
        PropertyInfo property = type.GetProperty(propertyName);

        if (property != null)
        {
            Console.WriteLine($"Enter a new value for {propertyName}:");
            string newValue = Console.ReadLine();

            try
            {
                object convertedValue = Convert.ChangeType(newValue, property.PropertyType);
                property.SetValue(obj, convertedValue);
                Console.WriteLine($"Successfully changed {propertyName} to {convertedValue}.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid format for this property.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Error: Property does not exist.");
        }
    }

    public static void ExecuteMethod(object obj, Type type)
    {
        Console.WriteLine("Available methods:");
        MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (MethodInfo method in methods)
        {
            if (method.DeclaringType == typeof(FantasyCharacter))
            {
                Console.WriteLine($"- {method.Name}");
            }
        }

        Console.WriteLine("\nEnter the name of the method you want to execute:");
        string methodName = Console.ReadLine();
        MethodInfo methodInfo = type.GetMethod(methodName);

        if (methodInfo != null)
        {
            methodInfo.Invoke(obj, null);
        }
        else
        {
            Console.WriteLine("Error: Method does not exist.");
        }
    }

    public static void UncoverSecretAbility(object obj, Type type)
    {
        FieldInfo secretField = type.GetField("secretAbility", BindingFlags.NonPublic | BindingFlags.Instance);

        if (secretField != null)
        {
            string ability = (string)secretField.GetValue(obj);
            Console.WriteLine($"The secret ability of the hero is: {ability}");
        }
        else
        {
            Console.WriteLine("Error: No secret abilities found.");
        }
    }
}