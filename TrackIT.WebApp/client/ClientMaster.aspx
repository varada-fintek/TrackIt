<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="ClientMaster.aspx.cs" Inherits="TrackIT.WebApp.client.ClientMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>

<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>

<asp:Content ID="client" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        function editRow(obj) {
            var grid = $find("lwdg_FundMasterGrid");
            var row = $(obj).parents("tr[type='row']").get(0);
            var rowid = row.cells[1].innerHTML;
            var pop_open = '1';
            document.getElementById("hdnClientID").value = rowid;
            document.getElementById("hdnpop").value = pop_open;
            // $find("mpe").show();
            return true;
        }
        function ClosePopup() {
            //alert();
            var pop_open = '0';
            document.getElementById("hdnpop").value = pop_open;
        }


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
                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterPDF" DataExportMode="AllDataInDataSource" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterExcel" DataExportMode="AllDataInDataSource" />
                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-user"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreateClient" runat="server"></asp:Label>
                    </h1>
                </div>
                <div id="pnl_clientGrid" runat="server"></div>
            </div>
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

                        <cc1:ModalPopupExtender ID="mpe_clientPopup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                            PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                        </cc1:ModalPopupExtender>

                        <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">

                            <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">

                                <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                    <h4 class="modal-title">Clients</h4>
                                </asp:Panel>

                                <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">

                                    <asp:ValidationSummary ID="valSumUser" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="vgrpSave" />
                                    <div class="form-horizontal">

                                        <div class="form-group align-popcontent">
                                            <asp:Label ID="lblclientcode" class="control-label col-md-2" runat="server"></asp:Label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtclientCode" CssClass="form-control" runat="server" TabIndex="1"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1" style="display: none;">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                                                    runat="server" Enabled="True" TargetControlID="txtclientCode"
                                                    ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="reqvclientcode" runat="server"
                                                    ControlToValidate="txtclientCode" Display="Static" SetFocusOnError="True"
                                                    ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                                <asp:RequiredFieldValidator ID="reqvclientcodeUNQ" runat="server"
                                                    ControlToValidate="txtclientCode" Display="Static" SetFocusOnError="True"
                                                    ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group align-popcontent">
                                            <asp:Label ID="lblclientname" class="control-label col-md-2" runat="server"></asp:Label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtclientName" ClientIDMode="Static" CssClass="form-control" runat="server" TabIndex="2"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1" style="display: none;">
                                                <cc1:FilteredTextBoxExtender ID="ftbextxtclientname"
                                                    runat="server" Enabled="True" TargetControlID="txtclientName"
                                                    ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="reqvtxtclientname" runat="server"
                                                    ControlToValidate="txtclientName" Display="Static" SetFocusOnError="True"
                                                    ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>


                                            </div>
                                        </div>
                                        <div class="form-group align-popcontent">
                                            <asp:Label ID="lblIsactive" class="control-label col-md-2" runat="server"></asp:Label>
                                            <div class="col-md-1">
                                                <span class="input-icon">
                                                    <asp:CheckBox ID="chkisactive" class="checkbox" runat="server" />
                                                </span>
                                            </div>
                                        </div>
                                        <hr />

                                        <div class="col-md-4">
                                            <h3>Address info  </h3>
                                            <div class="form-group">
                                                <asp:Label ID="lbladdressline1" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtaddressline1" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="ccaddressline1_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtaddressline1"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqvtxtaddressLine1" runat="server"
                                                        ControlToValidate="txtaddressline1" Display="Static" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lbladdressline2" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtaddressline2" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <cc1:FilteredTextBoxExtender ID="ccaddressline2_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtaddressline2"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lbladdresscity" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtaddresscity" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <cc1:FilteredTextBoxExtender ID="cccity_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtaddresscity"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lbladdressstate" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtaddressstate" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <cc1:FilteredTextBoxExtender ID="ccstate_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtaddressstate"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lbladdresszip" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtaddresszip" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="cctxtaddresszip_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtaddresszip"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lbladdresscountry" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddladdresscountry" runat="server" CssClass="form-control chzn-select" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <asp:RequiredFieldValidator ID="reqvcountry" runat="server"
                                                        ControlToValidate="ddladdresscountry" Display="Static" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblbillinfosame" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-1">
                                                    <span class="input-icon">
                                                        <asp:CheckBox ID="chkbillinfosame" class="checkbox" OnCheckedChanged="chkbillinfosame_CheckedChanged" runat="server" AutoPostBack="true" />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <h3>Contact Info  </h3>
                                            <div class="form-group">
                                                <asp:Label ID="lblclientcontactname" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientcontactname" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="ccclientcontactnam_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtclientcontactname"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqvtxtclientcontactname" runat="server"
                                                        ControlToValidate="txtclientcontactname" Display="Dynamic" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lblclientcontactdesignation" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtclientcontactdesignation" CssClass="form-control" runat="server" TabIndex="1" MaxLength="20" ToolTip="Maximum Character 20"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="cc_clientcontactdesignationFilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtclientcontactdesignation"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqvtxtclientcontactdesignation" runat="server"
                                                        ControlToValidate="txtclientcontactdesignation" Display="Static" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <h3>Billing Info </h3>

                                            <div class="form-group">
                                                <asp:Label ID="lblbilladdress1" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtbilladdress1" CssClass="form-control" runat="server" TabIndex="1" MaxLength="50" ToolTip="Maximum Character 50"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none">
                                                    <cc1:FilteredTextBoxExtender ID="ccbilladdressline1_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtbilladdress1"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="reqvtxtbilladdressline1" runat="server"
                                                        ControlToValidate="txtbilladdress1" Display="Dynamic" SetFocusOnError="True"
                                                        ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lblbilladdress2" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtbilladdress2" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="ccbilladdressline2_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtbilladdress2"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lblbillcity" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtbillcity" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="ccbillcity_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtbillcity"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lblbillstate" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtbillstate" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="ccbillstate_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtbillstate"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lblbillzip" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtbillzip" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <cc1:FilteredTextBoxExtender ID="ccbillzip_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" TargetControlID="txtbillzip"
                                                        ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                    </cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="lblbillcountry" class="control-label col-md-2" runat="server"></asp:Label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ddlbillCountry" runat="server" CssClass="form-control chzn-select" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1" style="display: none;">
                                                    <asp:RequiredFieldValidator ID="reqvddlbillcountry" runat="server"
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
                                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass=" btn btn-orange" OnClientClick="ClosePopup();" TabIndex="8" CausesValidation="false" OnClick="btnCancel_Click" />
                                        </div>
                                        <div class="col-md-1 floatright">
                                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClick="btnSave_Click" ValidationGroup="vgrpSave" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>

                    </div>
            </div>
            </div>
            <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnClientID" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
