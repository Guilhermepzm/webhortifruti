﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce2021a.Data
{
    public abstract class Data : IDisposable
    {
        //atributo: vai nos permitir conectar com o Banco de Dados
        protected SqlConnection connectionDB;
        //string de conexão como banco de dados



        protected Data()
        {
            try
            {
                string strConexao = "Data Source = DESKTOP-RQAQ2G0; " +
                    "Initial Catalog = BDEcommerce;" +
                    " Integrated Security = true;";

                connectionDB = new SqlConnection(strConexao);

                connectionDB.Open();
            }
            catch (SqlException er)
            {
                Console.WriteLine("Erro do banco" + er);
            }
        }

        public void Dispose()
        {
            connectionDB.Close();
        }
    }
}
