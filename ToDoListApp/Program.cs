using System;
using System.Collections.Generic;
using System.IO;

enum Priority
{
    Low,
    Medium,
    High
}

class Program
{
    static List<Task> tasks = new List<Task>();
    static int nextId = 1;

    static void Main()
    {
        LoadTasksFromFile();

        while (true)
        {
            Console.WriteLine("\n--- To-Do List ---");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View Tasks");
            Console.WriteLine("3. Mark Task as Complete");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Edit Task Description");
            Console.WriteLine("6. Sort Tasks by Priority");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ViewTasks();
                    break;
                case "3":
                    MarkTaskAsComplete();
                    break;
                case "4":
                    DeleteTask();
                    break;
                case "5":
                    EditTask();
                    break;
                case "6":
                    SortTasksByPriority();
                    break;
                case "7":
                    SaveTasksToFile();
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again!");
                    break;
            }
        }
    }

    static void SaveTasksToFile()
    {
        using (StreamWriter writer = new StreamWriter("tasks.txt"))
        {
            foreach (var task in tasks)
            {
                writer.WriteLine($"{task.Id},{task.Description},{task.IsCompleted},{task.TaskPriority}");
            }
        }
        Console.WriteLine("Tasks saved successfully!");
    }

    static void LoadTasksFromFile()
    {
        if (File.Exists("tasks.txt"))
        {
            using (StreamReader reader = new StreamReader("tasks.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

                    var parts = line.Split(',');
                    if (parts.Length == 4) // Ensure the line has all expected parts
                    {
                        try
                        {
                            tasks.Add(new Task
                            {
                                Id = int.Parse(parts[0]),
                                Description = parts[1],
                                IsCompleted = bool.Parse(parts[2]),
                                TaskPriority = (Priority)Enum.Parse(typeof(Priority), parts[3])
                            });
                            nextId = Math.Max(nextId, int.Parse(parts[0]) + 1);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing line: {line}. Details: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid task format: {line}");
                    }
                }
            }
            Console.WriteLine("Tasks loaded successfully!");
        }
        else
        {
            Console.WriteLine("No saved tasks found.");
        }
    }

    static void AddTask()
    {
        Console.Write("Enter task description: ");
        string description = Console.ReadLine();

        Console.WriteLine("Choose task priority (0: Low, 1: Medium, 2: High): ");
        if (int.TryParse(Console.ReadLine(), out int priorityChoice) && priorityChoice >= 0 && priorityChoice <= 2)
        {
            Priority taskPriority = (Priority)priorityChoice;
            tasks.Add(new Task { Id = nextId++, Description = description, IsCompleted = false, TaskPriority = taskPriority });
            Console.WriteLine("Task added successfully!");
        }
        else
        {
            Console.WriteLine("Invalid priority choice. Task not added.");
        }
    }

    static void ViewTasks()
    {
        Console.WriteLine("\nTasks:");
        foreach (var task in tasks)
        {
            Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Priority: {task.TaskPriority}, Status: {(task.IsCompleted ? "Completed" : "Pending")}");
        }
    }

    static void MarkTaskAsComplete()
    {
        Console.Write("Enter task ID to mark as complete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = true;
                Console.WriteLine("Task marked as complete!");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
        }
    }

    static void DeleteTask()
    {
        Console.Write("Enter task ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var task = tasks.RemoveAll(t => t.Id == id);
            if (task > 0)
                Console.WriteLine("Task deleted successfully!");
            else
                Console.WriteLine("Task not found.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
        }
    }

    static void EditTask()
    {
        Console.Write("Enter task ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                Console.Write("Enter new description: ");
                task.Description = Console.ReadLine();
                Console.WriteLine("Task description updated successfully!");
            }
            else
            {
                Console.WriteLine("Task not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid task ID.");
        }
    }

    static void SortTasksByPriority()
    {
        var sortedTasks = new List<Task>(tasks);
        sortedTasks.Sort((task1, task2) => task1.TaskPriority.CompareTo(task2.TaskPriority));

        Console.WriteLine("\nSorted Tasks by Priority:");
        foreach (var task in sortedTasks)
        {
            Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Priority: {task.TaskPriority}, Status: {(task.IsCompleted ? "Completed" : "Pending")}");
        }
    }
}

class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Priority TaskPriority { get; set; }
}
