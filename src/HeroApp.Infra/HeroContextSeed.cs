using HeroApp.Domain;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroApp.Infra
{


    public static class HeroContextSeedSeed
    {
        public static async Task SeedAsync(IHeroContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // Create roles 
            if (!roleManager.Roles.Any())
            {
                var admin = new AppRole()
                {
                    Id = AppProfileConstants.ADMIN,
                    Name = AppProfileConstants.ADMIN,
                };
                var user = new AppRole()
                {
                    Id = AppProfileConstants.USER,
                    Name = AppProfileConstants.USER,
                };
                await roleManager.CreateAsync(admin);
                await roleManager.CreateAsync(user);
            }
            // Create default administrator
            var defaultUser = new AppUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
                await userManager.AddToRoleAsync(defaultUser, AppProfileConstants.ADMIN);
            }
            // Seed, if necessary
            if (!context.Ongs.Any())
            {
                context.Ongs.Add(new Ong
                {
                    Id = "AAAABBBB",
                    Name = "HurtCats Org",
                    City = "San Francisco",
                    State = "CA",
                    Email = "contact@hurtcats.org",
                    Owner = defaultUser,
                    Incidents = new List<Incident>
                    {
                        new Incident {
                        Title = "FrontRight Leg Broken",
                        Description =  "this cats needs a surgery",
                        Value = "120",
                        },
                        new Incident {
                        Title = "FrontRight Leg Broken",
                        Description =  "this cats needs a surgery",
                        Value = "120",
                        },
                        new Incident {
                        Title = "FrontRight Leg Broken",
                        Description =  "this cats needs a surgery",
                        Value = "120",
                        },
                        new Incident {
                        Title = "FrontRight Leg Broken",
                        Description =  "this cats needs a surgery",
                        Value = "120",
                        },
                        new Incident {
                        Title = "FrontRight Leg Broken",
                        Description =  "this cats needs a surgery",
                        Value = "120",
                        },
                    }
                });
                await context.SaveChangesAsync();
            }




        }
    }
}
