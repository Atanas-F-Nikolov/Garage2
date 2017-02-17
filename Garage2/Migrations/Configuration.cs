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

            var members = new[]
            {
                new Member { SocialSecurityNumber = "111111-1111", FirstName = "FName", LastName = "LName", DateOfBirth = DateTime.Now },
                new Member { SocialSecurityNumber = "222222-2222", FirstName = "FName2", LastName = "LName2", DateOfBirth = DateTime.Now },
                new Member { SocialSecurityNumber = "333333-3333", FirstName = "FName3", LastName = "LName3", DateOfBirth = DateTime.Now },
            };

            context.Members.AddOrUpdate(x => x.SocialSecurityNumber, members);
            context.SaveChanges();

            var types = new[]
            {
                new VehicleType { Type = "Car"},
                new VehicleType { Type = "Buss"},
                new VehicleType { Type = "Boat"},
                new VehicleType { Type = "Motorcycle"},
                new VehicleType { Type = "Airplane"}
            };

            context.VehicleTypes.AddOrUpdate(x => x.Type, types);
            context.SaveChanges();

            var vehicles = new[]
            {
                new Vehicle { ParkingSpace = 1, Size = 1, MemberId = members[0].Id, VehicleTypeId = types[0].Id, RegNumber = "CC 1234-SE", Brand = "Volvo", Model = "Shelter9", Wheels = 4, Color = "Green" },
                new Vehicle { ParkingSpace = 3, Size = 2, MemberId = members[0].Id, VehicleTypeId = types[1].Id, RegNumber = "BS 1234-SE", Brand = "STHLM Lines", Model = "M87", Wheels = 8, Color = "Red" },
                new Vehicle { ParkingSpace = 7, Size = 3, MemberId = members[1].Id, VehicleTypeId = types[2].Id, RegNumber = "B 1234-SE", Brand = "Viking Lines", Model = "B23", Wheels = 0, Color = "Blue" },
                new Vehicle { ParkingSpace = 11, Size = 1,MemberId = members[1].Id, VehicleTypeId = types[3].Id, RegNumber = "M 1234-SE", Brand = "Honda", Model = "H746", Wheels = 2, Color = "Red" },
                new Vehicle { ParkingSpace = 13, Size = 1,MemberId = members[2].Id, VehicleTypeId = types[3].Id, RegNumber = "MR 1224-SE", Brand = "Honda", Model = "H746", Wheels = 2, Color = "White" },
                new Vehicle { ParkingSpace = 17, Size = 1,MemberId = members[2].Id, VehicleTypeId = types[1].Id, RegNumber = "CR 1234-SE", Brand = "Saab", Model = "EY37", Wheels = 4, Color = "Red" },
                new Vehicle { ParkingSpace = 18, Size = 3,MemberId = members[2].Id, VehicleTypeId = types[4].Id, RegNumber = "AR 6736-SE", Brand = "SAS", Model = "HR72", Wheels = 12, Color = "White" }
            };

            context.Vehicles.AddOrUpdate(x => x.RegNumber, vehicles);
            context.SaveChanges();
        }
    }
}
