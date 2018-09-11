using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projeto.Apresentacao.Models
{
    public class UsuarioRegistroViewModel
    {
        [Display  (Name = "Login")]
        [MinLength(3 , ErrorMessage = "Informe no mínimo {1} caracteres.")]
        [MaxLength(50, ErrorMessage = "Informe no máximo {1} caracteres.")]
        [Required (ErrorMessage     = "Informe o login do usuário.")]
        public string Login          { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        [Required    (ErrorMessage = "Informe o e-mail do usuário.")]        
        public string Email          { get; set; }

        [Display(Name = "Senha")]
        [MinLength(6  , ErrorMessage = "Informe no mínimo {1} caracteres.")]
        [MaxLength(100, ErrorMessage = "Informe no máximo {1} caracteres.")]
        [Required(ErrorMessage       = "Informe a senha do usuário.")]        
        public string Senha          { get; set; }

        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "Senhas não conferem.")]
        [Required(ErrorMessage = "Confirme a senha do usuário.")]        
        public string ConfirmarSenha { get; set; }
    }
}