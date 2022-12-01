using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.HoaDon
{
    public class InfoModel : PageModel
    {
        public CTHDInfo cthdInfo = new CTHDInfo();
        public List<CthdInfo> listCThd = new List<CthdInfo>();
        public void OnGet()
        {
            string MaHD = Request.Query["MaHD"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select MaHD, NgayBan, HOADON.MaKH, TenKH, TongTien, ChietKhau, ThanhToan from KHACHHANG join HOADON on KHACHHANG.MaKH = HOADON.MaKH where MaHD=@MaHD";
                    String sql1 = "select MaHD, Hang.MaHang, TenHang, CT_HOADON.SoLuong, GiaBan, ThanhTien from HANG join CT_HOADON on HANG.MaHang = CT_HOADON.MaHang where MaHD=@MaHD";
                    String sql2 = "select count(MaHD), sum(SoLuong) from CT_HOADON where MaHD=@MaHD";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", MaHD);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cthdInfo.MaHD = reader.GetString(0);
                                cthdInfo.NgayBan = reader.GetDateTime(1);
                                cthdInfo.MaKH = reader.GetString(2);
                                cthdInfo.TenKH = reader.GetString(3);
                                cthdInfo.TongTien = reader.GetDecimal(4);
                                cthdInfo.ChietKhau = reader.GetDouble(5);
                                cthdInfo.ThanhToan = reader.GetDecimal(6);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", MaHD);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CthdInfo CThd = new CthdInfo();
                                CThd.MaHang = reader.GetString(1);
                                CThd.TenHang = reader.GetString(2);
                                CThd.SoLuong = reader.GetInt32(3);
                                CThd.GiaBan = reader.GetDecimal(4);
                                CThd.ThanhTien = reader.GetDecimal(5);

                                listCThd.Add(CThd);
                            }

                        }

                    }
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", MaHD);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cthdInfo.TongHang = reader.GetInt32(0);
                                cthdInfo.TongSL = reader.GetInt32(1);

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

    public class CTHDInfo
    {
        public string? MaHD;
        public DateTime NgayBan;
        public string? MaKH;
        public string? TenKH;
        public decimal TongTien;
        public double ChietKhau;
        public decimal ThanhToan;
        public int TongSL;
        public int TongHang;
    }

    public class CthdInfo
    {
        public string? MaHang;
        public string? TenHang;
        public int SoLuong;
        public decimal GiaBan;
        public decimal ThanhTien;
    }
}