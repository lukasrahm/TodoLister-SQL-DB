using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace TODOLister
{
    class Database
    {
        //String used to connect to the database
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=TODOLister;Integrated Security=True";

        ///<summary>
        ///Gets the tasks from the database
        ///</summary>
        ///<returns>A list of all tasks</returns>
        public List<Task> GetAllTasks()
        {

            string sqlQuery = "SELECT * FROM Task"; //Query to run through the database, selects all tasks

            List<Task> tasks = new List<Task>(); //Create an empty list of tasks

            using (var myConnection = new SqlConnection(connectionString)) //Prepare connection to the db
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection); //Prepare the query for the db

                myConnection.Open(); //Open connection to the db

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) //Run query on db
                {
                    while (dataReader.Read()) //Read response from db (all rows)
                    {
                        Task task = new Task(); //Create new task object

                        task.Id = int.Parse(dataReader["Id"].ToString()); //Set task id from db
                        task.Title = dataReader["Title"].ToString(); //Set task title from db
                        task.Description = dataReader["Description"].ToString(); //Set task description from db
                        tasks.Add(task);
                    }

                    myConnection.Close(); //Close connection to the db
                }
            }

            return tasks;
        }

        /// <summary>
        /// Adds new task to the database
        /// </summary>
        /// <param name="task">The new task that is to be added</param>
        public void AddTask(Task task)
        {
            string sqlQuery = $"INSERT INTO Task (Title, Description) VALUES ('{task.Title}', '{task.Description}')"; //Query to run through the database

            using (SqlConnection myConnection = new SqlConnection(connectionString)) //Prepare connection to the db
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection); //Prepare the query for the db

                myConnection.Open(); //Open connection to the db

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) //Run query on db
                {
                    myConnection.Close(); //Close connection to the db
                    Console.WriteLine($"Task: '{task.Title}' has been saved");
                }
            }
        }
        /// <summary>
        /// Edits or removes task from the database
        /// </summary>
        /// <param name="task">The task to edit/remove</param>
        public void EditTask(Task task)
        {
            bool editedTask = true;     //Shows if the task was edited or removed
            string sqlQuery;

            #region decideQueryToRun
            //If the user didn't write a new title
            if (String.IsNullOrWhiteSpace(task.Title))
            {
                //If the user didn't write any new title nor description
                if (String.IsNullOrWhiteSpace(task.Description))
                {
                    Console.WriteLine($"Task {task.Id} has not been updated");
                    return;     //Go back and don't update anything
                }
                //If the user only wanted to update the description
                else
                {
                    sqlQuery = $"UPDATE Task SET Description = '{task.Description}' WHERE id = {task.Id}"; //Query to run through the database
                }
            }
            //If the user wrote a new title
            else
            {
                //If the user only wrote a new title
                if (String.IsNullOrWhiteSpace(task.Description))
                {
                    //If user wants to delete a task
                    if(task.Title == "-.opDELETE")
                    {
                        editedTask = false;
                        sqlQuery = $"DELETE FROM Task WHERE id = {task.Id}"; //Query to run through the database
                    }
                    //If user wants to edit
                    else
                    {
                        sqlQuery = $"UPDATE Task SET Title = '{task.Title}' WHERE id = {task.Id}"; //Query to run through the database
                    }  
                }
                //If the user wrote both new title and description
                else
                {
                    sqlQuery = $"UPDATE Task SET Title = '{task.Title}', Description = '{task.Description}' WHERE id = {task.Id}"; //Query to run through the database
                }
            }
            #endregion

            using (SqlConnection myConnection = new SqlConnection(connectionString)) //Prepare connection to the db
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, myConnection); //Prepare the query for the db

                myConnection.Open(); //Open connection to the db

                using (SqlDataReader dataReader = sqlCommand.ExecuteReader()) //Run query on db
                {
                    myConnection.Close(); //Close connection to the db
                    if(editedTask)
                    {
                        Console.WriteLine($"Task {task.Id} has been updated");
                    }
                    else
                    {
                        Console.WriteLine($"Task {task.Id} has been removed");
                    }
                }
            }
        }
    }
}
