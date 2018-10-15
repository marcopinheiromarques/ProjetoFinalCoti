using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Projeto.Negocio;
using Projeto.Utilitario;

namespace Projeto.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly string stringConexao;

        public UsuarioRepositorio()
        {
            stringConexao = ConexaoBanco.stringConexao;
        }

        public Usuario Inserir(Usuario usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "INSERT INTO Usuarios(Login, Email, Senha, DataCriacao) VALUES (@Login, @Email, @Senha, GETDATE()); " +
                                   "SELECT CONVERT(int, SCOPE_IDENTITY())";

                    int id  = (int) con.ExecuteScalar(query,
                        new {
                              Login = usuario.Login,
                              Email = usuario.Email,
                              Senha = Criptografia.Encriptar(usuario.Senha)                              
                            });

                    usuario.IdUsuario = id;
                }

                return usuario;
            }
            catch (Exception e)
            {
                throw e;
            }           
        }

        public Usuario Alterar(Usuario usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "UPDATE Usuarios Login = @Login, Email = @Email WHERE IdUsuario = @IdUsuario";                                   

                    con.Execute(query, new {
                                             Login     = usuario.Login,
                                             Email     = usuario.Email,
                                             IdUsuario = usuario.IdUsuario
                                           });                    
                }

                return usuario;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Usuario Excluir(Usuario usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "DELETE FROM Usuarios WHERE IdUsuario = @IdUsuario";

                    con.Execute(query, new { IdUsuario = usuario.IdUsuario });
                }

                return usuario;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Usuario> ListarTodos()
        {
            string query        = "SELECT * FROM Usuarios";
            List<Usuario> lista = null;

            using (SqlConnection con = new SqlConnection(stringConexao))
            {
               lista = con.Query<Usuario>(query).ToList();               
            }

            return lista;
        }

        public Usuario EncontrarPorId(int id)
        {
            string query    = "SELECT * FROM Usuarios WHERE IdUsuario = @IdUsuario";
            Usuario usuario = null;

            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                usuario = con.Query<Usuario>(query, new { IdContato = id }).FirstOrDefault();
            }

            return usuario;
        }

        public Usuario EncontrarPorLogin(string login)
        {
            string query = "SELECT * FROM Usuarios WHERE Login = @Login";
            Usuario usuario = null;

            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                usuario = con.Query<Usuario>(query, new { Login = login }).FirstOrDefault();
            }

            return usuario;
        }

        public bool ExisteLoginOuEmailJaCadastrados(Usuario usuario)
        {
            Usuario usuarioCadastrado = null;

            using(SqlConnection con = new SqlConnection(stringConexao))
            {
                string query = "SELECT * FROM Usuarios WHERE (Login = @Login OR Email = @Email)";

                usuarioCadastrado = con.Query<Usuario>(query, 
                    new { Login = usuario.Login, Email = usuario.Email }).FirstOrDefault();
            }

            return usuarioCadastrado != null; 
        }

        public bool ValidarLoginUsuario(Usuario usuario)
        {
            using(SqlConnection con = new SqlConnection(stringConexao))
            {
                string query = "SELECT * FROM Usuarios WHERE Login = @Login AND Senha = @Senha";

                return con.Query<Usuario>(query, new { Login = usuario.Login, Senha = Criptografia.Encriptar(usuario.Senha) }).FirstOrDefault() != null;
            }
        }
    }
}
