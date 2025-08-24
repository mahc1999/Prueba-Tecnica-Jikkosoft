using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Clases
{

    public class Library
    {
        // Clave = ISBN (case-insensitive para evitar problemas de mayúsculas/minúsculas)
        private readonly Dictionary<string, Book> _books = new Dictionary<string, Book>(StringComparer.OrdinalIgnoreCase);
        // Clave = Id del miembro
        private readonly Dictionary<int, Member> _members = new Dictionary<int, Member>();
        // Préstamos activos. Clave simple "isbn::memberId" para esta demo.
        private readonly Dictionary<string, LoanInfo> _loans = new Dictionary<string, LoanInfo>();

        private const int DefaultLoanDays = 14;

        public void AddBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            var isbn = book.Isbn?.Trim();
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("El ISBN no puede estar vacío.", nameof(book));

            if (_books.ContainsKey(isbn))
                throw new InvalidOperationException($"Ya existe un libro con ISBN {isbn}.");

            _books[isbn] = book;
        }

        public void RegisterMember(Member member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            if (_members.ContainsKey(member.Id))
                throw new InvalidOperationException($"Ya existe un miembro con Id {member.Id}.");

            _members[member.Id] = member;
        }

        public IEnumerable<Book> ListBooks()
        {
            return _books.Values.OrderBy(b => b.Title);
        }

        public IEnumerable<Book> Search(string term)
        {
            term ??= string.Empty;

            return _books.Values.Where(b =>
                b.Title.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 ||
                b.Author.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void Borrow(string isbn, int memberId)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("El ISBN es requerido.", nameof(isbn));

            if (!_books.TryGetValue(isbn, out var book))
                throw new KeyNotFoundException($"No se encontró el ISBN {isbn}.");

            if (!_members.TryGetValue(memberId, out var member))
                throw new KeyNotFoundException($"No se encontró el miembro {memberId}.");

            if (!member.CanBorrow())
                throw new InvalidOperationException($"El miembro '{member.Name}' alcanzó el máximo de préstamos ({member.MaxLoans}).");

            if (!book.IsAvailable())
                throw new InvalidOperationException($"No hay copias disponibles de '{book.Title}'.");

            // Registrar préstamo
            book.TakeOne();
            member.BorrowedIsbns.Add(book.Isbn);

            var dueDate = DateTime.UtcNow.AddDays(DefaultLoanDays);
            _loans[MakeLoanKey(book.Isbn, memberId)] = new LoanInfo
            {
                MemberId = memberId,
                DueDateUtc = dueDate
            };
        }

        public void Return(string isbn, int memberId)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("El ISBN es requerido.", nameof(isbn));

            if (!_books.TryGetValue(isbn, out var book))
                throw new KeyNotFoundException($"No se encontró el ISBN {isbn}.");

            if (!_members.TryGetValue(memberId, out var member))
                throw new KeyNotFoundException($"No se encontró el miembro {memberId}.");

            var key = MakeLoanKey(isbn, memberId);
            if (!_loans.ContainsKey(key))
                throw new InvalidOperationException("No existe un préstamo activo para ese ISBN y miembro.");

            // Reversa del préstamo
            book.ReturnOne();
            member.BorrowedIsbns.Remove(isbn);
            _loans.Remove(key);
        }

        private static string MakeLoanKey(string isbn, int memberId)
        {
            return isbn + "::" + memberId;
        }

        private class LoanInfo
        {
            public int MemberId { get; set; }
            public DateTime DueDateUtc { get; set; }
        }

        // Utilidad para depuración/demostración
        public void DumpLoans()
        {
            if (_loans.Count == 0)
            {
                Console.WriteLine("(sin préstamos)");
                return;
            }

            foreach (var kv in _loans)
            {
                var parts = kv.Key.Split(new[] { "::" }, StringSplitOptions.None);
                var isbn = parts[0];
                var memberId = kv.Value.MemberId;
                var due = kv.Value.DueDateUtc;

                var book = _books[isbn];
                var member = _members[memberId];

                Console.WriteLine($"- {book.Title} → {member.Name} (vence {due:yyyy-MM-dd})");
            }
        }
    }

}
