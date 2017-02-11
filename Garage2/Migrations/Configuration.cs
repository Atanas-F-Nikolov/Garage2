namespace Garage2.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage2.DAL.Garage2Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Garage2.DAL.Garage2Context";
        }

        protected override void Seed(Garage2.DAL.Garage2Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Vehicles.AddOrUpdate(
                new Vehicle { ParkingSpace = 1, Size = 1, Type = VehicleType.Car, RegNumber = "CC 1234-SE", Brand = "Volvo", Model = "Shelter9", Wheels = 4, Color = "Green" },
                new Vehicle { ParkingSpace = 3, Size = 2, Type = VehicleType.Buss, RegNumber = "BS 1234-SE", Brand = "STHLM Lines", Model = "M87", Wheels = 8, Color = "Red" },
                new Vehicle { ParkingSpace = 7, Size = 3, Type = VehicleType.Boat, RegNumber = "B 1234-SE", Brand = "Viking Lines", Model = "B23", Wheels = 0, Color = "Blue" },
                new Vehicle { ParkingSpace = 11, Size = 1, Type = VehicleType.Motorcycle, RegNumber = "M 1234-SE", Brand = "Honda", Model = "H746", Wheels = 2, Color = "Red" },
                new Vehicle { ParkingSpace = 13, Size = 1, Type = VehicleType.Motorcycle, RegNumber = "MR 1224-SE", Brand = "Honda", Model = "H746", Wheels = 2, Color = "Red" },
                new Vehicle { ParkingSpace = 17, Size = 1, Type = VehicleType.Car, RegNumber = "AR 1234-SE", Brand = "SV Lines", Model = "PV45", Wheels = 2, Color = "White" }
                );
        }
    }
}
