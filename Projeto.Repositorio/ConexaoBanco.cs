using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Repositorio
{
    public class ConexaoBanco
    {
        public static string stringConexao = 
            ConfigurationManager.ConnectionStrings["BancoLocal"].ConnectionString;
    }
}
