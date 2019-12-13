using System;
using System.Collections.Generic;
using System.Text;


namespace TODOLister
{
    class TodoHandler
    {
        private List<Task> tasks = new List<Task>();        //Create list of tasks
        private Database db = new Database();               //Create database object

        public void Start() //Start the program by showing the menu
        {
            ShowMenu();
        }

        /// <summary>
        /// Shows all the tasks saved in the task list for the user
        /// </summary>
        private void ShowTasks()
        {
            foreach (Task t in tasks)
            {
                Console.WriteLine($"{t.Id}. Title: { t.Title}");
                Console.WriteLine(t.Description);
                Console.WriteLine();
            }
        }
        private void ShowMenu()
        {
            tasks = db.GetAllTasks();       //Update the task list by getting all tasks from database
            while (true)
            {
                Console.Clear();

                ShowTasks();    //Show tasks for the user

                //Show the menu
                Console.WriteLine(" ~ Menu ~ ");
                Console.WriteLine("1. Create new task");
                Console.WriteLine("2. Edit existing task");
                Console.WriteLine("3. Remove task");
                Console.WriteLine("0. Exit application");
                Console.WriteLine();

                Console.Write("Select: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    default:    //If the user did not write any of the available options
                        Console.WriteLine();
                        Console.WriteLine("That option does not exist!");
                        Console.Write("Press enter to try again!");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case "1":   //If the user wants to create a task
                        Console.Clear();
                        CreateTask();
                        break;
                    case "2":   //If the user wants to edit a task
                        Console.Clear();
                        EditTask();
                        break;
                    case "3":   //If the user wants to remove a task
                        Console.Clear();
                        RemoveTask();
                        break;
                    case "0":   //If the user wants to exit the application
                        Environment.Exit(1);        //Exits application without error message
                        break;

                }

            }


        }

