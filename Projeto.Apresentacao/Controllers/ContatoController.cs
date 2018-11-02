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
    public class ContatoController : Controller
    {
        // GET: Contato
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
                ContatoRepositorio rep                = new ContatoRepositorio();
                UsuarioRepositorio urep               = new UsuarioRepositorio();
                Usuario            usuario            = urep.EncontrarPorLogin(usuarioLogado);
                List<Contato> listaRep                = rep.ListarTodos(usuario.IdUsuario);
                List<ContatoViewModel> listaViewModel = new List<ContatoViewModel>();

                foreach (var contato in listaRep)
                {
                    listaViewModel.Add(new ContatoViewModel()
                    {
                        IdContato = contato.IdContato,
                        Nome      = contato.Nome,
                        Email     = contato.Email,
                        Telefone  = contato.Telefone
                    });
                }

                return Json(listaViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;                
            }
        }

        
        public JsonResult Alterar(ContatoEdicaoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContatoRepositorio rep = new ContatoRepositorio();
                    Contato contato = new Contato()
                    {
                        IdContato = model.IdContato,
                        Nome      = model.Nome,
                        Email     = model.Email,
                        Telefone  = model.Telefone
                    };

                    contato = rep.Alterar(contato);

                    if (contato != null)
                    {
                        return Json(new { sucesso = true, dados = "Contato alterado com sucesso!" });
                    }
                    else
                    {
                        return Json(new { sucesso = false, dados = "Não foi possível alterar o contato." });
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

        
        public JsonResult BuscarContato(int id)
        {
            try
            {
                ContatoRepositorio rep     = new ContatoRepositorio();
                Contato            contato = rep.EncontrarPorId(id);

                if (contato != null)
                {
                    ContatoViewModel contatoViewModel = new ContatoViewModel()
                    {
                        IdContato = contato.IdContato,
                        Nome      = contato.Nome,
                        Email     = contato.Email,
                        Telefone  = contato.Telefone
                    };

                    return Json(new { sucesso = true, contato = contatoViewModel }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Não foi possível encontrar o registro na base de dados."}, JsonRequestBehavior.AllowGet);
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
                ContatoRepositorio rep     = new ContatoRepositorio();
                Contato            contato = rep.EncontrarPorId(id);
 
                if (contato != null)
                {
                    contato = rep.Excluir(contato);

                    if (contato != null)
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

        
        public JsonResult Inserir(string usuarioLogado, ContatoInclusaoViewModel contatoInclusaoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContatoRepositorio rep     = new ContatoRepositorio();
                    UsuarioRepositorio urep    = new UsuarioRepositorio();
                    Usuario            usuario = urep.EncontrarPorLogin(usuarioLogado);
                    Contato            contato = new Contato()
                    {
                        IdContato = 0,
                        Nome      = contatoInclusaoViewModel.Nome,
                        Email     = contatoInclusaoViewModel.Email,
                        Telefone  = contatoInclusaoViewModel.Telefone
                    };

                    Contato contato_inserido = rep.Inserir(usuario.IdUsuario, contato);

                    ContatoViewModel contatoViewModel = new ContatoViewModel()
                    {
                        IdContato = contato_inserido.IdContato,
                        Nome      = contato_inserido.Nome,
                        Email     = contato_inserido.Email,
                        Telefone  = contato_inserido.Telefone
                    };

                    return Json(new { sucesso = true, dados = contatoViewModel });
                }
                else
                {
                    return Json(new { sucesso = false, dados = ModelState.Values.SelectMany(v => v.Errors).ToList()});
                }                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
        public JsonResult Filtrar(string usuarioLogado, ContatoFiltroViewModel filtro)
        {
            try
            {
                ContatoRepositorio rep     = new ContatoRepositorio();
                UsuarioRepositorio urep    = new UsuarioRepositorio();
                Usuario            usuario = urep.EncontrarPorLogin(usuarioLogado);

                Contato contato = new Contato()
                {
                    IdContato = filtro.IdContato,
                    Nome      = filtro.Nome,
                    Email     = filtro.Email,
                    Telefone  = filtro.Telefone
                };

                List<Contato> listaRep = rep.ListarTodos(usuario.IdUsuario, contato);
                List<ContatoViewModel> listaViewModel = new List<ContatoViewModel>();

                foreach (var model in listaRep)
                {
                    listaViewModel.Add(new ContatoViewModel()
                    {
                        IdContato = model.IdContato,
                        Nome      = model.Nome,
                        Email     = model.Email,
                        Telefone  = model.Telefone
                    });
                }

                return Json(listaViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}