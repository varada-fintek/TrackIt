<%@ Page Language="C#" MasterPageFile="~/master-templates/LoginMaster.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="TrackIT.WebApp.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="Loginvalid" runat="server" ShowMessageBox="True" ShowSummary="False" DisplayMode="List" />
            <div class="container-login">
                <div class="form-centering-wrapper">
                    <div class="form-window-login ">
                        <div class="form-window-login-logo">
                            <div class="login-logo">
                                <img src="../images/logo_fin.png" alt="Fintek Solutions">
                            </div>
                            <h2 class="login-title">Welcome to Fintek!</h2>
                            <asp:Panel ID="loginpanel" runat="server">
                            <div class="login-input-area">
                                <span class="help-block">Login With Your Account</span>
                                <asp:TextBox ID="txtUsername" runat="server" placeholder="User Name" autocomplete="off" />
                                <cc1:FilteredTextBoxExtender ID="txtUsername_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtUsername"
                                    ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@._/-">
                                </cc1:FilteredTextBoxExtender>
                              <%--  <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtUsername" ID="RegularExpressionValidator2" ValidationExpression="^[\s\S]{10,}$" runat="server" ErrorMessage="Minimum 10 characters required."></asp:RegularExpressionValidator>--%>

                                <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" TextMode="Password" />
                                <asp:LinkButton ID="btnuserlogin" AlternateText="Login" Style="" class="btn btn-success" BorderStyle="None" runat="server" OnClick="btnLogin_Click">Login</asp:LinkButton>
                                <%--OnClientClick="Login_validation();"--%>
                                <br />
                                <div>
                                    <a href="#" class="forgot-pass" data-toggle="modal" data-target="#reassign">Forgot Username/Password 
                                    </a>

                                </div>

                                <div class="modal fade" id="reassign" role="dialog">
                                    <div class="modal-dialogsmall">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4>
                                                    <asp:Label class="modal-title" ID="lblForgetPasswordCaption" runat="server"></asp:Label></h4>
                                            </div>
                                            <div class="modal-body-small text-center">
                                                <asp:ValidationSummary ID="varForgetPasswordSummary" runat="server" ShowMessageBox="false" ShowSummary="true" DisplayMode="List" ValidationGroup="vgrpSavePWD" />

                                                <div class="form-group">
                                                    <br />
                                                    <br />
                                                    <asp:Label ID="lblUserNameMail" class="control-label col-md-2" CssClass="form-control" runat="server"></asp:Label>
                                                    <br />
                                                    <br />
                                                    <div class="col-md-5">
                                                        <asp:TextBox ID="txtForgetUserName" CssClass="form-control" runat="server" TabIndex="10"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" CssClass="validator_msg"
                                                            ControlToValidate="txtForgetUserName" Display="Static" SetFocusOnError="True" Text="*"
                                                            ValidationGroup="vgrpSavePWD" InitialValue=""></asp:RequiredFieldValidator>
                                                        <asp:RequiredFieldValidator ID="reqInvalidUserName" runat="server" CssClass="validator_msg"
                                                            ControlToValidate="txtForgetUserName" Display="Static" SetFocusOnError="True" Text="Invalid User Name"
                                                            ValidationGroup="vgrpSavePWD" InitialValue="" Visible="false"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="txtForgetUserName_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" TargetControlID="txtForgetUserName"
                                                            ValidChars="abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890">
                                                        </cc1:FilteredTextBoxExtender>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-success" data-dismiss="modal">Close</button>
                                                <asp:Button ID="btnSavePW" Text="Send" runat="server" class="btn btn-success" TabIndex="28" ValidationGroup="vgrpSavePWD" OnClick="btnSavePW_Click" />
                                                <asp:Button ID="btnCancelPW" Visible="false" Text="Cancel" runat="server" class="btn btn-success" TabIndex="7" OnClick="btnCancelPW_Click" />

                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                                </asp:Panel>

                        </div>
                    </div>
                </div>

                <script type="text/javascript">
                    function sentmail() {
                        alert("Auto Generation Password Sent to Your Mail....Use the password while login again");
                        location.reload();
                    }
                    function styleload() {
                        alert("Invalid User Name / Email");
                    }
                    function EnterError() {
                        alert("Please Enter valid User Name / Email");
                    }


                </script>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

