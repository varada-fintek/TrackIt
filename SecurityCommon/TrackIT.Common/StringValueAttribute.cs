using System;
using System.Reflection;

namespace TrackIT.Common
{    
    public sealed class StringAttribute : System.Attribute
    {
        private string _value; 
     
        public StringAttribute(string value) 
        { 
            _value = value; 
        } 
        
        public string Value 
        { 
            get { return _value; } 
        } 
    }

    public static class StringEnum
    {    
        public static string GetStringValue(Enum enumValue)    
        {   
            FieldInfo fiEnum = enumValue.GetType().GetField(enumValue.ToString());
            StringAttribute[] sAttributes = fiEnum.GetCustomAttributes(typeof(StringAttribute), false) as StringAttribute[];
            return (sAttributes.Length > 0) ? sAttributes[0].Value : enumValue.ToString();            
        }
    }    
}
