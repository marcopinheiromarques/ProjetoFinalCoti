using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto.Apresentacao.Models
{
    public class ContatoFiltroViewModel
    {
        public int    IdContato { get; set; }
        public string Nome      { get; set; }
        public string Email     { get; set; }
        public string Telefone  { get; set; }
    }
}