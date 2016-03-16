<%@ Page Language="C#" MasterPageFile="~/master-templates/Layout.Master" UICulture="en-US" AutoEventWireup="true" CodeBehind="TaxMaster.aspx.cs" Inherits="TrackIT.WebApp.Taxes.TaxMaster" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Infragistics4.WebUI.WebDataInput.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics4.Web.v14.1, Version=14.1.20141.2328, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>


<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="adminhead" runat="server">
</asp:Content>
<asp:Content ID="tax" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

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

        function fnGetSelectID() {
            var alretmsg = "";
            var msg = true;
            var grid = $find("ctl00_ContentPlaceHolder1_iwdg_taxdetails_grid");
            var rows = grid.get_rows();
            var fromdate = "";
            var todate = "";
            var percent = "";
            var type = "";
            var appliedon = "";
            for (var i = 0; i < rows.get_length() ; i++) {
                fromdate = rows.get_row(i).get_cell(1).get_value();
                if (fromdate == "" || fromdate == null) {
                    msg = false;
                    alretmsg = "\nPlease Select From Date\n";
                }
                todate = rows.get_row(i).get_cell(2).get_value();
                if (todate == "" || todate == null) {
                    msg = false;
                    alretmsg += "\nPlease Select To Date\n";
                }
                percent = rows.get_row(i).get_cell(3).get_value();
                if (percent == "" || percent == null) {
                    msg = false;
                    alretmsg += "\nPlease enter Tax Percentage\n";
                }
                type = rows.get_row(i).get_cell(4).get_value();
                if (type == "" || type == null) {
                    msg = false;
                    alretmsg += "\nPlease Select Tax Type\n";
                }
                appliedon = rows.get_row(i).get_cell(5).get_value();
                if (appliedon == "" || appliedon == null) {
                    msg = false;
                    alretmsg += "\nPlease Select Tax Applied on\n";
                }
            }
            if (alretmsg != "" && alretmsg != null || msg == false) {
                alert(alretmsg);
                return msg;
            }
            else {
                return msg;
            }


        }

        function DeleteRow() {
            if (confirm("Are you sure want to delete?")) {
                var grid = $find("ctl00_ContentPlaceHolder1_iwdg_taxdetails_grid");
                var gridRows = grid.get_rows()

                var selectedRows = grid.get_behaviors().get_selection().get_selectedRows();
                for (var i = selectedRows.get_length() - 1; i >= 0; i--) {
                    var row = selectedRows.getItem(i);
                    gridRows.remove(row);
                }
            }
        }

    </script>
    <style type="text/css">
        .igdd_DropDownListContainer {
            width: 250px !important;
        }

        .igdd_ValueDisplay {
            width: 250px !important;
        }
    </style>
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
                                        <div class="form-group align-popcontent">
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
                                        <div class="form-group align-popcontent">
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
                                        <div class="col-md-3">
                                            <asp:LinkButton ID="lnkAddrow" runat="server" ValidationGroup="valgrid" OnClientClick="return fnGetSelectID();" OnClick="lnkAddrow_Click1" Text="+Add" />
                                        </div>
                                        <div class="col-sm-12">
                                            <ig:WebDataGrid ID="iwdg_taxdetails_grid" runat="server" AutoGenerateColumns="False" DataKeyFields="tax_details_PK" OnRowsDeleting="iwdg_taxdetails_grid_RowsDeleting">
                                                <Columns>
                                                    <%--<ig:BoundDataField DataFieldName="tax_tax_details_key" DataType="System.Int16" Key="taxdetailskey" Hidden="true">
                    <Header Text="">
                    </Header>
                </ig:BoundDataField>--%>
                                                    <ig:TemplateDataField Key="DeleteItem" Width="20px">
                                                        <ItemTemplate>
                                                       <asp:LinkButton runat="server" ID="DeleteItem" CssClass="fa fa-trash-o" OnClientClick="DeleteRow(); return false;" />
                                                        </ItemTemplate>
                                                    </ig:TemplateDataField>
                                                    <ig:BoundDataField DataFieldName="tax_from" DataType="System.DateTime" Key="fromdate">
                                                        <Header Text="From date">
                                                        </Header>
                                                    </ig:BoundDataField>

                                                    <ig:BoundDataField DataFieldName="tax_to" DataType="System.DateTime" Key="todate">
                                                        <Header Text="To date">
                                                        </Header>
                                                    </ig:BoundDataField>

                                                    <ig:BoundDataField DataFieldName="tax_percent" DataType="System.Int16" Key="taxpercent">
                                                        <Header Text="Percent">
                                                        </Header>
                                                    </ig:BoundDataField>

                                                    <ig:BoundDataField DataFieldName="tax_type" Key="taxtype">
                                                        <Header Text="Type">
                                                        </Header>
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="tax_applied_on" Key="taxappliedon">
                                                        <Header Text="Applied on">
                                                        </Header>
                                                    </ig:BoundDataField>


                                                </Columns>
                                                <EditorProviders>
                                                    <ig:DatePickerProvider ID="WebDataGrid1_DatePickerProvider1">
                                                        <EditorControl runat="server" ClientIDMode="Predictable">
                                                        </EditorControl>
                                                    </ig:DatePickerProvider>

                                                    <ig:DatePickerProvider ID="WebDataGrid1_DatePickerProvider2">
                                                        <EditorControl runat="server" ClientIDMode="Predictable">
                                                        </EditorControl>
                                                    </ig:DatePickerProvider>
                                                    <ig:DropDownProvider ID="ddptaxestype">
                                                        <EditorControl runat="server" ClientIDMode="Predictable" DropDownContainerMaxHeight="200px" DropDownContainerWidth="100%" EnableAnimations="False" EnableDropDownAsChild="False">
                                                        </EditorControl>
                                                    </ig:DropDownProvider>
                                                    <ig:DropDownProvider ID="ddpappliedon">
                                                        <EditorControl runat="server" ClientIDMode="Predictable" DropDownContainerMaxHeight="200px" EnableAnimations="False" EnableDropDownAsChild="False">
                                                        </EditorControl>
                                                    </ig:DropDownProvider>

                                                </EditorProviders>
                                                <Behaviors>
                                                    <ig:Activation />
                                                    <ig:Selection RowSelectType="Multiple" CellClickAction="Row" />
                                                    <ig:EditingCore>
                                                        <Behaviors>

                                                            <ig:RowDeleting Enabled="true" />
                                                            <ig:CellEditing>
                                                                <ColumnSettings>
                                                                    <ig:EditingColumnSetting ColumnKey="fromdate" EditorID="WebDataGrid1_DatePickerProvider1" />
                                                                    <ig:EditingColumnSetting ColumnKey="todate" EditorID="WebDataGrid1_DatePickerProvider2" />
                                                                    <ig:EditingColumnSetting ColumnKey="taxtype" EditorID="ddptaxestype" />
                                                                    <ig:EditingColumnSetting ColumnKey="taxappliedon" EditorID="ddpappliedon" />

                                                                </ColumnSettings>
                                                            </ig:CellEditing>
                                                        </Behaviors>

                                                    </ig:EditingCore>
                                                </Behaviors>
                                            </ig:WebDataGrid>
                                        </div>
                                        <div class="col-sm-12 align-popcontent">
                                            <div runat="server" id="pnl_taxdetailsGrid">
                                            </div>
                                        </div>

                                        <%--   <dx:ASPxGridView ID="dve_taxdetails" runat="server" Width="100%"
                                            AutoGenerateColumns="false" EnableRowsCache="false" Theme="Youthful" Visible="false">
                                  
                                            <Columns>
                                                
                                              <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="True" />
                                                <dx:GridViewDataDateColumn FieldName="tax_from" Caption="From Date">
                                                    <EditItemTemplate>
                                                        <dx:ASPxDateEdit ID="dx_txtfromdate" runat="server"></dx:ASPxDateEdit>
                                                    </EditItemTemplate>
                                                </dx:GridViewDataDateColumn>

                                                <dx:GridViewDataDateColumn FieldName="tax_to" Caption="To Date">
                                                    <EditItemTemplate>
                                                        <dx:ASPxDateEdit ID="dx_txttodate" runat="server"></dx:ASPxDateEdit>
                                                    </EditItemTemplate>
                                                </dx:GridViewDataDateColumn>

                                                 <dx:GridViewDataTextColumn FieldName="tax_percent" Caption="Percentage">
                                                     <EditItemTemplate>
                                                        <dx:ASPxTextBox ID="dx_txtpercent" runat="server" Width="100%">
                                                        </dx:ASPxTextBox>
                                                    </EditItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                               
                                             
                                            </Columns>
                                            <SettingsEditing Mode="Inline" />
                                              <ClientSideEvents BatchEditStartEditing="Grid_BatchEditStartEditing" BatchEditEndEditing="Grid_BatchEditEndEditing" />
                                               
                                        </dx:ASPxGridView>--%>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="pnlfooter" CssClass="modal-footer" runat="server">
                                    <div class="form-group">
                                        <div class="col-md-1 floatright">
                                            <asp:Button ID="btnClear" Text="Cancel" runat="server" CssClass=" btn btn-orange" TabIndex="8" OnClientClick="removequery();" OnClick="btnClear_Click" CausesValidation="false" />
                                        </div>
                                        <div class="col-md-1 floatright">
                                            <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="btn btn-blue" TabIndex="7" OnClientClick="fnGetSelectID();removequery();" ValidationGroup="vgrpSave" OnClick="btnSave_Click" />
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
            <asp:HiddenField ID="hdntaxdetailskey" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnto" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnfrom" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdntaxpercent" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdntaxtype" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdntaxappliedon" runat="server" ClientIDMode="Static" />
            <script type="text/javascript">


            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportExcel" />
            <asp:PostBackTrigger ControlID="btnExportPDF" />
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

