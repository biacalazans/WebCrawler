using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Coletado o título e o autos dos primeiros livros da secção de ficção fantasia do site LeLivros...");

            using (var context = new WebCrawlerContext())
            {
                context.Database.EnsureCreated();
            }

            StartCrawler();

            try
            {
                Console.WriteLine("Dados salvos com sucesso");
            }

            catch (Exception e)
            {
                Console.WriteLine("Houve algum erro na busca de dados\n");
            }
        }

        public static string getTitulo(string info)
        {
            string titulo = "";
            int count=0, count2=0;

            for (int i = 0; i < info.Length; i++)
            {
                if (info[i] == ';') count++; 
            }

                for (int i=0; i<info.Length; i++)
                 {
                if (info[i] == '&')
                {
                    count2++;

                    if (count2 == count) break;

                    else
                    {
                        titulo += '-';
                        i += 7;
                    }
                }

                else
                {
                    titulo += info[i];
                }
            }

            return titulo;
        }

        public static string getAutor(string info)
        {
            string autor = "", reverseAutor = "";

            for(int i = (info.Length-1); i>0; i--)
            {
                if (info[i] == ';') break;

                else
                {
                    autor += info[i];
                }
            }

            reverseAutor = new string(autor.Reverse().ToArray());

            return reverseAutor;
        }

        private static void StartCrawler()
        {
            var url = "http://lelivros.love/categoria/ficcao-fantastica/";
            var client = new WebClient();
            string html = client.DownloadString(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var elementos = htmlDocument.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "").Equals("post-17105 product type-product status-publish has-post-thumbnail hentry first instock")).ToList();

            using (var context = new WebCrawlerContext())
            {

                foreach (var li in elementos)
                {
                    string info = li.Descendants("h3").FirstOrDefault().InnerText;
                    string aux = getAutor(info);
                    string aux2 = getTitulo(info);
                    bool existeAutor = false, existeLivro = false;

                    foreach (var l in context.Livros)
                    {
                        if (l.Titulo == aux2) existeLivro = true;
                    }

                    if (existeLivro == false) // quando rodar novamente não salvar os mesmos livros
                    {

                        var livro = new Livro();

                        foreach (var autor in context.Autores)
                        {
                            if (autor.Nome == aux)
                            {
                                livro.Autor = autor;
                                existeAutor = true;
                            }
                        }

                        livro.Titulo = getTitulo(info);

                        if (existeAutor == false) // nao salvar o mesmo autor várias vezes
                        {
                            Autor a = new Autor { Nome = getAutor(info) };
                            context.Autores.Add(a);
                            livro.Autor = a;
                        }

                        context.Livros.Add(livro);

                        context.SaveChanges();

                    }
                }

            }

            }
        }
       
    }

