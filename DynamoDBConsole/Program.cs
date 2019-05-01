using System;

namespace DynamoDBConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DDBOperations operations = new DDBOperations();
            operations.RestoreBackup();
            Console.ReadLine();
        }
    }
}
