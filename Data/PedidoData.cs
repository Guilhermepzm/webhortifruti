using Ecommerce2021a.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;


namespace Ecommerce2021a.Data
{
    public class PedidoData : Data
    {
        public void Create(Pedido pedido)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connectionDB;
                // cmd.Transaction = transaction;
                cmd.CommandText = @"Insert into Pedido values (@idCliente, @data); SELECT SCOPE_IDENTITY();";
                cmd.Parameters.AddWithValue("@idCliente", pedido.IdCliente);
                cmd.Parameters.AddWithValue("@data", DateTime.Now);

                //ExecuteScalar: executa a consula e retorna a primeira coluna da primeira linha
                // no conjunto de resultados retornado pela consulta
                //colunas ou linhas adicionais são ignorados

                int idPedido = Convert.ToInt32(cmd.ExecuteScalar());


                foreach (var item in pedido.Itens)
                {
                    SqlCommand cmdItem = new SqlCommand();
                    cmdItem.Connection = connectionDB;
                    cmdItem.CommandText = @"Insert into ItemPedido values" +
                                           "(@idpedido, @idproduto, @quantidade)";

                    cmdItem.Parameters.AddWithValue("@idpedido", idPedido);
                    cmdItem.Parameters.AddWithValue("@idproduto", item.Produto.IdProduto);
                    cmdItem.Parameters.AddWithValue("@quantidade", item.Quantidade);

                    cmdItem.ExecuteNonQuery();
                }

                //Executa as inserções da transação nas tabelas
                // transaction.Commit();
            }
            catch (Exception ex)
            {
                //desfaz as operações de insert caso dê algum problema e elas não
                //possam ser executadas
            }
        }

        public List<Pedido> Read()
        {
            List<Pedido> lista = new List<Pedido>();
            List<Item> listaItens = new List<Item>();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connectionDB;
            cmd.CommandText = @"select p.IdPedido, p.IdCliente, p.Data, ip.Quantidade, (pr.Valor*ip.Quantidade) as ValorTotal, pr.* from pedido p inner join itemPedido ip on p.IdPedido = ip.IdPedido inner join produtos pr on ip.IdProduto = pr.IdProduto";

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Produto produto = new Produto();
                produto.IdProduto = (int)reader["IdProduto"];
                produto.Nome = (string)reader["Nome"];
                produto.Descricao = (string)reader["Descricao"];
                produto.Valor = (decimal)reader["Valor"];

                Item item = new Item();
                item.IdPedido = (int)reader["IdPedido"];
                item.Quantidade = (int)reader["Quantidade"];
                item.Valor = (decimal)reader["ValorTotal"];
                item.Produto = produto;

                listaItens.Add(item);

                Pedido p = new Pedido();
                p.IdPedido = (int)reader["IdPedido"];
                p.IdCliente = (int)reader["IdCliente"];
                p.Data = (DateTime)reader["Data"];
                p.Itens = listaItens;
                lista.Add(p);
            }
            return lista;
        }

        public List<Cliente> ClientesPedido()
        {
            SqlCommand cmdCli = new SqlCommand();
            cmdCli.Connection = connectionDB;
            cmdCli.CommandText = @"select IdCliente, Nome from Cliente";
            List<Cliente> listaCliente = new List<Cliente>();

            SqlDataReader readerCli = cmdCli.ExecuteReader();

            while (readerCli.Read())
            {
                Cliente cli = new Cliente();
                cli.IdCliente = (int)readerCli["IdCliente"];
                cli.Nome = (string)readerCli["Nome"];

                listaCliente.Add(cli);
            }
            return listaCliente;
        }
    }
}