using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Data.Sql;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Management;
using ClosedXML.Excel;

namespace spare_tool_system
{

    public partial class xspare_partList : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Sql"].ConnectionString);
        datacon dc = new datacon();
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int COL_REGISTER = 1;
        int COL_History = 2;

        int COL_TOOL_NO = 6;
        //int COL_PART_ID = 8;
        int COL_PART_NO = 9;
        int COL_IMG = 8;
        string IMAGE_PART_INPUT = @"\\cptfile02\Pro\PRJ2016001\05_spare_part\01_Image\";
        string IMAGE_PART = @"img_part/"; //@"\\cptfile02\Pro\PRJ2016001\05_spare_part\01_Image\";

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        private void RegisterPostBackControl()
        {
         //   ScriptManager.GetCurrent(this).RegisterPostBackControl(Btn_sunmit_toolPart);
            //foreach (GridViewRow row in gridview.Rows)
            //{
            //    DropDownList ddl_loca1 = row.FindControl("ddl_loca1") as DropDownList;
            //    ScriptManager.GetCurrent(this).RegisterPostBackControl(ddl_loca1);
            //}
        }
        public void Page_Init(object sender, EventArgs e)
        {
            if (Session["user_group"] == null)
            {
                Response.Redirect("Default.aspx");

            }
            Get_ddl_Group();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!(Page.IsPostBack))
            {
                //  bind_table();
                if (GridView1.Rows.Count == 0)
                {
                    lb_Text.Visible = true;
                }
            }
          //  RegisterPostBackControl();
           // test.Text = Session["user_dept"].ToString();
        }
        protected void Get_ddl_Group()
        {
            string dept_id = Session["user_dept"].ToString();
            string where = Get_where_Dept();

            string str = " select* from group_master where group_id is not null "+where;
            dc.QryDT(conn, str);

            ddl_group.DataSource = dc.dt;
            ddl_group.DataValueField = "group_id";
            ddl_group.DataTextField = "group_name";
            ddl_group.DataBind();

        }
        protected string Get_where_Dept()
        {
            string dept = Session["user_dept"].ToString();
            string where = "";
            string admin = Session["user_admin"].ToString();  // faii 
            if (admin != "99")  //Not Admin
            {
                if (dept != "")
                {
                    if (dept == "3" || dept == "5")
                    {
                        where += " and dept_id in (3,5) ";
                    }
                    else
                    {
                        where = " and dept_id = 6 "; //dept other 
                    }
                }
                else
                {
                    where = " and dept_id = 6 ";
                }
            }
            else
            {
                where = " and dept_id != 99 ";
            }

            return where;
        }
        protected void bind_table()
        {
            string where = "";
            if (ddl_group.SelectedValue != "")
            {
                where += " AND group_id = " + ddl_group.SelectedValue + " ";
            }
            if (txt_tool_no.Text != "")
            {
                where += " AND tool_no LIKE N'%" + txt_tool_no.Text.Trim() + "%' ";
            }
            if (txt_part_no.Text != "")
            {
                where += " AND part_no LIKE N'%" + txt_part_no.Text.Trim() + "%' ";
            }
            if (txt_part_name.Text != "")
            {
                where += " AND part_name LIKE N'%" + txt_part_name.Text.Trim() + "%' ";
            }
            string part_id = txt_partID_search.Text;
            if (part_id != "")
            {
                where += " AND part_id2 like '%" + part_id.Trim() + "%' ";
            }

            where += Get_where_Dept();
            string str = " select* from view_spare_part_list where part_id is not null  " + where ;

           // test.Text = str;
            //return;

            dc.QryDT(conn, str);
            GridView1.DataSource = dc.dt;
            GridView1.DataBind();

            lb_Text.Visible = false;
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            bind_table();
        }
        protected void ddl_group_SelectedIndexChanged(object sender, EventArgs e)
        {
            bind_table();

        }
        protected void btn_search_Click(object sender, ImageClickEventArgs e)
        {
            bind_table();
        }
        // Export: query only the 24 columns the Excel needs, with parameters and a longer timeout.
        // (Does NOT re-bind the GridView, and does NOT go through datacon's default 30s timeout.)
        private DataTable Get_export_data()
        {
            string sql = " SELECT tool_id2, group_name, model_name, tool_no, qty_toolModel, part_id2, part_no, part_name, "
                       + " usage, period_change, leadtime, moq, pcs_unitMoq, unit_name, order_unit, used, unused, loc_qty, "
                       + " brand_name, price, location, registerDate, regis_fname, remark "
                       + " FROM view_spare_part_list WHERE part_id IS NOT NULL ";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Sql"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandTimeout = 300; // 5 min (default is 30s -> Timeout)

                if (ddl_group.SelectedValue != "")
                {
                    sql += " AND group_id = @group_id ";
                    cmd.Parameters.Add("@group_id", SqlDbType.Int).Value = Convert.ToInt32(ddl_group.SelectedValue);
                }
                if (txt_tool_no.Text.Trim() != "")
                {
                    sql += " AND tool_no LIKE @tool_no ";
                    cmd.Parameters.Add("@tool_no", SqlDbType.NVarChar, 200).Value = "%" + txt_tool_no.Text.Trim() + "%";
                }
                if (txt_part_no.Text.Trim() != "")
                {
                    sql += " AND part_no LIKE @part_no ";
                    cmd.Parameters.Add("@part_no", SqlDbType.NVarChar, 200).Value = "%" + txt_part_no.Text.Trim() + "%";
                }
                if (txt_part_name.Text.Trim() != "")
                {
                    sql += " AND part_name LIKE @part_name ";
                    cmd.Parameters.Add("@part_name", SqlDbType.NVarChar, 400).Value = "%" + txt_part_name.Text.Trim() + "%";
                }
                if (txt_partID_search.Text.Trim() != "")
                {
                    sql += " AND part_id2 LIKE @part_id2 ";
                    cmd.Parameters.Add("@part_id2", SqlDbType.NVarChar, 50).Value = "%" + txt_partID_search.Text.Trim() + "%";
                }

                sql += Get_where_Dept(); // built from Session only, no user input
                cmd.CommandText = sql;

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        protected void btn_excel_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt_db = Get_export_data();

            // if (dt_db.Rows.Count < 4000)
            // {
            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("sheet1");
            ws.Cell(1, 1).Value = "No";
            ws.Cell(1, 2).Value = "Tool ID";
            ws.Cell(1, 3).Value = "Group";
            ws.Cell(1, 4).Value = "Model";
            ws.Cell(1, 5).Value = "Tool No";
            ws.Cell(1, 6).Value = "Tool Qty";
            ws.Cell(1, 7).Value = "Part ID";
            ws.Cell(1, 8).Value = "Part No";
            ws.Cell(1, 9).Value = "Part Name";
            ws.Cell(1, 10).Value = "Usage";
            ws.Cell(1, 11).Value = "Period";
            ws.Cell(1, 12).Value = "Leadtime";
            ws.Cell(1, 13).Value = "MOQ (Pcs.)";
            ws.Cell(1, 14).Value = "Pcs./Order_Unit";
            ws.Cell(1, 15).Value = "Unit";
            ws.Cell(1, 16).Value = "Order Unit";
            ws.Cell(1, 17).Value = "Used Stock";
            ws.Cell(1, 18).Value = "Unused Stock";
            ws.Cell(1, 19).Value = "Total Stock";
            ws.Cell(1, 20).Value = "Brand";
            ws.Cell(1, 21).Value = "Price";
            ws.Cell(1, 22).Value = "Location";
            ws.Cell(1, 23).Value = "Register Date";
            ws.Cell(1, 24).Value = "Register By";
            ws.Cell(1, 25).Value = "Remark";


            IXLRange range = ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, 25).Address);
            range.Style.Fill.SetBackgroundColor(XLColor.CanaryYellow);
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            // dt_db columns are already in Excel column order (see Get_export_data), so loop by index
            int colCount = dt_db.Columns.Count; // 24
            int r = 2;
            foreach (DataRow dr in dt_db.Rows)
            {
                ws.Cell(r, 1).Value = (r - 1).ToString();
                for (int c = 0; c < colCount; c++)
                {
                    ws.Cell(r, c + 2).Value = dr[c].ToString();
                }
                r++;
            }
            // AdjustToContents over every row is very slow on big sheets -> measure only the first 200 rows
            ws.Columns(1, 25).AdjustToContents(1, Math.Min(dt_db.Rows.Count + 1, 200));
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Spare Part List.xlsx");

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                workbook.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }
        
        protected void btn_register_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
            ImageButton btn_register = (ImageButton)row.FindControl("btn_register");
            LinkButton link_Edit_part = (LinkButton)row.FindControl("link_Edit_part");
            string part_no = Server.HtmlDecode(row.Cells[COL_PART_NO].Text.ToUpperInvariant());
            string part_id = link_Edit_part.Text;
            string str = " window.open('gen_new_regis_pdf.aspx?part_id=" + part_id + "');";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", str, true);
        }
        protected void btn_history_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
            ImageButton btn_register = (ImageButton)row.FindControl("btn_register");
            LinkButton link_Edit_part = (LinkButton)row.FindControl("link_Edit_part");
            string part_id = link_Edit_part.Text;
            string str = " window.open('his_of_parts_pdf.aspx?part_id=" + part_id + "');";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", str, true);
        }
        protected void btn_Picture_Click(object sender, EventArgs e)
        {

            GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
            //ImageButton partimage = (ImageButton)row.FindControl("partimage");
            //Button partimage = (Button)row.FindControl("partimage");
            HiddenField hd_img = (HiddenField)row.FindControl("hd_img");

            partimage1.ImageUrl = hd_img.Value.ToString();
            //lb_test.Text = hd_img.Value.ToString();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#modal_Picture').modal('show')");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false);
            return;
        }
        protected void link_Edit_part_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
                if (check_Not_group(row))
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : This part is not in your group. Can not edit.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }

                // PART ID
                LinkButton link_Edit_part = (LinkButton)row.FindControl("link_Edit_part");
                string part_id = link_Edit_part.Text;
                txt_partID.Text = part_id;

                // PART NO
                txt_partNo.Text = Return_SpeceilText(row.Cells[10].Text);
                // PART NAME
                txt_partName.Text = Return_SpeceilText(row.Cells[11].Text);
                // LEAD TIME ORDER
                txt_leadtime.Text = Return_SpeceilText(row.Cells[14].Text);
                // MOQ
                txt_MOQ.Text = Return_SpeceilText(row.Cells[15].Text);
                // Pcs. / ORDER UNIT
                txt_qty_orderUnit.Text = Return_SpeceilText(row.Cells[16].Text);
                // UNIT
                ddl_unit.SelectedValue = Get_unitID(row.Cells[17].Text);
                // ORDER UNIT
                ddl_orderUnit.SelectedValue = Get_unitID(row.Cells[18].Text);

                // BRAND
                ddl_brand.SelectedValue = Get_brandID(row.Cells[22].Text);

                // PRICE
                txt_price.Text = Return_SpeceilText(row.Cells[23].Text);

                // REMARK
                txt_remark.Text = Return_SpeceilText(row.Cells[27].Text);

                //img
                HiddenField hd_img = (HiddenField)row.FindControl("hd_img");
                ImageEdit.ImageUrl = hd_img.Value.ToString();

                //LOCATION
                string str = " SELECT part_id,room_id,shelve_id,box_id FROM xspare_loca WHERE part_id = " + Get_partID(part_id) + " ";
                dc.QryDT(conn, str);
                if (dc.dt.Rows.Count > 0)
                {
                    ddl_room.SelectedValue = dc.dt.Rows[0]["room_id"].ToString();
                    ddl_subroom.SelectedValue = dc.dt.Rows[0]["shelve_id"].ToString();
                    ddl_box.SelectedValue = dc.dt.Rows[0]["box_id"].ToString();
                    ddl_room.Enabled = true;
                    ddl_subroom.Enabled = true;
                    ddl_box.Enabled = true;
                }
                else
                {
                    ddl_room.SelectedValue = "";
                    ddl_subroom.SelectedValue = "";
                    ddl_box.SelectedValue = "";
                    //ddl_room.Enabled = false;
                    //ddl_subroom.Enabled = false;
                    //ddl_box.Enabled = false;
                }

                // BIND PIC
                str = " SELECT '' as emp_id,'--' as NAME  UNION";
                str += " SELECT '000369' as emp_id,'Suksan S.' as NAME  UNION ";
                str += " SELECT emp_id,fname + ' ' + SUBSTRING(lname,1,1)+'.' AS NAME FROM xspare_part A  ";
                str += "  LEFT JOIN cptsvs528.user_master.dbo.sys_user_master B ON A.group_id = B.eng_group_id  and B.user_dept in (3,4)  ";
                str += " WHERE DBO.get_part_id(part_id) = '" + part_id.Replace("P-", "") + "' order by NAME";

                dc.QryDT(conn, str);
                DataTable temp_dt = new DataTable();
                temp_dt = dc.dt;
                ddl_pic.DataSource = dc.dt;
                ddl_pic.DataValueField = "emp_id";
                ddl_pic.DataTextField = "NAME";
                ddl_pic.DataBind();

            
                    try
                    {
                        str = "select pic from xspare_part where dbo.get_part_id( part_id) = '" + part_id.Replace("P-", "") + "'";
                        dc.QryDT(conn, str);
                        ddl_pic.SelectedValue = dc.dt.Rows[0][0].ToString();
                    }
                    catch(Exception)
                    {

                    }

                    try
                    {
                        //str = "select * from xspare_min where dbo.get_part_id( part_id) = '" + part_id.Replace("P-", "") + "'";
                        str = " select * from GET_NEED_MIN where dbo.get_part_id( part_id) = '" + part_id.Replace("P-", "") + "' ";
                        dc.QryDT(conn, str);
                        if(dc.dt.Rows.Count > 0)
                        {
                            txt_manual_min.Text = dc.dt.Rows[0]["min_qty"].ToString();
                        }
                        else
                        {
                            txt_manual_min.Text = "";

                        }
                    }
                    catch (Exception)
                    {

                    }

                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#Modal_EditPath').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false);
                return;
            }
            catch (Exception ex)
            {
                sb.Append(@"<script type='text/javascript'>");
                sb.Append(@"alert('ERROR : " + ex.Message + "')");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false);
                return;
            }
        }
        protected void btn_EditPath_submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_partNo.Text == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please input new part no you want to change.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }
                if (txt_partName.Text == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please input new part name.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }
                if (txt_MOQ.Text == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please input new MOQ.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }
                if (txt_leadtime.Text == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please input new lead time order.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }
                if (ddl_unit.SelectedValue == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please input unit control.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }

                // UPDATE XSPARE_PART
                string partID_db = Get_partID(txt_partID.Text);
                string part_no = Return_SpeceilText(txt_partNo.Text);
                string part_name = Return_SpeceilText(txt_partName.Text);
                string brand_id = ddl_brand.SelectedValue;
                if (brand_id == "") { brand_id = "NULL"; }
                string lead_time = txt_leadtime.Text;
                string moq = txt_MOQ.Text;

                string unit_id = ddl_unit.SelectedValue;
                string remark = Return_SpeceilText(txt_remark.Text);
                string unit_orderID = ddl_orderUnit.SelectedValue;
                if (unit_orderID == "") { unit_orderID = "NULL"; }
                string qty_orderUnit = txt_qty_orderUnit.Text;
                if (qty_orderUnit == "") { qty_orderUnit = "NULL"; }
                string price = txt_price.Text;
                string pic = ddl_pic.SelectedValue;
              

                if (img_fileUpload.HasFile == true)
                {
                    DirectoryInfo DirInfo = new DirectoryInfo(IMAGE_PART_INPUT + img_fileUpload.FileName);
                    string file_type =  Path.GetExtension(img_fileUpload.FileName);
                    img_fileUpload.SaveAs(IMAGE_PART_INPUT + partID_db + ".jpg");
                    
                }
            

                if (price == "") { price = "NULL"; }
                else
                {
                    if (Convert.ToDouble(price) < 0)
                    {
                        sb.Append(@"<script type='text/javascript'>alert('ERROR : Price is invalid')</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                    }
                    price = "'" + price + "'";
                }

                if(txt_manual_min.Text != "" && txt_remark.Text == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please Input remark, that why have to set Manual-Minimum')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }

                string update = "";
                update += " UPDATE xspare_part SET part_no = N'" + part_no + "' ";
                update += " ,part_name = N'" + part_name + "' ";
                update += " ,brand = " + brand_id + " ";
                update += " ,leadtime = " + lead_time + " ";
                update += " ,moq = " + moq + " ";
                update += " ,unit_id = " + unit_id + " ";
                update += " ,remark = N'" + remark + "' ";
                update += " ,status = 1 ";
                update += " ,moq_unitID = " + unit_orderID + " ";
                update += " ,pcs_unitMoq = " + qty_orderUnit + " ";
                update += " ,price = " + price + "";
                update += " ,pic = '" + pic + "'";
                update += " WHERE part_id = " + partID_db + " ";

                update += " update spare_part_tana set part_no = N'" + part_no + "' where file_loc is null and part_id = " + partID_db + " ";

                //LOCATION
                string room_id = ddl_room.SelectedValue;
                if (room_id == "") { room_id = "NULL"; }
                string shelve_id = ddl_subroom.SelectedValue;
                if (shelve_id == "") { shelve_id = "NULL"; }
                string box_id = ddl_box.SelectedValue;
                if (box_id == "") { box_id = "NULL"; }

                string str = " SELECT part_id,room_id,shelve_id,box_id FROM xspare_loca WHERE part_id = " + partID_db + " ";
                dc.QryDT(conn, str);
                if (dc.dt.Rows.Count > 0)
                {
                    update += " UPDATE xspare_loca ";
                    update += " SET room_id = " + room_id + " ";
                    update += " ,shelve_id = " + shelve_id + " ";
                    update += " ,box_id = " + box_id + " ";
                    update += " WHERE part_id = " + partID_db + " ";
                }
                else
                {
                    update += " insert into  xspare_loca( part_id,room_id,shelve_id,box_id) values ("+ partID_db + ","+ room_id + ","+ shelve_id + ","+ box_id + ") ";
                }

                if (txt_manual_min.Text != "")
                {
                    try
                    {
                        Convert.ToInt32(txt_manual_min.Text);
                    }
                    catch(Exception)
                    {
                        sb.Append(@"<script type='text/javascript'>");
                         sb.Append(@"alert('Min(ByManual) is invalid Value');");
                        sb.Append(@"</script>");
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false);

                    }
                    str = "select * from xspare_min where dbo.get_part_id( part_id) = '" + txt_partID.Text.Replace("P-", "") + "'";
                    dc.QryDT(conn, str);
                    if (dc.dt.Rows.Count > 0)
                    {
                        // Update
                        update += " update xspare_min set min = '" + txt_manual_min.Text +"' where dbo.get_part_id( part_id) = '" + txt_partID.Text.Replace("P-", "") + "'";

                    }
                    else
                    {
                        // Insert
                        update += " INSERT INTO xspare_min (part_id,min) VALUES ('"+ partID_db + "','"+ txt_manual_min.Text + "') ";
                    }

                }
                else
                {
                    update += " update xspare_min set min =  null where dbo.get_part_id( part_id) = '" + txt_partID.Text.Replace("P-", "") + "'";
                }

            //    return;

                dc.RunCommand(conn, update);
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#Modal_EditPath').modal('show');");
                sb.Append(@"alert('Completed!');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false);
                bind_table();
                return;
            }
            catch (Exception ex)
            {
                sb.Append(@"<script type='text/javascript'>alert('ERROR : " + ex.Message + "')</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
            }
        }
        protected void link_Edit_partTool_Click(object sender, EventArgs e)
        {
            GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
            if (check_Not_group(row))
            {
                sb.Append(@"<script type='text/javascript'>alert('ERROR : This part is not in your group. Can not edit.')</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
            }
            LinkButton link_Edit_part = (LinkButton)row.FindControl("link_Edit_part");
            LinkButton link_Edit_partTool = (LinkButton)row.FindControl("link_Edit_partTool");
            // PART ID
            string part_id = link_Edit_part.Text;
            txt_EditToolPath_partID.Text = part_id;
            // PART NO
            txt_EditToolPath_partNo.Text = Return_SpeceilText(row.Cells[10].Text);
            // TOOL ID
            txt_EditToolPath_toolId.Text = link_Edit_partTool.Text;
            // TOOL NO
            txt_EditToolPath_toolNo.Text = Return_SpeceilText(row.Cells[6].Text);
            // USAGE
            txt_EditToolPath_Usage.Text = Return_SpeceilText(row.Cells[12].Text);
            // PERIOD CHANG
            txt_EditToolPath_Period.Text = Return_SpeceilText(row.Cells[13].Text);

            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#Modal_EditToolPath').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false);
            return;
        }
        protected void Btn_sunmit_toolPart_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_EditToolPath_Period.Text == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please input new usage you want to change.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }
                if (txt_EditToolPath_Usage.Text == "")
                {
                    sb.Append(@"<script type='text/javascript'>alert('ERROR : Please input new period change.')</script>");
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
                }
                string partID_db = Get_partID(txt_EditToolPath_partID.Text);
                string toolID_db = Get_partID(txt_EditToolPath_toolId.Text);
                string usage = txt_EditToolPath_Usage.Text;
                string period = txt_EditToolPath_Period.Text;
                string update = "";
                update += " UPDATE xspare_toolPart ";
                update += " SET period_change = " + period + " ";
                update += " ,usage = " + usage + " ";
                //update += " ,registerDate = GETDATE() ";
                //update += " ,registerby = '" + Session["emp_id"].ToString() + "' ";
                update += " WHERE toolGroup_id = " + toolID_db + " and part_id = " + partID_db + " ";

                dc.RunCommand(conn, update);
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#Modal_EditToolPath').modal('show');");
                sb.Append(@"alert('Completed!');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false);
                bind_table();
                return;
            }
            catch (Exception ex)
            {
                sb.Append(@"<script type='text/javascript'>alert('ERROR : " + ex.Message + "')</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closemodal", sb.ToString(), false); return;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string Return_SpeceilText(string str)
        {
            if (str == "" || str == "&nbsp;")
            {
                return "";
            }
            else
            {
                str = Server.HtmlDecode(str);
                return str.Trim().ToUpperInvariant();
            }
        }
        private string Get_brandID(string brand_name)
        {
            string str = " select brand_id from spare_brand_master where brand_name = N'" + brand_name + "' ";
            dc.QryDT(conn, str);
            if (dc.dt.Rows.Count > 0) { return dc.dt.Rows[0][0].ToString(); }
            else { return ""; }
        }
        private string Get_unitID(string unit)
        {
            if (unit == "" || unit == "&nbsp;")
            {
                return "";
            }
            else
            {
                string str = " select unit_id from spare_unit_master where unit_name = N'" + unit + "' ";
                dc.QryDT(conn, str);
                if (dc.dt.Rows.Count > 0) { return dc.dt.Rows[0][0].ToString(); }
                else { return ""; }
            }
        }
        private string Get_partID(string PartID)
        {
            string letter = PartID.Substring(2, 2);
            string number = PartID.Substring(4, 3);
            string str = " select (number * 1000) + " + number + " from xspare_converse_id where letter = '" + letter + "' ";
            dc.QryDT(conn, str);
            if (dc.dt.Rows.Count > 0) { return dc.dt.Rows[0][0].ToString(); }
            else { return ""; }
        }
        private Boolean check_Not_group(GridViewRow row)
        {
            string str2 = " select * from xspare_admin where emp_id = '" + Session["emp_id"].ToString() + "'";
            dc.QryDT(conn, str2);
            if (dc.dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                str2 = " select group_name from cptsvs528.user_master.dbo.sys_user_master a ";
                str2 += " left join group_master b on (case when  a.eng_group_id IS NULL then  a.oth_dept_id ELSE a.eng_group_id end)  = b.group_id    ";
                str2 += " where a.emp_id = '" + Session["emp_id"].ToString() + "' ";
                dc.QryDT(conn, str2);
                string group_name = dc.dt.Rows[0]["group_name"].ToString();
                if (group_name != row.Cells[4].Text)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            if (AsyncFileUpload1.HasFile)
            {
                String fn = System.IO.Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                string file_type = Path.GetExtension(fn);
                string filename = "test2";

                //string strPath = MapPath("~/Uploads/") + Path.GetFileName(e.filename);
                //AsyncFileUpload1.SaveAs(strPath);
                AsyncFileUpload1.SaveAs("\\\\cptfile01\\Div\\5300\\04_Fixed Assets Control (固定資産管理) [R2 CONFIDENTIAL]\\09_Tanaoroshi by month\\ENG3\\" + filename + file_type);
            }
        }
    }
}