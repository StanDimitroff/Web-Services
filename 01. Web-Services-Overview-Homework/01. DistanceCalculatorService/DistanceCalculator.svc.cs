namespace DistanceCalculator
{
    using System;

    public class DistanceCalculator : IDistanceCalculator
    {
        public double CalculateDistance(Point startPoint, Point endPoint)
        {
            return Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2));
        }
    }
}