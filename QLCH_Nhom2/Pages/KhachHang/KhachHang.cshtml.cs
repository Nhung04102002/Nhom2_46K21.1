using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
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

        public KHInfo searchInfo = new KHInfo();
        public void OnPost()
        {
            searchInfo.Search = Request.Form["Search"];
            if (searchInfo.Search.Length == 0)
            {
                Response.Redirect("/KhachHang/KhachHang");
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql1 = "select * from KHACHHANG where SDT like '%" + search[0] + "%' or MaKH like '%" + search[0] + "%'";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@Search", searchInfo.Search);

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
            catch (Exception)
            {
            }
        }
    }

    public class KHInfo
    {
        public string Search;
        public string? MaKH;
        public string? TenKH;
        public string? SDT;
        public string DiaChi;
        public int result;
    }
}