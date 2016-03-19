<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="ProjectMaster.aspx.cs" Inherits="TrackIT.WebApp.project.ProjectMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>


<asp:Content ID="project" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <%--   <asp:scriptmanager id="ScriptManager1" runat="server">
</asp:scriptmanager>--%>
    <script type="text/javascript">
        function ShowModalPopup() {
            $find("mpe").show();
            $find("ctl00_ContentPlaceHolder1_pnlPopup").show();
            return false;
        }
        function HideModalPopup() {
            $find("mpe").hide();
            document.getElementById("createnew").style.display = "block";
            return false;
        }
        function editRow(obj) {
            //var grid = $find("lwdg_FundMasterGrid");
            var row = $(obj).parents("tr[type='row']").get(0);
            var rowid = row.cells[1].innerHTML;
            var pop_open = '1';
            document.getElementById("hdnprjID").value = rowid;
            document.getElementById("hdnpop").value = pop_open;
            return true;
        }
        function removequery() {
            var pop_open = '0';
            document.getElementById("hdnpop").value = pop_open;
        }


        function fnGetSelectID() {
            var grid = $find("ctl00_ContentPlaceHolder1_iwdg_projectphases");
            var rows = grid.get_rows();
            var ids = "";
            var drp_owner = "";
            var drp_resource = "";
            for (var i = 0; i < rows.get_length() ; i++)
                if (rows.get_row(i).get_cell(1).get_value()) {
                    ids += rows.get_row(i).get_cell(1).get_value();
                    if (rows.get_row(i).get_cell(1).get_value()) {
                        ids += ',';
                    }
                    drp_owner += rows.get_row(i).get_cell(3).get_value() + ',';
                    drp_resource += rows.get_row(i).get_cell(4).get_value() + ',';
                }
            document.getElementById("hdnphasesID").value = ids;
            document.getElementById("hdnphaseowner").value = drp_owner;
            document.getElementById("hdnphaseresource").value = drp_resource;

            return true;
        }

        function headerCheckedChangedHandler(checkbox) {
            var grid = $find("ctl00_ContentPlaceHolder1_iwdg_project_phases");
            var checkBoxState = checkbox.checked;
            var columnKey = checkbox.parentElement.getAttribute("key");
            var rows = grid.get_rows();
            // iterate through the rows and set the checkBox states and cell values
            for (i = 0; i < rows.get_length() ; i++) {
                var cell = rows.get_row(i).get_cellByColumnKey(columnKey);
                cell._setCheckState(checkBoxState);
                cell.set_value(checkBoxState);
            }
        }
        function OnExitedEditMode(sender, e) {
            var row = e.getCell(1).get_row();
            var owner = row.get_cellByColumnKey("phaseowner").get_value();
            var resource = row.get_cellByColumnKey("phaseresource").get_value();
            if (owner == "" && resource == "") {
                row.get_cellByColumnKey("check").set_value(false);
            }

        }
        function OnEnteredEditMode(sender, e) {
            var row = e.getCell(1).get_row();
            var text = row.get_cellByColumnKey("check").get_value();
            var grid = $find("ctl00_ContentPlaceHolder1_iwdg_project_phases");
            var rowsCount = grid.get_rows().get_length();
            if (text == false) {
                row.get_cellByColumnKey("check").set_value(true);
            }

        }

    </script>
    <style type="text/css">
        .igdd_DropDownListContainer {
            width: 420px !important;
        }

        .igdd_ValueDisplay {
            width: 100% !important;
        }
    </style>
    <asp:UpdatePanel ID="uplState" runat="server">
        <ContentTemplate>
            <div class="main-container" id="main-container">
                <div class="page-header">

                    <div class="floatright pull_right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/excel_icon.png"></asp:ImageButton>

                        <asp:ImageButton ID="btnExportPDF" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/pdf_icon.png"></asp:ImageButton>
                        &nbsp;
                             <span class="custom-createnew" style="float: right;" id="createnew" clientidmode="Static" runat="server">
                                 <a href="#" onclick="show(); ShowModalPopup();"></a></span>

                    </div>

                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterPDF" DataExportMode="AllDataInDataSource" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterExcel" DataExportMode="AllDataInDataSource" />

                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-file"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreateProjects" runat="server"></asp:Label>
                    </h1>
                </div>

                <div runat="server" id="pnl_projectGrid">
                </div>
            </div>
            <div class="main-content">

                <div class="page-content">


                    <div class="page-content-area">

                        <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                        <cc1:ModalPopupExtender ID="mpe_projectPopup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                            PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">
                            <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                                <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                    <h4 class="modal-title">Projects</h4>
                                </asp:Panel>

                                <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">

                                    <asp:ValidationSummary ID="ValProjects" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgrpSave" />
                                    <div class="form-horizontal">
                                     <%--   <div class="col-md-4" style="padding-right: 0px;">--%>

                                            <!--begin div-->
                                            <div class="col-sm-12" style="padding-left: 0px; padding-right: 0px; margin-top: 10px; padding-top:15px; border: 1px solid #e3e3e3;">
                                                <div class="widget-box transparent">
                                                  
                                                    <div class="widget-body" style="display: block;">
                                                        <div class="widget-main no-padding">
                                                            <!--begin control-->
                                                            <div class="col-md-6">
                                                                <div class="form-group align-popcontent">
                                                                    <asp:Label ID="lblclientname" class="control-label col-md-2" runat="server"></asp:Label>
                                                                    <div class="col-md-6">
                                                                        <asp:DropDownList ID="ddlClients" runat="server" TabIndex="1" CssClass="form-control chzn-select" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-1" style="display: none;">
                                                                        <asp:RequiredFieldValidator ID="reqvClient" runat="server"
                                                                            ControlToValidate="ddlClients" Display="Static" SetFocusOnError="True"
                                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group align-popcontent">
                                                                    <asp:Label ID="lblprojectcode" class="control-label col-md-2" runat="server"></asp:Label>
                                                                    <div class="col-md-3">
                                                                        <asp:TextBox ID="txtprojectcode" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100" ToolTip="Maximum Character 10"></asp:TextBox>
                                                                    </div>

                                                                    <div class="col-md-1" style="display: none;">
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                                            runat="server" Enabled="True" TargetControlID="txtprojectcode"
                                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="reqvcode" runat="server"
                                                                            ControlToValidate="txtprojectcode" Display="Static" SetFocusOnError="True"
                                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="reqvprojectIdUNQ" runat="server"
                                                                            ControlToValidate="txtprojectcode" Display="Static" SetFocusOnError="True"
                                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group align-popcontent">
                                                                    <asp:Label ID="lblprojectname" class="control-label col-md-2" runat="server"></asp:Label>
                                                                    <div class="col-md-3">
                                                                        <asp:TextBox ID="txtprojectname" CssClass="form-control" runat="server" TabIndex="3" MaxLength="100" ToolTip="Maximum Character 10"></asp:TextBox>
                                                                    </div>

                                                                    <div class="col-md-1" style="display: none;">
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                            runat="server" Enabled="True" TargetControlID="txtprojectname"
                                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="reqvpname" runat="server"
                                                                            ControlToValidate="txtprojectname" Display="Static" SetFocusOnError="True"
                                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>



                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group align-popcontent">
                                                                    <asp:Label ID="lblkickdate" class="control-label col-md-2" runat="server"></asp:Label>
                                                                    <div class="col-md-3">
                                                                        <input type="date" id="igwdp_kickoffdate" runat="server"></input>
                                                                    </div>
                                                                    <div class="col-md-1" style="display: none;">
                                                                        <asp:RequiredFieldValidator ID="reqvkickoffdate" runat="server"
                                                                            ControlToValidate="igwdp_kickoffdate" Display="Static" SetFocusOnError="True"
                                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group align-popcontent">
                                                                    <asp:Label ID="lblprojectowner" class="control-label col-md-2" runat="server"></asp:Label>
                                                                    <div class="col-md-6">
                                                                        <asp:DropDownList ID="ddlowner" runat="server" TabIndex="4" CssClass="form-control chzn-select" AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-1" style="display: none;">
                                                                        <asp:RequiredFieldValidator ID="reqvowner" runat="server"
                                                                            ControlToValidate="ddlowner" Display="Static" SetFocusOnError="True"
                                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group align-popcontent">
                                                                    <asp:Label ID="lblactive" class="control-label col-md-2" runat="server"></asp:Label>
                                                                    <div class="col-md-1">
                                                                        <span class="input-icon">
                                                                            <asp:CheckBox ID="chkinactive" class="checkbox" runat="server" TabIndex="5" />
                                                                        </span>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!--end control-->
                                                    </div>
                                                    <!-- /.widget-main -->
                                                </div>
                                                <!-- /.widget-body -->
                                            </div>
                                            <!-- /.widget-box -->
                                      <%--  </div>--%>
                                        <!--end div-->
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <div class="col=sm-10">


                                            <ig:WebDataGrid ID="iwdg_project_phases" runat="server" AutoGenerateColumns="False" DataKeyFields="prj_prjKEY" Width="100%">
                                                <Columns>

                                                    <%-- <ig:TemplateDataField Key="DeleteItem" Width="20px" Header-Text="<input type='checkbox' onchange='headerCheckedChangedHandler(this);'">
                                                        <ItemTemplate>
                                                     <asp:CheckBox ID="lcbrowselect" runat="server" onchange="checkedChangedHandler();" />
                                                        </ItemTemplate>
                                                    </ig:TemplateDataField>--%>
                                                    <ig:BoundCheckBoxField DataFieldName="CheckStatus" Key="check" DataType="System.Boolean" Width="22px">
                                                        <Header Text="<input type='checkbox' onchange='headerCheckedChangedHandler(this);'"></Header>
                                                    </ig:BoundCheckBoxField>
                                                    <ig:BoundDataField DataFieldName="Phase" DataType="System.string" Key="prjphases">
                                                        <Header Text="Phases">
                                                        </Header>
                                                    </ig:BoundDataField>
                                                    <%--      --%>
                                                    <ig:BoundDataField DataFieldName="Owner_key" Key="phaseowner">
                                                        <Header Text="Phase Owner">
                                                        </Header>
                                                    </ig:BoundDataField>

                                                    <ig:BoundDataField DataFieldName="Resource_key" Key="phaseresource">
                                                        <Header Text="Phase Resource">
                                                        </Header>
                                                    </ig:BoundDataField>
                                                </Columns>
                                                <EditorProviders>
                                                    <ig:TextBoxProvider ID="tbpphases"></ig:TextBoxProvider>
                                                    <ig:DropDownProvider ID="ddpphaseowners">
                                                        <EditorControl runat="server" ClientIDMode="Predictable" DropDownContainerMaxHeight="200px" DropDownContainerWidth="100%" EnableAnimations="False" EnableDropDownAsChild="False">
                                                        </EditorControl>
                                                    </ig:DropDownProvider>
                                                    <ig:DropDownProvider ID="ddpphaseresource">
                                                        <EditorControl runat="server" ClientIDMode="Predictable" DropDownContainerMaxHeight="200px" EnableAnimations="False" EnableDropDownAsChild="False">
                                                        </EditorControl>
                                                    </ig:DropDownProvider>
                                                </EditorProviders>
                                                <Behaviors>
                                                    <ig:Activation />
                                                    <ig:Selection RowSelectType="Multiple" CellClickAction="Row" />
                                                    <ig:EditingCore>
                                                        <Behaviors>
                                                            <ig:CellEditing EditModeActions-MouseClick="Single">
                                                                <ColumnSettings>
                                                                    <ig:EditingColumnSetting ColumnKey="prjphases" EditorID="tbpphases" ReadOnly="true" />
                                                                    <ig:EditingColumnSetting ColumnKey="Check" ReadOnly="true" />
                                                                    <ig:EditingColumnSetting ColumnKey="phaseowner" EditorID="ddpphaseowners" />
                                                                    <ig:EditingColumnSetting ColumnKey="phaseresource" EditorID="ddpphaseresource" />
                                                                </ColumnSettings>
                                                                <CellEditingClientEvents EnteredEditMode="OnEnteredEditMode" ExitedEditMode="OnExitedEditMode" />
                                                            </ig:CellEditing>
                                                        </Behaviors>

                                                    </ig:EditingCore>
                                                </Behaviors>
                                            </ig:WebDataGrid>

                                        </div>

                                    </div>



                                </asp:Panel>
                                <asp:Panel ID="pnlfooter" CssClass="modal-footer" runat="server">

                                    <div class="form-group">
                                        <div class="col-md-1 floatright customfooter-btn-cancel">
                                            <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass=" btn btn-orange" TabIndex="8" OnClick="btnClear_Click" CausesValidation="false" />
                                        </div>
                                        <div class="col-md-1 floatright cust-footer-btn-save">
                                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClientClick="removequery();" ValidationGroup="vgrpSave" OnClick="btnSave_Click" />
                                        </div>
                                    </div>


                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>

                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnprjID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnphasesID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnphaseowner" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnphaseresource" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <%-- <asp:PostBackTrigger ControlID="iwdg_projectphases" />--%>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>
