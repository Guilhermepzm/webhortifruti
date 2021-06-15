using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce2021a.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public DateTime Data { get; set; }
        public List<Item> Itens { get; set; }

    }
}
