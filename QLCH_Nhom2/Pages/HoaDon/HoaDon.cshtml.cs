using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    }

    public class HDInfo
    {
        public string? MaHD;
        public string? TenKH;
        public DateTime NgayBan;
        public decimal TongTien;
        public double ChietKhau;
        public decimal ThanhToan;
    }

    
}
