<%@ page language="C#" autoeventwireup="true" codefile="xspare_partList.aspx.cs"
    inherits="spare_tool_system.xspare_partList" maintainscrollpositiononpostback="true"
    masterpagefile="~/MasterPagex.master" enableeventvalidation="false" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%-- <link rel="stylesheet" href="css/bootstrap.css" />
    <link rel="stylesheet" href="css/jquery-ui.css" />
    <link rel="stylesheet" href="css/GridviewScroll.css" />
    <link href="chosen/chosen.css" rel="stylesheet" />

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script src="js/jquery-1.10.2.js"></script>
    <script src="js/jquery-ui.js"></script>


    <script src="js/bootstrap.js"></script>
    <script src="js/gridviewScroll.min.js"></script>--%>
    <%--<script src="js/jquery.dm-uploader.js"></script>--%>
    <%--<script src="chosen/chosen.jquery.js"></script>--%>

    <link rel="stylesheet" href="css/bootstrap.css" />
    <%--<link rel="stylesheet" href="css/bootstrap.min.css" />--%>
    <link rel="stylesheet" href="css/jquery-ui.css" />
    <link rel="stylesheet" href="css/chosen.min.css" />
    <link rel="stylesheet" href="css/GridviewScroll.css" />

    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script src="js/jquery-1.10.2.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script src="js/bootstrap.js"></script>
    <script src="js/gridviewScroll.min.js"></script>
    <script src="js/chosen.jquery.min.js"></script>

    <script type="text/javascript"> 
        $(window).load(function () {
            gridviewScroll();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(gridviewScroll);

            $(window).resize(function () {
                gridviewScroll();
            });
        });

        function gridviewScroll() {

            var gridWidth = $(window).width();
            var gridHeight = $(window).height();

            gridView1 = $('#ContentPlaceHolder1_GridView1').gridviewScroll({
                width: gridWidth,
                height: 550,
                arrowsize: 30,
                freezesize: 3,
                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }
            });
        }
    </script>
    <script type="text/javascript">
        function uploadComplete(sender) {
            $get("<%=lblMesg.ClientID%>").innerHTML = "File Uploaded Successfully";
        }

        function uploadError(sender) {
            $get("<%=lblMesg.ClientID%>").innerHTML = "File upload failed.";
        }
    </script>

    <script> 
        $(window).load(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoaded)
            function PageLoaded(sender, args) {
                $(".chzn-select").chosen({ search_contains: true });
            }
        });
    </script>

    <style>
        .chzn-container-single .chzn-single {
            height: 40px !important;
            font-size: 16px;
            width: 180px !important;
        }

        .chosen-container-single .chosen-single {
            width: 180px;
        }

        .chosen-container-single .chosen-search input[type="text"] {
            width: 180px;
        }

        .chosen-container .chosen-results {
            width: 180px;
            background-color: #fff;
            margin: 0px;
        }

        .inner {
            width: 50%;
            margin: 0 auto;
        }

        .my-light-blue {
            background-color: #d4edf7;
        }

        div table label {
            font-family: "Times New Roman", Times, serif;
        }

        .modal-body {
            position: relative;
            max-height: 90%;
            padding: 15px;
            overflow-y: auto;
        }
    </style>


    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>
    <div>

        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UP1">
            <progresstemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                    <span style="border-width: 0px; position: fixed; padding: 50px; background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Loading ...</span>
                </div>
            </progresstemplate>
        </asp:UpdateProgress>
        <asp:HiddenField ID="hfGridView1SV" runat="server" />
        <asp:HiddenField ID="hfGridView1SH" runat="server" />
        <asp:UpdatePanel ID="UP1" runat="server">
            <contenttemplate>
                <asp:Panel ID="PN1" runat="server" DefaultButton="btn_search">
                    <div class="w3-xxlarge  w3-center">
                        <h2 style="margin-bottom: 0px;">Spare Part : Control List</h2>
                        <p style="font-size: 16px;">รายการ Spare Part (อ้างอิงจาก Tool) </p>
                        <%--<br />--%>
                    </div>
                    <br />
                    <div style="width: 100%; text-align: center!important;">
                        <div class="" style="width: 99%;">
                            <br />
                            <table>
                                <tr>
                                    <td>&nbsp;&nbsp;Group :</td>
                                    <td>
                                        <asp:DropDownList ID="ddl_group" runat="server" AppendDataBoundItems="true" CssClass="w3-input w3-medium"
                                            Height="35px" OnSelectedIndexChanged="ddl_group_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value=""></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                    <td>&nbsp;&nbsp;Tool No :</td>
                                    <td>
                                        <asp:TextBox ID="txt_tool_no" runat="server" CssClass="w3-medium w3-input" Height="35px" Width="200px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;&nbsp;Part ID :</td>
                                    <td>
                                        <asp:TextBox ID="txt_partID_search" runat="server" CssClass="w3-medium w3-input" Height="35px" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;&nbsp;Part No :</td>
                                    <td>
                                        <asp:TextBox ID="txt_part_no" runat="server" CssClass="w3-medium w3-input" Height="35px" Width="200px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;&nbsp;Part Name :</td>
                                    <td>
                                        <asp:TextBox ID="txt_part_name" runat="server" CssClass="w3-medium w3-input" Height="35px" Width="200px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;&nbsp;
                                        <asp:ImageButton ID="btn_search" ToolTip="Search" OnClick="btn_search_Click" runat="server" ImageUrl="~/images/icons8-view-100.png" Width="35px" Height="35px" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btn_excel" ToolTip="Export to excel" OnClick="btn_excel_Click" runat="server" ImageUrl="~/images/excel.png" Width="35px" Height="35px" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>

                        <div class="inner w3-round-medium" style="width: 99%;">
                            <asp:GridView ID="GridView1" runat="server" Width="100%" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                                AutoGenerateColumns="false" PageSize="100">
                                <pagerstyle cssclass="GridviewScrollC3Pager" />
                                <headerstyle horizontalalign="Center" />
                                <columns>
                                    <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign="Center">
                                        <itemtemplate><%# Container.DataItemIndex + 1 %></itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Register" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <itemtemplate>
                                            <asp:ImageButton ID="btn_register" OnClick="btn_register_Click" ToolTip="Register" runat="server" ImageUrl="~/images/icons8-report-card-40.png" Width="30px" Height="30px" />
                                        </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="History" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                        <itemtemplate>
                                            <asp:ImageButton ID="btn_history" OnClick="btn_history_Click" ToolTip="History" runat="server" ImageUrl="~/images/icons8-order-history-40.png" Width="30px" Height="30px" />
                                        </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tool_ID">
                                        <itemtemplate>
                                            <asp:LinkButton ID="link_Edit_partTool" runat="server" OnClick="link_Edit_partTool_Click" Text='<%#Eval("tool_id2") %>'></asp:LinkButton>
                                        </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Group" ItemStyle-HorizontalAlign="Center" DataField="group_name" />
                                    <asp:BoundField HeaderText="Model" ItemStyle-HorizontalAlign="Center" DataField="model_name" />
                                    <asp:BoundField HeaderText="Tool No." DataField="tool_no" />
                                    <asp:BoundField HeaderText="ToolQty" ItemStyle-HorizontalAlign="Center" DataField="qty_toolModel" />
                                    <asp:TemplateField HeaderText="Picture" ItemStyle-HorizontalAlign="Center">
                                        <itemtemplate>
                                            <asp:HiddenField ID="hd_img" runat="server" Value='<%#Bind("image")%>' />
                                            <asp:ImageButton ID="partimage" runat="server"
                                                Width="30px" Height="30px"
                                                ImageUrl='<%#Bind("image")%>' alt=""
                                                OnClick="btn_Picture_Click" />
                                            <%--<asp:ImageButton ID="ImageButton1" runat="server"
                                                Width="30px" Height="30px"
                                                ImageUrl='images/imageButton.png' alt=""
                                                OnClick="btn_Picture_Click" />  --%>
                                        </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Part_ID" HeaderStyle-CssClass="partId">
                                        <itemtemplate>
                                            <asp:HiddenField ID="hd_partID" runat="server" Value='<%#Bind("part_id")%>' />
                                            <asp:LinkButton ID="link_Edit_part" runat="server" OnClick="link_Edit_part_Click" Text='<%#Eval("part_id2") %>'></asp:LinkButton>
                                        </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Part No" DataField="part_no" />
                                    <asp:BoundField HeaderText="Part Name" DataField="part_name" />
                                    <asp:BoundField HeaderText="Usage" ItemStyle-HorizontalAlign="Center" DataField="usage" />
                                    <asp:BoundField HeaderText="Period" ItemStyle-HorizontalAlign="Center" DataField="period_change" />
                                    <asp:BoundField HeaderText="Leadtime" ItemStyle-HorizontalAlign="Center" DataField="leadtime" />
                                    <asp:BoundField HeaderText="MOQ (Pcs.)" ItemStyle-HorizontalAlign="Center" DataField="moq" />
                                    <asp:BoundField HeaderText="Pcs./ Order_Unit" ItemStyle-HorizontalAlign="Center" DataField="pcs_unitMoq" HeaderStyle-Wrap="true" />
                                    <asp:BoundField HeaderText="Unit" ItemStyle-HorizontalAlign="Center" DataField="unit_name" />
                                    <asp:BoundField HeaderText="Order Unit" ItemStyle-HorizontalAlign="Center" DataField="order_unit" />
                                    <asp:BoundField HeaderText="Used Stock" DataField="used" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="Unused Stock" DataField="unused" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="Total Stock" DataField="loc_qty" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="Brand" ItemStyle-HorizontalAlign="Center" DataField="brand_name" />
                                    <asp:BoundField HeaderText="Price" ItemStyle-HorizontalAlign="Center" DataField="price" />
                                    <asp:BoundField HeaderText="Location" DataField="location" />
                                    <asp:BoundField HeaderText="Register Date" DataField="registerDate" />
                                    <asp:BoundField HeaderText="Register By" DataField="regis_fname" />
                                    <%--<asp:BoundField HeaderText="Part PIC" DataField="part_pic" />--%>
                                    <asp:BoundField HeaderText="Remark" DataField="remark" />
                                </columns>
                                <headerstyle cssclass="GridviewScrollC3Header" />
                                <rowstyle cssclass="GridviewScrollC3Item" />
                                <pagerstyle cssclass="GridviewScrollC3Pager" />
                            </asp:GridView>
                        </div>
                        <asp:Label ID="test" runat="server" Text=""></asp:Label>
                        </br>
                         <asp:Label ID="lb_Text" runat="server" Font-Size="30px" Text="กรุณาเลือกข้อมูลที่ต้องการ" Visible="false"></asp:Label>
                    </div>

                    <script type="text/javascript">
                        $(document).ready(function () {

                            $(".partId").attr("title", "รหัสชิ้นส่วน");

                        });
                    </script>
                </asp:Panel>

                <cc1:asyncfileupload onclientuploaderror="uploadError" visible="false"
                    onclientuploadcomplete="uploadComplete" runat="server"
                    id="AsyncFileUpload1" width="400px" height="40px" uploaderstyle="Traditional"
                    completebackcolor="White" clientidmode="AutoID"
                    uploadingbackcolor="#CCFFFF" throbberid="imgLoader"
                    onuploadedcomplete="AsyncFileUpload1_UploadedComplete" />
                <asp:Image ID="imgLoader" runat="server" Visible="false"
                    ImageUrl="~/images/loader.gif" />
                <br />
                <asp:Label ID="lblMesg" runat="server" Text=""></asp:Label>

            </contenttemplate>
            <triggers>
                <asp:PostBackTrigger ControlID="btn_excel" />
            </triggers>
        </asp:UpdatePanel>
    </div>

    <div id="Modal_EditPath" class="modal  fade" style="width: 1000px; height:90%!important; margin-left: -500px" tabindex="-1" role="dialog" aria-labelledby="delModalLabel" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="true">
            <contenttemplate>
                <div class="modal-header w3-light-blue w3-xlarge">Edit path</div>
                <div class="modal-body w3-center" style="width: 100%;">
                    <div>
                        <asp:Image ID="ImageEdit" runat="server" style="max-height: 400px; max-width: 400px;" />
                        <asp:FileUpload ID="img_fileUpload" CssClass="w3-small" runat="server"  />
                        <asp:Label runat="server" ID="test1"></asp:Label>
                    </div>

                    <table class="w3-table w3-border-0" style="width: 80%; margin: auto">
                        <tr>
                            <td style="height: 35px; vertical-align: middle; text-align: right">Part ID : </td>
                            <td colspan="5">
                                <asp:TextBox runat="server" Height="35px" Width="100px" Enabled="false" ID="txt_partID"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px; vertical-align: middle; text-align: right">Part No : </td>
                            <td colspan="5">
                                <asp:TextBox runat="server" Height="35px" Width="100%" ID="txt_partNo" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px; vertical-align: middle; text-align: right">Part Name : </td>
                            <td colspan="5">
                                <asp:TextBox runat="server" Height="35px" Width="100%" ID="txt_partName"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Brand : </td>
                            <td style="width: 35%;">
                                <asp:DropDownList ID="ddl_brand" runat="server" AppendDataBoundItems="true" CssClass="w3-input w3-medium"
                                    DataSourceID="SqlDataSource1" Height="35px"
                                    DataValueField="brand_id" DataTextField="brand_name">
                                    <asp:ListItem Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:spare_toolConnectionString %>"
                                    SelectCommand=" select brand_id,brand_name from spare_brand_master order by brand_name "></asp:SqlDataSource>
                            </td>
                            <td style="height: 35px; width: 10%; vertical-align: middle; text-align: right">MOQ : </td>
                            <td style="width: 8%;">
                                <asp:TextBox runat="server" Height="35px" Width="100%" ID="txt_MOQ" CssClass="w3-center"></asp:TextBox>
                            </td>
                            <td style="height: 35px; width: 10%; vertical-align: middle; text-align: right">Unit : </td>
                            <td style="width: 25%;">
                                <asp:DropDownList ID="ddl_unit" runat="server" AppendDataBoundItems="true" CssClass="w3-input w3-medium"
                                    DataSourceID="SqlDataSource2" Height="35px"
                                    DataValueField="unit_id" DataTextField="unit_name">
                                    <asp:ListItem Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:spare_toolConnectionString %>"
                                    SelectCommand=" select unit_id,unit_name from spare_unit_master order by unit_name "></asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Price : </td>
                            <td>
                                <asp:TextBox runat="server" Height="35px" Width="50%" ID="txt_price" CssClass="w3-center"></asp:TextBox>
                                THB 
                            </td>

                            <td colspan="2" style="height: 35px; vertical-align: middle; text-align: right">Min(ByManual) : </td>
                            <td colspan="2">
                                <asp:TextBox runat="server" Height="35px" Width="50%" ID="txt_manual_min" CssClass="w3-center"></asp:TextBox>
                            </td>

                        </tr>
                    </table>
                    <table class="w3-table w3-border-0" style="width: 80%; margin: auto">
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Lead Time :  </td>
                            <td style="width: 10%;">
                                <asp:TextBox runat="server" Height="35px" Width="100%" ID="txt_leadtime" CssClass="w3-center"></asp:TextBox>
                            </td>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Order Unit : </td>
                            <td style="width: 25%;">
                                <asp:DropDownList ID="ddl_orderUnit" runat="server" AppendDataBoundItems="true" CssClass="w3-input w3-medium"
                                    DataSourceID="SqlDataSource3" Height="35px"
                                    DataValueField="unit_id" DataTextField="unit_name">
                                    <asp:ListItem Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:spare_toolConnectionString %>"
                                    SelectCommand=" select unit_id,unit_name from spare_unit_master order by unit_name "></asp:SqlDataSource>
                            </td>
                            <td style="height: 35px; width: 20%; vertical-align: middle; text-align: right">Qty / Order Unit : </td>
                            <td style="width: 10%;">
                                <asp:TextBox runat="server" Height="35px" Width="100%" ID="txt_qty_orderUnit" class="w3-center"></asp:TextBox>
                            </td>
                            <td style="height: 35px; width: 10%; vertical-align: middle; text-align: right">Pcs.</td>
                        </tr>
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Remark :  </td>
                            <td colspan="5">
                                <asp:TextBox runat="server" Height="60px" Width="100%" ID="txt_remark" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <hr>
                    <table class="w3-table w3-border-0" style="width: 80%; margin: auto">
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Room : </td>
                            <td style="width: 20%;">
                                <asp:DropDownList ID="ddl_room" runat="server" AppendDataBoundItems="true"
                                    DataSourceID="SqlDataSource_ddl_room" DataTextField="room_name" CssClass="chzn-select"
                                    DataValueField="room_id" Width="100%" Height="35px">
                                    <asp:ListItem Value="">Select ...</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_ddl_room" runat="server" ConnectionString="<%$ ConnectionStrings:spare_toolConnectionString %>"
                                    SelectCommand=" select * from loc_master "></asp:SqlDataSource>
                            </td>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Shelve : </td>
                            <td style="width: 20%;">
                                <asp:DropDownList ID="ddl_subroom" runat="server" AppendDataBoundItems="true"
                                    DataSourceID="SqlDataSource_ddl_subroom" DataTextField="shelve_name" CssClass="chzn-select"
                                    DataValueField="sub_id" Height="35px" Width="100%">
                                    <asp:ListItem Value="">Select ...</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_ddl_subroom" runat="server" ConnectionString="<%$ ConnectionStrings:spare_toolConnectionString %>"
                                    SelectCommand=" select * from room_sub_master "></asp:SqlDataSource>
                            </td>
                            <td style="height: 35px; width: 10%; vertical-align: middle; text-align: right">Box : </td>
                            <td style="width: 15%;">
                                <asp:DropDownList ID="ddl_box" runat="server" AppendDataBoundItems="true"
                                    DataSourceID="SqlDataSource_ddl_box" DataTextField="list_box_name" CssClass="chzn-select"
                                    DataValueField="list_id" Height="35px">
                                    <asp:ListItem Value="">Select ...</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource_ddl_box" runat="server" ConnectionString="<%$ ConnectionStrings:spare_toolConnectionString %>"
                                    SelectCommand=" select * from room_sub_list_master "></asp:SqlDataSource>
                            </td>
                        </tr>
                    </table>
                    <hr>
                    <table class="w3-table w3-border-0" style="width: 80%; margin: auto">
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">PIC : </td>
                            <td style="width: 85%;">
                                <asp:DropDownList ID="ddl_pic" runat="server" AppendDataBoundItems="False"
                                    CssClass="w3-input" Height="35px">
                                    <asp:ListItem Value="">Select ...</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <br />
                    <asp:Button ID="btn_EditPath_submit" runat="server" Text="Submit" CssClass="-large w3-button w3-xlarge w3-green w3-border w3-round" OnClick="btn_EditPath_submit_Click" OnClientClick="return confirm('Are you sure?')" />
                    <asp:Button ID="Button3" runat="server" Text="Cancel" data-dismiss="modal" CssClass="-large w3-button w3-xlarge w3-red w3-border w3-round" />
                </div>

            </contenttemplate>
            <triggers>
                <asp:PostBackTrigger ControlID="btn_EditPath_submit" />
            </triggers>
        </asp:UpdatePanel>
    </div>

    <div id="Modal_EditToolPath" class="modal fade" style="width: 1000px; height: 480px; margin-left: -500px" tabindex="-1" role="dialog" aria-labelledby="delModalLabel" aria-hidden="true">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" EnableViewState="true">
            <contenttemplate>
                <div class="modal-header w3-light-blue w3-xlarge">Edit Part Information in Tool </div>
                <div class="modal-body w3-center" style="width: 100%">
                    <table class="w3-table w3-border-0" style="width: 80%; margin: auto">
                        <tr>
                            <td style="height: 35px; width: 20%; vertical-align: middle; text-align: right">Part ID :  </td>
                            <td style="width: 10%;">
                                <asp:TextBox runat="server" Height="35px" Width="100px" Enabled="false" ID="txt_EditToolPath_partID"></asp:TextBox>
                            </td>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Tool ID :  </td>
                            <td style="width: 10%;">
                                <asp:TextBox runat="server" Height="35px" Width="100px" Enabled="false" ID="txt_EditToolPath_toolId"></asp:TextBox>
                            </td>
                            <td style="width: 50%;"></td>
                        </tr>
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Part No :  </td>
                            <td style="width: 85%;" colspan="4">
                                <asp:TextBox runat="server" Height="35px" Width="100%" Enabled="false" ID="txt_EditToolPath_partNo"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 35px; width: 15%; vertical-align: middle; text-align: right">Tool No :  </td>
                            <td style="width: 80%;" colspan="4">
                                <asp:TextBox runat="server" Height="35px" Width="100%" Enabled="false" ID="txt_EditToolPath_toolNo"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="w3-table w3-border-0" style="width: 80%; margin: auto">
                        <tr>
                            <td style="height: 35px; width: 20%; vertical-align: middle; text-align: right">Qty Usage :  </td>
                            <td style="width: 80%;">
                                <asp:TextBox runat="server" Height="35px" Width="100px" ID="txt_EditToolPath_Usage" CssClass="w3-center"></asp:TextBox>
                                Pcs.
                        </tr>
                        <tr>
                            <td style="height: 35px; width: 20%; vertical-align: middle; text-align: right">Period Change :  </td>
                            <td style="width: 80%;">
                                <asp:TextBox runat="server" Height="35px" Width="100px" ID="txt_EditToolPath_Period" CssClass="w3-center"></asp:TextBox>
                                / Year
                            </td>

                        </tr>
                    </table>
                    <hr>
                    <br />
                    <asp:Button ID="Btn_sunmit_toolPart" runat="server" Text="Submit" CssClass="-large w3-button w3-xlarge w3-green w3-border w3-round" OnClick="Btn_sunmit_toolPart_Click" OnClientClick="return confirm('Are you sure?')" />
                    <asp:Button ID="Button2" runat="server" Text="Cancel" data-dismiss="modal" CssClass="-large w3-button w3-xlarge w3-red w3-border w3-round" />
                    <%--  ------%>
                </div>
            </contenttemplate>
            <triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="btn_EditPath_submit" EventName="Click" />--%>
            </triggers>
        </asp:UpdatePanel>
    </div>
    <%-- -----%>
    <%--Modal Picture--%>
    <div class="modal fade" id="modal_Picture" role="dialog" style="height: 500px; align-content: center; text-align: center;">
        <asp:UpdatePanel ID="UpdatePanel20" runat="server">
            <contenttemplate>
                <div>
                    <asp:Image ID="partimage1" runat="server" style="max-height: 400px; max-width: 400px;" />
                    <%--<asp:Label ID="lb_test" runat="server"></asp:Label>--%>
                </div>
            </contenttemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
