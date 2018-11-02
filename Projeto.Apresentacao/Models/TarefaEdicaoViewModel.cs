using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto.Apresentacao.Models
{
    public class TarefaEdicaoViewModel
    {
        public int      IdTarefa    { get; set; }

        [Required(ErrorMessage = "O nome da tarefa é obrigatória")]
        public string   Nome        { get; set; }

        [Required(ErrorMessage = "A data de entrega da tarefa é obrigatória")]
        public DateTime DataEntrega { get; set; }

        [Required(ErrorMessage = "A descrição da tarefa é obrigatória")]
        public string   Descricao   { get; set; }
    }
}