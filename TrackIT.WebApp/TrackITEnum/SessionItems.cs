using System;
using TrackIT.Common;

namespace TrackIT.WebApp.TrackITEnum
{
    public enum SessionItems
    {
       
        [StringAttribute("USERID")]
        User_ID,
        [StringAttribute("LOGINNAME")]
        Login_Name,
        [StringAttribute("USERNAME")]
        User_Name,
        [StringAttribute("USER_TYPE")]
        User_Type,
        [StringAttribute("MODULEID")]
        Module_ID,
        [StringAttribute("LEFTNODE")]
        Left_Node,
        [StringAttribute("SCREENID")]
        Screen_ID,
       
        [StringAttribute("SUPER_USER")]
        Super_User,
        [StringAttribute("TOP_MENU")]
        Top_Menu,
        [StringAttribute("LEFT_MENU")]
        Left_Menu,
        [StringAttribute("MODULENAME")]
        Module_Name,
        
       
        [StringAttribute("loggedin_User_ID")]
        loggedin_User_ID,
        
        [StringAttribute("ROLE_ID")]
        Role_ID,
        [StringAttribute("USER_DISPLAY_NAME")]
        User_Display_Name,
        [StringAttribute("ROLE_NAME")]
        Role_Name,
        [StringAttribute("ROLE_TYPE")]
        Role_Type,
        [StringAttribute("IS_ADMIN_ROLE")]
        Is_Admin_Role,
        
        [StringAttribute("USER_PHOTO_PATH")]
        User_Photo_Path,
       
        [StringAttribute("IS_FIRST_LOGIN")]
        Is_First_Login,
    }
}
