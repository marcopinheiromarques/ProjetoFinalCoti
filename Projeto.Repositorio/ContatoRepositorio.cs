using Projeto.Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Projeto.Repositorio
{
    public class ContatoRepositorio
    {
        private readonly string stringConexao;

        public ContatoRepositorio()
        {
            stringConexao = ConexaoBanco.stringConexao;
        }

        public Contato Inserir(int idUsuario, Contato contato)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "INSERT INTO Contatos (Nome, Email, Telefone, IdUsuario) VALUES (@Nome, @Email, @Telefone, @IdUsuario);" +
                        "SELECT CONVERT(int, SCOPE_IDENTITY())";

                    int id = (int)con.ExecuteScalar(query, new
                    {
                        IdContato = contato.IdContato,
                        Nome      = contato.Nome,
                        Email     = contato.Email,
                        Telefone  = contato.Telefone,
                        IdUsuario = idUsuario
                    });

                    contato.IdContato = id;
                }

                return contato;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Contato Alterar(Contato contato)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "UPDATE Contatos SET Nome = @Nome, Email = @Email, Telefone = @Telefone " +
                        "WHERE IdContato = @IdContato";

                    con.Execute(query, contato);
                }

                return contato;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Contato Excluir(Contato contato)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "DELETE FROM Contatos WHERE IdContato = @IdContato";

                    con.Execute(query, contato);
                }

                return contato;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Contato EncontrarPorId(int id)
        {
            try
            {
                Contato contato = null;

                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "SELECT * FROM Contatos WHERE IdContato = @IdContato";

                    contato = con.Query<Contato>(query, new { IdContato = id }).FirstOrDefault();
                }

                return contato;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Contato> ListarTodos(int idUsuario, Contato contato = null)
        {
            try
            {
                List<Contato> lista = null;

                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    if (contato == null)
                    {
                        string query = "SELECT * FROM Contatos WHERE IdUsuario = @IdUsuario";

                        lista = con.Query<Contato>(query, new { IdUsuario = idUsuario }).ToList();
                    }
                    else
                    {
                        string query = "SELECT * FROM Contatos WHERE IdUsuario = @IdUsuario ";

                        if (contato.IdContato > 0)
                        {
                            query += "AND IdContato = @IdContato ";
                        }

                        if (contato.Nome != null && contato.Nome != "")
                        {
                            query += "AND Nome LIKE '%" + contato.Nome + "%' ";
                        }

                        if (contato.Email != null && contato.Email != "")
                        {
                            query += "AND Email LIKE '%" + contato.Email + "%' ";
                        }

                        if (contato.Telefone != null && contato.Telefone != "")
                        {
                            query += "AND Telefone LIKE '%" + contato.Telefone + "%' ";
                        }

                        lista = con.Query<Contato>(query, new
                        {
                            IdContato = contato.IdContato,
                            Nome      = contato.Nome,
                            Email     = contato.Email,
                            Telefone  = contato.Telefone,
                            IdUsuario = idUsuario
                        }).ToList();
                    }
                }

                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
