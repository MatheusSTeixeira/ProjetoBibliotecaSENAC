using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System;
using Biblioteca.Models;
namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult ListaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View(new UsuarioService().Listar());
        }
        public IActionResult editarUsuario(int id)
        {
            Usuario u = new UsuarioService().Listar(id);
            return View(u);
        }
        [HttpPost]
        public IActionResult editarUsuario(Usuario userEditado)
        {
            UsuarioService us = new UsuarioService();
            us.editarUsuario(userEditado);

            return RedirectToAction("ListaDeUsuarios");
        }
        public IActionResult RegistrarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();
        }
        [HttpPost]
        public IActionResult RegistrarUsuarios(Usuario novoUser)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            novoUser.Senha = Criptografo.TextoCriptografado(novoUser.Senha);
            UsuarioService us = new UsuarioService();
            us.incluirUsuario(novoUser);

            return RedirectToAction("cadastroRealizado");
        }
        public IActionResult ExcluirUsuario(int id)
        {
            Usuario user2 = new UsuarioService().Listar(id);
            return View(user2);
        }
        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao,int id)
        {
            if(decisao=="EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do usuario "+new UsuarioService().Listar(id).Nome+" realizada com sucesso";
                new UsuarioService().excluirUsuario(id);
                return View("ListaDeUsuarios",new UsuarioService().Listar());
            }
            else {
                ViewData["Mensagem"] = "Exclusão cancelada";
                return View("ListaDeUsuarios",new UsuarioService().Listar(id));
            }
        }
        public IActionResult cadastroRealizado()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }
        public IActionResult NeedAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}