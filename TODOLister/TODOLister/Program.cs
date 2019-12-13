using System;

namespace TODOLister
{
    class Program
    {
        static void Main(string[] args)
        {
            TodoHandler todoHandler = new TodoHandler();
            todoHandler.Start();
        }
    }
}
