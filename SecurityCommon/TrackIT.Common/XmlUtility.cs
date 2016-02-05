using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace TrackIT.Common
{
    /// <summary>
    /// The XmlUtility class contains a set of static methods used to 
    /// help simplify Xml Operations.
    /// </summary>
    public static class XmlUtility
    {
        #region GetXmlAttributeAsString()
        /// <summary>
        /// Return a string-based Value for the given XmlAttribute.
        /// </summary>
        /// <param name="StartNode">The <see cref="XmlNode"/> to inspect.</param>
        /// <param name="sXPath">The Path to the Attribute to inspect.</param>
        /// <param name="sAttribute">The name of the Attribute to return.</param>
        /// <returns>The value associated with the specified Attribute, or string.empty if it doesn't exist.</returns>
        public static string GetXmlAttributeAsString(XmlNode StartNode, string sXPath, string sAttribute)
        {
            // Was the node specified null?
            if (StartNode == null)
                return string.Empty;

            // Look to see if a XPath expression was specified.
            XmlNode foundNode = StartNode;
            if (sXPath.Length > 0)
            {
                foundNode = StartNode.SelectSingleNode(sXPath);
            }

            // Was a node acutally found?
            if (foundNode == null)
                return (string.Empty);

            XmlAttribute attr = foundNode.Attributes[sAttribute];
            if (attr == null)
                return (string.Empty);

            return (attr.Value);
        }
        #endregion

        #region GetXmlAttributeAsInt()
        /// <summary>
        /// Returns a int-based Value for the given XmlAttribute.
        /// </summary>
        /// <param name="StartNode">The <see cref="XmlNode"/> to inspect.</param>
        /// <param name="sXPath">The Path to the Attribute to inspect.</param>
        /// <param name="sAttribute">The name of the Attribute to return.</param>
        /// <returns>The value associated with the specified Attribute, or 0 if it doesn't exist.</returns>
        public static int GetXmlAttributeAsInt(XmlNode StartNode, string sXPath, string sAttribute)
        {
            int iValue = 0;

            // Attempt to locate the Value.
            try
            {
                iValue = int.Parse(XmlUtility.GetXmlAttributeAsString(StartNode, sXPath, sAttribute));
            }
            catch
            {
                // Choose to do nothing.  ARGH!  NOOOOOOO!  You suck!  don't do this.   
                // This is causing errors where required ids are missing but the program just clunks along merrily, suppressing the source of error!
            }

            // Return the Value, or O if one couldn't be retrieved.
            return iValue;
        }
        #endregion

        #region GetXmlAttributeAsFloat()
        /// <summary>
        /// Returns a float-based Value for the given XmlAttribute.
        /// </summary>
        /// <param name="StartNode">The <see cref="XmlNode"/> to inspect.</param>
        /// <param name="sXPath">The Path to the Attribute to inspect.</param>
        /// <param name="sAttribute">The name of the Attribute to return.</param>
        /// <returns>The value associated with the specified Attribute, or 0 if it doesn't exist.</returns>
        public static float GetXmlAttributeAsFloat(XmlNode StartNode, string sXPath, string sAttribute)
        {
            float fValue = 0;

            // Attempt to locate the Value.
            try
            {
                fValue = float.Parse(XmlUtility.GetXmlAttributeAsString(StartNode, sXPath, sAttribute));
            }
            catch
            {
                // Choose to do nothing.
            }

            // Return the Value, or O if one couldn't be retrieved.
            return fValue;
        }
        #endregion

        #region GetXmlValueAsString()
        /// <summary>
        /// Returns a string-based Value for the given Element.
        /// </summary>
        /// <param name="StartNode">The <see cref="XmlNode"/> to inspect.</param>
        /// <param name="sXPath">The Path to the Attribute to inspect.</param>
        /// <returns>The Value associated with the Element, or string.Empty if it doesn't exist.</returns>
        public static string GetXmlValueAsString(XmlNode StartNode, string sXPath)
        {
            // Was the node specified null?
            if (StartNode == null)
                return string.Empty;

            // Look to see if a XPath expression was specified.
            XmlNode foundNode = StartNode;
            if (sXPath.Length > 0)
            {
                foundNode = StartNode.SelectSingleNode(sXPath);
            }

            // Was a node acutally found?
            if (foundNode != null)
                return foundNode.InnerText;
            else
                return string.Empty;
        }
        #endregion

        #region GetXmlValueAsInt()
        /// <summary>
        /// Returns a int-based Value for the given Element.
        /// </summary>
        /// <param name="StartNode">The <see cref="XmlNode"/> to inspect.</param>
        /// <param name="sXPath">The Path to the Attribute to inspect.</param>
        /// <returns>The Value associated with the Element, or 0 if it doesn't exist.</returns>
        public static int GetXmlValueAsInt(XmlNode StartNode, string sXPath)
        {
            int _value = 0;

            // Attempt to locate the Value.
            try
            {
                _value = int.Parse(XmlUtility.GetXmlValueAsString(StartNode, sXPath));
            }
            catch
            {
                // Choose to do nothing
            }

            // Return the Value, or O if one couldn't be retrieved.
            return (_value);
        }
        #endregion

        #region GetXmlValueAsDateTime()
        /// <summary>
        /// Returns a DateTime-based Value for the given Element.
        /// </summary>
        /// <param name="StartNode">The <see cref="XmlNode"/> to inspect.</param>
        /// <param name="sXPath">The Path to the Attribute to inspect.</param>
        /// <returns>The Value associated with the Element, or a beginning DateTime if it doesn't exist.</returns>
        public static DateTime GetXmlValueAsDateTime(XmlNode StartNode, string sXPath)
        {
            // Assign a "beginning" DateTime.
            DateTime dtValue = DateTime.Parse("1900-01-01 12:00:00 AM");

            // Attempt to locate the Value.
            try
            {
                dtValue = DateTime.Parse(XmlUtility.GetXmlValueAsString(StartNode, sXPath));
            }
            catch
            {
                // Choose to do nothing
            }

            // Return the Value, or a beginning DateTime if it doesn't exist.
            return dtValue;
        }
        #endregion

        #region AddChildXmlNode()
        /// <summary>
        /// Add a new Node to the specified ParentNode.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlNode will live in.</param>
        /// <param name="sNodeName">The Name of the Node to add.</param>
        /// <returns>The newly created XmlNode.</returns>
        public static XmlNode AddChildXmlNode(XmlDocument MyDocument, string sNodeName)
        {
            return (XmlUtility.AddChildXmlNode(MyDocument, null, sNodeName, string.Empty));
        }

        /// <summary>
        /// Add a new Node to the specified ParentNode.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlNode will live in.</param>
        /// <param name="sNodeName">The Name of the Node to add.</param>
        /// <param name="sNodeValue">The Value to associate with the Node.</param>
        /// <returns>The newly created XmlNode.</returns>
        public static XmlNode AddChildXmlNode(XmlDocument MyDocument, string sNodeName, string sNodeValue)
        {
            return (XmlUtility.AddChildXmlNode(MyDocument, null, sNodeName, string.Empty));
        }

        /// <summary>
        /// Add a new Node to the specified ParentNode.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlNode will live in.</param>
        /// <param name="ParentNode">The Parent XmlNode for the newly created Node.</param>
        /// <param name="sNodeName">The Name of the Node to add.</param>
        /// <returns>The newly created XmlNode.</returns>
        public static XmlNode AddChildXmlNode(XmlDocument MyDocument, XmlNode ParentNode, string sNodeName)
        {
            return (XmlUtility.AddChildXmlNode(MyDocument, ParentNode, sNodeName, string.Empty));
        }

        /// <summary>
        /// Add a new Node to the specified ParentNode.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlNode will live in.</param>
        /// <param name="ParentNode">The Parent XmlNode for the newly created Node.</param>
        /// <param name="sNodeName">The Name of the Node to add.</param>
        /// <param name="sNodeValue">The Value to associate with the Node.</param>
        /// <returns>The newly created XmlNode.</returns>
        public static XmlNode AddChildXmlNode(XmlDocument MyDocument, XmlNode ParentNode, string sNodeName, string sNodeValue)
        {
            // Create a new Node.
            XmlNode ChildNode = MyDocument.CreateNode(XmlNodeType.Element, sNodeName, null);

            // Assign a value if specified.
            if (sNodeValue.Length > 0)
            {
                ChildNode.InnerText = sNodeValue;
            }

            // Add to a Parent Node
            if (ParentNode != null)
            {
                ParentNode.AppendChild(ChildNode);
            }

            // Return the newly created XmlNode.
            return (ChildNode);
        }
        #endregion

        #region AddXmlAttribute()
        /// <summary>
        /// Add a new Attribute to the specified Node.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlAttribute will live in.</param>
        /// <param name="sAttrName">The Name of the Attribute to add.</param>
        /// <param name="sAttrValue">The Value to associate with the Attribute.</param>
        /// <returns>The newly created XmlAttribute.</returns>
        public static XmlAttribute AddXmlAttribute(XmlDocument MyDocument, string sAttrName)
        {
            return (XmlUtility.AddXmlAttribute(MyDocument, null, sAttrName, string.Empty));
        }

        /// <summary>
        /// Add a new Attribute to the specified Node.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlAttribute will live in.</param>
        /// <param name="sAttrName">The Name of the Attribute to add.</param>
        /// <param name="sAttrValue">The Value to associate with the Attribute.</param>
        /// <returns>The newly created XmlAttribute.</returns>
        public static XmlAttribute AddXmlAttribute(XmlDocument MyDocument, string sAttrName, string sAttrValue)
        {
            return (XmlUtility.AddXmlAttribute(MyDocument, null, sAttrName, sAttrValue));
        }

        /// <summary>
        /// Add a new Attribute to the specified Node.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlAttribute will live in.</param>
        /// <param name="MyNode">The XmlNode the Attribute is for.</param>
        /// <param name="sAttrName">The Name of the Attribute to add.</param>
        /// <returns>The newly created XmlAttribute.</returns>
        public static XmlAttribute AddXmlAttribute(XmlDocument MyDocument, XmlNode MyNode, string sAttrName)
        {
            return (XmlUtility.AddXmlAttribute(MyDocument, MyNode, sAttrName, string.Empty));
        }

        /// <summary>
        /// Add a new Attribute to the specified Node.
        /// </summary>
        /// <param name="MyDocument">The XmlDocument the XmlAttribute will live in.</param>
        /// <param name="MyNode">The XmlNode the Attribute is for.</param>
        /// <param name="sAttrName">The Name of the Attribute to add.</param>
        /// <param name="sAttrValue">The Value to associate with the Attribute.</param>
        /// <returns>The newly created XmlAttribute.</returns>
        public static XmlAttribute AddXmlAttribute(XmlDocument MyDocument, XmlNode MyNode, string sAttrName, string sAttrValue)
        {
            // Create a new Attribute.
            XmlAttribute attr = MyDocument.CreateAttribute(null, sAttrName, null);

            // Assign a value if specified
            if (sAttrValue.Length > 0)
            {
                attr.Value = sAttrValue;
            }

            // Add to a Parent Node.
            if (MyNode != null)
            {
                MyNode.Attributes.Append(attr);
            }

            // Return the newly created XmlAttribute
            return (attr);
        }
        #endregion
    }
}