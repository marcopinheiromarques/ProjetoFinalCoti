using Projeto.Apresentacao.Models;
using Projeto.Negocio;
using Projeto.Repositorio;
using Projeto.Utilitario;
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

                        Usuario u = repositorio.EncontrarPorLogin(usuario.Login);

                        if (u.AlterouSenha == 1)
                        {
                            return View("AlterarSenhaManualmente",
                                        new UsuarioAlterarSenhaViewModel()
                                        {
                                            Login          = u.Login,
                                            Senha          = "",
                                            ConfirmarSenha = ""
                                        });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
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

        public JsonResult AlterarSenhaAutomaticamente(string email)
        {
            try
            {
                UsuarioRepositorio rep = new UsuarioRepositorio();
                Usuario            u   = rep.EncontrarPorEmail(email);  
                
                if (u != null)
                {
                    //gera nova senha aleatória
                    string novaSenha = Randomico.alfanumericoAleatorio(6);

                    //altera a senha do usuário
                    bool alterou = rep.AlterarSenha(u, novaSenha, 1);
                    
                    if (alterou)
                    {
                        //envia a nova senha por email
                        try
                        {
                            string corpo = $"Olá {u.Login},\n\n conforme solicitado, segue sua nova senha na agenda virtual: " + novaSenha;

                            Email e = new Email();
                            e.EnviarEmail(email,
                                          "Agenda Virtual - Alteração de Senha",
                                          corpo,
                                          null);

                            return Json(new { sucesso = true, mensagem = "Email com a senha enviado com sucesso!" });
                        }
                        catch (Exception e)
                        {
                            return Json(new { sucesso = false, mensagem = "Não foi possível enviar a senha por email: " + e.Message +
                                " A nova senha é: " + novaSenha });
                        }
                    }
                    else
                    {
                        return Json(new { sucesso = false, mensagem = "Não foi possível alterar a senha." });
                    }                   
                }
                else
                {
                    return Json(new { sucesso = false, mensagem = "Email não cadastrado na base de dados." });
                }
            }
            catch (Exception e)
            {
                return Json(new { sucesso = false, mensagem = e.Message });                
            }
        }

        public ActionResult AlterarSenhaManualmente(string login)
        {
            return View("AlterarSenhaManualmente", 
                new UsuarioAlterarSenhaViewModel()
                {
                    Login          = login,
                    Senha          = "",
                    ConfirmarSenha = ""
                });
        }

        public ActionResult AlterarSenha(UsuarioAlterarSenhaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioRepositorio rep = new UsuarioRepositorio();
                    Usuario            u   = rep.EncontrarPorLogin(model.Login);
                    rep.AlterarSenha(u, model.Senha, 0);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    throw e;
                }                 
            }
            else
            {
                return View("AlterarSenhaManualmente");
            }
        }

    }
}