using System;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using task_cli.Models;

class Program
{
    static string exeDirectory = AppDomain.CurrentDomain.BaseDirectory; // path .exe file
    static string fileName = "tasks.json";
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.WriteLine("\n\nNo command privided. Use 'help' for a list of commands.\n\n");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "add":
                    AddTask(args);
                    break;
                case "list":
                    chooseListView(args);
                    break;
                case "update":
                    UpdateTask(args);
                    break;
            }
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    public static void chooseListView(string[] args)
    {
        if(args.Length < 2)
        {
            ListTasks();
        }
        else
        {
            string typeListCommand = args[1];

            switch (typeListCommand)
            {
                case "done":
                    ListTasks(EnumTaskStatus.Done.ToString());
                    break;
                case "todo":
                    ListTasks(EnumTaskStatus.Todo.ToString());
                    break;
                case "in-progress":
                    ListTasks(EnumTaskStatus.In_progress.ToString());
                    break;
                case "mark-in-progress":
                case "mark-done":
                case "mark-todo":
                    break;
                case "update":
                    UpdateTask(args);
                    break;
                default:
                    Console.WriteLine("\n\nThe status provider is not valid");
                    break;
            }
        }
    }

    public static void ListTasks([Optional] string? status)
    {
        List<Tasks> tasks = GetListTaks();

        if(status != null)
        {
            tasks = tasks.FindAll(t => t.status == status);
        }

        if (tasks.Any())
        {
            Console.WriteLine("ID".PadRight(5) + "Description".PadRight(30) + "Status".PadRight(15) + "CreationAt".PadRight(25) + "UpdatedAt".PadRight(25));
            Console.WriteLine(new string('-', 100));
            foreach (var task in tasks)
            {
                Console.WriteLine(
                    task.id.ToString().PadRight(5) +
                    task.description.PadRight(30) +
                    task.status.PadRight(15) +
                    task.createdAt.ToString("dd/MM/yyyy hh:mm:ss").PadRight(25) +
                    task.createdAt.ToString("dd/MM/yyyy hh:mm:ss").PadRight(25)
                );
            }
            Console.WriteLine("\n\n");
        }
        else
        {
            Console.WriteLine($"\n\nNot exist tasks with \"{status}\" status.");
        }
    }

    public static void AddTask(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Error - Do you need to add a description for the task. Example: \n\ntask-cli add \"But groceries and cook dinner\"");
        }

        CreateFileIfNotExists();
        SaveTask(args[1]);
    }

    public static void SaveTask(string description)
    {
        List<Tasks> tasks = GetListTaks();
        Tasks task = new Tasks();

        task.id = GetLastTaskId(tasks) + 1;
        task.description = description;

        tasks.Add(task);

        string filePath = GetFilePath();
        string json = File.ReadAllText(filePath);
        json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        
        File.WriteAllText(filePath, json);

        Console.Write($"\n\nTask added successfully (ID: {task.id})");
    }

    public static int GetLastTaskId(List<Tasks> tasks)
    {
        int countTasks = tasks.Count;
        Tasks lastTaks = tasks[countTasks - 1];
        return lastTaks.id;
    }

    public static string GetFilePath()
    {
        return Path.Combine(exeDirectory, fileName);
    }
    public static List<Tasks> GetListTaks()
    {
        string filePath = GetFilePath();

        string json = File.ReadAllText(filePath);
        List<Tasks> tasksList = JsonSerializer.Deserialize<List<Tasks>>(json);

        return tasksList;
    }

    public static void UpdateTask(string[] args)
    {
        var existTasks = ValidateTaskFileContainsTask();

        if (existTasks)
        {
            if (args.Length == 3) {
                string json = File.ReadAllText(GetFilePath());
                var tasks = GetListTaks();
                var taskToEdit = tasks.Find(t => t.id == Int32.Parse(args[1]));

                if(taskToEdit != null)
                {
                    taskToEdit.description = args[2];
                    taskToEdit.updatedAt = DateTime.Now;

                    json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(GetFilePath(), json);
                    
                    Console.WriteLine("Task updated!");
                }
                else
                {
                    Console.WriteLine($"\n\nNot exists a task with the Id: {args[1]}");
                }
                
            }
            else
            {
                Console.WriteLine("\n\nYou need to specify the Task Id and the Description. Example: task-cli update 1 \"Buy some candys\"");
            }
        }
        else
        {
            Console.WriteLine("\n\nNot exists task to edit. Please add some tasks.");
        }
    }

    public static void CreateFileIfNotExists()
    {
        string filePath = GetFilePath();
        
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            
        }
    }

    public static bool ValidateTaskFileContainsTask()
    {
        CreateFileIfNotExists();

        List<Tasks> tasks = GetListTaks();

        if (!tasks.Any())
        {
            return false;
        }
        
        return true;
    }
}