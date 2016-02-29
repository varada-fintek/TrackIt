<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="CompanyMaster.aspx.cs" Inherits="TrackIT.WebApp.company.CompanyMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>
<asp:Content ID="company" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
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
            //Unit Testing- Security_ASPX_001
            var grid = $find("lwdg_companyMasterGrid");
            var row = $(obj).parents("tr[type='row']").get(0);
            var rowid = row.cells[1].innerHTML;
            var pop_open = '1';
            document.getElementById("hdnCompID").value = rowid;
            document.getElementById("hdnpop").value = pop_open;
            //$find("mpe").show();
            return true;
        }
        function removequery() {
            //alert();
            var pop_open = '0';
            document.getElementById("hdnpop").value = pop_open;
        }

    </script>
    <asp:UpdatePanel ID="uplState" runat="server">
        <ContentTemplate>
            <div class="main-container" id="main-container">
                <div class="page-header">

                    <div class="floatright pull_right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/excel_icon.png" OnClick="btnExportExcel_Click"></asp:ImageButton>
                        &nbsp;&nbsp;                       
                        <asp:ImageButton ID="btnExportPDF" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/pdf_icon.png" OnClick="btnExportPDF_Click"></asp:ImageButton>
                        &nbsp;&nbsp;
                             <span class="custom-createnew" style="float: right;" id="createnew" clientidmode="Static" runat="server">
                                 <a href="#" onclick="ShowModalPopup();"></a></span>
                    </div>
                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" EnableStylesExport="false" ExportMode="Download" DownloadName="companyMasterPDF" DataExportMode="AllDataInDataSource" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" EnableStylesExport="false" ExportMode="Download" DownloadName="companyMasterExcel" DataExportMode="AllDataInDataSource" />

                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-university"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreateCompanies" runat="server"></asp:Label>
                    </h1>
                </div>
                <%-- Landing Page Grid begins --%>
                <div runat="server" id="pnl_companyGrid"></div>
                <%-- Landing Page Grid End --%>
                <script type="text/javascript">
                    try { ace.settings.check('main-container', 'fixed') } catch (e) { }
                </script>
                <style type="text/css">
                    .Datealign {
                        text-align: center !important;
                    }
                </style>



                <div class="main-content">

                    <div class="page-content">

                        <div class="page-content-area">

                            <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                            <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>

                            <cc1:ModalPopupExtender ID="mpe_CompanyPopup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                                PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                            </cc1:ModalPopupExtender>


                            <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">
                                <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                                    <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                        <h4 class="modal-title">Companies</h4>
                                    </asp:Panel>


                                    <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">
                                        <asp:ValidationSummary ID="valSumUser" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgrpSave" />
                                        <div class="form-horizontal">

                                            <div class="form-group align-popcontent">
                                                <asp:Label ID="lblcompanycode" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtcompanycode" CssClass="form-control" runat="server" TabIndex="1" MaxLength="100" ToolTip="Maximum Character 10"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                                        runat="server" Enabled="True" TargetControlID="txtcompanycode"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqvcompanycode" runat="server"
                                                        ControlToValidate="txtcompanycode" Display="Static" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                    <asp:RequiredFieldValidator ID="reqvcompanycodeUNQ" runat="server"
                                                        ControlToValidate="txtcompanycode" Display="Static" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group align-popcontent">
                                                <asp:Label ID="lblcompanyname" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtcompanyname" CssClass="form-control" runat="server" TabIndex="1" MaxLength="100" ToolTip="Maximum Character 10"></asp:TextBox>
                                                </div>

                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                        runat="server" Enabled="True" TargetControlID="txtcompanyname"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqvtxtcompanyname" runat="server"
                                                        ControlToValidate="txtcompanyname" Display="Static" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                    <asp:RequiredFieldValidator ID="reqvcompanynameUNQ" runat="server"
                                                        ControlToValidate="txtcompanyname" Display="Static" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group align-popcontent">
                                                <asp:Label ID="lblactive" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-1">
                                                    <span class="input-icon">
                                                        <asp:CheckBox ID="chkactive" class="checkbox" runat="server" TabIndex="11" />
                                                    </span>
                                                </div>
                                            </div>

                                            <hr />
                                            

                                            <div class="col-md-4">
                                                <h3>Address info  </h3>
                                                <div class="form-group">
                                                    <asp:Label ID="lbladdressline1" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtaddressline1" CssClass="form-control" runat="server" TabIndex="3" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="ccaddressline1_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtaddressline1"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvtxtaddressline1" runat="server"
                                                            ControlToValidate="txtaddressline1" Display="Dynamic" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lbladdressline2" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtaddressline2" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="ccaddressline2_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtaddressline2"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lblcity" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtcity" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="cccity_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtcity"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblstate" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtstate" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="ccstate_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtcity"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblzip" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtzip" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="cczip_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtzip"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblcountry" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlcountry" runat="server" TabIndex="2" CssClass="form-control chzn-select" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <asp:RequiredFieldValidator ID="reqvcountry" runat="server"
                                                            ControlToValidate="ddlcountry" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblsameinfo" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-1">
                                                        <span class="input-icon">
                                                            <asp:CheckBox ID="chksameinfo" class="checkbox" runat="server" AutoPostBack="true" TabIndex="11" OnCheckedChanged="chksameinfo_CheckedChanged" />
                                                        </span>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="col-md-4">
                                                <h3>Contact Info  </h3>
                                                <div class="form-group">
                                                    <asp:Label ID="lblname" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtname" CssClass="form-control" runat="server" TabIndex="3" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="ccname_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtname"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvtxtname" runat="server"
                                                            ControlToValidate="txtname" Display="Dynamic" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <asp:Label ID="lbldesigination" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtdesigination" CssClass="form-control" runat="server" TabIndex="3" MaxLength="100"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="cc_desiginationFilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtdesigination"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvtxtdesigination" runat="server"
                                                            ControlToValidate="txtdesigination" Display="Dynamic" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <h3>Billing Info </h3>
                                                <div class="form-group">
                                                    <asp:Label ID="lblbilladdressline1" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtbilladdressline1" CssClass="form-control" runat="server" TabIndex="3" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <cc1:FilteredTextBoxExtender ID="ccbilladdressline1_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtbilladdressline1"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="reqvtxtbilladdressline1" runat="server"
                                                            ControlToValidate="txtbilladdressline1" Display="Dynamic" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblbilladdressline2" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtbilladdressline2" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="ccbilladdressline2_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtbilladdressline2"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblbillcity" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtbillcity" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="ccbillcity_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtbillcity"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblbillstate" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtbillstate" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="ccbillstate_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtbillstate"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblbillzip" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtbillzip" CssClass="form-control" runat="server" TabIndex="2" MaxLength="100"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <cc1:FilteredTextBoxExtender ID="ccbillzip_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtbillzip"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <asp:Label ID="lblbillcountry" class="control-label col-md-2" runat="server"></asp:Label>
                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ddlbillcountry" runat="server" TabIndex="2" CssClass="form-control chzn-select" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1" style="display: none;">
                                                        <asp:RequiredFieldValidator ID="reqvbillcountry" runat="server"
                                                            ControlToValidate="ddlbillcountry" Display="Static" SetFocusOnError="True"
                                                            ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlfooter" runat="server" CssClass="modal-footer">
                                        <div class="form-group">
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass=" btn btn-orange" TabIndex="8" OnClick="btnClear_Click" CausesValidation="false" />
                                            </div>
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClick="btnSave_Click" OnClientClick="removequery();" ValidationGroup="vgrpSave" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>

                            </asp:Panel>

                        </div>
                    </div>

                    <a href="#" id="btn-scroll-up" class="btn-scroll-up btn btn-sm btn-inverse">
                        <i class="ace-icon fa fa-angle-double-up icon-only bigger-110"></i>
                    </a>
                </div>
                <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnCompID" runat="server" ClientIDMode="Static" />
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportExcel" />
            <asp:PostBackTrigger ControlID="btnExportPDF" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".e1").select2();
            $(".e1").css("border", "none");
        });
    </script>
</asp:Content>


