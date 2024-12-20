using System;
using System.Xml.Linq;

namespace WebApplication1
{
    public class PopulationData
    {
        public List<Individual> Population { get; set; } = null!;
        public int Iter_num { get; set; }
        public int Anum { get; set; }
        public int Bnum { get; set; }
        public int Cnum { get; set; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapGet("/initial/{id}", (string id) =>
            {
                string[] parametrs = id.Split(' ');
                int a = int.Parse(parametrs[0]);
                int b = int.Parse(parametrs[1]);
                int c = int.Parse(parametrs[2]);

                Evolution square = new Evolution(a, b, c);
                PopulationData populationData = new PopulationData();
                populationData.Anum = a;
                populationData.Bnum = b;
                populationData.Cnum = c;
                populationData.Iter_num = 0;
                populationData.Population = square.Population;

                return Results.Json(populationData);
            });

            app.MapPost("/next", (PopulationData popData) => {

                Evolution square = new Evolution(popData.Population, popData.Iter_num,
                                                 popData.Anum, popData.Bnum, popData.Cnum);
                square.Iteration();
                popData.Population = square.Population;
                popData.Iter_num = square.iter_num;
                return Results.Json(popData);
            });

            app.Run();
        }
    }
}
