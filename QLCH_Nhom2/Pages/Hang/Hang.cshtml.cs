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
                    string sql = "SELECT * FROM HANG where SoLuong >= 0";
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
        public HangInfo searchInfo = new HangInfo();
        public void OnPost()
        {
            searchInfo.Search = Request.Form["Search"];
            if (searchInfo.Search.Length == 0 )
            {
                Response.Redirect("/Hang/Hang");
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() {searchInfo.Search};
                    String sql1 = "select * from HANG where (MaHang like '%" + search[0] + "%' or TenHang like '%" + search[0] + "%') and SoLuong>=0";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
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
            catch (Exception)
            {
            }
        }
    }

    public class HangInfo
    {
        public string Search;
        public string? MaHang;
        public string? TenHang;
        public Int32 SoLuong;
        public Decimal GiaNhap;
        public Decimal GiaBan;
    }
}

