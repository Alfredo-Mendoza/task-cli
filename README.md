# Task CLI Tool

A simple command-line application to manage tasks. It allows you to add, update, list, delete, and change the status of tasks.

## Features
- Add tasks with descriptions.
- List all tasks or filter by status (`todo`, `in-progress`, `done`).
- Update task descriptions.
- Change task statuses (`todo`, `in-progress`, `done`).
- Delete tasks by ID.
- Display help with `help`.

## Installation

Follow these steps to clone, build, and run the project on your local machine.

### Prerequisites
- [Download and install .NET SDK 8.0](https://dotnet.microsoft.com/download).
- A code editor or IDE (e.g., Visual Studio Code, Visual Studio, or Rider).
- Git installed for cloning the repository.

---

### Steps to Set Up

#### 1. Clone the Repository
   1. Open your preferred web browser and go to the repository's GitHub page.
   2. Click the green "Code" button and select "Download ZIP" or copy the repository link if you want to clone using Git.
   3. If using Git:
      - Open **Git Bash** (or terminal on macOS/Linux).
      - Navigate to the folder where you want to save the project.
      - Run the following command:
        ```bash
        git clone https://github.com/Alfredo-Mendoza/task-cli.git
        ```
   4. Once downloaded or cloned, open the project folder.

#### 2. Publish the Project
   1. Open **Visual Studio** or **Visual Studio Code**.
   2. Open the cloned project folder in the editor.
   3. For **Visual Studio**:
      - Go to the **Build** menu at the top.
      - Click on **Publish**.
      - Choose **Folder** and set the output directory to `./dist`.
      - Click on **Publish**.
   4. For **Visual Studio Code** (using terminal):
      - Open the **Terminal** in Visual Studio Code (from the **View** menu, select **Terminal**).
      - In the terminal, type:
        ```bash
        dotnet publish -c Release -o ./dist
        ```
      - This will create a `dist` folder with the published executable files.

#### 3. Set the Environment Variable for the Executable
   1. **Windows**:
      - Press `Win + R` to open the **Run** dialog.
      - Type `sysdm.cpl` and hit **Enter** to open the System Properties.
      - Go to the **Advanced** tab.
      - Click **Environment Variables**.
      - Under **System Variables**, click **New**.
      - Set the **Variable Name** as `TASK_CLI_PATH` and **Variable Value** as the path to the `task-cli.exe` file in the `dist` folder. For example: `C:\path\to\dist\task-cli.exe`.
      - Click **OK** and then **OK** again to close the window.

#### 4. Additional Notes
- If you make any changes to the source code, remember to repeat the publish step to update the published files.
- To get help with the application, you can run:
  ```bash
  task-cli help
  ```


## Usage Examples

### Main Commands

#### 1. Add a Task
Create a new task with a description.  
```bash
task-cli add "<Task description>"
```

#### 2. Update a task
Update a task changin description
```bash
task-cli update <Task-id> "<New task description>"
```

#### 3. Update a task status
Update a task status
```bash
task-cli mark-done <Task-id>
task-cli mark-in-progress <Task-id>
task-cli mark-todo <Task-id>
```

#### 4. List tasks 
1. List all tasks

    ```bash
    task-cli list
    ```
2. List all tasks with 'done' status

    ```bash
    task-cli list done
    ```
3. List all tasks with 'in-progress' status

    ```bash
    task-cli list in-progress
    ```
4. List all tasks with 'todo' status

    ```bash
    task-cli list todo
    ```

#### 4. Delete a task
Delete a task
```bash
task-cli delete <Task-id>
```

## Contributions
Thank you for your interest in contributing to this project! To keep it clean and accessible for everyone, please follow these guidelines:

### Report Issues
If you encounter a bug or have suggestions for new features, open an [issue](#).

## Submit Improvements

1. **Fork the repository.**  
2. **Create a new branch for your changes:**  
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. Make your changes and commit them with clear messages:
    ```bash
   git commit -m "Description of changes"
   ```
4. Submit a pull request to the main repository.

## Follow Style Guidelines
Ensure your code adheres to the project's coding standards and document all changes.

## License
This project is licensed under the MIT License, allowing the use, copying, modification, and distribution of the code, provided this license is preserved in all versions and derivatives. This ensures that the project remains open source.