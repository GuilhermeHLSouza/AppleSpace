﻿using Applespace.Data;
using Applespace.Libraries.LoginClientes;
using Applespace.Models;
using MySql.Data.MySqlClient;

namespace Applespace.Repositorio.Compra
{
    public class CompraRepositorio : ICompraRepositorio
    {
        private readonly Database _db;

        public CompraRepositorio(Database db)
        {
            _db = db;
        }

        public void Venda(Vendas venda)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"INSERT INTO Venda 
                        (Forma_Pgm, Statu, Id_Carrinho) 
                       VALUES 
                        (@forma, @status, @idCarrinho)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.Add("@forma", MySqlDbType.VarChar).Value = venda.formPgm;
                cmd.Parameters.Add("@status", MySqlDbType.VarChar).Value = venda.status;
                cmd.Parameters.Add("@idCarrinho", MySqlDbType.Int32).Value = venda.idCarrinho;

                cmd.ExecuteNonQuery();
            }
        }

        public void RegistroEndereço(int idCliente, int cep, int numero, string rua, string bairro, string complemento)
        {
            using (MySqlConnection conn = _db.GetConnection())
            {
                string sql = @"INSERT INTO Enderecos (CEP, Numero, Rua, Bairro, Complemeto, Id_Cliente)
                            VALUES (@cep, @num, @rua, @bairro, @complemento, @idCliente)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cep", cep);
                cmd.Parameters.AddWithValue("@num", numero);
                cmd.Parameters.AddWithValue("@rua", rua);
                cmd.Parameters.AddWithValue("@bairro", bairro);
                cmd.Parameters.AddWithValue("@complemento", complemento);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);

                cmd.ExecuteNonQuery();
            }
        }

        public void SelectUsuario(Clientes cliente)
        {
            if (cliente != null)
            {
                using (MySqlConnection conn = _db.GetConnection())
                {
                    string sql = @"SELECT * FROM Clientes WHERE Clientes.Email = @email and Clientes.Senha = @senha";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", cliente.email);
                    cmd.Parameters.AddWithValue("@senha", cliente.senha);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente.idCliente = reader.GetInt32("Id_Cliente");
                            cliente.senha = reader.GetString("Senha");
                            cliente.nome = reader.GetString("Nome");
                            cliente.email = reader.GetString("Email");
                            cliente.CPF = reader.GetString("Cpf");
                            cliente.telefone = reader.GetString("Telefone");
                        }
                    }
                }
            }
        }
    }
}