// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Runtime.CompilerServices;

IList<City> cities = new List<City>(){
    new City { Name = "Seattle", Country = "USA", Population = 1000000 },
    new City { Name = "Redmond", Country = "USA", Population = 100000 },
    new City { Name = "Vancouver", Country = "Canada", Population = 2000000 },
    new City { Name = "Portland", Country = "USA", Population = 500000 },
    new City { Name = "Toronto", Country = "Canada", Population = 3000000 },
    new City { Name = "New York", Country = "USA", Population = 8000000 },
    new City { Name = "Los Angeles", Country = "USA", Population = 4000000 },
    new City { Name = "Chicago", Country = "USA", Population = 3000000 },
    new City { Name = "Houston", Country = "USA", Population = 2000000 },
    new City { Name = "Montreal", Country = "Canada", Population = 2000000 },
    new City { Name = "Calgary", Country = "Canada", Population = 1000000 },
    new City { Name = "Ottawa", Country = "Canada", Population = 1000000 },
    new City { Name = "Edmonton", Country = "Canada", Population = 1000000 },
    new City { Name = "Winnipeg", Country = "Canada", Population = 1000000 },
    new City { Name = "Quebec", Country = "Canada", Population = 1000000 },
    new City { Name = "Halifax", Country = "Canada", Population = 1000000 },
    new City { Name = "Victoria", Country = "Canada", Population = 1000000 },
};

AsyncPageable<City> ListCities()
{
    async Task<Page<City>> FirstPageFunc(int? pageSize)
    {
        return Page.FromValues(cities.Take(pageSize.Value).ToList(), "5");
    }

    async Task<Page<City>> NextPageFunc(string? continuationToken, int? pageSize)
    {
        int? token = int.Parse(continuationToken);
        var nextPage = cities.Skip(token.Value).Take(pageSize.Value).ToList();
        return Page.FromValues(nextPage, nextPage.Count > 0 ? (token.Value + pageSize.Value).ToString() : "");
    }

    return PageableHelpers.CreateAsyncEnumerable<City>(FirstPageFunc, NextPageFunc, 5);
}

/*int i = 0;

await foreach (City city in ListCities())
{
    Console.WriteLine($"{city.Name}, {city.Country} - Population: {city.Population}");
    Console.WriteLine(i++);
}*/

AsyncPageable<City> cities1 = ListCities();

await foreach (Page<City> page in cities1.GetPagesAsync())
{
    Console.WriteLine($"Page Count {page.Items.Count}");
    foreach (City item in page.Items)
    {
        Console.WriteLine($"{item.Name}, {item.Country} - Population: {item.Population}");
    }
}