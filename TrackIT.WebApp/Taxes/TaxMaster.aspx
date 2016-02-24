<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" AutoEventWireup="true" CodeBehind="TaxMaster.aspx.cs" Inherits="TrackIT.WebApp.Taxes.TaxMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>


<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>
<asp:Content ID="tax" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
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
            var grid = $find("iwdg_TaxDetailsGrid");
            var row = $(obj).parents("tr[type='row']").get(0);
            var rowid = row.cells[1].innerHTML;
            var pop_open = '1';
            //alert("ID="+rowid);
            document.getElementById("hdntaxkey").value = rowid;
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="main-container" id="main-container">
                <div class="page-header">

                    <div class="floatright pull_right">
                        <asp:ImageButton ID="btnExportExcel" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/excel_icon.png"></asp:ImageButton>
                        &nbsp;&nbsp;                       
                        <asp:ImageButton ID="btnExportPDF" runat="server" CausesValidation="False" ImageAlign="Middle" ImageUrl="~/images/pdf_icon.png"></asp:ImageButton>
                        &nbsp;&nbsp;
                             <span class="custom-createnew" style="float: right;" id="createnew" clientidmode="Static" runat="server">
                                 <a href="#" onclick="ShowModalPopup();"></a></span>
                    </div>
                    <ig:WebDocumentExporter ID="WebPDFExporter" runat="server" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterPDF" DataExportMode="AllDataInDataSource" Format="PDF" />
                    <ig:WebExcelExporter runat="server" ID="WebExcelExporter" EnableStylesExport="false" ExportMode="Download" DownloadName="UserMasterExcel" DataExportMode="AllDataInDataSource" />

                    <h1>
                        <i class="menu-icon fa fa-lg fa-fw fa-money"></i>
                        <i class="icon-angle-right"></i>
                        <asp:Label ID="lblCreatetaxes" runat="server"></asp:Label>
                    </h1>
                </div>

                <div runat="server" id="pnl_taxGrid">
                </div>
            </div>
            <div class="main-content">

                <div class="page-content">

                    <div class="page-content-area">
                        <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" Visible="false" OnClientClick="return ShowModalPopup()" />
                        <asp:LinkButton ID="lnkDummy" runat="server"></asp:LinkButton>
                        <cc1:ModalPopupExtender ID="mpe_taxPopup" BehaviorID="mpe" runat="server" CancelControlID="lnkDummy"
                            PopupControlID="pnlPopup" TargetControlID="createnew" BackgroundCssClass="modal-backdrop">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlPopup" runat="server" CssClass="modal-dialog" Style="display: none;">
                            <asp:Panel ID="pnlcontent" runat="server" CssClass="modal-content">
                                <asp:Panel ID="pnlheader" runat="server" CssClass="modal-header-green">
                                    <h4 class="modal-title">Taxes</h4>
                                </asp:Panel>
                                <asp:Panel ID="pnlbody" runat="server" CssClass="modal-body text-center">
                                    <asp:ValidationSummary ID="valSumproject" runat="server" ShowMessageBox="true" ShowSummary="false" DisplayMode="BulletList" ValidationGroup="vgrpSave" />
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label ID="lbltaxcode" class="control-label col-md-2" runat="server"></asp:Label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txttaxcode" CssClass="form-control" runat="server" TabIndex="1" MaxLength="100"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1" style="display: none;">
                                                <cc1:FilteredTextBoxExtender ID="ccfirstname_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" TargetControlID="txttaxcode"
                                                    ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="reqvtaxcode" runat="server"
                                                    ControlToValidate="txttaxcode" Display="Static" SetFocusOnError="True"
                                                    ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                                 <asp:RequiredFieldValidator ID="reqvtaxcodeUNQ" runat="server"
                                                    ControlToValidate="txttaxcode" Display="Static" SetFocusOnError="True"
                                                    ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label ID="lbltaxname" class="control-label col-md-2" runat="server"></asp:Label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txttaxname" CssClass="form-control" runat="server" TabIndex="1" MaxLength="100" ToolTip="Maximum Character 10"></asp:TextBox>
                                               
                                                    </div>
                                            <div class="col-md-1" style="display: none;">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                    runat="server" Enabled="True" TargetControlID="txttaxname"
                                                    ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="reqvtxttaxname" runat="server"
                                                    ControlToValidate="txttaxname" Display="Static" SetFocusOnError="True"
                                                    ValidationGroup="vgrpSave" InitialValue=""></asp:RequiredFieldValidator>

                                               

                                            </div>
                                        </div>
                                        <asp:LinkButton ID="lnkAddrow" runat="server" CssClass="label" OnClick="lnkAddrow_Click"></asp:LinkButton>
                                        <div class="col-sm-12">
                                            <div runat="server" id="pnl_taxdetailsGrid">
                                            </div>

        <ig:WebDataGrid ID="checkgrid" runat="server"
         AutoGenerateColumns="true" Width="1000px">
        <EditorProviders>
            <ig:DatePickerProvider ID="FromdateProvider" />
            <ig:DatePickerProvider ID="TodateProvider" />
            <ig:TextBoxProvider ID="Taxpercentage"/>

            <ig:DropDownProvider ID="TaxtypeProvider">
                <EditorControl ID="taxdetailsEditorControl" runat="server" DisplayMode="DropDownList" />
            </ig:DropDownProvider>

            <ig:DropDownProvider ID="TaxappliedonProvider">
                <EditorControl ID="taxappliedEditorControl" runat="server" DisplayMode="DropDownList"  />
            </ig:DropDownProvider>
        </EditorProviders>

       <%-- <Columns>
            <ig:BoundDataField DataFieldName="OrderID" Key="OrderID" Width="50px">
                <Header Text="<%$ Resources:WebDataGrid, EditorColumnOrderID %>" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="ContactName" Key="CustomerID" Width="120px">
                <Header Text="<%$ Resources:WebDataGrid, EditorColumnContactName %>" />
            </ig:BoundDataField>
            <ig:BoundDataField Key="OrderDate" DataFormatString="{0:d}">
                <Header Text="<%$ Resources:WebDataGrid, EditorColumnOrderDate %>" />
            </ig:BoundDataField>
            <ig:BoundDataField Key="ShippedDate" DataFormatString="{0:d}">
                <Header Text="<%$ Resources:WebDataGrid, EditorColumnShippingDate %>" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="ShippingCompany" Key="ShipperID" Width="120px">
                <Header Text="<%$ Resources:WebDataGrid, EditorColumnShippingCompany %>" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Freight" Key="Freight" Width="60px">
                <Header Text="<%$ Resources:WebDataGrid, ColumnHeaderFreight %>" />
            </ig:BoundDataField>
            <ig:BoundDataField DataFieldName="Phone" Key="Phone" Width="110px">
                <Header Text="<%$ Resources:WebDataGrid, ColumnHeaderPhone %>" />
            </ig:BoundDataField>
        </Columns>--%>

       <%-- <Behaviors>
            <ig:EditingCore>
                <Behaviors>
                    <ig:CellEditing Enabled="true">
                        <ColumnSettings>
                            <ig:EditingColumnSetting ColumnKey="CustomerID" EditorID="FromdateProvider" />
                            <ig:EditingColumnSetting ColumnKey="ShipperID" EditorID="ShipperProvider" />
                            <ig:EditingColumnSetting ColumnKey="OrderDate" EditorID="DateInputProvider" />
                            <ig:EditingColumnSetting ColumnKey="ShippedDate" EditorID="DateInputProvider" />
                            <ig:EditingColumnSetting ColumnKey="Freight" EditorID="FreightValueProvider" />
                            <ig:EditingColumnSetting ColumnKey="Phone" EditorID="PhoneInputProvider" />
                        </ColumnSettings>
                    </ig:CellEditing>
                </Behaviors>
            </ig:EditingCore>
        </Behaviors>--%>
    </ig:WebDataGrid>
                                        </div>


                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="pnlfooter" CssClass="modal-footer" runat="server">
                                <div class="form-group">
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass=" btn btn-orange" TabIndex="8" OnClientClick="removequery();" OnClick="btnClear_Click" CausesValidation="false" />
                                            </div>
                                            <div class="col-md-1 floatright">
                                                <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClientClick="removequery();"  ValidationGroup="vgrpSave" OnClick="btnSave_Click" />
                                            </div>
                                        </div>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>

                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnpop" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdntaxkey" runat="server" ClientIDMode="Static" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

