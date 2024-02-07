using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace store.Pages.Clients
{
    public class CreateModel : PageModel // Класс страницы для создания нового клиента.
    {
        public ClientsInfo clientInfo = new ClientsInfo();  // Экземпляр класса для хранения информации о клиенте.

        public string errorMessage = "";  // Сообщение об ошибке, если что-то пойдет не так.

        public string successMessage = ""; // Сообщение об успешном выполнении операции.

        public void OnGet()  // Метод OnGet обрабатывает GET-запросы и отображает форму для ввода. Не используем
        {
        }

        public void OnPost()   // Метод OnPost обрабатывает POST-запросы, отправленные из формы.
        {
            // Извлечение данных формы из запроса и сохранение в объекте clientInfo.
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            // Проверка на заполненность всех полей формы.
            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "Please enter all the fields";
                return; // Возвращаемся раньше, чтобы не продолжать выполнение с неполными данными.
            }

            // Попытка сохранения нового клиента в базу данных.
            try
            {
                // Строка подключения к базе данных.
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True;Encrypt=False";

                // Создание и открытие подключения к базе данных.
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL запрос на вставку данных нового клиента.
                    String sql = "INSERT INTO clients (name, email, phone, address) VALUES (@name, @email, @phone, @address)";

                    // Создание SQL команды с использованием параметров для избежания SQL инъекций.
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();  // Выполнение запроса без возврата данных.
                    }
                }
            }
            catch (Exception ex) // Ловим исключения при работе с базой данных.
            {
                errorMessage = ex.Message;
                return;
            }

            // Очистка полей формы после успешного добавления клиента.
            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            successMessage = "New client added correctly"; // Сообщаем об успехе.

            Response.Redirect("/Clients/Index"); // Перенаправление на страницу списка клиентов.
        }
    }
}
