using Projeto.Apresentacao.Models;
using Projeto.Negocio;
using Projeto.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Projeto.Apresentacao.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        public ActionResult EsqueceuSenha()
        {
            return View();
        }

        public ActionResult RegistrarUsuario(UsuarioRegistroViewModel usuarioRegistro)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario()
                {
                    Login = usuarioRegistro.Login,
                    Email = usuarioRegistro.Email,
                    Senha = usuarioRegistro.Senha
                };

                //verifica se o login ou o e-mail já existe no banco de dados
                UsuarioRepositorio repositorio = new UsuarioRepositorio();

                try
                {
                    if (repositorio.ExisteLoginOuEmailJaCadastrados(usuario))
                    {
                        ViewBag.Mensagem = "O login ou e-mail já foram cadastrados. Favor escolhar outro.";
                    }
                    else
                    {
                        //Não existindo, poderá ser cadastrado no banco de dados.
                        usuario                  = repositorio.Inserir(usuario);
                        ModelState.Clear();
                        ViewBag.MensagemCadastro = "Conta Criada com Sucesso: " + usuario.ToString();
                        return View("Registro");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Mensagem = "Ocorreu um erro ao criar nova conta: " + e.Message;
                    return View("Registro");
                }
            }
            
            return View("Registro");            
        }

        public ActionResult AutenticarUsuario(LoginViewModel loginInserido)
        {
            if (ModelState.IsValid)
            {
                //valida os dados do login
                Usuario usuario = new Usuario()
                {
                    Login = loginInserido.Login,
                    Senha = loginInserido.Senha
                };

                try
                {
                    UsuarioRepositorio repositorio = new UsuarioRepositorio();

                    if (repositorio.ValidarLoginUsuario(usuario))
                    {
                        //cria o cookie de autenticação
                        ModelState.Clear();

                        FormsAuthentication.SetAuthCookie(loginInserido.Login, loginInserido.LembrarMe);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Mensagem = "Login ou Senha Inválidos.";
                        return View("Login");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Mensagem = "Erro: " + e.Message;
                    return View("Login");
                }
            }

            return View("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return View("Login");
        }
    }
}