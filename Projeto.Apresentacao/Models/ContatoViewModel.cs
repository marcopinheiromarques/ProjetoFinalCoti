using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Projeto.Apresentacao.Models
{
    public class ContatoViewModel
    {
        public int    IdContato { get; set; }
        public string Nome      { get; set; }
        public string Email     { get; set; }
        public string Telefone  { get; set; }
    }
}