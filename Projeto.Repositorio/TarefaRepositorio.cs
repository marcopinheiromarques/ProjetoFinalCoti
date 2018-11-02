using Projeto.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace Projeto.Repositorio
{
    public class TarefaRepositorio
    {
        private readonly string stringConexao;

        public TarefaRepositorio()
        {
            stringConexao = ConexaoBanco.stringConexao;
        }

        public Tarefa Inserir(int idUsuario, Tarefa tarefa)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "INSERT INTO Tarefas (Nome, DataEntrega, Descricao, IdUsuario) VALUES (@Nome, @DataEntrega, @Descricao, @IdUsuario);" +
                       "SELECT CONVERT(int, SCOPE_IDENTITY())";

                    int id = (int)con.ExecuteScalar(query, new
                    {
                        IdTarefa    = tarefa.IdTarefa,
                        Nome        = tarefa.Nome,
                        DataEntrega = tarefa.DataEntrega,
                        Descricao   = tarefa.Descricao,
                        IdUsuario   = idUsuario
                    });

                    tarefa.IdTarefa = id;
                }

                return tarefa;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Tarefa Alterar(Tarefa tarefa)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "UPDATE Tarefas SET Nome = @Nome, DataEntrega = @DataEntrega, Descricao = @Descricao" +
                        " WHERE IdTarefa = @IdTarefa";

                    con.Execute(query, tarefa);
                }

                return tarefa;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Tarefa Excluir(Tarefa tarefa)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "DELETE FROM Tarefas WHERE IdTarefa = @IdTarefa";

                    con.Execute(query, tarefa);                    
                }

                return tarefa;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Tarefa EncontrarPorId(int id)
        {
            try
            {
                Tarefa tarefa = null;

                using(SqlConnection con = new SqlConnection(stringConexao))
                {
                    string query = "SELECT * FROM Tarefas WHERE IdTarefa = @IdTarefa";

                    tarefa = con.Query<Tarefa>(query, new { IdTarefa = id }).FirstOrDefault();
                }

                return tarefa;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Tarefa> ListarTodos(int idUsuario, Tarefa tarefa = null)
        {
            try
            {
                List<Tarefa> lista = null;

                using (SqlConnection con = new SqlConnection(stringConexao))
                {
                    if (tarefa == null)
                    {
                        string query = "SELECT * FROM Tarefas WHERE IdUsuario = @IdUsuario";

                        lista = con.Query<Tarefa>(query, new { IdUsuario = idUsuario }).ToList();
                    }
                    else
                    {
                        string query = "SELECT * FROM Tarefas WHERE IdUsuario = @IdUsuario ";

                        if (tarefa.IdTarefa > 0)
                        {
                            query += "AND IdTarefa = @IdTarefa ";
                        }

                        if (tarefa.Nome != null && tarefa.Nome != "")
                        {
                            query += "AND Nome LIKE '%" + tarefa.Nome + "%' ";
                        }

                        if (tarefa.DataEntrega != null && tarefa.DataEntrega != new DateTime())
                        {
                            query += "AND DataEntrega = @DataEntrega ";
                        }

                        if (tarefa.Descricao != null && tarefa.Descricao != "")
                        {
                            query += "AND Descricao LIKE '%" + tarefa.Descricao + "%' ";
                        }

                        lista = con.Query<Tarefa>(query, new
                        {
                            IdTarefa    = tarefa.IdTarefa,
                            Nome        = tarefa.Nome,
                            DataEntrega = tarefa.DataEntrega,
                            Descricao   = tarefa.Descricao,
                            IdUsuario   = idUsuario
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
