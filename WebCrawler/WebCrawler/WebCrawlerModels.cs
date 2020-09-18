using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
    public class Autor
    {
        public long AutorId { get; set; }

        public string Nome { get; set; }

        public virtual ICollection<Livro> Livros { get; set; }
    }

    public class Livro
    {
        public long LivroId { get; set; }

        public string Titulo { get; set; }

        public virtual Autor Autor { get; set; }
    }
}
