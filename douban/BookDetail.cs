using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace douban
{
    public partial class BookDetail : Form
    {        
        //定义全局bookUrl
        private string _bookUrl = "";

        public BookDetail(string url)
        {
            InitializeComponent();

            _bookUrl = url;//赋值
            Bind();
        }

        protected void Bind()
        {
            try
            {
                Json getJson = new Json();
                JObject file = JObject.Parse(getJson.GetJsonByUrl(_bookUrl));
                
                this.labelBook.Text = "《" + file["title"].ToString().Trim() + "》";
                this.labAuthor.Text = file["author"].ToString().Trim() == "[]" ? file["author"].ToString().Trim() : file["author"][0].ToString().Trim();
                this.labPublisher.Text = file["publisher"].ToString().Trim();
                this.labPubDate.Text = file["pubdate"].ToString().Trim();
                this.labPage.Text = file["pages"].ToString().Trim();
                this.labISBN.Text = file["isbn10"].ToString().Trim();
                this.labPrice.Text = file["price"].ToString().Trim();
                this.pictureBox.ImageLocation = file["image"].ToString().Trim();
                this.textContent.Text = file["summary"].ToString().Trim();
            }
            catch (Exception)
            {
            }
        }
 
    }
}
