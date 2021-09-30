using System;
using System.Data.SqlClient;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var constring = "Data source = localhost; Initial catalog = IntermediateProject; Integrated security = true";
                SqlConnection connection = new SqlConnection(constring);
                connection.Open();
                if (!(connection.State == System.Data.ConnectionState.Open))
                {
                    Console.WriteLine("Соединение не было установлено. Пожалуйста, убедитесь в наличии соединения.");
                    return;
                }
                var repeat_main = true;
                while (repeat_main)
                {
                    Console.WriteLine("\n\n                     -   -   -   Тест Банк   -   -   -\n\n");
                    Console.WriteLine("Добро пожаловать в кредитную систему Тест Банка!\nПожалуйста, пройдите регистрацию для заполнения анкеты и отправки заявки на кредит.\nВ качестве логина будет использоваться Ваш номер телефона.\nКредиты выдаются лицам старше 18 лет в размере до 100 000 сомони.\n\n");
                    Console.Write("1.   Регистрация;\n2.   Войти;\n3.   Выход.\n\nВвод:   ");
                    int.TryParse(Console.ReadLine(), out var signup_choice);
                    switch (signup_choice)
                    {
                        case 1:
                            Console.Clear();
                            Methods.Signup(connection);
                            break;
                        case 2:
                            Console.Clear();
                            Methods.Login(connection);
                            break;
                        case 3:
                            repeat_main = false;
                            break;
                        default:
                            Console.WriteLine("Неправильный ввод. Пожалуйста, повторите попытку.");
                            Console.WriteLine("\nНажмите любую клавишу . . . ");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                    }
                    Console.Clear();
                }
                connection.Close();
            }
            catch
            {
                Console.WriteLine("Ошибка при выполнении операции. Пожалуйста, повторите попытку.");
                Console.WriteLine("Нажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
                return;
            }
        }
    }
   

    class Methods
    {
        internal static void Signup(SqlConnection connection)
        {
            try
            {
                User user = new User();
                SqlCommand command = connection.CreateCommand();
                Console.WriteLine("\n                ---  Регистрация нового пользователя  ---\n");
                Console.Write("\n\nИмя:   ");
                user.First_Name = Console.ReadLine();
                if (string.IsNullOrEmpty(user.First_Name))
                    {
                    WrongInput();
                    return;
                    }
                Console.Write("\nФамилия:   ");
                user.Last_Name = Console.ReadLine();
                if (string.IsNullOrEmpty(user.Last_Name))
                {
                    WrongInput();
                    return;
                }
                Console.Write("\nДата рождения (формат: 1992-02-12):   ");
                user.Date_Of_Birth = DateTime.Parse(Console.ReadLine());
                Console.Write("\nГород:   ");
                user.City = Console.ReadLine();
                if (string.IsNullOrEmpty(user.City))
                {
                    WrongInput();
                    return;
                }
                Console.Write("\nАдрес:   ");
                user.Address = Console.ReadLine();
                if (string.IsNullOrEmpty(user.Address))
                {
                    WrongInput();
                    return;
                }
                Console.Write("\nНомер паспорта:   ");
                user.Passport_Number = Console.ReadLine();
                if (string.IsNullOrEmpty(user.Passport_Number))
                {
                    WrongInput();
                    return;
                }
                Console.Write("\nДата выдачи паспорта (формат: 1992-02-12):   ");
                user.Date_Of_Issue = DateTime.Parse(Console.ReadLine());
                Console.Write("\nДата истечения паспорта (формат: 1992-02-12):   ");
                user.Date_Of_Expiry = DateTime.Parse(Console.ReadLine());
                Console.Write("\nНомер телефона, 12-значный с указанием кода Таджикистана(992):   (+)");
                user.Phone_Number = Console.ReadLine();
                char[] number_chars = user.Phone_Number.ToCharArray();
                bool t = true;
                int char_count = 0;
                foreach (char ch in number_chars)
                {
                    char_count++;
                    if (!char.IsDigit(ch))
                    {
                        t = false;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(user.Phone_Number) || !t || char_count != 12)
                {
                    WrongInput();
                    return;
                }
                string check_existing_number = null;
                command.CommandText = "SELECT First_Name FROM Users WHERE Phone_Number = @phone_number";
                command.Parameters.AddWithValue("@phone_number", user.Phone_Number);
                var read_existing_number = command.ExecuteReader();
                while (read_existing_number.Read())
                {
                    check_existing_number = read_existing_number.GetString(0);
                }
                read_existing_number.Close();
                command.Parameters.Clear();
                if (!(string.IsNullOrEmpty(check_existing_number)))
                    {
                    Console.WriteLine("\nПользователь с данным логином уже существует. Пожалуйста, используйте другой номер.");
                    Console.WriteLine("\nНажмите любую клавишу. . .");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                    }
                Console.Write("\nЖелаемый пароль:   ");
                user.Password = Console.ReadLine();
                if (string.IsNullOrEmpty(user.Password))
                {
                    WrongInput();
                    return;
                }
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
                    WrongInput();
                    return;
                }
                command.Parameters.Clear();
                Console.Write("\nЯвляется ли пользователь администратором?\n\n1. Да;\n2. Нет.\n\nВвод:   ");
                int admin_signup_choice = int.Parse(Console.ReadLine());
                string admin_password = "admin1234";
                switch (admin_signup_choice)
                {
                    case 1:
                        Console.Write("\nВведите пароль для регистрации администратора:   ");
                        string admin_signup_password = Console.ReadLine();
                        if (admin_signup_password == admin_password)
                        {
                            command.CommandText = "UPDATE Users SET User_Type = 'Администратор' WHERE Phone_Number = @user_Phone_Number ";
                            command.Parameters.AddWithValue("@user_Phone_Number", user.Phone_Number);
                            var admin_signup_query = command.ExecuteNonQuery();
                            if (admin_signup_query == 0)
                            {
                                WrongInput();
                                return;
                            }
                            command.Parameters.Clear();
                            Console.WriteLine("\nПользователь успешно добавлен.");
                            Console.WriteLine("Нажмите любую клавишу . . . ");
                            Console.ReadLine();
                            Console.Clear();
                        }
                        else
                        {
                            WrongInput();
                        }
                        break;
                    case 2:
                        Console.WriteLine("\nПользователь успешно добавлен.");
                        Console.WriteLine("Нажмите любую клавишу . . . ");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    default:
                        WrongInput();
                        break;
                }
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }

        }
        internal static void Login(SqlConnection connection)
        {
            try
            {
                Console.WriteLine("\n                ---  Вход в систему  ---\n");
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Phone_Number, Password, User_Type FROM Users";
                var login_selection = command.ExecuteReader();
                Console.Write("\n\nНомер телефона:   (+)");
                string login_number = Console.ReadLine();
                Console.Write("\nПароль:   ");
                string login_password = Console.ReadLine();
                int user_type = 0;
                while (login_selection.Read())
                {
                    if (login_number == login_selection["Phone_Number"].ToString() && login_password == login_selection["Password"].ToString() && login_selection["User_Type"].ToString() == "Клиент")
                    {
                        user_type++;
                    }
                    if (login_number == login_selection["Phone_Number"].ToString() && login_password == login_selection["Password"].ToString() && login_selection["User_Type"].ToString() == "Администратор")
                    {
                        user_type += 2;
                    }
                }
                login_selection.Close();
                Console.Clear();
                if (user_type == 1)
                {
                    Login_User(connection, login_number);
                }

                if (user_type == 2)
                {
                    Login_Admin(connection);
                }

                if (user_type != 1 && user_type != 2)
                {
                    WrongInput();
                }
        }
            catch (Exception)
            {
                WrongInput();
                return;
            }

}
        internal static void Login_User(SqlConnection connection, string login_number)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                var repeat_login = true;
                while (repeat_login)
                {
                    Console.WriteLine("\n                ---  Личный кабинет  ---\n");
                    Console.Write("\n\n1.   Информация о пользователе;\n2.   Анкета;\n3.   Кредиты;\n4.   Выход в главное меню.\n\nВвод:   ");
                    int.TryParse(Console.ReadLine(), out int login_choice);
                    switch (login_choice)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("\n                ---  Информация о пользователе  ---\n");
                            command.CommandText = $"SELECT First_Name, Last_Name, Date_Of_Birth, City, Address, Passport_Number, Date_Of_Issue, Date_Of_Expiry, Phone_Number, Password FROM Users WHERE Phone_Number = @phone_number";
                            command.Parameters.AddWithValue("@phone_number", login_number);
                            var client_acc_info = command.ExecuteReader();
                            while (client_acc_info.Read())
                            {
                                var date_of_birth = DateTime.Parse(client_acc_info["Date_Of_Birth"].ToString());
                                var parsed_date_of_birth = date_of_birth.ToString("dd-MM-yyyy");

                                var date_of_expiry = DateTime.Parse(client_acc_info["Date_Of_Expiry"].ToString());
                                var parsed_date_of_expiry = date_of_expiry.ToString("dd-MM-yyyy");

                                var date_of_issue = DateTime.Parse(client_acc_info["Date_Of_Issue"].ToString());
                                var parsed_date_of_issue = date_of_issue.ToString("dd-MM-yyyy");

                                Console.WriteLine($"Имя:   {client_acc_info["First_Name"].ToString()},\nФамилия:   {client_acc_info["Last_Name"].ToString(),10},\nДата рождения:   {parsed_date_of_birth,10},\nГород:   {client_acc_info["City"].ToString()},\nАдрес:   {client_acc_info["Address"].ToString()},\nНомер паспорта:   {client_acc_info["Passport_Number"].ToString()},\nДата выдачи:   {parsed_date_of_issue},\nДата истечения:   {parsed_date_of_expiry},\nНомер телефона:   (+){client_acc_info["Phone_Number"].ToString()},\nПароль:  {client_acc_info["Password"].ToString()} ");
                            }
                            command.Parameters.Clear();
                            client_acc_info.Close();
                            Console.WriteLine("\nНажмите любую клавишу . . . ");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 2:
                            Console.Clear();
                            var repeat_form = true;
                            while (repeat_form)
                            {
                                Console.WriteLine("\n                ---  Анкета  ---\n");
                                Console.Write("\n\n1.   Заполнить анкету;\n2.   Просмотреть анкету;\n3.   Удалить анкету;\n4.   Выход из меню анкеты.\n\nВвод:   ");
                                int.TryParse(Console.ReadLine(), out int form_choice);
                                int accNumber = GetAccountId(connection, login_number);
                                switch (form_choice)
                                {
                                    case 1:
                                        Console.Clear();
                                        FillForm(connection, login_number);
                                        break;
                                    case 2:
                                        Console.Clear();
                                        ViewForm(connection, accNumber);
                                        break;
                                    case 3:
                                        Console.Clear();
                                        DeleteForm(connection, accNumber);
                                        break;
                                    case 4:
                                        repeat_form = false;
                                        break;
                                    default:
                                        WrongInput();
                                        break;
                                }
                                Console.Clear();

                            }
                            break;


                        case 3:
                            Console.Clear();
                            var repeat_credit = true;
                            while (repeat_credit)
                            {
                                Console.WriteLine("\n                ---  Кредиты  ---\n");
                                Console.Write("\n\n1.   Отправить заявку на кредит;\n2.   График погашения;\n3.   История заявок;\n4.   Выход из меню кредитов.\n\nВвод:   ");
                                int.TryParse(Console.ReadLine(), out int credit_choice);
                                int accNumber = GetAccountId(connection, login_number);
                                switch (credit_choice)
                                {
                                    case 1:
                                        Console.Clear();
                                        FillCredit(connection, login_number);
                                        break;
                                    case 2:
                                        Console.Clear();
                                        CreditStatus(connection, login_number);
                                        break;
                                    case 3:
                                        Console.Clear();
                                        ViewCredit(connection, login_number);
                                        break;
                                    case 4:
                                        repeat_credit = false;
                                        break;
                                    default:
                                        WrongInput();
                                        break;
                                }
                                Console.Clear();
                            }
                            break;
                        case 4:
                            repeat_login = false;
                            break;
                        default:
                            WrongInput();
                            break;
                    }
                    Console.Clear();
                }
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void Login_Admin(SqlConnection connection)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                var repeat_login = true;
                while (repeat_login)
                {
                    Console.WriteLine("\n                ---  Кабинет администратора  ---\n");
                    Console.Write("\n\n1.   Информация о пользователе;\n2.   Удалить данные;\n3.   Выход в главное меню.\n\nВвод:   ");
                    int.TryParse(Console.ReadLine(), out int login_choice);
                    switch (login_choice)
                    {
                        case 1:
                            ViewAll(connection);
                            Console.Clear();
                            break;
                        case 2:
                            DeleteAll(connection);
                            Console.Clear();
                            break;
                        case 3:
                            repeat_login = false;
                            Console.Clear();
                            break;
                        default:
                            WrongInput();
                            break;
                    }
                    Console.Clear();
                }
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void FillForm(SqlConnection connection, string login_number)
        {
            try
            {
                Client client = new Client();
                string check_marital_status = null;
                int accNumber = GetAccountId(connection, login_number);
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Marital_Status FROM Clients WHERE User_Id = @user_id";
                command.Parameters.AddWithValue("@user_id", accNumber);
                var check_existing_form = command.ExecuteReader();
                while (check_existing_form.Read())
                {
                    check_marital_status = check_existing_form.GetString(0);
                }
                check_existing_form.Close();
                command.Parameters.Clear();
                if (string.IsNullOrEmpty(check_marital_status))
                {
                    Console.WriteLine("\n                ---  Заполнение анкеты  ---\n");
                    Console.Write("Пол (м/ж):   ");
                    client.Gender = Console.ReadLine();
                    if (client.Gender != "м" && client.Gender != "ж")
                    {
                        WrongInput();
                        return;
                    }
                    Console.Write("\nВозраст:   ");
                    client.Age = int.Parse(Console.ReadLine());
                    if (client.Age <= 18 || client.Age >= 100)
                    {
                        WrongInput();
                        return;
                    }
                    Console.Write("\nГражданство(наименование страны с большой буквы):   ");
                    client.Nationality = Console.ReadLine();
                    bool t = true;
                    char[] nationality_char = client.Nationality.ToCharArray();
                    foreach (char ch in nationality_char)
                    {
                        if (char.IsDigit(ch))
                        {
                            t = false;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(client.Nationality) || !t)
                    {
                        WrongInput();
                        return;
                    }
                    Console.Write("\nСемейное положение(холост(а)/семьянин(ка)/в разводе/вдова(ец):   ");
                    client.Marital_Status = Console.ReadLine();
                    if (client.Marital_Status != "холост" && client.Marital_Status != "холоста" && client.Marital_Status != "семьянин" && client.Marital_Status != "семьянинка" && client.Marital_Status != "в разводе" && client.Marital_Status != "вдова" && client.Marital_Status != "вдовец")
                    {
                        WrongInput();
                        return;
                    }
                    Console.Write("\nМесячная заработная плата после вычета налогов (в сомони):   ");
                    client.Salary = int.Parse(Console.ReadLine());
                    if (client.Salary < 0)
                    {
                        WrongInput();
                        return;
                    }
                    Console.Write("\nКоличество закрытых кредитов:   ");
                    client.Credit_History = int.Parse(Console.ReadLine());
                    if (client.Credit_History < 0)
                    {
                        WrongInput();
                        return;
                    }
                    Console.Write("\nКоличество просрочек по кредитам:   ");
                    client.Number_Of_Delays = int.Parse(Console.ReadLine());
                    if (client.Number_Of_Delays < 0)
                    {
                        WrongInput();
                        return;
                    }
                    command.CommandText = "INSERT INTO Clients (User_Id, Marital_Status, Age, Nationality, Salary, Credit_History, Number_Of_Delays, Gender) VALUES(@user_id, @marital_status, @age, @nationality, @salary, @credit_history, @number_of_delays, @gender)";
                    command.Parameters.AddWithValue("@user_id", accNumber);
                    command.Parameters.AddWithValue("@marital_status", client.Marital_Status);
                    command.Parameters.AddWithValue("@age", client.Age);
                    command.Parameters.AddWithValue("@nationality", client.Nationality);
                    command.Parameters.AddWithValue("@salary", client.Salary);
                    command.Parameters.AddWithValue("@credit_history", client.Credit_History);
                    command.Parameters.AddWithValue("@number_of_delays", client.Number_Of_Delays);
                    command.Parameters.AddWithValue("@gender", client.Gender);
                    var form_query = command.ExecuteNonQuery();
                    if (form_query == 0)
                    {
                        WrongInput();
                        return;
                    }
                    command.Parameters.Clear();
                    Console.WriteLine("\nФорма успешно добавлена.\n\n Нажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                }
                if (!(string.IsNullOrEmpty(check_marital_status)))
                {
                    Console.WriteLine("\nУ вас уже имеется заполненная анкета. Для создания новой анкеты удалите нынешнюю.\n\nНажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }

        }
        internal static int GetAccountId(SqlConnection connection, string login_number)
        {
            
            int accNumber = 0;
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Id FROM Users WHERE Phone_Number = @phone_number";
            command.Parameters.AddWithValue("@phone_number", login_number);
            var get_acc_id = command.ExecuteReader();
            while (get_acc_id.Read())
            {
                accNumber = get_acc_id.GetInt32(0);
            }
            get_acc_id.Close();
            command.Parameters.Clear();
            return accNumber;
        }
        internal static void ViewForm(SqlConnection connection, int accNumber)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                Client client = new Client();
                command.CommandText = "SELECT Marital_Status, Age, Nationality, Salary, Credit_History, Number_Of_Delays, Gender FROM Clients WHERE User_Id = @user_id";
                command.Parameters.AddWithValue("@user_id", accNumber);
                var view_form = command.ExecuteReader();
                while (view_form.Read())
                {
                    client.Marital_Status = view_form.GetValue(0).ToString();
                    client.Age = view_form.GetInt32(1);
                    client.Nationality = view_form.GetValue(2).ToString();
                    client.Salary = view_form.GetInt32(3);
                    client.Credit_History = view_form.GetInt32(4);
                    client.Number_Of_Delays = view_form.GetInt32(5);
                    client.Gender = view_form.GetValue(6).ToString();
                }
                view_form.Close();
                command.Parameters.Clear();
                if (string.IsNullOrEmpty(client.Marital_Status))
                {
                    Console.WriteLine("\nВаша анкета не заполнена. \n");
                    Console.WriteLine("Нажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                Console.WriteLine("\n                ---  Ваша анкета  ---\n");
                Console.WriteLine($"\nПол:   {client.Gender},\nСемейное положение:   {client.Marital_Status},\nВозраст:   {client.Age},\nГражданство:   {client.Nationality},\nМесячная заработная плата:   {client.Salary},\nКоличество закрытых кредитов:   {client.Credit_History},\nКоличество просрочек в кредитной истории:   {client.Number_Of_Delays}\n");
                Console.WriteLine("\nНажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void DeleteForm(SqlConnection connection, int accNumber)
        {
            try
            {
                Console.WriteLine("Вы уверены, что хотите удалить свою анкету?\n\n1. Да;\n\n2. Нет.\n\nВвод:   \n");
                int delete_form_choice = int.Parse(Console.ReadLine());
                switch (delete_form_choice)
                {
                    case 1:
                        SqlCommand command = connection.CreateCommand();
                        string check_marital_status = null;
                        command.CommandText = "SELECT Marital_Status FROM Clients WHERE User_Id = @user_id";
                        command.Parameters.AddWithValue("@user_id", accNumber);
                        var check_existing_form2 = command.ExecuteReader();
                        while (check_existing_form2.Read())
                        {
                            check_marital_status = check_existing_form2.GetString(0);
                        }
                        check_existing_form2.Close();
                        command.Parameters.Clear();
                        if (string.IsNullOrEmpty(check_marital_status))
                        {
                            Console.WriteLine("\nВаша анкета не заполнена.\n");
                            Console.WriteLine("Нажмите любую клавишу . . . ");
                            Console.ReadLine();
                            Console.Clear();
                            return;
                        }
                        command.CommandText = "DELETE FROM Clients WHERE User_Id = @user_id";
                        command.Parameters.AddWithValue("@user_id", accNumber);
                        var delete_form = command.ExecuteNonQuery();
                        if (delete_form == 0)
                        {
                            WrongInput();
                            return;
                        }
                        Console.WriteLine("\nВаша анкета была удалена.\n");
                        Console.WriteLine("\nНажмите любую клавишу . . . ");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 2:
                        break;
                    default:
                        WrongInput();
                        break;
                }
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void FillCredit(SqlConnection connection, string login_number)
        {
            try
            {
                Credit credit = new Credit();
                Client client = new Client();
                int accNumber = GetAccountId(connection, login_number);
                int decision_count = 1; // 1 так как любая длительность кредита дает 1 балл согласно калькулятору
                string check_existing_credit = null;
                SqlCommand command = connection.CreateCommand();
                Console.WriteLine("\n                ---  Заполнение заявки на кредит  ---\n\n");
                command.CommandText = "SELECT cl.Marital_Status, cl.Age, cl.Nationality, cl.Salary, cl.Credit_History, cl.Number_Of_Delays, cl.Gender, cr.Status FROM Clients cl LEFT JOIN Credits cr ON cl.User_Id = cr.User_Id WHERE cl.User_Id = @user_id";
                command.Parameters.AddWithValue("@user_id", accNumber);
                var view_form = command.ExecuteReader();
                while (view_form.Read())
                {
                    client.Marital_Status = view_form.GetValue(0).ToString();
                    client.Age = view_form.GetInt32(1);
                    client.Nationality = view_form.GetValue(2).ToString();
                    client.Salary = view_form.GetInt32(3);
                    client.Credit_History = view_form.GetInt32(4);
                    client.Number_Of_Delays = view_form.GetInt32(5);
                    client.Gender = view_form.GetValue(6).ToString();
                    check_existing_credit = view_form.GetValue(7).ToString();
                }
                command.Parameters.Clear();
                view_form.Close();
                if (check_existing_credit == "1")
                {
                    Console.WriteLine("У вас уже имеется активный кредит. Пожалуйста, обратитесь в колл-центр банка.");
                    Console.WriteLine("\nНажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }

                if (string.IsNullOrEmpty(client.Marital_Status))
                {
                    Console.WriteLine("\nВаша анкета не заполнена. Перед заполнением заявки на кредит необходимо заполнить анкету. ");
                    Console.WriteLine("\nНажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                Console.Write("\nЦель кредита(бытовая техника/ремонт/электроника/прочее):   ");
                credit.Purpose = Console.ReadLine();
                if (credit.Purpose != "бытовая техника" && credit.Purpose != "ремонт" && credit.Purpose != "электроника" && credit.Purpose != "прочее")
                {
                    WrongInput();
                    return;
                }
                Console.Write("\nРазмер кредита (в сомони):   ");
                credit.Amount = int.Parse(Console.ReadLine());
                if (credit.Amount <= 0 || credit.Amount > 100000)
                {
                    WrongInput();
                    return;
                }
                Console.Write("\nДлительность погашения кредита в месяцах:   ");
                credit.Duration = int.Parse(Console.ReadLine());
                if (credit.Duration <= 0 || credit.Duration > 36)
                {
                    WrongInput();
                    return;
                }
                #region Calculator
                if (client.Marital_Status == "холост" || client.Marital_Status == "холоста")
                {
                    decision_count += 1;
                }
                if (client.Marital_Status == "семьянин" || client.Marital_Status == "семьянинка")
                {
                    decision_count += 2;
                }
                if (client.Marital_Status == "в разводе")
                {
                    decision_count += 1;
                }
                if (client.Marital_Status == "вдовец" || client.Marital_Status == "вдова")
                {
                    decision_count += 2;
                }
                if (client.Gender == "м")
                {
                    decision_count += 1;
                }
                if (client.Gender == "ж")
                {
                    decision_count += 2;
                }
                if (client.Age >= 26 && client.Age <= 35)
                {
                    decision_count += 1;
                }
                if (client.Age >= 36 && client.Age <= 62)
                {
                    decision_count += 2;
                }
                if (client.Age >= 63)
                {
                    decision_count += 1;
                }
                if (client.Nationality == "Таджикистан")
                {
                    decision_count += 1;
                }
                if (((credit.Amount / client.Salary) * 100) <= 80)
                {
                    decision_count += 4;
                }
                if (((credit.Amount / client.Salary) * 100) > 80 && ((credit.Amount / client.Salary) * 100) <= 150)
                {
                    decision_count += 3;
                }
                if (((credit.Amount / client.Salary) * 100) > 150 && ((credit.Amount / client.Salary) * 100) <= 250)
                {
                    decision_count += 2;
                }
                if (((credit.Amount / client.Salary) * 100) > 250)
                {
                    decision_count += 1;
                }
                if (client.Credit_History > 3)
                {
                    decision_count += 2;
                }
                if (client.Credit_History == 1 && client.Credit_History == 2)
                {
                    decision_count += 1;
                }
                if (client.Credit_History == 0)
                {
                    decision_count -= 1;
                }
                if (client.Number_Of_Delays > 7)
                {
                    decision_count -= 3;
                }
                if (client.Number_Of_Delays >= 5 && client.Number_Of_Delays <= 7)
                {
                    decision_count -= 2;
                }
                if (client.Number_Of_Delays == 4)
                {
                    decision_count -= 1;
                }
                if (credit.Purpose == "бытовая техника")
                {
                    decision_count += 2;
                }
                if (credit.Purpose == "ремонт")
                {
                    decision_count += 1;
                }
                if (credit.Purpose == "прочее")
                {
                    decision_count -= 1;
                }
                #endregion
                if (decision_count > 11)
                {
                    credit.Status = 1;
                }
                if (decision_count <= 11)
                {
                    credit.Status = 0;
                }
                command.CommandText = "INSERT INTO Credits (User_Id, Purpose, Amount, Duration, Status, Created_At) VALUES (@user_id, @purpose, @amount, @duration, @status, @created_at)";
                command.Parameters.AddWithValue("@user_id", accNumber);
                command.Parameters.AddWithValue("@purpose", credit.Purpose);
                command.Parameters.AddWithValue("@amount", credit.Amount);
                command.Parameters.AddWithValue("@duration", credit.Duration);
                command.Parameters.AddWithValue("@status", credit.Status);
                command.Parameters.AddWithValue("@created_at", DateTime.Now);
                var result = command.ExecuteNonQuery();
                command.Parameters.Clear();
                if (result == 0)
                {
                    WrongInput();
                    return;
                }

                Console.WriteLine($"\nЗаявка на кредит успешно отправлена.");
                Console.WriteLine("\nНажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void CreditStatus (SqlConnection connection, string login_number)
        {
            try
            {
                Console.WriteLine("\n                ---  График погашения  ---\n");
                Credit credit = new Credit();
                Client client = new Client();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Purpose, Amount, Duration, Created_At FROM Credits WHERE User_Id = @user_id AND Status = 1";
                command.Parameters.AddWithValue("@user_id", GetAccountId(connection, login_number));
                var read_approved_credit = command.ExecuteReader();
                while (read_approved_credit.Read())
                {
                    credit.Purpose = read_approved_credit.GetString(0);
                    credit.Amount = read_approved_credit.GetInt32(1);
                    credit.Duration = read_approved_credit.GetInt32(2);
                    credit.Created_At = read_approved_credit.GetDateTime(3);
                }
                var parsed_created_at = credit.Created_At.ToString("dd-MM-yyyy");
                command.Parameters.Clear();
                read_approved_credit.Close();

                if (string.IsNullOrEmpty(credit.Purpose))
                {
                    Console.WriteLine("У вас пока не имеется одобренных кредитов или отсутствует заявка на кредит.");
                    Console.WriteLine("\nНажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                var monthly_payment = credit.Amount / credit.Duration;
                var monthly_interest = credit.Amount * 0.03;
                var monthly_payment_total = ((credit.Amount * 1.03) / credit.Duration);
                Console.WriteLine($"\nПоздравляем! Одна из Ваших заявок была одобрена:\n\nДата подачи:   {parsed_created_at};\nЦель кредита:   {credit.Purpose};\nДлительность кредита:   {credit.Duration} мес.;\nРазмер кредита:   {credit.Amount} сомони.\n\n\n                     Ваш график погашения:\n\n  * выплаты указаны в сомони.\n\n");
                Console.WriteLine($"\nОсталось выплатить:   {monthly_payment_total*credit.Duration}\n");
                for (int i = 30; i <= credit.Duration * 30; i += 30)
                {
                    Console.WriteLine($"Месяц:{(i / 30),5};   Месяч. выплата:{Math.Round(Convert.ToDecimal(monthly_payment), 2),5} с.;   Проц.:{Math.Round(Convert.ToDecimal(monthly_interest), 2),5} с.;   Суммарно к оплате:{Math.Round(Convert.ToDecimal(monthly_payment_total), 2),5} с.;   Крайн. срок погашения:   {credit.Created_At.AddDays(i).ToString("dd-MM-yyyy"),5}");
                }
                Console.WriteLine("\nНажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();

            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void ViewCredit (SqlConnection connection, string login_number)
        {
            try
            {
                Console.WriteLine("\n                ---  История заявок  ---\n");
                Credit[] credits = new Credit[0];
                var credit_count = 0;
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Purpose, Amount, Duration, Created_At, Status FROM Credits WHERE User_Id = @user_id";
                command.Parameters.AddWithValue("@user_id", GetAccountId(connection, login_number));
                var read_credits = command.ExecuteReader();
                while (read_credits.Read())
                {
                    Credit credit = new Credit();
                    credit.Purpose = read_credits.GetString(0);
                    credit.Amount = read_credits.GetInt32(1);
                    credit.Duration = read_credits.GetInt32(2);
                    credit.Created_At = read_credits.GetDateTime(3);
                    credit.Status = read_credits.GetInt32(4);
                    if (credit.Status == 0)
                    {
                        credit.Status_Name = "Не одобрено";
                    }
                    if (credit.Status == 1)
                    {
                        credit.Status_Name = "Одобрено";
                    }
                    AddCredit(ref credits, credit);
                    credit_count++;
                }
                command.Parameters.Clear();
                read_credits.Close();
                if (credits.Length == 0)
                {
                    Console.WriteLine("У Вас не имеется заявок на кредит.");
                    Console.WriteLine("\nНажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                Console.WriteLine($"\nКоличество заявок на кредит:   {credit_count}\n");
                foreach (var credit in credits)
                {
                    Console.WriteLine($"\nЦель кредита: {credit.Purpose,5};   Размер кредита: {credit.Amount,5};   Срок погашения: {credit.Duration,5} мес.;   Дата заявки: {credit.Created_At.ToString("dd-MM-yyyy"),5}; Статус: {credit.Status_Name}");                  
                }
                Console.WriteLine("\nНажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void AddCredit (ref Credit[] credits, Credit credit)
        {
            try
            {
                if (credits == null)
                {
                    return;
                }

                Array.Resize(ref credits, credits.Length + 1);

                credits[credits.Length - 1] = credit;
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }
        }
        internal static void ViewAll (SqlConnection connection)
        {
            try
            {
                Console.Clear();
            User user = new User();
            Client client = new Client();
            SqlCommand command = connection.CreateCommand();
            Console.WriteLine($"\n                ---  Информация о пользователе  ---\n");
            Console.Write("Укажите логин пользователя для запроса информации:   ");
            string user_login = Console.ReadLine();
            string check_existing_login = null;
            command.CommandText = "SELECT Phone_Number FROM Users WHERE Phone_Number = @phone_number";
            command.Parameters.AddWithValue("@phone_number", user_login);
            var check_existing_user = command.ExecuteReader();
            while (check_existing_user.Read())
            {
                check_existing_login = check_existing_user.GetString(0);
            }
            check_existing_user.Close();
            command.Parameters.Clear();
            if(string.IsNullOrEmpty(check_existing_login))
            {
                Console.WriteLine("\nДанного пользователя не существует.");
                Console.WriteLine("\nНажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            Console.Write($"\n                   --- Данные об аккаунте пользователя {user_login} ---   \n");
            command.CommandText = $"SELECT First_Name, Last_Name, Date_Of_Birth, City, Address, Passport_Number, Date_Of_Issue, Date_Of_Expiry, Phone_Number, Password FROM Users WHERE Phone_Number = @phone_number";
            command.Parameters.AddWithValue("@phone_number", user_login);
            var client_acc_info = command.ExecuteReader();
            while (client_acc_info.Read())
            {
                var date_of_birth = DateTime.Parse(client_acc_info["Date_Of_Birth"].ToString());
                var parsed_date_of_birth = date_of_birth.ToString("dd-MM-yyyy");

                var date_of_expiry = DateTime.Parse(client_acc_info["Date_Of_Expiry"].ToString());
                var parsed_date_of_expiry = date_of_expiry.ToString("dd-MM-yyyy");

                var date_of_issue = DateTime.Parse(client_acc_info["Date_Of_Issue"].ToString());
                var parsed_date_of_issue = date_of_issue.ToString("dd-MM-yyyy");

                Console.WriteLine($"Имя:   {client_acc_info["First_Name"].ToString()},\nФамилия:   {client_acc_info["Last_Name"].ToString(),10},\nДата рождения:   {parsed_date_of_birth,10},\nГород:   {client_acc_info["City"].ToString()},\nАдрес:   {client_acc_info["Address"].ToString()},\nНомер паспорта:   {client_acc_info["Passport_Number"].ToString()},\nДата выдачи:   {parsed_date_of_issue},\nДата истечения:   {parsed_date_of_expiry},\nНомер телефона:   (+){client_acc_info["Phone_Number"].ToString()},\nПароль:  {client_acc_info["Password"].ToString()} ");
            }
            command.Parameters.Clear();
            client_acc_info.Close();
            Console.Write($"\n                   --- Анкетные данные пользователя {user_login} ---   \n");
            int accNumber = GetAccountId(connection, user_login);
            command.CommandText = "SELECT Marital_Status, Age, Nationality, Salary, Credit_History, Number_Of_Delays, Gender FROM Clients WHERE User_Id = @user_id";
            command.Parameters.AddWithValue("@user_id", accNumber);
            var view_form = command.ExecuteReader();
            while (view_form.Read())
            {
                client.Marital_Status = view_form.GetValue(0).ToString();
                client.Age = view_form.GetInt32(1);
                client.Nationality = view_form.GetValue(2).ToString();
                client.Salary = view_form.GetInt32(3);
                client.Credit_History = view_form.GetInt32(4);
                client.Number_Of_Delays = view_form.GetInt32(5);
                client.Gender = view_form.GetValue(6).ToString();
            }
            view_form.Close();
            command.Parameters.Clear();
            if (string.IsNullOrEmpty(client.Marital_Status))
            {
                Console.WriteLine("\nАнкета пользователя не заполнена. \n");
                Console.WriteLine("Нажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            Console.WriteLine($"\nПол:   {client.Gender},\nСемейное положение:   {client.Marital_Status},\nВозраст:   {client.Age},\nГражданство:   {client.Nationality},\nМесячная заработная плата:   {client.Salary},\nКоличество закрытых кредитов:   {client.Credit_History},\nКоличество просрочек в кредитной истории:   {client.Number_Of_Delays}\n");
            Console.Write($"\n                   --- Кредитные заявки пользователя {user_login} ---   \n\n");
            Credit [] credits = new Credit[0];
            int credit_count = 0;
            command.CommandText = "SELECT Purpose, Amount, Duration, Created_At, Status FROM Credits WHERE User_Id = @user_id";
            command.Parameters.AddWithValue("@user_id", GetAccountId(connection, user_login));
            var read_credits = command.ExecuteReader();
            while (read_credits.Read())
            {
                Credit credit = new Credit();
                credit.Purpose = read_credits.GetString(0);
                credit.Amount = read_credits.GetInt32(1);
                credit.Duration = read_credits.GetInt32(2);
                credit.Created_At = read_credits.GetDateTime(3);
                credit.Status = read_credits.GetInt32(4);
                if (credit.Status == 0)
                {
                    credit.Status_Name = "Не одобрено";
                }
                if (credit.Status == 1)
                {
                    credit.Status_Name = "Одобрено";
                }
                AddCredit(ref credits, credit);
                credit_count++;
            }
            command.Parameters.Clear();
            read_credits.Close();
            if (credits.Length == 0)
            {
                Console.WriteLine("У пользователя нет заявок на кредит.");
                Console.WriteLine("\nНажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            Console.WriteLine($"\nКоличество заявок на кредит:   {credit_count}\n");
            foreach (var credit in credits)
            {
                Console.WriteLine($"\nЦель кредита: {credit.Purpose,5};   Размер кредита: {credit.Amount,5};   Срок погашения: {credit.Duration,5} мес.;   Дата заявки: {credit.Created_At.ToString("dd-MM-yyyy"),5}; Статус: {credit.Status_Name}");
            }
            Console.WriteLine("\nНажмите любую клавишу . . . ");
            Console.ReadLine();
            Console.Clear();
            }
            catch (Exception)
            {
                WrongInput();
                return;
            }

        }
        internal static void DeleteAll (SqlConnection connection)
        {
            try { 
            Console.Clear();
            SqlCommand command = connection.CreateCommand();
            Console.WriteLine("\n                ---  Удаление аккаунта  ---\n\n");
            Console.Write("Укажите логин пользователя, которого хотите удалить:   ");
            string user_login = Console.ReadLine();
            string check_existing_login = null;
            command.CommandText = "SELECT Phone_Number FROM Users WHERE Phone_Number = @phone_number";
            command.Parameters.AddWithValue("@phone_number", user_login);
            var check_existing_user = command.ExecuteReader();
            while (check_existing_user.Read())
            {
                check_existing_login = check_existing_user.GetString(0);
            }
            check_existing_user.Close();
            command.Parameters.Clear();
            if (string.IsNullOrEmpty(check_existing_login))
            {
                Console.WriteLine("\nДанного пользователя не существует.");
                Console.WriteLine("\nНажмите любую клавишу . . . ");
                Console.ReadLine();
                Console.Clear();
                return;
            }
            Console.WriteLine("\nВы уверены, что хотите удалить данного пользователя и все его данные?\n\n1. Да;\n\n2. Нет.\n\nВвод:   \n");
            int delete_account_choice = int.Parse(Console.ReadLine());
            int accNumber = GetAccountId(connection, user_login);
            switch (delete_account_choice)
            {
                case 1:
                    command.CommandText = "DELETE FROM Users WHERE Id = @user_id";
                    command.Parameters.AddWithValue("@user_id", accNumber);
                    var delete_acc = command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    Console.WriteLine("\nАккаунт был удален.\n");
                    Console.WriteLine("\nНажмите любую клавишу . . . ");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 2:
                    break;
                default:
                    WrongInput();
                    break;
            }
            Console.Clear();
        }
            catch (Exception)
            {
                WrongInput();
                return;
            }
}
        internal static void WrongInput()
        {
            Console.WriteLine("\nОшибка. Пожалуйста, убедитесь в правильности введённых данных и повторите попытку.");
            Console.WriteLine("\nНажмите любую клавишу . . . ");
            Console.ReadLine();
            Console.Clear();
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
    class Client
    {
        public string Gender { get; set; }
        public string Marital_Status { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public int Salary { get; set; }
        public int Credit_History { get; set; }
        public int Number_Of_Delays { get; set; }
    }
    class Credit
    {
        public string Purpose { get; set; }
        public int Amount { get; set; }
        public int Duration { get; set; }
        public int Status { get; set; }
        public DateTime Created_At { get; set; }
        public string Status_Name { get; set; }
    }
}
