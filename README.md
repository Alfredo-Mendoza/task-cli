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

1. Clone the repository or copy the code to your local machine:
   ```bash
   git clone https://github.com/your-username/task-cli.git


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