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
                PrintMessage("NotCommandProvided");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "add":
                    AddTask(args);
                    break;
                case "list":
                    ChooseListView(args);
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
                case "help":
                    DisplayHelp();
                    break;
                default:
                    PrintMessage("NotCommandFounded");
                    break;
            }
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    public static void DisplayHelp()
    {
        Console.WriteLine("Available Commands:");
        Console.WriteLine("add <description>             - Adds a new task with the given description.");
        Console.WriteLine("list                          - Lists all tasks.");
        Console.WriteLine("list done                     - Lists all completed tasks.");
        Console.WriteLine("list todo                     - Lists all tasks to do.");
        Console.WriteLine("list in-progress              - Lists all tasks in progress.");
        Console.WriteLine("update <id> <new description> - Updates the description of a task with the given ID.");
        Console.WriteLine("mark-done <id>                - Marks a task with the given ID as done.");
        Console.WriteLine("mark-todo <id>                - Marks a task with the given ID as to-do.");
        Console.WriteLine("mark-in-progress <id>         - Marks a task with the given ID as in progress.");
        Console.WriteLine("delete <id>                   - Deletes a task with the given ID.");
        Console.WriteLine("help                          - Displays this help message.");
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
                    PrintMessage("ConfirmTaskDeletion");
                    string responseToDelete = Console.ReadLine().ToLower();

                    switch (responseToDelete)
                    {
                        case "yes":
                            tasks.Remove(taskToEdit);

                            json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(GetFilePath(), json);

                            PrintMessage("TaskDeletedConfirmation");
                            break;
                        case "no":
                            PrintMessage("OperationCanceledConfirmation");
                            break;
                        default:
                            PrintMessage("TaskDeletionError");
                            break;
                    }
                    
                }
                else
                {
                    PrintMessage("NotExistsTaskId", args[1]);
                }

            }
            else
            {
                PrintMessage("NeedSpecifyTaskIdForDelete");
            }
        }
        else
        {
            PrintMessage("TaskToDeleteNotExist");
        }
    }

    public static void ChooseListView(string[] args)
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
                    PrintMessage("StatusProvidedNotExist");
                    break;
            }
        }
    }

    public static void ChangeTasksStatus(string[] args)
    {

        if (!ValidateTaskFileContainsTask())
        {
            PrintMessage("TaskToEditNotExist");
            return;
        }

        if (args.Length < 2)
        {
            PrintMessage("NeedSpecifyTaskIdForChangeStatus");
            return;
        }
        
        var tasks = GetListTaks();

        if (!TryFindTaskById(tasks, args[1], out var taskToEdit))
        {
            PrintMessage("NotExistsTaskId", args[1]);
            return;
        }

        UpdateTaskStatus(taskToEdit, args[0]);
        SaveTasksToFile(tasks);

        PrintMessage("TaskUpdatedConfirmation");
    }

    public static void ListTasks([Optional] string? status)
    {
        List<Tasks> tasks = GetListTaks();

        if(!tasks.Any())
        {
            PrintMessage("TasksListEmpty");
            return;
        }

        if(!string.IsNullOrEmpty(status))
        {
            tasks = tasks.FindAll(t => string.Equals(t.status, status, StringComparison.OrdinalIgnoreCase));
            
            if (!tasks.Any())
            {
                PrintMessage("NotExistTasksWithStatus", status);
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
            Console.Write(task.id.ToString().PadRight(5));
            Console.Write(task.description.PadRight(30));
            FormatStatusColor(task.status);
            Console.Write(task.status.PadRight(15));
            Console.ResetColor();
            Console.Write(task.createdAt.ToString("dd/MM/yyyy HH:mm:ss").PadRight(25));
            Console.Write(task.updatedAt.ToString("dd/MM/yyyy HH:mm:ss").PadRight(25));
            Console.WriteLine();
        }
        Console.WriteLine("\n\n");
    }

    public static void FormatStatusColor(string status)
    {
        switch (status.ToLower())
        {
            case "done":
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case "in_progress":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "todo":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
    }

    public static void AddTask(string[] args)
    {
        if (args.Length < 2)
        {
            PrintMessage("NeedTaskDescription");
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

        SaveTasksToFile(tasks);

        PrintMessage("TaskAddedSuccessfully", task.id);
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
        if (!ValidateTaskFileContainsTask()){
            PrintMessage("TaskToEditNotExist");
            return;
        }
        
        if (args.Length < 3) {
            PrintMessage("NeedTaskDescriptionExample");
            return;
        }

        var tasks = GetListTaks();
        
        if (!TryFindTaskById(tasks, args[1], out var taskToEdit))
        {
            PrintMessage("NotExistsTaskId", args[1]);
            return;
        }

        UpdateTaskDetails(taskToEdit, args[2]);
        SaveTasksToFile(tasks);
        
        PrintMessage("TaskUpdatedConfirmation");
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

    public static void PrintMessage(string messageKey, params object[] args)
    {
        var message = MessageRepository.GetMessage(messageKey);
        Console.WriteLine(args.Length > 0 ? string.Format(message, args) : message);
    }

    public static bool TryFindTaskById(List<Tasks> tasks, string idString, out Tasks? task)
    {
        task = null;

        if (int.TryParse(idString, out var id))
        {
            task = tasks.Find(t => t.id == id);
        }

        return task != null;
    }

    public static void UpdateTaskDetails(Tasks task, string newDescription)
    {
        task.description = newDescription;
        task.updatedAt = DateTime.Now;
    }

    public static void UpdateTaskStatus(Tasks task, string newStatus)
    {
        task.status = (newStatus.Contains("done") ? EnumTaskStatus.Done : newStatus.Contains("in-progress") ? EnumTaskStatus.In_progress : EnumTaskStatus.Todo).ToString();
        task.updatedAt = DateTime.Now;
    }

    public static void SaveTasksToFile(List<Tasks> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(GetFilePath(), json);
    }
}