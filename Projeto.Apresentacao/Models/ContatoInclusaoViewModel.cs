using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto.Apresentacao.Models
{
    public class ContatoInclusaoViewModel
    {
        [Required(ErrorMessage = "O nome do contato é obrigatório")]
        public string Nome      { get; set; }

        [Required(ErrorMessage = "O e-mail do contato é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido")]
        public string Email     { get; set; }

        [Required(ErrorMessage = "O telefone do contato é obrigatório")]
        public string Telefone  { get; set; }
    }
}