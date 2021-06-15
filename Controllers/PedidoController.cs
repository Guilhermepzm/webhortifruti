using Ecommerce2021a.Data;
using Ecommerce2021a.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Ecommerce2021a.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            //Criando um objeto data da classe ProdutoData
            using (var data = new PedidoData())
            {
                var clientes = data.ClientesPedido();
                var listaClientes = System.Text.Json.JsonSerializer.Serialize<List<Cliente>>(clientes);

                HttpContext.Session.SetString("Clientes", listaClientes);
            }

            using (var data = new PedidoData())
                return View(data.Read());
            //data.Read() chama a execução do método Read (Select + From Produtos)
        }

        [HttpPost]
        public IActionResult Create(Pedido pedido)
        {
            //O ModeState é uma propriedade da classe Controller e pode ser
            //acessada a partir das classes que herdam de System.Web.Mvc.Controller.
            //Ele representa uma coleção de pares nome/valor que são submetidos
            //ao servidor durante o POST e também contém uma coleção de mensagens
            //de erros para cada calor submetido
            string json = HttpContext.Session.GetString("itens");
            List<Item> itens = JsonSerializer.Deserialize<List<Item>>(json);

            pedido.Itens = itens;
            List<Item> lista = new List<Item>();
            var carrinho = System.Text.Json.JsonSerializer.Serialize<List<Item>>(lista);
            HttpContext.Session.SetString("Carrinho", carrinho);
            using (var data = new PedidoData())
                data.Create(pedido);
            return RedirectToAction("Index", "Home");
        }
    }
}
