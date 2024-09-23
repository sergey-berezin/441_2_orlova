// See https://aka.ms/new-console-template for more information
using genetics;

Evolution square = new Evolution(20, 11, 5);
//Evolution square = new Evolution(20, 0, 0);
Console.WriteLine("----- First Population -----");
Console.WriteLine(square.MakeString());
Console.WriteLine("----- Next Populations -----");
while (!Console.KeyAvailable)
{
    square.Iteration();
    Console.WriteLine(square.MakeString());
    Thread.Sleep(300);
}
Console.WriteLine("----- Solution -----");
Console.WriteLine(square.MakeString());