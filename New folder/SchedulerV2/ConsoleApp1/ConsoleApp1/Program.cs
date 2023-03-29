using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;

namespace XmlEditor.Tests
{

    public class CannotRemoveRootElementException: Exception
    {

    }

    public interface IXmlEditor
    {

    }
    public class XmlEditor : IXmlEditor
    {
        /// <summary>
        /// Updates a name of all nodes, with a given old name, to a new one.
        /// It does not change the order of nodes in an input document.
        /// Searching for nodes is case-sensitive.
        /// </summary>
        /// <param name="xml">A document to update</param>
        /// <param name="oldName">An old node name</param>
        /// <param name="newName">A new node name</param>
        /// <returns>An updated document</returns>
        /// <exception cref="ArgumentNullException">xml is null</exception>
        /// <exception cref="ArgumentNullException">oldName is null</exception>
        /// <exception cref="ArgumentNullException">newName is null</exception>
        /// <exception cref="ArgumentException">xml is empty</exception>
        /// <exception cref="ArgumentException">oldName is empty</exception>
        /// <exception cref="ArgumentException">newName is empty</exception>
        public string ReplaceNodeName(string xml, string oldName, string newName)
        {
            // Handle Exceptions
            if (xml == null) throw new ArgumentNullException("XML is null");
            if (oldName == null) throw new ArgumentNullException("oldName is null");
            if (newName == null) throw new ArgumentNullException("newName is null");
            if (xml == "") throw new ArgumentException("XML is empty");
            if (oldName == "") throw new ArgumentException("oldName is empty");
            if (newName == "") throw new ArgumentException("newName is empty");

            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            XmlNode root = document.DocumentElement;

            XmlNodeList elementList = document.GetElementsByTagName(oldName);
            foreach(XmlNode node in elementList)
            {
                // Create an element with the new name
                XmlElement newNode = document.CreateElement(newName, root.NamespaceURI);

                // For each child of the old Node, recreate it and add it to the newNode
                foreach(XmlNode n in node.ChildNodes)
                {
                    XmlElement child = document.CreateElement(n.Name, n.NamespaceURI);
                    child.InnerText = n.InnerText;
                    newNode.AppendChild(child);
                }
                // Add that child to the parent node
                node.ParentNode.AppendChild(newNode);
            }

            // remove all the old nodes by that name
            for(int i = 0; i < elementList.Count; i++)
            {
                elementList[0].ParentNode.RemoveChild(elementList[0]);
            }

            return document.ToString();

        }

        /// <summary>
        /// Removes all nodes with a given name.
        /// If a node being remove has children they also should be removed (recursively).
        /// Searching for nodes is case-sensitive.
        /// If after removal some nodes are empty (have no content and no children), then
        /// they should formatted without explicit closing tags.
        /// </summary>
        /// <param name="xml">A document to update</param>
        /// <param name="name">A name of nodes to be removed</param>
        /// <returns>An updated document</returns>
        /// <exception cref="ArgumentNullException">xml is null</exception>
        /// <exception cref="ArgumentNullException">name is null</exception>
        /// <exception cref="ArgumentException">xml is empty</exception>
        /// <exception cref="ArgumentException">name is empty</exception>
        /// <exception cref="CannotRemoveRootElementException">If you try to delete a root element</exception>
        public string RemoveNodesByName(string xml, string name)
        {
            // Handle Exceptions
            if (xml == null) throw new ArgumentNullException("XML is null");
            if (name == null) throw new ArgumentNullException("name is null");
            if (xml == "") throw new ArgumentException("XML is empty");
            if (name == "") throw new ArgumentException("name is empty");
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            XmlNodeList elementList = document.GetElementsByTagName(name);
            for (int i = 0; i < elementList.Count; i++)
            {
                elementList[0].ParentNode.RemoveChild(elementList[0]);
            }
            return document.ToString();
        }

    }
}
