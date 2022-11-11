using NPOI.XSSF.UserModel;
using System.Text;

namespace ScriptHelper
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        StringBuilder sb = new StringBuilder();
        public void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "Excel文件|*.xlsx|所有文件|*.*";
            openFileDialog.RestoreDirectory = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = Path.GetFullPath(openFileDialog.FileName);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == DialogResult.OK/*|| dialog.ShowDialog()==DialogResult.Yes*/)
            {
                SavePosition.Text = dialog.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Boolean b = true;
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    if (string.IsNullOrEmpty((c as TextBox).Text))
                    {
                        MessageBox.Show("请完成全部输入框的填写！");
                        b = false;
                        return;
                    }
                }
            }
            if (true)
            {
                try
                {
                    using (FileStream stream = File.OpenRead(this.textBox1.Text))
                    {
                        XSSFWorkbook wb = new XSSFWorkbook(stream);
                        sb.Append(
                            "USE [" + this.DBName.Text + "]\r\nGO\r\n\r\n/****** Object:  Table [" + this.TableName.Text + "]" +
                            "Script Date: 2022/11/8 19:00:16 ******/\r\nSET ANSI_NULLS ON\r\nGO" +
                            "\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO" +
                            "\r\n\r\nCREATE TABLE [dbo].[" + this.TableName.Text + "](" +
                            "\r\n\t[INDX] [int] IDENTITY(1,1) NOT NULL,"
                            );
                        try
                        {
                            XSSFSheet sheet = (XSSFSheet)wb.GetSheet(this.Sheet.Text);
                            int RowCount = sheet.LastRowNum;
                            int StartRow = int.Parse(this.StartRow.Text);
                            for (int i = StartRow - 1; i <= RowCount; i++)
                            {
                                sb.Append(
                                  sheet.GetRow(i).GetCell(1).StringCellValue.Replace(" ", "") + " " +
                                  sheet.GetRow(i).GetCell(4).StringCellValue.Replace(" ", "") + ",\r\n\t");
                            }


                            sb.Append("[ORG_ID] [varchar](50) NOT NULL,\r\n\t" +
                                "[CREATE_USER_D] [varchar](50) NOT NULL,\r\n\t" +
                                "[CREATE_DATE] [datetime] NOT NULL,\r\n\t" +
                                "[LAST_UPDATE_USER_ID] [decimal](10, 0) NULL,\r\n\t" +
                                "[LAST_UPDATE_DATE] [datetime] NULL,\r\n\t" +
                                "[DELETED] [nvarchar](1) NULL,\r\n\t" +
                                "[RESERVER0] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER1] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER2] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER3] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER4] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER5] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER6] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER7] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER8] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER9] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER10] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER11] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER12] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER13] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER14] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER15] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER16] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER17] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER18] [nvarchar](max) NULL,\r\n\t" +
                                "[RESERVER19] [nvarchar](max) NULL,\r\n" +
                                " CONSTRAINT [PK_BMS_VGM_HP] PRIMARY KEY CLUSTERED \r\n(\r\n\t" +
                                "[INDX] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]\r\n)" +
                                " ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n");

                            for (int i = StartRow - 1; i <= RowCount; i++)
                            {
                                sb.Append("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + sheet.GetRow(i).GetCell(2).StringCellValue + "' , @level0type=N'SCHEMA'," +
                                    "@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + this.TableName.Text + "', @level2type=N'COLUMN',@level2name=N'" + sheet.GetRow(i).GetCell(1).StringCellValue.Replace(" ","") + "'" +
                                    "\r\nGO\r\n");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("未找到Sheet,请正确输入！");
                            return;
                        }


                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("文件路径错误，请输入正确的路径！");
                    return;
                }
                try
                {
                    if (!File.Exists(this.SavePosition.Text + @"\" + this.TableName.Text + ".sql"))
                    {
                        //FileStream file = new FileStream(this.SavePosition.Text + @"\" + this.TableName.Text + ".sql", FileMode.Create, FileAccess.Write);
                        //StreamWriter sw = new StreamWriter(file);
                        //sw.Write(sb);
                        //sw.Flush();
                        //sw.Close();
                        //file.Close();


                        try
                        {
                            using (var file = System.IO.File.Create(this.SavePosition.Text + @"\" + this.TableName.Text + ".sql"))
                            {

                                using (StreamWriter sw = new StreamWriter(file))
                                {
                                    sw.Write(sb);
                                    sw.Flush();
                                }

                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("文件夹路径错误，请输入正确的路径！");
                            return;
                        }


                        MessageBox.Show("创建成功");

                    }
                    else
                    {
                        try
                        {
                            using (var file = System.IO.File.Create(this.SavePosition.Text + @"\" + this.TableName.Text + this.TableName.Text + DateTime.Now.Second.ToString() + ".sql"))
                            {

                                using (StreamWriter sw = new StreamWriter(file))
                                {
                                    sw.Write(sb);
                                    sw.Flush();
                                }

                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("文件夹路径错误，请输入正确的路径！");
                            return;
                        }

                        MessageBox.Show("创建成功");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("创建失败");
                    throw;
                }
            }
        }
    }
}