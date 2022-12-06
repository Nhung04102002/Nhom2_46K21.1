using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using QLCH_Nhom2.Pages.NhapHang;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.HoaDon
{
    public class CTHDModel : PageModel
    {
        public HDInfo HD = new HDInfo();
        public List<HangInfo> listHang = new List<HangInfo>();
        public HangInfo searchInfo = new HangInfo();
        public cthdInfo CTHD = new cthdInfo();
        public List<cthdInfo> listCTHD = new List<cthdInfo>();
        public List<cthdInfo> list = new List<cthdInfo>();

        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            string MaH = Request.Query["MaHang"];
            searchInfo.Search = Request.Query["Search"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql1 = "select MaHang, TenHang, SoLuong, GiaBan from HANG where (MaHang like '%" + search[0] + "%' or TenHang like '%" + search[0] + "%') and SoLuong>0";
                    String sql2 = "select * from HOADON where MaHD=@MaHD";
                    String sql3 = "select max(MaHD) from HOADON";
                    String sql4 = "select MaHD, HANG.MaHang, TenHang, CT_HOADON.SoLuong, GiaBan, ThanhTien from CT_HOADON join HANG on CT_HOADON.MaHang = HANG.MaHang where MaHD=@MaHD";
                    using (SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CTHD.MaHD = reader.GetString(0);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand(sql4, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", CTHD.MaHD);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cthdInfo cthd = new cthdInfo();
                                cthd.MaHD = reader.GetString(0);
                                cthd.MaHang = reader.GetString(1);
                                cthd.TenHang = reader.GetString(2);
                                cthd.SoLuong = reader.GetInt32(3);
                                cthd.GiaBan = reader.GetDecimal(4);
                                cthd.ThanhTien = reader.GetDecimal(5);

                                listCTHD.Add(cthd);
                            }
                        }
                    }
                   
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", CTHD.MaHD);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                HD.MaHD = reader.GetString(0);
                                HD.MaKH = reader.GetString(1);
                                HD.NgayBan = reader.GetDateTime(2);
                                HD.TongTien = reader.GetDecimal(3);
                                HD.ChietKhau = reader.GetDouble(4);
                                HD.ThanhToan = reader.GetDecimal(5);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@Search", searchInfo.Search);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                HangInfo Hang = new HangInfo();
                                Hang.MaHang = reader.GetString(0);
                                Hang.TenHang = reader.GetString(1);
                                Hang.SoLuong = reader.GetInt32(2);
                                Hang.GiaBan = reader.GetDecimal(3);

                                listHang.Add(Hang);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select MaHang, TenHang, GiaBan from HANG where MaHang=@MaHang";
                   
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MaHang", MaH);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CTHD.MaHang = reader.GetString(0);
                                CTHD.TenHang = reader.GetString(1);
                                CTHD.SoLuong = 1;
                                CTHD.GiaBan = reader.GetDecimal(2);
                                CTHD.ThanhTien = CTHD.GiaBan * CTHD.SoLuong;
                                list.Add(CTHD);
                            }
                        }

                    }
                    
                }
            }
            catch (Exception)
            {
            }
        }
        


        public void OnPost()
        {
            CTHD.MaHD = Request.Form["MaHD"];
            CTHD.MaHang = Request.Form["MaHang"];
            CTHD.TenHang = Request.Form["TenHang"];
            CTHD.GiaBan = Convert.ToDecimal(Request.Form["GiaBan"]);
            CTHD.SoLuong = Convert.ToInt32(Request.Form["SoLuong"]);
            CTHD.ThanhTien = Convert.ToInt32(Request.Form["SoLuong"]) * Convert.ToDecimal(Request.Form["GiaBan"]);

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql1 = "insert into CT_HOADON values (@MaHD, @MaHang, @SoLuong, @ThanhTien)";

                    using (SqlCommand command = new SqlCommand(sql1, connection))
                    {
                        command.Parameters.AddWithValue("@MaHD", CTHD.MaHD);
                        command.Parameters.AddWithValue("@MaHang", CTHD.MaHang);
                        command.Parameters.AddWithValue("@SoLuong", CTHD.SoLuong);
                        command.Parameters.AddWithValue("@ThanhTien", CTHD.ThanhTien);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception)
            {
            }
            Response.Redirect("/HoaDon/CTHD");
        }
    }
    public class cthdInfo
    {
        public string MaHD;
        public string MaHang;
        public string TenHang;
        public int SoLuong;
        public decimal GiaBan;
        public decimal ThanhTien;

    }
}
