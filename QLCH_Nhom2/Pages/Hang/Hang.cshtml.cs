using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.Hang
{
    public class HangModel : PageModel
    {
        public List<HangInfo> listHang = new List<HangInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM HANG where MaHang not like '%SP000000%'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HangInfo Hang = new HangInfo();
                                Hang.MaHang = reader.GetString(0);
                                Hang.TenHang = reader.GetString(1);
                                Hang.SoLuong = reader.GetInt32(2);
                                Hang.GiaNhap = reader.GetDecimal(3);
                                Hang.GiaBan = reader.GetDecimal(4);

                                listHang.Add(Hang);
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class HangInfo
    {
        public string? MaHang;
        public string? TenHang;
        public Int32 SoLuong;
        public Decimal GiaNhap;
        public Decimal GiaBan;
    }
}

