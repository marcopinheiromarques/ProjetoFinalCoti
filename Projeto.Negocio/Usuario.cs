using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Negocio
{
    public class Usuario
    {
        public int      IdUsuario            { get; set; }
        public string   Login                { get; set; }
        public string   Email                { get; set; }
        public string   Senha                { get; set; }
        public DateTime DataCriacao          { get; set; }
        public int      AlterouSenha         { get; set; }

        public override string ToString()
        {
            return $"Id: {IdUsuario}, Login: {Login}, Email: {Email}";
        }
    }
}
