using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace store.Pages.Clients
{
	// ����� ������ �������� ��� �������� ������� ��������.
	// ����������� �� PageModel ��� ������ � Razor Pages.
	public class IndexModel : PageModel
	{
		// ������ ��� �������� ���������� � ��������, ���������� �� ���� ������.
		public List<ClientsInfo> listClients = new List<ClientsInfo>();

		// ����� OnGet ������������ GET-������ � ��������� ������ �������� �������.
		public void OnGet()
		{
			try
			{
				// ������ ����������� � ���� ������ SQL Server.
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;Encrypt=False";

				using (SqlConnection connection = new SqlConnection(connectionString)) // �������� ����������� � ���� ������ � �������������� ������ �����������.
				{
					connection.Open(); // - �������� ����������� � ���� ������.
					String sql = "SELECT * FROM clients"; // SQL ������ �� ������� ���� �������� �� �������.
					using (SqlCommand command = new SqlCommand(sql, connection)) // �������� ������� ��� ���������� SQL �������.
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read()) // ������ ������, ���������� �� ���� ������, ���������.
							{
								ClientsInfo clientsInfo = new ClientsInfo(); // �������� ������ ������� ClientsInfo ��� �������� ������ �������.
								clientsInfo.id = "" + reader.GetInt32(0);
								clientsInfo.name = reader.GetString(1);
								clientsInfo.email = reader.GetString(2);
								clientsInfo.phone = reader.GetString(3);
								clientsInfo.address = reader.GetString(4);
								clientsInfo.created_at = reader.GetDateTime(5).ToString();

								listClients.Add(clientsInfo); // ���������� ������� � ������� ������� � ������.
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.ToString()); // ����� ���������� �� ���������� � ������� (��� ������ ������).
			}
		}
	}

	public class ClientsInfo // ����� ��� �������� ���������� � �������, ��������������� ��������� ������� � ���� ������.
	{
		public string id; 
		public string name; 
		public string email; 
		public string phone; 
		public string address; 
		public string created_at; 
	}
}
