using Ecommerce2021a.Data;
using Ecommerce2021a.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce2021a.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index()
        {
            List<Item> lista = new List<Item>();

            var carrinho = HttpContext.Session.GetString("Carrinho");

            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            }

            return View(lista);

        }

        [HttpGet]
        public IActionResult Comprar(int id, int qtd)
        {
            List<Item> lista = new List<Item>();

            var carrinho = HttpContext.Session.GetString("Carrinho");

            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            }


            using (var data = new ProdutoData())
            {

                var item = lista.SingleOrDefault(i => i.Produto.IdProduto == id);

                if (item == null)
                {
                    Produto produto = data.Read(id);

                    item = new Item();
                    item.Produto = produto;
                    item.Quantidade = qtd;
                    item.Valor = item.Produto.Valor;
                    lista.Add(item);
                }
                else
                {
                    item.Quantidade += qtd;
                }


                //TODO Converter Lista para String (Json)
                carrinho = System.Text.Json.JsonSerializer.Serialize<List<Item>>(lista);

                HttpContext.Session.SetString("Carrinho", carrinho);

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Remover(int id)
        {
            List<Item> lista = new List<Item>();

            var carrinho = HttpContext.Session.GetString("Carrinho");

            if (carrinho != null)
            {
                //TODO Converter String para Lista(Json)
                lista = System.Text.Json.JsonSerializer.Deserialize<List<Item>>(carrinho);
            }


            using (var data = new ProdutoData())
            {

                var item = lista.SingleOrDefault(i => i.Produto.IdProduto == id);
                lista.Remove(item);

                //TODO Converter Lista para String (Json)
                carrinho = System.Text.Json.JsonSerializer.Serialize<List<Item>>(lista);

                HttpContext.Session.SetString("Carrinho", carrinho);

                return RedirectToAction("Index");
            }
        }
    }
}
