using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.HoaDon
{
    public class CTHDModel : PageModel
    {
        public cthdInfo CTHD = new cthdInfo();
        public List<cthdInfo> listCTHD = new List<cthdInfo>();
        public List<cthdInfo> list = new List<cthdInfo>();
        public void OnGet()
        {
            string MaH = Request.Query["MaHang"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select MaHang, TenHang, GiaBan from HANG where MaHang=@MaHang";
                    String sql2 = "select MaHD, HANG.MaHang, TenHang, CT_HOADON.SoLuong, GiaBan, ThanhTien from CT_HOADON join HANG on CT_HOADON.MaHang = HANG.MaHang where MaHD=@MaHD";
                    String sql3 = "select max(MaHD) from HOADON";
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
                    using (SqlCommand command = new SqlCommand(sql2, connection))
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
                }
            }
            catch (Exception)
            {
            }
        }
        public List<HangInfo> listHang = new List<HangInfo>();
        public HangInfo searchInfo = new HangInfo();


        public void OnPost()
        {
            CTHD.MaHD = Request.Form["MaHD"];
            CTHD.MaHang = Request.Form["MaHang"];
            CTHD.TenHang = Request.Form["TenHang"];
            CTHD.GiaBan = Convert.ToDecimal(Request.Form["GiaBan"]);
            CTHD.SoLuong = Convert.ToInt32(Request.Form["SoLuong"]);

            CTHD.ThanhTien = Convert.ToInt32(Request.Form["SoLuong"]) * Convert.ToDecimal(Request.Form["GiaBan"]);

            searchInfo.Search = Request.Form["Search"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql1 = "select MaHang, TenHang, SoLuong, GiaBan from HANG where MaHang like '%" + search[0] + "%' or TenHang like '%" + search[0] + "%'";

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
