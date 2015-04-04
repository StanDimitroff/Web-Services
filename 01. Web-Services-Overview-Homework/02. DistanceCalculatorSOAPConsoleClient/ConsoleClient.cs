namespace DistanceCalculatorConsoleClient
{
    using System;
    using _02.DistanceCalculatorConsoleClient.DistanceCalculatorReference;

    class ConsoleClient
    {
        static void Main()
        {
            Point startPoint = new Point();
            Console.Write("Enter startPoint X: ");
            startPoint.X = int.Parse(Console.ReadLine());
            Console.Write("Enter startPoint Y: ");
            startPoint.Y = int.Parse(Console.ReadLine());
            Point endPont = new Point();
            Console.Write("Enter endPoint X: ");
            endPont.X = int.Parse(Console.ReadLine());
            Console.Write("Enter endPoint Y: ");
            endPont.Y = int.Parse(Console.ReadLine());

            DistanceCalculatorClient calculator = new DistanceCalculatorClient();
            double result = calculator.CalculateDistance(startPoint, endPont);
            Console.WriteLine("Distance: " + result);
        }
    }
}