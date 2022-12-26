using System;
using Market.Data.Addresses;

namespace Market.Services.Helpers
{
	public class Location
	{
        private const double EarthRadiusInKilometers = 6371;
        public static double distanceInKilometers(UserLocation location1, Address location2)
        {
            double dLat = ToRadians(location2.Lat - location1.Lat);
            double dLon = ToRadians(location2.Long - location1.Long);

            location1.Lat = ToRadians(location1.Lat);
            location2.Lat = ToRadians(location2.Lat);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) *
                       Math.Cos(location1.Lat) * Math.Cos(location2.Lat);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusInKilometers * c;
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}


