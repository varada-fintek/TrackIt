<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagingControl.ascx.cs" Inherits="ProjMngTrack.WebApp.UserControls.PagingControl" %>

<table border="0">
	<tr>
		<td>
			<asp:Button ID="BtnFirst" CausesValidation="False" CssClass="paginate_button previous" Style="cursor: hand; font-weight: bold"
				ToolTip="First Page" runat="server" Visible="true" Text="<<" Width="30px" 
				OnClick="BtnFirst_Click" ></asp:Button>
			<asp:Button ID="BtnPrev" Visible="true" CausesValidation="False" CssClass="button" Style="cursor: hand;
				font-weight: bold" ToolTip="Previous" runat="server" Text="<" Width="30px" 
				OnClick="BtnPrev_Click"></asp:Button>&nbsp;
		</td>
		<td>
			<asp:Repeater ID="RptrPageIndex" runat="server" OnItemDataBound="RptrPageIndex_ItemDataBound">
				<ItemTemplate>
					<asp:LinkButton ID="lnkPageIndex" runat="server" CausesValidation="false" OnClick="GetIndex" CommandName="Filter"
						Font-Size="11px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PageIndex")%>'
						Text='<%# DataBinder.Eval(Container, "DataItem.PageIndex")%>'>
          
					</asp:LinkButton>
				</ItemTemplate>
			</asp:Repeater>
		</td>
		<td>
			<asp:Button ID="BtnNext" Visible="true" CausesValidation="False" CssClass="button" Style="cursor: hand;
				font-weight: bold" ToolTip="Next" runat="server" Text=">" Width="30px" 
				OnClick="BtnNext_Click"></asp:Button>
			<asp:Button ID="BtnLast" Visible="true" CausesValidation="False" CssClass="button" Style="cursor: hand;
				font-weight: bold" ToolTip="Last Page" runat="server" Text=">>" Width="30px"
				OnClick="BtnLast_Click"></asp:Button>
			<input id="hidIndex" type="hidden" value="" runat="server" />
			<input id="hidPageIndex" type="hidden" value="0" runat="server" />
		</td>
	</tr>
</table>