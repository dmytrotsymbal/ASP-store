using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace store.Pages.Clients
{
	// Класс модели страницы для страницы индекса клиентов.
	// Наследуется от PageModel для работы с Razor Pages.
	public class IndexModel : PageModel
	{
		// Список для хранения информации о клиентах, полученной из базы данных.
		public List<ClientsInfo> listClients = new List<ClientsInfo>();

		// Метод OnGet обрабатывает GET-запрос и заполняет список клиентов данными.
		public void OnGet()
		{
			try
			{
				// Строка подключения к базе данных SQL Server.
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;Encrypt=False";

				using (SqlConnection connection = new SqlConnection(connectionString)) // Создание подключения к базе данных с использованием строки подключения.
				{
					connection.Open(); // - Открытие подключения к базе данных.
					String sql = "SELECT * FROM clients"; // SQL запрос на выборку всех клиентов из таблицы.
					using (SqlCommand command = new SqlCommand(sql, connection)) // Создание команды для выполнения SQL запроса.
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read()) // Чтение данных, полученных из базы данных, построчно.
							{
								ClientsInfo clientsInfo = new ClientsInfo(); // Создание нового объекта ClientsInfo для хранения данных клиента.
								clientsInfo.id = "" + reader.GetInt32(0);
								clientsInfo.name = reader.GetString(1);
								clientsInfo.email = reader.GetString(2);
								clientsInfo.phone = reader.GetString(3);
								clientsInfo.address = reader.GetString(4);
								clientsInfo.created_at = reader.GetDateTime(5).ToString();

								listClients.Add(clientsInfo); // Добавление объекта с данными клиента в список.
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.ToString()); // Вывод информации об исключении в консоль (или журнал ошибок).
			}
		}
	}

	public class ClientsInfo // Класс для хранения информации о клиенте, соответствующий структуре таблицы в базе данных.
	{
		public string id; 
		public string name; 
		public string email; 
		public string phone; 
		public string address; 
		public string created_at; 
	}
}
