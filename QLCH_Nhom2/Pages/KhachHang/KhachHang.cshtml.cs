using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.KhachHang
{
    public class KhachHangModel : PageModel
    {
        public List<KHInfo> listKH = new List<KHInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from KHACHHANG where MaKH not like '%KH000000%'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                KHInfo KH = new KHInfo();
                                KH.MaKH = reader.GetString(0);
                                KH.TenKH = reader.GetString(1);
                                KH.SDT = reader.GetString(2);
                                KH.DiaChi = reader.GetString(3);

                                listKH.Add(KH);
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

    public class KHInfo
    {
        public string? MaKH;
        public string? TenKH;
        public string? SDT;
        public string DiaChi;
    }
}