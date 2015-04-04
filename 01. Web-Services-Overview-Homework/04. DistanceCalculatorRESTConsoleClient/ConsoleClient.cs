namespace DistanceCalculatorRESTConsoleClient
{
    using System;
    using RestSharp;

    class ConsoleClient
    {
        static void Main()
        {
            Console.Write("Enter startPoint X: ");
            int sx = int.Parse(Console.ReadLine());
            Console.Write("Enter startPoint Y: ");
            int sy = int.Parse(Console.ReadLine());
            Console.Write("Enter endPoint X: ");
            int ex = int.Parse(Console.ReadLine());
            Console.Write("Enter endPoint Y: ");
            int ey = int.Parse(Console.ReadLine());

            var client = new RestClient("http://localhost:52720/api");
            var request = new RestRequest("distance", Method.GET);
            request.AddParameter("sx", sx);
            request.AddParameter("sy", sy);
            request.AddParameter("ex", ex);
            request.AddParameter("ey", ey);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            Console.WriteLine("Distance: " + content);
        }
    }
}