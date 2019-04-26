using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDBConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DDBOperations operations = new DDBOperations();
            operations.CreateTable();
            Console.ReadLine();
        }
    }
}
