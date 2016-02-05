<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="ucApplicationRequestControls.ascx.cs" Inherits="ProjMngTrack.WebApp.UserControls.ucApplicationRequestControls" %>

        <div class="form-horizontal">
            <div class="row">
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="false" ShowSummary="true" DisplayMode="List" ValidationGroup="vgrpShowRequest" />
            </div> 
            <div class="row">
                <div class="form-group">
                    <asp:Label ID="Label1" class="control-label col-md-2" runat="server"></asp:Label>
                    <div class="col-md-6">
                    </div>
                    <div class="col-md-1">
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblUCAppName" class="control-label col-md-2" runat="server"></asp:Label>
                    <div class="col-md-6">
                        <asp:DropDownList ID="ddlUCAppName" runat="server" TabIndex="1" CssClass="form-control chzn-select" AutoPostBack="true" OnSelectedIndexChanged="ddlUCAppName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-1">
                        <asp:RequiredFieldValidator ID="reqvUCApplicationName" runat="server" CssClass="validator_msg"
                            ControlToValidate="ddlUCAppName" Display="Static" SetFocusOnError="True" Text="*"
                            ValidationGroup="vgrpShowRequest"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div> 
            <div class="row">
                <asp:placeholder runat="server" id="phControls"></asp:placeholder>
            </div> 
        </div> 
