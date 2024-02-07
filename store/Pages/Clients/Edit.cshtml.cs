using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace store.Pages.Clients
{
    public class EditModel : PageModel
    {
		public ClientsInfo clientInfo = new ClientsInfo();  // Экземпляр класса для хранения информации о клиенте.
		public string errorMessage = "";  // Сообщение об ошибке, если что-то пойдет не так.
		public string successMessage = ""; // Сообщение об успешном выполнении операции.
		public void OnGet()
        {
			String id = Request.Query["id"];

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;Encrypt=False";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					// SQL запрос на вставку данных нового клиента.
					String sql = "SELECT * FROM clients WHERE id=@id";
					// Создание SQL команды с использованием параметров для избежания SQL инъекций.
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using(SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								clientInfo.id = "" + reader.GetInt32(0);
								clientInfo.name = reader.GetString(1);
								clientInfo.email = reader.GetString(2);
								clientInfo.phone = reader.GetString(3);
								clientInfo.address = reader.GetString(4);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
        }

        public void OnPost()
        {
			// Извлечение данных формы из запроса и сохранение в объекте clientInfo.
			clientInfo.id = Request.Form["id"];
			clientInfo.name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.address = Request.Form["address"];

			// Проверка на заполненность всех полей формы.
			if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 || 
				clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || 
				clientInfo.address.Length == 0)
			{
				errorMessage = "Please enter all the fields";
				return; // Возвращаемся раньше, чтобы не продолжать выполнение с неполными данными.
			}

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;Encrypt=False";

				// Создание и открытие подключения к базе данных.
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE clients " + 
								 "SET name=@name, email=@email, phone=@phone, address=@address " + 
								 "WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@address", clientInfo.address);
						command.Parameters.AddWithValue("@id", clientInfo.id);

						command.ExecuteNonQuery();  // Выполнение запроса без возврата данных.
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

            // Очистка полей формы после успешного добавления клиента.
            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successMessage = "Client updated successfully";
            Response.Redirect("/Clients/Index"); // Перенаправление на страницу списка клиентов.
		}
    }
}
