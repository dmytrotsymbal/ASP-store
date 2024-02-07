using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace store.Pages.Clients
{
    public class CreateModel : PageModel // ����� �������� ��� �������� ������ �������.
    {
        public ClientsInfo clientInfo = new ClientsInfo();  // ��������� ������ ��� �������� ���������� � �������.

        public string errorMessage = "";  // ��������� �� ������, ���� ���-�� ������ �� ���.

        public string successMessage = ""; // ��������� �� �������� ���������� ��������.

        public void OnGet()  // ����� OnGet ������������ GET-������� � ���������� ����� ��� �����. �� ����������
        {
        }

        public void OnPost()   // ����� OnPost ������������ POST-�������, ������������ �� �����.
        {
            // ���������� ������ ����� �� ������� � ���������� � ������� clientInfo.
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // �������� �� ������������� ���� ����� �����.
            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "Please enter all the fields";
                return; // ������������ ������, ����� �� ���������� ���������� � ��������� �������.
            }

            // ������� ���������� ������ ������� � ���� ������.
            try
            {
                // ������ ����������� � ���� ������.
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;Encrypt=False";

                // �������� � �������� ����������� � ���� ������.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL ������ �� ������� ������ ������ �������.
                    String sql = "INSERT INTO clients (name, email, phone, address) VALUES (@name, @email, @phone, @address)";

                    // �������� SQL ������� � �������������� ���������� ��� ��������� SQL ��������.
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();  // ���������� ������� ��� �������� ������.
                    }
                }
            }
            catch (Exception ex) // ����� ���������� ��� ������ � ����� ������.
            {
                errorMessage = ex.Message;
                return;
            }

            // ������� ����� ����� ����� ��������� ���������� �������.
            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successMessage = "New client added correctly"; // �������� �� ������.

            Response.Redirect("/Clients/Index"); // ��������������� �� �������� ������ ��������.
        }
    }
}
