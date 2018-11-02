using Projeto.Apresentacao.Filters;
using Projeto.Apresentacao.Models;
using Projeto.Negocio;
using Projeto.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projeto.Apresentacao.Controllers
{
    [Authorize]
    public class TarefaController : Controller
    {
        // GET: Tarefa
        [NoCache]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Listar(string usuarioLogado)
        {
            try
            {
                TarefaRepositorio rep   = new TarefaRepositorio();
                UsuarioRepositorio urep = new UsuarioRepositorio();
                Usuario usuario = urep.EncontrarPorLogin(usuarioLogado);
                List<Tarefa> listaRep = rep.ListarTodos(usuario.IdUsuario);
                List<TarefaViewModel> listaViewModel = new List<TarefaViewModel>();

                foreach (var tarefa in listaRep)
                {
                    string descricao = "";

                    if (tarefa.Descricao.Length > 28)
                    {
                        descricao = tarefa.Descricao.Substring(0, 28) + "...";
                    }
                    else
                    {
                        descricao = tarefa.Descricao;
                    }

                    listaViewModel.Add(new TarefaViewModel()
                    {
                        IdTarefa    = tarefa.IdTarefa,
                        Nome        = tarefa.Nome,
                        DataEntrega = tarefa.DataEntrega,
                        Descricao   = descricao
                    });
                }

                return Json(listaViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public JsonResult Alterar(TarefaEdicaoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TarefaRepositorio rep = new TarefaRepositorio();
                    Tarefa tarefa = new Tarefa()
                    {
                        IdTarefa    = model.IdTarefa,
                        Nome        = model.Nome,
                        DataEntrega = model.DataEntrega,
                        Descricao   = model.Descricao
                    };

                    tarefa = rep.Alterar(tarefa);

                    if (tarefa != null)
                    {
                        return Json(new { sucesso = true, dados = "Tarefa alterada com sucesso!" });
                    }
                    else
                    {
                        return Json(new { sucesso = false, dados = "Não foi possível alterar a tarefa." });
                    }
                }
                else
                {
                    return Json(new { sucesso = false, dados = ModelState.Values.SelectMany(v => v.Errors).ToList() });
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public JsonResult BuscarTarefa(int id)
        {
            try
            {
                TarefaRepositorio rep    = new TarefaRepositorio();
                Tarefa            tarefa = rep.EncontrarPorId(id);

                if (tarefa != null)
                {
                    TarefaViewModel tarefaViewModel = new TarefaViewModel()
                    {
                        IdTarefa    = tarefa.IdTarefa,
                        Nome        = tarefa.Nome,
                        DataEntrega = tarefa.DataEntrega,
                        Descricao   = tarefa.Descricao
                    };

                    return Json(new { sucesso = true, tarefa = tarefaViewModel }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Não foi possível encontrar o registro na base de dados." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public JsonResult Excluir(int id)
        {
            try
            {
                TarefaRepositorio rep = new TarefaRepositorio();
                Tarefa tarefa = rep.EncontrarPorId(id);

                if (tarefa != null)
                {
                    tarefa = rep.Excluir(tarefa);

                    if (tarefa != null)
                    {
                        return Json(new { sucesso = true, mensagem = "Registro excluído com sucesso!" });
                    }
                    else
                    {
                        return Json(new { sucesso = false, mensagem = "Não foi possível excluir o registro da base de dados!" });
                    }
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "O registro não foi encontrado na base de dados!" });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public JsonResult Inserir(string usuarioLogado, TarefaInclusaoViewModel tarefaInclusaoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TarefaRepositorio  rep  = new TarefaRepositorio();
                    UsuarioRepositorio urep = new UsuarioRepositorio();
                    Usuario usuario         = urep.EncontrarPorLogin(usuarioLogado);
                    Tarefa tarefa           = new Tarefa()
                    {
                        IdTarefa    = 0,
                        Nome        = tarefaInclusaoViewModel.Nome,
                        DataEntrega = tarefaInclusaoViewModel.DataEntrega,
                        Descricao   = tarefaInclusaoViewModel.Descricao
                    };

                    Tarefa tarefa_inserida = rep.Inserir(usuario.IdUsuario, tarefa);

                    TarefaViewModel tarefaViewModel = new TarefaViewModel()
                    {
                        IdTarefa    = tarefa_inserida.IdTarefa,
                        Nome        = tarefa_inserida.Nome,
                        DataEntrega = tarefa_inserida.DataEntrega,
                        Descricao   = tarefa_inserida.Descricao
                    };

                    return Json(new { sucesso = true, dados = tarefaViewModel });
                }
                else
                {
                    return Json(new { sucesso = false, dados = ModelState.Values.SelectMany(v => v.Errors).ToList() });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public JsonResult Filtrar(string usuarioLogado, TarefaFiltroViewModel filtro)
        {
            try
            {
                TarefaRepositorio rep   = new TarefaRepositorio();
                UsuarioRepositorio urep = new UsuarioRepositorio();
                Usuario usuario = urep.EncontrarPorLogin(usuarioLogado);

                Tarefa tarefa = new Tarefa()
                {
                    IdTarefa    = filtro.IdTarefa,
                    Nome        = filtro.Nome,
                    DataEntrega = filtro.DataEntrega,
                    Descricao   = filtro.Descricao
                };

                List<Tarefa>          listaRep       = rep.ListarTodos(usuario.IdUsuario, tarefa);
                List<TarefaViewModel> listaViewModel = new List<TarefaViewModel>();

                foreach (var model in listaRep)
                {
                    string descricao = "";

                    if (tarefa.Descricao.Length > 28)
                    {
                        descricao = tarefa.Descricao.Substring(0, 28) + "...";
                    }
                    else
                    {
                        descricao = tarefa.Descricao;
                    }

                    listaViewModel.Add(new TarefaViewModel()
                    {
                        IdTarefa    = model.IdTarefa,
                        Nome        = model.Nome,
                        DataEntrega = model.DataEntrega,
                        Descricao   = descricao
                    });
                }

                return Json(listaViewModel);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}