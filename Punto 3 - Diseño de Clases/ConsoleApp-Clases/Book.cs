using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Clases
{
    public class Book
    {
        public string Isbn { get; }
        public string Title { get; }
        public string Author { get; }
        public int TotalCopies { get; }
        public int AvailableCopies { get; private set; }

        public Book(string isbn, string title, string author, int totalCopies)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("El ISBN es requerido.", nameof(isbn));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("El título es requerido.", nameof(title));
            if (totalCopies <= 0)
                throw new ArgumentException("El total de copias debe ser mayor a 0.", nameof(totalCopies));

            Isbn = isbn.Trim();
            Title = title.Trim();
            Author = (author ?? string.Empty).Trim();
            TotalCopies = totalCopies;
            AvailableCopies = totalCopies;
        }

        public bool IsAvailable() => AvailableCopies > 0;

        internal void TakeOne()
        {
            if (!IsAvailable())
                throw new InvalidOperationException($"No hay copias disponibles de '{Title}'.");
            AvailableCopies--;
        }

        internal void ReturnOne()
        {
            if (AvailableCopies >= TotalCopies)
                throw new InvalidOperationException("No se puede exceder el total de copias.");
            AvailableCopies++;
        }
    }
}
