using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLCH_Nhom2.Pages.Hang;
using QLCH_Nhom2.Pages.HoaDon;
using QLCH_Nhom2.Pages.NCC;
using System.Data.SqlClient;

namespace QLCH_Nhom2.Pages.NhapHang
{
    public class CreateNKModel : PageModel
    {
        public NKInfo nk = new NKInfo();
        public List<NCCInfo> listNCC = new List<NCCInfo>();
        public HangInfo searchInfo = new HangInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            searchInfo.Search = Request.Query["Search"];
            string MaNcc = Request.Query["MaNCC"];
            DateTime now = DateTime.Now;
            nk.MaNCC = MaNcc;
            nk.NgayNhap = now;
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var search = new List<string>() { searchInfo.Search };
                    String sql = "select dbo.fNewMaNK()";
                    String sql1 = "select * from NCC where (TenNCC like '%" + search[0] + "%' or MaNCC like '%" + search[0] + "%') and DiaChi <>'1'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nk.MaNK = reader.GetString(0);
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
                                NCCInfo ncc = new NCCInfo();
                                ncc.MaNCC = reader.GetString(0);
                                ncc.TenNCC = reader.GetString(1);
                                ncc.DiaChi = reader.GetString(2);
                                ncc.SDT = reader.GetString(3);

                                listNCC.Add(ncc);
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
            nk.MaNK = Request.Form["MaNK"];
            nk.MaNCC = Request.Form["MaNCC"];
            nk.NgayNhap = Convert.ToDateTime(Request.Form["NgayNhap"]);
            nk.ChietKhau = Convert.ToDouble(Request.Form["ChietKhau"]);
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Nhom2_QLBH;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql2 = "insert into PhieuNK values (@MaNK, @MaNCC, @NgayNhap, 0, @ChietKhau, 0)";

                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@MaNK", nk.MaNK);
                        command.Parameters.AddWithValue("@MaNCC", nk.MaNCC);
                        command.Parameters.AddWithValue("@NgayNhap", nk.NgayNhap);
                        command.Parameters.AddWithValue("@ChietKhau", nk.ChietKhau);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            Response.Redirect("/NhapHang/Create");
        }
        
    }
}
