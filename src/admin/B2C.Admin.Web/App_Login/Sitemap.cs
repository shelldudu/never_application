using Never;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace B2C.Admin.Web
{
    /// <summary>
    ///
    /// </summary>
    public class Sitemap
    {
        #region field

        /// <summary>
        /// 菜单文件名
        /// </summary>
        private const string sitemap = "sitemap.xml";

        /// <summary>
        ///菜单
        /// </summary>
        private Menu menu = null;

        private FileInfo file = null;

        private readonly System.IO.FileSystemWatcher filewatcher = null;
        #endregion field

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Sitemap"/> class.
        /// </summary>
        public Sitemap() : this(new FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", sitemap)))
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        public Sitemap(string file) : this(new FileInfo(file))
        {
        }

        public Sitemap(FileInfo file)
        {
            this.file = file;
            this.filewatcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(this.file.FullName), this.file.Extension);
            this.filewatcher.Changed += Filewatcher_Changed;
        }

        private void Filewatcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.menu = this.LoadMenu();
        }

        #endregion ctor

        #region menu

        /// <summary>
        ///菜单
        /// </summary>
        public Menu Menu
        {
            get
            {
                return this.menu;
            }
        }

        #endregion menu

        #region base

        /// <summary>
        ///
        /// </summary>
        public void Startup()
        {
            this.menu = this.LoadMenu();
        }

        /// <summary>
        ///
        /// </summary>
        public void Shutdown()
        {
        }

        #endregion base

        #region loadfile

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Menu LoadMenu()
        {
            if (this.file == null || !this.file.Exists)
                return new Menu();

            var xml = new XmlDocument();
            xml.Load(this.file.FullName);
            return LoadMenu(xml);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private Menu LoadMenu(XmlDocument xml)
        {
            if (!xml.HasChildNodes)
                return new Menu();

            var xmlNode = FindSiteMapNode(xml.FirstChild);
            if (xmlNode == null)
                return new Menu { };

            var root = PopulateNode(xmlNode, null);
            InitTree(xmlNode, root, root);
            return root;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="menu"></param>
        /// <param name="parent"></param>
        private void InitTree(XmlNode xmlNode, Menu menu, Menu parent)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                var that = PopulateNode(node, parent);
                menu.Childrens.Add(that);
                InitTree(node, that, that);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Menu PopulateNode(XmlNode node, Menu parent)
        {
            var title = GetStringValueFromAttribute(node, "title");
            var icon = GetStringValueFromAttribute(node, "icon");
            var area = GetStringValueFromAttribute(node, "area");
            var controller = GetStringValueFromAttribute(node, "controller");
            var actionResult = GetStringValueFromAttribute(node, "actionResult");
            var visible = GetStringValueFromAttribute(node, "visible");

            var isnull = parent == null;
            return new Menu()
            {
                Title = title,
                Icon = icon,
                Area = area.IsNotNullOrEmpty() ? area : (isnull ? area : parent.Area),
                Controller = controller.IsNotNullOrEmpty() ? controller : (isnull ? controller : parent.Controller),
                ActionResult = actionResult,
                Visible = visible.IsNullOrEmpty() ? visible.AsBool(true) : (isnull ? visible.AsBool(true) : parent.Visible)
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            if (node == null || node.Attributes == null || node.Attributes.Count <= 0)
                return string.Empty;

            var attribute = node.Attributes[attributeName];
            if (attribute == null)
                return string.Empty;

            return attribute.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="localName"></param>
        /// <returns></returns>
        private XmlNode FindSiteMapNode(XmlNode node, string localName = "sitemap")
        {
            var xmlNode = node;
            while (true)
            {
                if (xmlNode == null || (!xmlNode.HasChildNodes && xmlNode.NextSibling == null))
                {
                    node = null;
                    break;
                }

                if (xmlNode.LocalName.Equals(localName) || xmlNode.Name.Equals(localName))
                    break;

                xmlNode = xmlNode.NextSibling;
            }
            return xmlNode;
        }

        #endregion loadfile
    }
}