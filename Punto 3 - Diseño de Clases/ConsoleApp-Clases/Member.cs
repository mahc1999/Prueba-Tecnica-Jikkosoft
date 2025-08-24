using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_Clases
{
    public class Member
    {
        public int Id { get; }
        public string Name { get; }
        public List<string> BorrowedIsbns { get; } = new List<string>();
        public int MaxLoans { get; } = 3;

        public Member(int id, string name)
        {
            if (id <= 0) throw new ArgumentException("El Id debe ser mayor a 0.", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es requerido.", nameof(name));

            Id = id;
            Name = name.Trim();
        }

        public bool CanBorrow()
        {
            return BorrowedIsbns.Count < MaxLoans;
        }
    }

}
