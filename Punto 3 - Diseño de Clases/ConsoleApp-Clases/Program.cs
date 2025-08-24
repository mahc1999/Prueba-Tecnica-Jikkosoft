using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp_Clases;

Console.WriteLine("=== Sistema de Gestión de Biblioteca (Demo) ===");

var library = new Library();

library.AddBook(new Book("9780141187761", "1984", "George Orwell", totalCopies: 3));
library.AddBook(new Book("9780307474278", "The Road", "Cormac McCarthy", totalCopies: 2));
library.AddBook(new Book("9780307387899", "The Girl with the Dragon Tattoo", "Stieg Larsson", totalCopies: 1));

library.RegisterMember(new Member(id: 1, name: "Ana"));
library.RegisterMember(new Member(id: 2, name: "Miguel"));

Console.WriteLine("\nInventario inicial:");
PrintBooks(library.ListBooks());

Console.WriteLine("\nPréstamo: Ana toma '1984' (ISBN 9780141187761)");
library.Borrow("9780141187761", 1);

Console.WriteLine("\nPréstamo: Miguel toma 'The Road' (ISBN 9780307474278)");
library.Borrow("9780307474278", 2);

Console.WriteLine("\nInventario luego de préstamos:");
PrintBooks(library.ListBooks());

Console.WriteLine("\nIntento de préstamo sin copias: Miguel toma de nuevo 'The Road'");
Try(() => library.Borrow("9780307474278", 2)); // puede o no fallar según copias

Console.WriteLine("\nBúsqueda por término 'the':");
PrintBooks(library.Search("the"));

Console.WriteLine("\nDevolución: Ana devuelve '1984'");
library.Return("9780141187761", 1);

Console.WriteLine("\nInventario final:");
PrintBooks(library.ListBooks());

Console.WriteLine("\nPréstamos activos (debug):");
library.DumpLoans();

static void PrintBooks(IEnumerable<Book> books)
{
    foreach (var b in books)
        Console.WriteLine($"- {b.Title} ({b.Isbn}) | Autor: {b.Author} | Disp: {b.AvailableCopies}/{b.TotalCopies}");
}

static void Try(Action action)
{
    try { action(); }
    catch (Exception ex) { Console.WriteLine($"{ex.Message}"); }
}

