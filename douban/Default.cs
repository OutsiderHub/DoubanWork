using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace douban
{
    public partial class Default : Form
    {
        public Default()
        {
            InitializeComponent();
            //this.picBox.ImageLocation = System.AppDomain.CurrentDomain.BaseDirectory + "douban.png";
        }

        /// <summary>
        /// 搜索按钮的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            string tagString = tbxSearch.Text.Trim();
            if (tagString != "输入图书关键字进行查询" && tagString != "")
            {
                try
                {
                    //创建一个table储存读到的数据
                    DataTable dt = new DataTable();
                    dt.Columns.Add("序号", typeof(int));
                    dt.Columns.Add("书名", typeof(string));
                    dt.Columns.Add("作者", typeof(string));
                    dt.Columns.Add("出版社", typeof(string));
                    dt.Columns.Add("出版日期", typeof(string));
                    dt.Columns.Add("页数", typeof(string));
                    dt.Columns.Add("单价", typeof(string));
                    dt.Columns.Add("ISBN", typeof(string));
                    dt.Columns.Add("bookUrl", typeof(string));

                    int number = 0;
                    string bookName = "";
                    string bookAuthor = "";
                    string publisher = "";
                    string pubDate = "";
                    string pages = "";
                    string price = "";
                    string ISBN = "";
                    string bookUrl = "";

                    Json getJson = new Json();
                    //利用Newtonsoft解析Json
                    JObject jobject = JObject.Parse(getJson.GetJsonByTag(tagString));
                    var jsonFile = from file in jobject["books"].Children() select file;
                    foreach (var file in jsonFile)
                    {
                        number++;//序列递增
                        bookName = file["title"].ToString().Trim();
                        bookAuthor = file["author"].ToString().Trim() == "[]" ? file["author"].ToString().Trim() : file["author"][0].ToString().Trim();
                        publisher = file["publisher"].ToString().Trim();
                        pubDate = file["pubdate"].ToString().Trim();
                        pages = file["pages"].ToString().Trim();
                        ISBN = file["isbn10"].ToString().Trim();
                        price = file["price"].ToString().Trim();
                        bookUrl = file["url"].ToString().Trim();

                        //循环向table里面写入数据
                        dt.Rows.Add(number, bookName, bookAuthor, publisher, pubDate, pages, ISBN, price, bookUrl);
                    }

                    if (number == 0)
                    {
                        MessageBox.Show("没有查询到相关书籍！");
                    }
                    else
                    {
                        //绑定控件
                        this.dataGridView1.AutoGenerateColumns = false;
                        this.dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("查询出错,请检查您的网络连接！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入相应的图书关键字,比如书名、ISBN号等");
                return;
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                string url = "";
                url = dataGridView1[8, e.RowIndex].Value.ToString().Trim(); //此处数字8不稳定，应换成字符串"bookUrl"
                //url = dataGridView1.Rows[e.RowIndex].Cells["bookUrl"].Value.ToString().Trim(); //此处未经调试验证
                if (!string.IsNullOrEmpty(url))
                {
                    Form form = new BookDetail(url);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("该书没有详细信息");
                }
            }
        }

        private void dataGridView1_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                e.ToolTipText = "点击查看《" + this.dataGridView1.Rows[e.RowIndex].Cells["BookName"].Value.ToString() + "》详细信息";
            }
        }
    }
}
