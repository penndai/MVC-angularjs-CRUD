namespace Test.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Test.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Test.Models.CustomerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Test.Models.CustomerContext context)
        {
            var items = Enumerable.Range(1, 10).Select(i => new Customer { FirstName = "firstname " + i, LastName="lastname " + i,Gender=new Random().Next(0,2)}).ToArray();
            context.Customers.AddOrUpdate(item => new { item.FirstName}, items);
        }
    }
}
