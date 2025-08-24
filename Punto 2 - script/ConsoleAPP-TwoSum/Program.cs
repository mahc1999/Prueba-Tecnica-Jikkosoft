using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("Inicio del proceso: " + DateTime.Now);
Console.WriteLine("-----------------------------------------------");
Console.WriteLine("");
Console.WriteLine("Por favor ingrese un listado de números enteros separados por comas (deben contener como mínimo 3 números, ejemplo: 2,7,11,15):");

int[] numeros;

while (true)
{
    string? input = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(input))
    {
        try {
            numeros = input.Split(',').Select(n => int.Parse(n.Trim())).ToArray();

            if (numeros.Length >= 3) { 
                break;
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Debe ingresar al menos 3 números. Intente de nuevo:");
            }
        }
        catch {
            Console.WriteLine("");
            Console.WriteLine("Entrada inválida. Ingrese solo números separados por comas:");
        }
    }
    else
    {
        Console.WriteLine("");
        Console.WriteLine("La lista no puede estar vacía. Intente de nuevo:");
    }
}

Console.WriteLine("----------------------------------------");
Console.WriteLine("");
Console.WriteLine("Ingrese el número objetivo (target):");

int target;

while (!int.TryParse(Console.ReadLine(), out target))
{
    Console.WriteLine("Entrada inválida. Ingrese un número válido:");
}


int[] resultado = TwoSum(numeros, target);


if (resultado.Length > 0)
    Console.WriteLine($"Los índices encontrados son: [{resultado[0]}, {resultado[1]}]");
else
    Console.WriteLine("No se encontraron dos números que sumen el target.");


static int[] TwoSum(int[] nums, int target)
{
    Dictionary<int, int> lista = new Dictionary<int, int>();

    for (int i = 0; i < nums.Length; i++)
    {
        int complemento = target - nums[i];

        if (lista.ContainsKey(complemento))
        {
            return new int[] { lista[complemento], i };
        }

        if (!lista.ContainsKey(nums[i]))
        {
            lista.Add(nums[i], i);
        }
    }

    return Array.Empty<int>();
}