        #region HandleTasks
        /// <summary>
        /// Creates a new task to save in the database
        /// </summary>
        private void CreateTask()
        {
            Task task = new Task();     //Create a new task which will hold the chosen task from the task list

            Console.WriteLine(" ~ Create task ~ ");
            Console.WriteLine("Write 'abort' to cancel and go back to the menu");
            Console.WriteLine();
            Console.WriteLine();


            //Set title for new task
            Console.Write("Title for the task: ");
            string input = Console.ReadLine();

            //If the input was empty or only had white spaces
            while (String.IsNullOrWhiteSpace(input))
            {   
                //Show error message in the right place
                Console.SetCursorPosition(0, 3);
                Console.WriteLine("Please try again...");

                //Gives the user a new try
                Console.Write("Title for the task: ");
                input = Console.ReadLine();
            }

            //If the user wants to cancel creating a new task
            if (input.ToLower() == "abort")
            {
                return;     //Leaves current function and goes back to the start menu
            }

            task.Title = input;     //Set the title of the new task to the users input

            Console.WriteLine();

            //Set description for new task
            Console.Write("Description for the task: ");
            input = Console.ReadLine();

            //If the input was empty or only had white spaces
            while (String.IsNullOrWhiteSpace(input))
            {
                //Show error message in the right place
                Console.SetCursorPosition(0, 5);
                Console.WriteLine("Please try again...");

                //Gives the user a new try
                Console.Write("Description for the task: ");
                input = Console.ReadLine();
            }

            //If the user wants to cancel creating a new task
            if (input.ToLower() == "abort")
            {
                return;     //Leaves current function and goes back to the start menu
            }

            task.Description = input;       //Set the description of the new task to the users input

            db.AddTask(task);       //Send the new task to the database

            tasks = db.GetAllTasks();   //Update the list containing all tasks
        }
        /// <summary>
        /// Edits a task with new title and/or description
        /// </summary>
        private void EditTask()
        {
            Task task = new Task();     //Create new task containing new title and/or description for an old task

            while (true)
            {
                int taskId = 0;     //taskId is the task that the user wrote he/she wants to edit converted to an int

                while (true)
                {
                    string input = null;    //the users chosen task id to edit

                    //Main use of try is to see if the users input can be converted to an int
                    try
                    {
                        Console.WriteLine(" ~ Edit task ~ ");
                        Console.WriteLine();
                        Console.WriteLine();

                        ShowTasks();        //Show all current tasks to the user

                        Console.WriteLine("Write 'abort' to cancel and go back to the edit menu");
                        Console.WriteLine("Which task do you want to edit?");

                        Console.Write("Task: ");
                        input = Console.ReadLine();

                        //if the user wants to go back to the main menu
                        if (input.ToLower() == "abort")
                        {
                            return; //Return to the main menu
                        }
                        taskId = int.Parse(input);  //Convert user input to an int, id will always be an int
                        break;    //If the conversion was successfull the while-loop will end
                    }
                    catch (Exception) //If the user input could not be converted to an int
                    {
                        ErrorMessage($"{input}");   //Show error message
                    }
                }

                //Look for the chosen task id in the tasks list
                foreach (Task t in tasks)
                {
                    //If the task id exists in the list
                    if (t.Id == taskId)
                    {
                        
                        task = t;   //Set the task to the task found in the list/chosen by the user
                    }
                }

                //If the task does exist
                if (task.Id != 0)
                {
                    Task updatedTask = new Task();  //Create a new task containing the new info the user wants to update the old task with
                    updatedTask.Id = task.Id;   //Set the id of the new task info to the chosen task's id

                    //Show the choosen task's info
                    Console.Clear();
                    Console.WriteLine($" ~ Edit task {task.Id} ~ ");
                    Console.WriteLine($"Title of task: {task.Title}");
                    Console.WriteLine($"Description of task: {task.Description}");
                    Console.WriteLine();


                    ///Set the new title and description

                    Console.Write("New title for the task (Leave blank to keep current title): ");
                    updatedTask.Title = Console.ReadLine();

                    //If the user wrote a new title that is the same as the old title
                    if (updatedTask.Title == task.Title)
                    {
                        Console.WriteLine($"{updatedTask.Title} is the current title of task {task.Id}. The title will not change");
                        updatedTask.Title = null; //Make the title null. The same title will then not be written in the database
                    }

                    Console.Write("Description for the task (Leave blank to keep current title): ");
                    updatedTask.Description = Console.ReadLine();

                    //If the user wrote a new description that is the same as the old description
                    if (updatedTask.Description == task.Description)
                    {
                        Console.WriteLine($"{updatedTask.Description} is the current description of task {task.Id}. The description will not change");
                        updatedTask.Description = null; //Make the description null. The same description will then not be written in the database
                    }

                    db.EditTask(updatedTask);       //Sends the new task to db to update

                    tasks = db.GetAllTasks();       //Refresh the task list
                    Console.ReadLine();
                    break;  //Exit while-loop and this whole function
                }
                else        //If the task does not exist
                {
                    ErrorMessage($"{taskId}");      //Show error message
                }
            }
        }
        /// <summary>
        /// Removes a task
        /// </summary>
        private void RemoveTask()
        {
            Task task = new Task();         //Creates a new task object that will be the same id as the task that is to be deleted

            while (true)
            {
                Console.Clear();

                int taskId = 0;

                while (true)
                {
                    //Checks if the users input can be converted to an int/number
                    try
                    {
                        Console.WriteLine(" ~ Remove task ~ ");
                        Console.WriteLine();
                        Console.WriteLine();

                        ShowTasks();        //Show all tasks to user

                        Console.WriteLine("Write 'abort' to cancel and go back to the menu");
                        Console.WriteLine("Which task do you want to remove?");
                        Console.Write("Task: ");
                        string input = Console.ReadLine();

                        //if user wants to cancel removing a task
                        if (input.ToLower() == "abort")
                        {
                            return; //Return to main menu
                        }

                        taskId = int.Parse(input);  //Try converting from string to an int/number
                        break;  //If the conversion was successful the while-loop will end and the taskId will be looked for in the task list
                    }
                    catch (Exception)   //If the conversion was not successfull
                    {
                        ErrorMessage($"{taskId}");  //Show error message and then retry getting a task id
                    }
                }

                //Check through the task list to see if the chosen task exists in the list
                foreach (Task t in tasks)
                {
                    if (t.Id == taskId)
                    {
                        task = t;   //If the task existed it will be saved in task
                    }
                }


                //if the task did exist in the list
                if (task.Id != 0)
                {
                    while(true)
                    {
                        Console.Clear();
                        Console.WriteLine($" ~ Remove task {task.Id} - {task.Title} ~ ");
                        Console.WriteLine();

                        //Makes sure that the user wants to delete the chosen task
                        Console.Write("Are you sure you want to delete this task? YES/NO ");
                        string input = Console.ReadLine();

                        //If the user does want to remove the task
                        if (input.ToLower() == "yes")
                        {
                            task.Title = "-.opDELETE";  //set the task title, this will be read in database to make it know it should edit a task by removing it
                            task.Description = null;    //Set description of task to null
                            db.EditTask(task);          //Sends the task to db to remove
                            tasks = db.GetAllTasks();   //Update tasks list
                            Console.ReadLine();
                            break;                      //User is done removing
                        }
                        else if (input.ToLower() == "no")   //If user does not want to remove the task
                        {
                            //Show message
                            Console.WriteLine();
                            Console.WriteLine("Task was not removed!");
                            Console.WriteLine("Press enter to go back to the remove menu");
                            Console.ReadLine();
                            break;
                        }
                        else    //If the user did not write neither yes or no
                        {
                            Console.WriteLine("That was not an option...");
                            Console.WriteLine("Press enter to try again");
                            Console.ReadLine();
                        }
                    }
                    
                }
                else    //If the task chosen does not exist in the database/task list
                {
                    ErrorMessage($"{taskId}");  //Show error message
                }
            }
        }
        #endregion

        /// <summary>
        /// Shows an error message to the user
        /// </summary>
        /// <param name="id">The id of the task the user wanted to edit but did not exist</param>
        private void ErrorMessage(string id)
        {
            Console.WriteLine();
            Console.WriteLine($"Could not find a task with id: {id}");
            Console.WriteLine("Press enter to retry");
            Console.ReadLine();
        }
    }

}


