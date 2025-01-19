using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task_cli.Messages
{
    public static class MessageRepository
    {
        private static readonly Dictionary<string, string> Messages = new Dictionary<string, string>
    {
        { "NotCommandProvided", "Command no privided. Use 'help' for a list of commands." },
        { "NotCommandFounded", "No command founded. Use 'help' for a list of commands."},
        { "ConfirmTaskDeletion", "Are you sure want to remove the task? Yes/No" },
        { "TaskDeletedConfirmation", "Task Deleted!" },
        { "OperationCanceledConfirmation", "Operation canceled!" },
        { "TaskDeletionError", "Response not valid. Operation canceled!" },
        { "NeedSpecifyTaskIdForChangeStatus", "You need to specify the Task Id. Example: \n\ntask-cli mask-done 1"},
        { "NeedSpecifyTaskIdForDelete", "You need to specify the Task Id. Example: \n\ntask-cli delete 1"},
        { "TaskToEditNotExist", "Not exist tasks to edit. Please add first some tasks." },
        { "TaskToDeleteNotExist", "Not exist tasks to delete. Please add first some tasks." },
        { "StatusProvidedNotExist", "The status provided is not valid"},
        { "TaskUpdatedConfirmation", "Task updated!" },
        { "NotExistsTaskId", "Not exists a task with the Id: {0}"},
        { "NotExistTasksWithStatus", "Not exist tasks with \"{0}\" status."},
        { "NeedTaskDescription", "Error - Do you need to add a description for the task. Example: \n\ntask-cli add \"Buy groceries and cook dinner\""},
        { "NeedTaskDescriptionExample", "You need to specify the Task Id and the Description. Example: task-cli update 1 \"Buy some candys\""},
        { "TaskAddedSuccessfully", "Task added successfully (ID: {0})\n"}
    };

        public static string GetMessage(string key)
        {
            return Messages.ContainsKey(key) ? Messages[key] : "Mensaje desconocido.";
        }
    }
}
