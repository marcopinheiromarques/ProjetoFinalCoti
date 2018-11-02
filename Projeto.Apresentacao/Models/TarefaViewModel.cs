using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto.Apresentacao.Models
{
    public class TarefaViewModel
    {
        public int      IdTarefa    { get; set; }
        public string   Nome        { get; set; }
        public DateTime DataEntrega { get; set; }
        public string   Descricao   { get; set; }
    }
}