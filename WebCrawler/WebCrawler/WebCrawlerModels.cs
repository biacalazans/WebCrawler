using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
    public class Autor
    {
        public long AutorId { get; set; }

        public string Nom { get; set; }

        public long LivroId { get; set; }

        public Livro Livros { get; set; }
    }

    public class Livro
    {
        public long LivroId { get; set; }

        public string Titulo { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Autor> Autor { get; set; }
    }
}
