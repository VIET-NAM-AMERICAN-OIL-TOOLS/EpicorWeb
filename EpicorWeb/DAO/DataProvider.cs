using System.Data;
using System.Data.SqlClient;

namespace EpicorWeb.DAO
{
    public class DataProvider
    {
        private IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();


        public DataTable ExecuteQuery(string query, object[]? parameter = null)
        {
            var defaultConnection = configuration.GetConnectionString("DefaultConnection");
            DataTable data = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(defaultConnection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Đặt CommandTimeout trước khi thêm các tham số
                        command.CommandTimeout = 3600; // 3600 giây = 1 giờ

                        if (parameter != null)
                        {
                            AddParametersToCommand(command, query, parameter);
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(data);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý ngoại lệ tại đây
                Console.WriteLine($"Error in ExecuteQuery: {ex.Message}");
            }
            
            return data;
        }

        public int ExecuteNonQuery(string query, object[]? parameter = null)
        {
            var defaultConnection = configuration.GetConnectionString("DefaultConnection");
            int data = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(defaultConnection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Đặt CommandTimeout trước khi thêm các tham số
                        command.CommandTimeout = 3600; // 3600 giây = 1 giờ

                        if (parameter != null)
                        {
                            AddParametersToCommand(command, query, parameter);
                        }

                        data = command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý ngoại lệ tại đây
                Console.WriteLine($"Error in ExecuteNonQuery: {ex.Message}");
            }

            return data;
        }

        public object ExecuteScalar(string query, object[]? parameter = null)
        {
            var defaultConnection = configuration.GetConnectionString("DefaultConnection");
            object data = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(defaultConnection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Đặt CommandTimeout trước khi thêm các tham số
                        command.CommandTimeout = 3600; // 3600 giây = 1 giờ

                        if (parameter != null)
                        {
                            AddParametersToCommand(command, query, parameter);
                        }

                        data = command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý ngoại lệ tại đây
                Console.WriteLine($"Error in ExecuteScalar: {ex.Message}");
            }

            return data;
        }

        private void AddParametersToCommand(SqlCommand command, string query, object[] parameter)
        {
            string[] listPara = query.Split(' ');
            int i = 0;
            foreach (string item in listPara)
            {
                if (item.Contains('@'))
                {
                    command.Parameters.AddWithValue(item, parameter[i]);
                    i++;
                }
            }
        }

    }
}
