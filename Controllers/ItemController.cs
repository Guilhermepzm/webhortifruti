using Ecommerce2021a.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce2021a.Controllers
{
    public class ItemController : Controller
    {
        public IActionResult Index(int id)
            {
                //Criando um objeto data da classe ProdutoData
                using (var data = new ItemData())
                    return View(data.Read(id));
                //data.Read() chama a execução do método Read (Select + From Produtos)
            }
    }
}
