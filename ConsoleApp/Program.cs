using System;
using System.Data.SqlClient;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var constring = "Data source = localhost; Initial catalog = IntermediateProject; Integrated security = true";
            SqlConnection connection = new SqlConnection(constring);
            connection.Open();
            if (!(connection.State == System.Data.ConnectionState.Open))
            {
                Console.WriteLine("Соединение не было установлено. Пожалуйста, убедитесь в наличии соединения.");
                return;
            }
            var repeat = true;
            while (repeat)
            {
                Console.WriteLine("\n\n                     -   -   -   Алиф Банк   -   -   -\n\n");
                Console.Write("1.   Регистрация;\n2.   Войти;\n3.   Выход.\nВвод:   ");
                int.TryParse(Console.ReadLine(), out var signup_choice);
                switch (signup_choice)
                {
                    case 1:
                        Methods.Signup(connection);
                        break;
                    case 2:
                        Methods.Login(connection);
                        break;
                    case 3:
                        repeat = false;
                        break;
                    default:
                        Console.WriteLine("Неправильный ввод. Пожалуйста, повторите попытку.");
                        return;
                }
                Console.WriteLine("\n\nНажмите любую клавишу для выхода . . .");
                Console.ReadLine();
                Console.Clear();
            }
            connection.Close();        
        }
    }

    class Methods
    {

        internal static void Signup(SqlConnection connection)
        {
            User user = new User();
            SqlCommand command = connection.CreateCommand();

            try
            {
                Console.WriteLine("\n                ---  Регистрация нового пользователя  ---\n");
                Console.Write("Имя: ");
                user.First_Name = Console.ReadLine();
                Console.Write("\nФамилия: ");
                user.Last_Name = Console.ReadLine();
                Console.Write("\nДата рождения (формат: 1992-02-12): ");
                user.Date_Of_Birth = DateTime.Parse(Console.ReadLine());
                Console.Write("\nГород: ");
                user.City = Console.ReadLine();
                Console.Write("\nАдрес: ");
                user.Address = Console.ReadLine();
                Console.Write("\nНомер паспорта: ");
                user.Passport_Number = Console.ReadLine();
                Console.Write("\nДата выдачи паспорта (формат: 1992-02-12): ");
                user.Date_Of_Issue = DateTime.Parse(Console.ReadLine());
                Console.Write("\nДата истечения паспорта (формат: 1992-02-12): ");
                user.Date_Of_Expiry = DateTime.Parse(Console.ReadLine());
                Console.Write("\nНомер телефона: (+)");
                user.Phone_Number = Console.ReadLine();
                Console.Write("\nЖелаемый пароль (не менее 8 символов, должен содержать цифры и буквы латинского алфавита): ");
                user.Password = Console.ReadLine();
                command.CommandText = "INSERT INTO Users (Phone_Number, Password, Date_Of_Expiry, First_Name, Last_Name, Date_Of_Birth," +
                    "City, Address, Passport_Number, Date_Of_Issue) VALUES (@Phone_Number, @Password, @Date_Of_Expiry, @First_Name, " +
                    "@Last_Name, @Date_Of_Birth," +
                    "@City, @Address, @Passport_Number, @Date_Of_Issue)";
                command.Parameters.AddWithValue("@Phone_Number", user.Phone_Number);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Date_Of_Expiry", user.Date_Of_Expiry);
                command.Parameters.AddWithValue("@First_Name", user.First_Name);
                command.Parameters.AddWithValue("@Last_Name", user.Last_Name);
                command.Parameters.AddWithValue("@Date_Of_Birth", user.Date_Of_Birth);
                command.Parameters.AddWithValue("@City", user.City);
                command.Parameters.AddWithValue("@Address", user.Address);
                command.Parameters.AddWithValue("@Passport_Number", user.Passport_Number);
                command.Parameters.AddWithValue("@Date_Of_Issue", user.Date_Of_Issue);
                var signup_query = command.ExecuteNonQuery();
                if (signup_query == 0)
                {
                    Console.WriteLine("Операция не удалась. Пожалуйста, повторите попытку.");
                    return;
                }
                command.Parameters.Clear();
                Console.Write("\nЯвляется ли пользователь администратором?\n1. Да;\n2. Нет;\nВвод: ");
                int admin_signup_choice = int.Parse(Console.ReadLine());
                string admin_password = "admin1234";
                switch (admin_signup_choice)
                {
                    case 1:
                        Console.WriteLine("Введите пароль для регистрации администратора: ");
                        string admin_signup_password = Console.ReadLine();
                        if (admin_signup_password == admin_password)
                        {
                            command.CommandText = "UPDATE Users SET User_Type = 'Администратор' WHERE Phone_Number = @user_Phone_Number ";
                            command.Parameters.AddWithValue("@user_Phone_Number", user.Phone_Number);
                            var admin_signup_query = command.ExecuteNonQuery();
                            if (admin_signup_query == 0)
                            {
                                Console.WriteLine("Операция не удалась. Пожалуйста, повторите попытку.");
                                return;
                            }
                            command.Parameters.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Неправильный пароль. Пожалуйста, повторите попытку.");
                            return;
                        }
                        break;
                    case 2:
                        break;
                    default:
                        Console.WriteLine("Неправильный ввод. Пожалуйста, повторите попытку.");
                        return;
                        break;
                }
        }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при вводе данных. Пожалуйста, повторите попытку.");
                return;
            }

}

        internal static void Login (SqlConnection connection)
        {
            Console.WriteLine("Login");
        }
    }

    class User
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Passport_Number { get; set; }
        public DateTime Date_Of_Issue { get; set; }
        public DateTime Date_Of_Expiry { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string Password { get; set; }
        public string Phone_Number { get; set; }
        public string User_Type { get; set; }
    }
}
