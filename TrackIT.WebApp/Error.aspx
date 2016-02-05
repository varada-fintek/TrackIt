<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="TrackIT.WebApp.Error" %>

<html>
<body>
    <form runat="server">
        <table cellpadding="0" cellspacing="25" border="0" width="100%">
            <tr>
                <td height="10px"></td>
            </tr>
            <tr>
                <td class="ErrorMessage" align="center">Sorry for the inconvenience. The application<br />
                    has encountered an error...</td>
            </tr>
            <tr>
                <td align="center">
                    <img src="images/robot_error.png" /></td>
            </tr>
            <tr>
                <td height="10px"></td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnOK" CssClass="buttonOk" runat="server" PostBackUrl="~/Home.aspx" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
