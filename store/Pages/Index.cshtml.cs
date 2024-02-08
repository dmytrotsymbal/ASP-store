using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using store.Pages.Clients;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace store.Pages
{
    public class IndexModel : PageModel
    {
        public ClientsInfo clientInfo = new ClientsInfo();
        public List<ClientsInfo> serchedClients = new List<ClientsInfo>();

        public void OnGet()
        {
            clientInfo.name = Request.Query["name"];
            clientInfo.email = Request.Query["email"];
            clientInfo.phone = Request.Query["phone"];
            clientInfo.address = Request.Query["address"];

            if (!string.IsNullOrEmpty(clientInfo.address))
            {
                try
                {
                    string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;Encrypt=False";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM clients WHERE name LIKE @name AND email LIKE @email AND phone LIKE @phone AND address LIKE @address";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", $"%{clientInfo.name}%");
                            command.Parameters.AddWithValue("@email", $"%{clientInfo.email}%");
                            command.Parameters.AddWithValue("@phone", $"%{clientInfo.phone}%");
                            command.Parameters.AddWithValue("@address", $"%{clientInfo.address}%");

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ClientsInfo clientsInfo = new ClientsInfo();
                                    clientsInfo.id = "" + reader.GetInt32(0);
                                    clientsInfo.name = reader.GetString(1);
                                    clientsInfo.email = reader.GetString(2);
                                    clientsInfo.phone = reader.GetString(3);
                                    clientsInfo.address = reader.GetString(4);
                                    clientsInfo.created_at = reader.GetDateTime(5).ToString();

                                    serchedClients.Add(clientsInfo);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
            }
        }
    }


}
