using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.HoaDon
{
    public class HoaDonModel : PageModel
    {
        public List<HDInfo> listHD = new List<HDInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select MaHD, TenKH, NgayBan, TongTien, ChietKhau, ThanhToan from HOADON join KHACHHANG on HOADON.MaKH = KHACHHANG.MaKH";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HDInfo HD = new HDInfo();
                                HD.MaHD = reader.GetString(0);
                                HD.TenKH = reader.GetString(1);
                                HD.NgayBan = reader.GetDateTime(2);
                                HD.TongTien = reader.GetDecimal(3);
                                HD.ChietKhau = reader.GetDouble(4);
                                HD.ThanhToan = reader.GetDecimal(5);

                                listHD.Add(HD);
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
        public HDInfo searchInfo = new HDInfo();
        public void OnPost()
        {
            searchInfo.Search = Request.Form["Search"];
            if (searchInfo.Search.Length == 0)
            {
                Response.Redirect("/HoaDon/HoaDon");
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql1 = "select MaHD, TenKH, NgayBan, TongTien, ChietKhau, ThanhToan from HOADON join KHACHHANG on HOADON.MaKH = KHACHHANG.MaKH where MaHD like '%" + search[0] + "%'";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@Search", searchInfo.Search);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HDInfo HD = new HDInfo();
                                HD.MaHD = reader.GetString(0);
                                HD.TenKH = reader.GetString(1);
                                HD.NgayBan = reader.GetDateTime(2);
                                HD.TongTien = reader.GetDecimal(3);
                                HD.ChietKhau = reader.GetDouble(4);
                                HD.ThanhToan = reader.GetDecimal(5);

                                listHD.Add(HD);
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

    public class HDInfo
    {
        public string Search;
        public string? MaHD;
        public string? TenKH;
        public DateTime NgayBan;
        public decimal TongTien;
        public double ChietKhau;
        public decimal ThanhToan;
        public string MaKH;
    }

    
}
