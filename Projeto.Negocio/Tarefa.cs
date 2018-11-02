using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Negocio
{
    public class Tarefa
    {
        public int      IdTarefa    { get; set; }
        public string   Nome        { get; set; }
        public DateTime DataEntrega { get; set; }
        public string   Descricao   { get; set; }
    }
}
