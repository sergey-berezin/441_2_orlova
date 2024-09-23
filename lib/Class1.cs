using System.Globalization;
using System.IO.Compression;

namespace genetics;

public class Individual
{
    public int[] Genotype_X { get; set; }       // решение в координате Y
    public int[] Genotype_Y { get; set; }       // решение в координате X
    public int[] Side { get; set; }             // массив длин сторон квадратов
    public int Survival { get; set; }           // функция выживаемости (площадь)
    public Individual(int[] gtx, int[] gty, int[] s)
    {
        int n = gtx.Length;
        Genotype_X = new int[n];
        Genotype_Y = new int[n];
        Side = s;
        for (int i = 0; i < n; i++)
        {
            Genotype_X[i] = gtx[i];
            Genotype_Y[i] = gty[i];
        }
        
        // считаем функцию выживаемости
        int bottom = 1000, left = 1000;
        int top = 0, right = 0;
        bool f, intersect = false;
        n--;
        for (int i = 0; i < n; i++)
        {
            if(intersect) break;
            if(left > Genotype_X[i]) left = Genotype_X[i];
            if(bottom > Genotype_Y[i]) bottom = Genotype_Y[i];
            if(top < Genotype_Y[i] + Side[i]) top = Genotype_Y[i] + Side[i];
            if(right < Genotype_X[i] + Side[i]) right = Genotype_X[i] + Side[i];
            for (int j = i + 1; j < n; j++)
            {
                f = false;
                if(intersect) break;
                if (Genotype_X[i] < Genotype_X[j] + Side[j])
                    if (Genotype_X[j] < Genotype_X[i] + Side[i])
                        if(Genotype_Y[i] < Genotype_Y[j] + Side[j])
                            if(Genotype_Y[j] < Genotype_Y[i] + Side[i])
                                f = true;
                intersect = f;
            }
        }
        if(intersect)
            Survival = 10000000;
        else
            Survival = (top - bottom)*(right - left);
    }
    public int GetSurvival()    // для сортировки
    {
        return Survival;
    }
}

public class Evolution
{
    public List <Individual> Population { get; set; }
    public int[] Side { get; set; }     // массив длин сторон квадратов
    public int iter_num = 0;            // номер итерации эволюции
    public int Population_numbers = 50; // будем поддерживать фиксированное число особей в популяции
    public Evolution(int a, int b, int c)
    {
        Population = [];

        int n = a + b + c;
        Side = new int[n];
        for(int i = 0; i < a; i++)
            Side[i] = 1;
        for(int i = 0; i < b; i++)
            Side[a + i] = 2;
        for(int i = 0; i < c; i++)
            Side[a + b + i] = 3;

        var rand = new Random();
        int[] gtx = new int[n];
        int[] gty = new int[n];
        int f = 0;

        for(int ind = 0; ind < Population_numbers; ind++)
        {
            int border = ind % n;
            gtx[border] = 0;
            gty[border] = 0;
            for(int i = border + 1; i < n; i++)
            {
                f = rand.Next(2);
                if(f == 0)
                {
                    gtx[i] = gtx[i - 1] + Side[i - 1];
                    gty[i] = gty[i - 1];
                }
                else
                {
                    gtx[i] = gtx[i - 1];
                    gty[i] = gty[i - 1] + Side[i - 1];
                }
            }
            if (border > 0)
            {
                gtx[0] = gtx[n - 1] + Side[n - 1];
                gty[0] = gty[n - 1];
                for(int i = 1; i < border; i++)
                {
                    f = rand.Next(2);
                    if(f == 0)
                    {
                        gtx[i] = gtx[i - 1] + Side[i - 1];
                        gty[i] = gty[i - 1];
                    }
                    else
                    {
                        gtx[i] = gtx[i - 1];
                        gty[i] = gty[i - 1] + Side[i - 1];
                    }
                }
            }
            Population.Add(new Individual(gtx, gty, Side));
        }
    }
    public void Mutation(int ind, int coeff)
    {
        int gene, direction, len, n = Side.Length;
        var rand = new Random(coeff);
        int[] gtx = new int[n];
        int[] gty = new int[n];

        gene = rand.Next(0, n);         //выбор гена (смещаемого квадрата)
        direction = rand.Next(0, 4);    //направление смещения квадрата
        len = rand.Next(1, n);          //длина смещения (от 1 до n)

        for(int i = 0; i < n; i++)
        {
            gtx[i] = Population[ind].Genotype_X[i];
            gty[i] = Population[ind].Genotype_Y[i];
        }
        if(direction == 0)
            gtx[gene] -= len;
        if(direction == 1)
            gtx[gene] += len;
        if(direction == 2)
            gty[gene] -= len;
        if(direction == 3)
            gty[gene] += len;

        Population.Add(new Individual(gtx, gty, Side));

    }
    public void Crossing(int ind1, int ind2)
    {
        int border = 0, n = Side.Length;
        var rand = new Random(iter_num + ind2);
        border = rand.Next(1, n);
        int[] gtx_1 = new int[n];
        int[] gty_1 = new int[n];
        int[] gtx_2 = new int[n];
        int[] gty_2 = new int[n];
        for(int i = 0; i < border; i++)
        {
            gtx_1[i] = Population[ind1].Genotype_X[i];
            gty_1[i] = Population[ind1].Genotype_Y[i];
            gtx_2[i] = Population[ind2].Genotype_X[i];
            gty_2[i] = Population[ind2].Genotype_Y[i];
        }
        for(int i = border; i < n; i++)
        {
            gtx_1[i] = Population[ind2].Genotype_X[i];
            gty_1[i] = Population[ind2].Genotype_Y[i];
            gtx_2[i] = Population[ind1].Genotype_X[i];
            gty_2[i] = Population[ind1].Genotype_Y[i];
        }
        Population.Add(new Individual(gtx_1, gty_1, Side));
        Population.Add(new Individual(gtx_2, gty_2, Side));
    }

    // сортировка популяции и удаление лишних особей (всех кроме первых Population_numbers)
    public void Selection()
    {
        Population.Sort(
            delegate(Individual x, Individual y)
            {
                return x.GetSurvival().CompareTo(y.GetSurvival());
            });
        int n = Population.Count;
        Population.RemoveRange(Population_numbers, n - Population_numbers);
    }
    public void Iteration()
    {
        var rand = new Random(iter_num);
        int cross, n = Population.Count;
        for(int ind = 0; ind < n; ind++)
        {
            Mutation(ind, 1);
            Mutation(Population.Count - 1, 2);      // тут и далее мутации последней мутации
            Mutation(Population.Count - 1, 3);
            Mutation(Population.Count - 1, 4);
            Mutation(Population.Count - 1, 5);
            cross = rand.Next(n);                   // выбор пары для скрещивания
            if (ind != cross) Crossing(ind, cross);
            cross = rand.Next(n);
            if (ind != cross) Crossing(ind, cross);
            cross = rand.Next(n);
            if (ind != cross) Crossing(ind, cross);
            cross = rand.Next(n);
            if (ind != cross) Crossing(ind, cross);
        }
        Selection();
        iter_num++;
    }

    public  string MakeString()
        {
            string s = $"Number of iteration is {iter_num}\n" +
            $"Area: {Population[0].Survival}" +
            "\nX: " + string.Join(" ", Population[0].Genotype_X) +
            "\nY: " + string.Join(" ", Population[0].Genotype_Y) + "\n\n";
            return s;
        }

}