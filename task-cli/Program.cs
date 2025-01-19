using System.Runtime.InteropServices;
using System.Text.Json;
using task_cli.Messages;
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
                Console.WriteLine(MessageRepository.GetMessage("NotCommandProvided"));
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
                case "mark-in-progress":
                case "mark-done":
                case "mark-todo":
                    ChangeTasksStatus(args);
                    break;
                case "delete":
                    DeleteTask(args);
                    break;
                default:
                    Console.WriteLine(MessageRepository.GetMessage("NotCommandFounded"));
                    break;
            }
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    public static void DeleteTask(string[] args)
    {
        var existTasks = ValidateTaskFileContainsTask();

        if (existTasks)
        {
            if (args.Length == 2)
            {
                string json = File.ReadAllText(GetFilePath());
                var tasks = GetListTaks();
                var taskToEdit = tasks.Find(t => t.id == Int32.Parse(args[1]));

                if (taskToEdit != null)
                {
                    Console.WriteLine(MessageRepository.GetMessage("ConfirmTaskDeletion"));
                    string responseToDelete = Console.ReadLine().ToLower();

                    switch (responseToDelete)
                    {
                        case "yes":
                            tasks.Remove(taskToEdit);

                            json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(GetFilePath(), json);

                            Console.WriteLine(MessageRepository.GetMessage("TaskDeletedConfirmation"));
                            break;
                        case "no":
                            Console.WriteLine(MessageRepository.GetMessage("OperationCanceledConfirmation"));
                            break;
                        default:
                            Console.WriteLine(MessageRepository.GetMessage("TaskDeletionError"));
                            break;
                    }
                    
                }
                else
                {
                    Console.WriteLine(MessageRepository.GetMessage("NotExistsTaskId"), args[1]);
                }

            }
            else
            {
                Console.WriteLine(MessageRepository.GetMessage("NeedSpecifyTaskIdForDelete"));
            }
        }
        else
        {
            Console.WriteLine(MessageRepository.GetMessage("TaskToDeleteNotExist"));
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
            string typeListCommand = args[1].ToLower();

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
                default:
                    Console.WriteLine(MessageRepository.GetMessage("StatusProvidedNotExist"));
                    break;
            }
        }
    }

    public static void ChangeTasksStatus(string[] args)
    {
        var existTasks = ValidateTaskFileContainsTask();

        if (existTasks)
        {
            if (args.Length == 2)
            {
                string json = File.ReadAllText(GetFilePath());
                var tasks = GetListTaks();
                var taskToEdit = tasks.Find(t => t.id == Int32.Parse(args[1]));

                if (taskToEdit != null)
                {
                    taskToEdit.status = (args[0].Contains("done") ? EnumTaskStatus.Done : args[0].Contains("in-progress") ? EnumTaskStatus.In_progress : EnumTaskStatus.Todo).ToString();
                    taskToEdit.updatedAt = DateTime.Now;

                    json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(GetFilePath(), json);

                    Console.WriteLine(MessageRepository.GetMessage("TaskUpdatedConfirmation"));
                }
                else
                {
                    Console.WriteLine(MessageRepository.GetMessage("NotExistsTaskId"), args[1]);
                }

            }
            else
            {
                Console.WriteLine(MessageRepository.GetMessage("NeedSpecifyTaskIdForChangeStatus"));
            }
        }
        else
        {
            Console.WriteLine(MessageRepository.GetMessage("TaskToEditNotExist"));
        }
    }

    public static void ListTasks([Optional] string? status)
    {
        List<Tasks> tasks = GetListTaks();

        if(!tasks.Any())
        {
            Console.WriteLine(MessageRepository.GetMessage("TasksListEmpty"));
            return;
        }

        if(!string.IsNullOrEmpty(status))
        {
            tasks = tasks.FindAll(t => string.Equals(t.status, status, StringComparison.OrdinalIgnoreCase));
            
            if (!tasks.Any())
            {
                Console.WriteLine(MessageRepository.GetMessage("NotExistTasksWithStatus"), status);
                return;
            }
        }

        DisplayTaskAsTable(tasks);
    }

    public static void DisplayTaskAsTable(List<Tasks> tasks)
    {
        Console.WriteLine("ID".PadRight(5) + "Description".PadRight(30) + "Status".PadRight(15) + "CreationAt".PadRight(25) + "UpdatedAt".PadRight(25));
        Console.WriteLine(new string('-', 100));
        foreach (var task in tasks)
        {
            Console.WriteLine(
                task.id.ToString().PadRight(5) +
                task.description.PadRight(30) +
                task.status.PadRight(15) +
                task.createdAt.ToString("dd/MM/yyyy HH:mm:ss").PadRight(25) +
                task.createdAt.ToString("dd/MM/yyyy HH:mm:ss").PadRight(25)
            );
        }
        Console.WriteLine("\n\n");
    }

    public static void AddTask(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine(MessageRepository.GetMessage("NeedTaskDescription"));
        }
        else
        {
            CreateFileIfNotExists();
            SaveNewTask(args[1]);
        }

    }

    public static void SaveNewTask(string description)
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

        Console.Write(MessageRepository.GetMessage("TaskAddedSuccessfully"), task.id);
    }

    public static int GetLastTaskId(List<Tasks> tasks)
    {
        if (tasks.Any())
        {
            int countTasks = tasks.Count;
            Tasks lastTaks = tasks[countTasks - 1];
            return lastTaks.id;
        }

        return 0;
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
                    
                    Console.WriteLine(MessageRepository.GetMessage("TaskUpdatedConfirmation"));
                }
                else
                {
                    Console.WriteLine(MessageRepository.GetMessage("NotExistsTaskId"), args[1]);
                }
                
            }
            else
            {
                Console.WriteLine(MessageRepository.GetMessage("NeedTaskDescriptionExample"));
            }
        }
        else
        {
            Console.WriteLine(MessageRepository.GetMessage("TaskToEditNotExist"));
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