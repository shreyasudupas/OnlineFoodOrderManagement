using Identity.MicroService.Data.DatabaseContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Identity.MicroService.Data.SeedData
{
    public class MigrationSeedData
    {

        public static void InitialDataSeed(IApplicationBuilder app,string ConnectionString)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            using (var context = scope.ServiceProvider.GetService<UserContext>())
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    SeedData(context);

                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }


            }

        }

        private static void SeedData(UserContext dbContext)
        {
            if(!dbContext.Users.Any())
            {

                //add user Address

                dbContext.UserAddresses.Add(new UserAddress
                {
                    Address = "Citi Center Building, Balmatta",
                    CityEntityFK = new City
                    {
                        CityNames = "Bangalore",
                        CreatedDate = System.DateTime.Now,
                        StateEntityFK = new State
                        {
                            StateNames = "Karnataka",
                            CreatedDate = DateTime.Now
                        },
                    },
                    UserEntityFK = new User
                    {
                        UserName = "admin@test.com",
                        UserRoleId = 1,
                        FullName = "admin",
                        PictureLocation = "https://s.gravatar.com/avatar/5b37040e6200edb3c7f409e994076872?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fad.png",
                        Points = 0,
                        CartAmount = 0,
                        CreatedDate = System.DateTime.Now,
                        UserRoleEntityFK = new UserRole { Rolename = "user" }
                    }
                });

                
            }
            
        }
    }
}
