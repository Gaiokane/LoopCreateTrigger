using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoopCreateTrigger
{
    public partial class Form1 : Form
    {
        public TreeNode tn;
        public TreeView tv;
        public string databasename = "NULL";

        private Point pi;

        public string tablename;

        private string sqlGetTableStructureForMSSQL;
        private string sqlGetTableStructureForMySQL;

        //public SqlConnection mssqlconn;
        //public MySqlConnection mysqlconn;

        public string sqltype;


        string host, port, database, username, password, sqlconn;
        SqlConnection mssqlconn;
        MySqlConnection mysqlconn;

        #region 获取数据库名、表名、视图名字段
        string sqlGetDatabasesNameListForMSSQL = "SELECT name AS DATABASE_NAME FROM sysdatabases ORDER BY name;";
        string sqlGetDatabasesNameListForMySQL = "SELECT SCHEMA_NAME AS DATABASE_NAME FROM `information_schema`.`SCHEMATA` ORDER BY SCHEMA_NAME;";
        DataTable dtDatabasesNameList;
        List<string> listDatabasesName;

        string sqlGetDatabaseTablesNameListForMSSQL = "SELECT 2;";
        string sqlGetDatabaseTablesNameListForMySQL = "SELECT 2;";
        DataTable dtDatabaseTablesNameList;
        List<string> listDatabaseTablesName;

        string sqlGetDatabaseViewsNameListForMSSQL = "SELECT 3;";
        string sqlGetDatabaseViewsNameListForMySQL = "SELECT 3;";
        DataTable dtDatabaseViewsNameList;
        List<string> listDatabaseViewsName;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region 设置测试用默认数据库连接
        private void setTestConn()
        {
            radiobtnMYSQL.Checked = true;
            txtboxHost.Text = "127.0.0.1";
            txtboxPort.Text = "3306";
            txtboxDatabase.Text = "pagination";
            txtboxUsername.Text = "qkk";
            txtboxPassword.Text = "qkk";
        }
        #endregion

        #region 与运算，返回结果，0 or 非0
        /// <summary>
        /// 与运算，返回结果，0 or 非0
        /// </summary>
        /// <param name="value">value & num</param>
        /// <param name="num">value & num</param>
        /// <returns></returns>
        private int getAndOperationResult(int value, int num)
        {
            int result = value & num;
            return result;
        }
        #endregion

        #region 获取数据库名，将DataTable中第一列的数据转为List
        /// <summary>
        /// 获取数据库名，将DataTable中第一列的数据转为List
        /// </summary>
        /// <param name="dt">传入的DataTable</param>
        /// <returns>返回List<string></returns>
        public static List<string> DataTableToList(DataTable dt)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i][0].ToString());
            }

            return list;
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

            //MessageBox.Show(getDotLength(1.1).ToString());

            setTestConn();

            #region 读取并设置上一次数据库连接
            #region 替换下面方法，分别读取MSSQL/MySQL最近连接记录
            int isLastConnectionSettingError = 0;
            string LastConnectionType = ConfigSettings.getLastConnectionType();
            string[] LastConnectionStrings;

            #region 判断LastConnectionType、MSSQL的LastConnectionStrings、MySQL的LastConnectionStrings是否有问题，对有问题部分重置为默认值
            if (LastConnectionType == "MSSQL" || LastConnectionType == "MySQL")
            {

            }
            //LastConnectionType问题，值为1
            else
            {
                isLastConnectionSettingError += 1;
            }
            LastConnectionStrings = ConfigSettings.getLastConnectionStrings("MSSQL");
            if (LastConnectionStrings.Length != 7)
            {
                //MSSQL的LastConnectionStrings问题，值为2
                isLastConnectionSettingError += 2;
            }
            LastConnectionStrings = ConfigSettings.getLastConnectionStrings("MySQL");
            if (LastConnectionStrings.Length != 7)
            {
                //MySQL的LastConnectionStrings问题，值为4
                isLastConnectionSettingError += 4;
            }

            //LastConnectionType问题，值为1
            if (getAndOperationResult(isLastConnectionSettingError, 1) != 0)
            {
                ConfigSettings.setLastConnectionType("MySQL");
            }
            //MSSQL的LastConnectionStrings问题，值为2
            if (getAndOperationResult(isLastConnectionSettingError, 2) != 0)
            {
                ConfigSettings.setLastConnectionStrings(0, "docker,98", false, "1433", "qktest", "sa", "Qwer1234!");
            }
            //MySQL的LastConnectionStrings问题，值为4
            if (getAndOperationResult(isLastConnectionSettingError, 4) != 0)
            {
                ConfigSettings.setLastConnectionStrings(1, "127.0.0.1", true, "3306", "pagination", "qkk", "qkk");
            }
            if (isLastConnectionSettingError != 0)
            {
                MessageBox.Show("最新数据库连接值不正确，已重置为默认值，请重新运行该程序！");
                Application.Exit();
            }
            #endregion

            LastConnectionStrings = ConfigSettings.getLastConnectionStrings(LastConnectionType);
            bool isPort = false;
            if (LastConnectionStrings[2] == "True")
            {
                isPort = true;
            }
            else if (LastConnectionStrings[2] == "False")
            {
                isPort = false;
            }
            else
            {
                isPort = false;
            }
            //int sqlType, string Host, bool isPort, string Port, string Database, string Username, string Password
            if (LastConnectionStrings[0] == "0")
            {
                radiobtnMSSQL.Checked = true;
                txtboxHost.Text = LastConnectionStrings[1];
                chkboxPort.Checked = isPort;
                txtboxPort.Text = LastConnectionStrings[3];
                txtboxDatabase.Text = LastConnectionStrings[4];
                txtboxUsername.Text = LastConnectionStrings[5];
                txtboxPassword.Text = LastConnectionStrings[6];
            }
            else if (LastConnectionStrings[0] == "1")
            {
                radiobtnMYSQL.Checked = true;
                txtboxHost.Text = LastConnectionStrings[1];
                chkboxPort.Checked = isPort;
                txtboxPort.Text = LastConnectionStrings[3];
                txtboxDatabase.Text = LastConnectionStrings[4];
                txtboxUsername.Text = LastConnectionStrings[5];
                txtboxPassword.Text = LastConnectionStrings[6];
            }
            else
            {
                setTestConn();
            }
            #endregion

            #endregion

            //快捷插入配置
            ConfigSettings.getQuickInsertSettingsByappSettings();
            ConfigSettings.setDefaultQuickInsertSettingsIfIsNullOrEmptyByappSettings();
            ConfigSettings.getQuickInsertSettingsByappSettings();

            //常用SQL配置
            ConfigSettings.getCommonlyUsedSQLByappSettings();
            ConfigSettings.setDefaultCommonlyUsedSQLIfIsNullOrEmptyByappSettings();
            ConfigSettings.getCommonlyUsedSQLByappSettings();


        }

        private void radiobtnMSSQL_CheckedChanged(object sender, EventArgs e)
        {
            int isLastConnectionSettingError = 0;
            string[] LastConnectionStrings;
            string radiobtn;
            if (radiobtnMSSQL.Checked == true)
            {
                radiobtn = "MSSQL";
            }
            else
            {
                radiobtn = "MySQL";
            }

            #region 判断MSSQL的LastConnectionStrings、MySQL的LastConnectionStrings是否有问题，对有问题部分重置为默认值
            LastConnectionStrings = ConfigSettings.getLastConnectionStrings(radiobtn);
            if (radiobtn == "MSSQL" && LastConnectionStrings.Length != 7)
            {
                //MSSQL的LastConnectionStrings问题，值为2
                isLastConnectionSettingError += 2;
            }
            if (radiobtn == "MySQL" && LastConnectionStrings.Length != 7)
            {
                //MySQL的LastConnectionStrings问题，值为4
                isLastConnectionSettingError += 4;
            }

            //MSSQL的LastConnectionStrings问题，值为2
            if (getAndOperationResult(isLastConnectionSettingError, 2) != 0)
            {
                ConfigSettings.setLastConnectionStrings(0, "127.0.0.1", false, "1433", "qktest", "sa", "11111");
            }
            //MySQL的LastConnectionStrings问题，值为4
            if (getAndOperationResult(isLastConnectionSettingError, 4) != 0)
            {
                ConfigSettings.setLastConnectionStrings(1, "127.0.0.1", true, "3306", "pagination", "qkk", "qkk");
            }
            if (isLastConnectionSettingError != 0)
            {
                MessageBox.Show("最新数据库连接值不正确，已重置为默认值，请重新运行该程序！");
                Application.Exit();
            }
            #endregion

            bool isPort = false;
            if (LastConnectionStrings[2] == "True")
            {
                isPort = true;
            }
            else if (LastConnectionStrings[2] == "False")
            {
                isPort = false;
            }
            else
            {
                isPort = false;
            }
            //int sqlType, string Host, bool isPort, string Port, string Database, string Username, string Password
            if (LastConnectionStrings[0] == "0")
            {
                radiobtnMSSQL.Checked = true;
                txtboxHost.Text = LastConnectionStrings[1];
                chkboxPort.Checked = isPort;
                txtboxPort.Text = LastConnectionStrings[3];
                txtboxDatabase.Text = LastConnectionStrings[4];
                txtboxUsername.Text = LastConnectionStrings[5];
                txtboxPassword.Text = LastConnectionStrings[6];
            }
            else if (LastConnectionStrings[0] == "1")
            {
                radiobtnMYSQL.Checked = true;
                txtboxHost.Text = LastConnectionStrings[1];
                chkboxPort.Checked = isPort;
                txtboxPort.Text = LastConnectionStrings[3];
                txtboxDatabase.Text = LastConnectionStrings[4];
                txtboxUsername.Text = LastConnectionStrings[5];
                txtboxPassword.Text = LastConnectionStrings[6];
            }
            else
            {
                setTestConn();
            }
        }

        #region 数据库_连接按钮单击事件
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtboxHost.Text))
            {
                MessageBox.Show("Host不能为空！");
                txtboxHost.Focus();
            }
            else
            {
                if (chkboxPort.Checked == true && string.IsNullOrEmpty(txtboxPort.Text))
                {
                    MessageBox.Show("Port不能为空！");
                    txtboxPort.Focus();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtboxDatabase.Text))
                    {
                        MessageBox.Show("Database不能为空！");
                        txtboxDatabase.Focus();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtboxUsername.Text))
                        {
                            MessageBox.Show("Username不能为空！");
                            txtboxUsername.Focus();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(txtboxPassword.Text))
                            {
                                MessageBox.Show("Password不能为空！");
                                txtboxPassword.Focus();
                            }
                            else
                            {
                                host = txtboxHost.Text.Trim();
                                port = txtboxPort.Text.Trim();
                                database = txtboxDatabase.Text.Trim();
                                username = txtboxUsername.Text.Trim();
                                password = txtboxPassword.Text.Trim();

                                #region 使用MSSQL
                                if (radiobtnMSSQL.Checked == true)
                                {
                                    sqlconn = string.Empty;
                                    if (chkboxPort.Checked == true)//指定端口
                                    {
                                        sqlconn = "server=" + host + "," + port + "; database=" + database + "; uid=" + username + "; pwd=" + password + "";
                                    }
                                    else//不指定端口
                                    {
                                        sqlconn = "server=" + host + "; database=" + database + "; uid=" + username + "; pwd=" + password + "";
                                    }
                                    try
                                    {
                                        mssqlconn = new SqlConnection(sqlconn);
                                        mssqlconn.Open();
                                        //MessageBox.Show(mssqlconn.State.ToString());//Open
                                        if (mssqlconn.State == ConnectionState.Open)
                                        {
                                            labConnectStatus.Text = "状态：已连接";
                                            btnConnect.Enabled = false;
                                            radiobtnMSSQL.Enabled = false;
                                            radiobtnMYSQL.Enabled = false;
                                            txtboxHost.Enabled = false;
                                            chkboxPort.Enabled = false;
                                            txtboxPort.Enabled = false;
                                            txtboxDatabase.Enabled = false;
                                            txtboxUsername.Enabled = false;
                                            txtboxPassword.Enabled = false;
                                            btnShowDatabases.Enabled = false;
                                        }

                                        //设置最后连接数据库类型
                                        if (ConfigSettings.setLastConnectionType("MSSQL") == false)
                                        {
                                            MessageBox.Show("更新最后连接数据库类型出错！");
                                        }

                                        //设置上一次连接字符串
                                        if (ConfigSettings.setLastConnectionStrings(0, host, chkboxPort.Checked, port, database, username, password) == false)
                                        {
                                            MessageBox.Show("更新最后连接字符串出错！");
                                        }

                                        //保存连接成功的记录
                                        if (ConfigSettings.saveConnectionString(0, host, chkboxPort.Checked, port, database, username, password) == false)
                                        {
                                            MessageBox.Show("保存连接记录出错！");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                #endregion
                                #region 使用MYSQL
                                if (radiobtnMYSQL.Checked == true)
                                {
                                    sqlconn = string.Empty;
                                    if (chkboxPort.Checked == true)//指定端口
                                    {
                                        sqlconn = "Host = " + host + "; Port = " + port + "; Database = " + database + "; Username = " + username + "; Password = " + password + "";
                                    }
                                    else//不指定端口
                                    {
                                        sqlconn = "Host = " + host + "; Database = " + database + "; Username = " + username + "; Password = " + password + "";
                                    }
                                    try
                                    {
                                        mysqlconn = new MySqlConnection(sqlconn);
                                        mysqlconn.Open();
                                        //MessageBox.Show(mysqlconn.ConnectionTimeout.ToString());
                                        //MessageBox.Show(mysqlconn.State.ToString());//Open
                                        if (mysqlconn.State == ConnectionState.Open)
                                        {
                                            labConnectStatus.Text = "状态：已连接";
                                            btnConnect.Enabled = false;
                                            radiobtnMSSQL.Enabled = false;
                                            radiobtnMYSQL.Enabled = false;
                                            txtboxHost.Enabled = false;
                                            chkboxPort.Enabled = false;
                                            txtboxPort.Enabled = false;
                                            txtboxDatabase.Enabled = false;
                                            txtboxUsername.Enabled = false;
                                            txtboxPassword.Enabled = false;
                                            btnShowDatabases.Enabled = false;
                                        }

                                        //设置最后连接数据库类型
                                        if (ConfigSettings.setLastConnectionType("MySQL") == false)
                                        {
                                            MessageBox.Show("更新最后连接数据库类型出错！");
                                        }

                                        //设置上一次连接字符串
                                        if (ConfigSettings.setLastConnectionStrings(1, host, chkboxPort.Checked, port, database, username, password) == false)
                                        {
                                            MessageBox.Show("更新最后连接字符串出错！");
                                        }

                                        //保存连接成功的记录
                                        if (ConfigSettings.saveConnectionString(1, host, chkboxPort.Checked, port, database, username, password) == false)
                                        {
                                            MessageBox.Show("保存连接记录出错！");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 数据库_断开按钮单击事件
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (labConnectStatus.Text == "状态：已断开")
            {

            }
            else
            {
                if (radiobtnMSSQL.Checked == true)
                {
                    mssqlconn.Close();
                    labConnectStatus.Text = "状态：已断开";
                    btnConnect.Enabled = true;
                    radiobtnMSSQL.Enabled = true;
                    radiobtnMYSQL.Enabled = true;
                    txtboxHost.Enabled = true;
                    chkboxPort.Enabled = true;
                    txtboxPort.Enabled = true;
                    txtboxDatabase.Enabled = true;
                    txtboxUsername.Enabled = true;
                    txtboxPassword.Enabled = true;
                    btnShowDatabases.Enabled = true;
                }
                if (radiobtnMYSQL.Checked == true)
                {
                    mysqlconn.Close();
                    labConnectStatus.Text = "状态：已断开";
                    btnConnect.Enabled = true;
                    radiobtnMSSQL.Enabled = true;
                    radiobtnMYSQL.Enabled = true;
                    txtboxHost.Enabled = true;
                    chkboxPort.Enabled = true;
                    txtboxPort.Enabled = true;
                    txtboxDatabase.Enabled = true;
                    txtboxUsername.Enabled = true;
                    txtboxPassword.Enabled = true;
                    btnShowDatabases.Enabled = true;
                }
            }
        }
        #endregion

        #region 数据库_获取数据库列表按钮单击事件 获取服务器数据库名列表，双击可以快速填充到Database文本框中
        private void btnShowDatabases_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtboxHost.Text))
            {
                MessageBox.Show("Host不能为空！");
                txtboxHost.Focus();
            }
            else
            {
                if (chkboxPort.Checked == true && string.IsNullOrEmpty(txtboxPort.Text))
                {
                    MessageBox.Show("Port不能为空！");
                    txtboxPort.Focus();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtboxDatabase.Text))
                    {
                        MessageBox.Show("Database不能为空！");
                        txtboxDatabase.Focus();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtboxUsername.Text))
                        {
                            MessageBox.Show("Username不能为空！");
                            txtboxUsername.Focus();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(txtboxPassword.Text))
                            {
                                MessageBox.Show("Password不能为空！");
                                txtboxPassword.Focus();
                            }
                            else
                            {
                                host = txtboxHost.Text.Trim();
                                port = txtboxPort.Text.Trim();
                                username = txtboxUsername.Text.Trim();
                                password = txtboxPassword.Text.Trim();

                                #region 使用MSSQL
                                if (radiobtnMSSQL.Checked == true)
                                {
                                    sqlconn = string.Empty;
                                    database = "master";
                                    if (chkboxPort.Checked == true)//指定端口
                                    {
                                        sqlconn = "server=" + host + "," + port + "; database=" + database + "; uid=" + username + "; pwd=" + password + "";
                                    }
                                    else//不指定端口
                                    {
                                        sqlconn = "server=" + host + "; database=" + database + "; uid=" + username + "; pwd=" + password + "";
                                    }
                                    try
                                    {
                                        mssqlconn = new SqlConnection(sqlconn);
                                        mssqlconn.Open();
                                        if (mssqlconn.State == ConnectionState.Open)
                                        {
                                            FrmDatabasesNameList fdnl = new FrmDatabasesNameList();

                                            fdnl.txtboxdatabase = txtboxDatabase;

                                            //获取当前用户有权限的数据库名DataTable
                                            dtDatabasesNameList = SqlHelper.getDataSetMSSQL(sqlGetDatabasesNameListForMSSQL, mssqlconn).Tables[0];
                                            //将数据库名存到list
                                            listDatabasesName = DataTableToList(dtDatabasesNameList);
                                            fdnl.listdatabasesname = listDatabasesName;

                                            //设置只能打开一个，配合FrmDatabasesNameList中的GetFrmDatabasesNameList()设置
                                            FrmDatabasesNameList.GetFrmDatabasesNameList().Activate();

                                            //接收FrmDatabasesNameList返回的DialogResult，可自定义操作
                                            if (fdnl.ShowDialog() == DialogResult.OK)
                                            {

                                            }

                                            mssqlconn.Close();
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                #endregion
                                #region 使用MYSQL
                                if (radiobtnMYSQL.Checked == true)
                                {
                                    sqlconn = string.Empty;
                                    database = "information_schema";
                                    if (chkboxPort.Checked == true)//指定端口
                                    {
                                        sqlconn = "Host = " + host + "; Port = " + port + "; Database = " + database + "; Username = " + username + "; Password = " + password + "";
                                    }
                                    else//不指定端口
                                    {
                                        sqlconn = "Host = " + host + "; Database = " + database + "; Username = " + username + "; Password = " + password + "";
                                    }
                                    try
                                    {
                                        mysqlconn = new MySqlConnection(sqlconn);
                                        mysqlconn.Open();
                                        if (mysqlconn.State == ConnectionState.Open)
                                        {
                                            FrmDatabasesNameList fdnl = new FrmDatabasesNameList();

                                            fdnl.txtboxdatabase = txtboxDatabase;

                                            //获取当前用户有权限的数据库名DataTable
                                            dtDatabasesNameList = SqlHelper.getDataSetMySQL(sqlGetDatabasesNameListForMySQL, mysqlconn).Tables[0];
                                            //将数据库名存到list
                                            listDatabasesName = DataTableToList(dtDatabasesNameList);
                                            fdnl.listdatabasesname = listDatabasesName;

                                            //设置只能打开一个，配合FrmDatabasesNameList中的GetFrmDatabasesNameList()设置
                                            FrmDatabasesNameList.GetFrmDatabasesNameList().Activate();

                                            //接收FrmDatabasesNameList返回的DialogResult，可自定义操作
                                            if (fdnl.ShowDialog() == DialogResult.OK)
                                            {

                                            }

                                            mysqlconn.Close();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 查看表结构按钮单击事件
        private void btn_SQLTableStructure_Click(object sender, EventArgs e)
        {
            if (labConnectStatus.Text == "状态：已连接")
            {
                //MessageBox.Show(GetsqlGetDatabaseTablesNameListForMSSQLORMySQL("MySQL", txtboxDatabase.Text.Trim()));
                //MessageBox.Show(GetsqlGetDatabaseTablesNameListForMSSQLORMySQL("MSSQL", txtboxDatabase.Text.Trim()));

                //FrmSQLTableStructure fsqlts = new FrmSQLTableStructure();
                //fsqlts.databasename = txtboxDatabase.Text.Trim();
                databasename = txtboxDatabase.Text.Trim();

                TreeNode tn = new TreeNode();
                tn.Text = txtboxDatabase.Text.Trim();

                /*节点测试
                tn.Nodes.Add("节点1");
                tn.Nodes.Add("节点2");
                tn.Nodes[0].Nodes.Add("节点1的子节点1");
                tn.Nodes[0].Nodes[0].Nodes.Add("节点1的子节点1的子子节点1");
                tn.Nodes[1].Nodes.Add("节点2的子节点1");
                tn.Nodes[1].Nodes.Add("节点2的子节点2");
                */

                #region 使用MSSQL
                if (radiobtnMSSQL.Checked == true)
                {
                    sqlGetDatabaseTablesNameListForMSSQL = GetsqlGetDatabaseTablesNameListForMSSQLORMySQL("MSSQL", "TABLE", txtboxDatabase.Text.Trim());
                    sqlGetDatabaseViewsNameListForMSSQL = GetsqlGetDatabaseTablesNameListForMSSQLORMySQL("MSSQL", "VIEW", txtboxDatabase.Text.Trim());

                    try
                    {
                        //SqlHelper中把conn.close都去掉了
                        //处理单击树节点显示表结构后连接被关闭
                        /*if (mssqlconn.State == ConnectionState.Closed)
                        {
                            mssqlconn.Open();
                        }*/
                        if (mssqlconn.State == ConnectionState.Open)
                        {
                            //获取当前数据库下的表名
                            dtDatabaseTablesNameList = SqlHelper.getDataSetMSSQL(sqlGetDatabaseTablesNameListForMSSQL, mssqlconn).Tables[0];
                            //mssqlconn.Open();
                            //将表名存到list
                            listDatabaseTablesName = DataTableToList(dtDatabaseTablesNameList);
                            tn.Nodes.Add("表");
                            foreach (var item in listDatabaseTablesName)
                            {
                                tn.Nodes[0].Nodes.Add(item);
                            }

                            //获取当前数据库下的视图名
                            dtDatabaseViewsNameList = SqlHelper.getDataSetMSSQL(sqlGetDatabaseViewsNameListForMSSQL, mssqlconn).Tables[0];
                            //mssqlconn.Open();
                            //将表名存到list
                            listDatabaseViewsName = DataTableToList(dtDatabaseViewsNameList);
                            tn.Nodes.Add("视图");
                            foreach (var item in listDatabaseViewsName)
                            {
                                tn.Nodes[1].Nodes.Add(item);
                            }

                            //fsqlts.tn = tn;
                            //fsqlts.mssqlconn = mssqlconn;
                            //fsqlts.sqltype = "MSSQL";
                            this.tn = tn;
                            this.mssqlconn = mssqlconn;
                            this.sqltype = "MSSQL";

                            treeView1.Nodes.Add(tn);

                            treeView1.ExpandAll();//展开所有树节点
                                                  //treeView1.CollapseAll();//折叠所有树节点
                            treeView1.Nodes[0].EnsureVisible();//垂直滚动条在展开所有节点后回到顶端

                            //用了FrmSQLTableStructure.GetFrmSQLTableStructure().Activate();，就不能用fsqlts.Show();
                            //fsqlts.Show();

                            //设置只能打开一个，配合FrmSQLTableStructure中的GetFrmSQLTableStructure()设置
                            //FrmSQLTableStructure.GetFrmSQLTableStructure().Activate();

                            //接收FrmSQLTableStructure返回的DialogResult，可自定义操作
                            /*if (fsqlts.ShowDialog() == DialogResult.OK)
                            {
                                string str = fsqlts.tablename;
                                int index = richtxtboxInsertSQL.SelectionStart;
                                string s = richtxtboxInsertSQL.Text;
                                s = s.Insert(index, str);
                                richtxtboxInsertSQL.Text = s;
                                richtxtboxInsertSQL.SelectionStart = index + str.Length;
                                richtxtboxInsertSQL.Focus();
                            }*/
                        }
                        else
                        {
                            mssqlconn.Open();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                #endregion
                #region 使用MYSQL
                if (radiobtnMYSQL.Checked == true)
                {
                    sqlGetDatabaseTablesNameListForMySQL = GetsqlGetDatabaseTablesNameListForMSSQLORMySQL("MySQL", "TABLE", txtboxDatabase.Text.Trim());
                    sqlGetDatabaseViewsNameListForMySQL = GetsqlGetDatabaseTablesNameListForMSSQLORMySQL("MySQL", "VIEW", txtboxDatabase.Text.Trim());

                    try
                    {
                        //SqlHelper中把conn.close都去掉了
                        //处理单击树节点显示表结构后连接被关闭
                        /*if (mysqlconn.State == ConnectionState.Closed)
                        {
                            mysqlconn.Open();
                        }*/
                        if (mysqlconn.State == ConnectionState.Open)
                        {
                            //获取当前数据库下的表名
                            dtDatabaseTablesNameList = SqlHelper.getDataSetMySQL(sqlGetDatabaseTablesNameListForMySQL, mysqlconn).Tables[0];
                            //mysqlconn.Open();
                            //将表名存到list
                            listDatabaseTablesName = DataTableToList(dtDatabaseTablesNameList);
                            tn.Nodes.Add("表");
                            foreach (var item in listDatabaseTablesName)
                            {
                                tn.Nodes[0].Nodes.Add(item);
                            }

                            //获取当前数据库下的视图名
                            dtDatabaseViewsNameList = SqlHelper.getDataSetMySQL(sqlGetDatabaseViewsNameListForMySQL, mysqlconn).Tables[0];
                            //mysqlconn.Open();
                            //将表名存到list
                            listDatabaseViewsName = DataTableToList(dtDatabaseViewsNameList);
                            tn.Nodes.Add("视图");
                            foreach (var item in listDatabaseViewsName)
                            {
                                tn.Nodes[1].Nodes.Add(item);
                            }

                            /*fsqlts.tn = tn;
                            fsqlts.mysqlconn = mysqlconn;
                            fsqlts.sqltype = "MySQL";*/
                            this.tn = tn;
                            this.mysqlconn = mysqlconn;
                            this.sqltype = "MySQL";

                            treeView1.Nodes.Add(tn);

                            treeView1.ExpandAll();//展开所有树节点
                                                  //treeView1.CollapseAll();//折叠所有树节点
                            treeView1.Nodes[0].EnsureVisible();//垂直滚动条在展开所有节点后回到顶端

                            //用了FrmSQLTableStructure.GetFrmSQLTableStructure().Activate();，就不能用fsqlts.Show();
                            //fsqlts.Show();

                            //设置只能打开一个，配合FrmSQLTableStructure中的GetFrmSQLTableStructure()设置
                            //FrmSQLTableStructure.GetFrmSQLTableStructure().Activate();

                            //接收FrmSQLTableStructure返回的DialogResult，可自定义操作
                            /*if (fsqlts.ShowDialog() == DialogResult.OK)
                            {
                                //MessageBox.Show(fsqlts.tablename);

                                string str = fsqlts.tablename;
                                int index = richtxtboxInsertSQL.SelectionStart;
                                string s = richtxtboxInsertSQL.Text;
                                s = s.Insert(index, str);
                                richtxtboxInsertSQL.Text = s;
                                richtxtboxInsertSQL.SelectionStart = index + str.Length;
                                richtxtboxInsertSQL.Focus();
                            }*/
                        }
                        else
                        {
                            mysqlconn.Open();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                #endregion
            }
            else
            {
                MessageBox.Show("请先连接数据库！");
                btnConnect.Focus();
            }
        }
        #endregion

        #region 获取MSSQL/MySQL当前所选数据库中的表名/视图名
        /// <summary>
        /// 获取MSSQL/MySQL当前所选数据库中的表名/视图名
        /// </summary>
        /// <param name="sqlType">MSSQL/MySQL</param>
        /// <param name="Type">TABLE/VIEW</param>
        /// <param name="DataBaseName">数据库名</param>
        /// <returns></returns>
        private string GetsqlGetDatabaseTablesNameListForMSSQLORMySQL(string sqlType, string Type, string DataBaseName)
        {
            string result = "SELECT 1;";

            if (string.IsNullOrEmpty(sqlType) == false && string.IsNullOrEmpty(DataBaseName) == false)
            {
                //MSSQL
                if (sqlType == "MSSQL")
                {
                    //表
                    if (Type == "TABLE")
                    {
                        result = "USE [" + DataBaseName + "];SELECT name FROM sysobjects WHERE xtype = 'U' ORDER BY name;";
                        return result;
                    }
                    //视图
                    if (Type == "VIEW")
                    {
                        result = "USE [" + DataBaseName + "];SELECT name FROM sysobjects WHERE xtype = 'V' ORDER BY name;";
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
                //MySQL
                if (sqlType == "MySQL")
                {
                    //表
                    if (Type == "TABLE")
                    {
                        result = "SELECT TABLE_NAME FROM `information_schema`.`TABLES` WHERE TABLE_SCHEMA = '" + DataBaseName + "' AND TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME;";
                        return result;
                    }
                    //视图
                    if (Type == "VIEW")
                    {
                        result = "SELECT TABLE_NAME FROM `information_schema`.`TABLES` WHERE TABLE_SCHEMA = '" + DataBaseName + "' AND TABLE_TYPE = 'VIEW' ORDER BY TABLE_NAME;";
                        //result = "SELECT TABLE_NAME FROM `information_schema`.`VIEWS` WHERE TABLE_SCHEMA = '" + DataBaseName + "' ORDER BY TABLE_NAME;";
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }
        #endregion

        /**************************************************************************************************************************************************************/


        #region treeview获取鼠标坐标 用来判断是否选中节点
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            pi = new Point(e.X, e.Y);
        }
        #endregion

        #region treeview双击事件 如果双击的是节点，传值到insert语句文本框光标处
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = this.treeView1.GetNodeAt(pi);
            //获取深度，0：数据库名；1：表/视图；2：表名/视图名
            //MessageBox.Show(node.Level.ToString());
            if (node.Level == 2)
            {
                if (pi.X < node.Bounds.Left || pi.X > node.Bounds.Right)
                {
                    //不触发事件

                    //txtboxdatabase.Text = "no selected";
                    //this.Close();

                    return;
                }
                else
                {
                    //触发事件

                    //MessageBox.Show(treeView1.SelectedNode.Text);
                    tablename = treeView1.SelectedNode.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
        #endregion

        #region 设置该窗口只能打开一个，配合按钮设置
        /*private static FrmSQLTableStructure fsts = new FrmSQLTableStructure();
        public static FrmSQLTableStructure GetFrmSQLTableStructure()
        {
            if (fsts.IsDisposed)
            {
                fsts = new FrmSQLTableStructure();
                return fsts;
            }
            else
            {
                return fsts;
            }
        }*/
        #endregion

        #region 窗体关闭时返回一个DialogResult，FrmMain接收返回值
        private void FrmSQLTableStructure_FormClosed(object sender, FormClosedEventArgs e)
        {
            //在这里处理 正常关闭也会返回值 改在treeView1_DoubleClick中处理
            //this.DialogResult = DialogResult.OK;
        }
        #endregion

        #region treeView1按下ESC键关闭当前窗口
        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }
        }
        #endregion

        #region dataGridView1按下ESC键关闭当前窗口
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }
        }
        #endregion

        #region 创建变动日志表
        private void button1_Click(object sender, EventArgs e)
        {
            if (labConnectStatus.Text == "状态：已连接")
            {
                if (treeView1.Nodes.Count != 0)
                {
                    #region 使用MSSQL
                    if (radiobtnMSSQL.Checked == true)
                    {
                        try
                        {
                            if (mssqlconn.State == ConnectionState.Open)
                            {
                                string checkTableExists = "SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[TB_TablesChangeLogs]') AND type IN ('U');";
                                string dropTableTablesChangeLogs = "DROP TABLE [TB_TablesChangeLogs];";
                                string createTableTablesChangeLogs = "CREATE TABLE [TB_TablesChangeLogs](" +
                                                                     "[id] INT IDENTITY NOT NULL," +
                                                                     "[changeDate] datetime DEFAULT (getdate( ) ) NOT NULL," +
                                                                     "[changeType] nvarchar(64) NULL," +
                                                                     "[columnName] nvarchar(64) NULL," +
                                                                     "[oldValue] nvarchar(MAX) NULL," +
                                                                     "[newValue] nvarchar(MAX) NULL," +
                                                                     "[databaseName] nvarchar(255) NULL," +
                                                                     "[schemaName] nvarchar(255) NULL," +
                                                                     "[objectName] nvarchar(255) NULL," +
                                                                     "[hostName] VARCHAR(64) NULL," +
                                                                     "[iPAddress] VARCHAR(32) NULL," +
                                                                     "[programName] nvarchar(255) NULL," +
                                                                     "[loginName] nvarchar(255) NULL," +
                                                                     "PRIMARY KEY CLUSTERED( [id] ));";

                                int result = 0;
                                try
                                {
                                    result = Convert.ToInt32(SqlHelper.getRowsMSSQL(checkTableExists, mssqlconn));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                if (result == 1)
                                {
                                    if (DialogResult.OK == MessageBox.Show("变动日志表TB_TablesChangeLogs已存在，是否删除后重新创建？", "提示", MessageBoxButtons.OKCancel))
                                    {
                                        result = Convert.ToInt32(SqlHelper.getRowsMSSQL(dropTableTablesChangeLogs, mssqlconn));
                                        result = Convert.ToInt32(SqlHelper.getRowsMSSQL(createTableTablesChangeLogs, mssqlconn));
                                        MessageBox.Show("变动日志表TB_TablesChangeLogs创建成功！");
                                    }
                                    else
                                    {
                                        MessageBox.Show("取消操作");
                                    }
                                }
                                else
                                {
                                    result = Convert.ToInt32(SqlHelper.getRowsMSSQL(createTableTablesChangeLogs, mssqlconn));
                                    MessageBox.Show("变动日志表TB_TablesChangeLogs创建成功！");
                                }
                            }
                            else
                            {
                                mssqlconn.Open();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                    #region 使用MYSQL
                    if (radiobtnMYSQL.Checked == true)
                    {
                        try
                        {
                            if (mysqlconn.State == ConnectionState.Open)
                            {
                                string checkTableExists = "SELECT * FROM information_schema.TABLES WHERE TABLE_SCHEMA='" + txtboxDatabase.Text.Trim() + "' AND table_name='TB_TablesChangeLogs';";
                                string dropTableTablesChangeLogs = "DROP TABLE `" + txtboxDatabase.Text.Trim() + "`.`TB_TablesChangeLogs`;";
                                string createTableTablesChangeLogs = "CREATE TABLE `" + txtboxDatabase.Text.Trim() + "`.`TB_TablesChangeLogs`  (" +
                                                     "`id` int NOT NULL AUTO_INCREMENT," +
                                                     "`changeDate` datetime NULL COMMENT '变动时间'," +
                                                     "`changeType` varchar(255) NULL COMMENT '变动类型'," +
                                                     "`columnName` varchar(255) NULL COMMENT '变动列名'," +
                                                     "`oldValue` varchar(255) NULL COMMENT '原始值'," +
                                                     "`newValue` varchar(255) NULL COMMENT '更新值'," +
                                                     "`databaseName` varchar(255) NULL COMMENT '数据库名'," +
                                                     "`schemaName` varchar(255) NULL COMMENT '模式名'," +
                                                     "`objectName` varchar(255) NULL COMMENT '表名'," +
                                                     "`hostName` varchar(255) NULL COMMENT '主机名'," +
                                                     "`iPAddress` varchar(255) NULL COMMENT 'ip'," +
                                                     "`programName` varchar(255) NULL COMMENT '程序名'," +
                                                     "`loginName` varchar(255) NULL COMMENT '登录名'," +
                                                     "PRIMARY KEY(`id`));";

                                int result = 0;
                                try
                                {
                                    result = Convert.ToInt32(SqlHelper.getRowsMySQL(checkTableExists, mysqlconn));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                if (result == 1)
                                {
                                    if (DialogResult.OK == MessageBox.Show("变动日志表TB_TablesChangeLogs已存在，是否删除后重新创建？", "提示", MessageBoxButtons.OKCancel))
                                    {
                                        result = Convert.ToInt32(SqlHelper.getRowsMySQL(dropTableTablesChangeLogs, mysqlconn));
                                        result = Convert.ToInt32(SqlHelper.getRowsMySQL(createTableTablesChangeLogs, mysqlconn));
                                        MessageBox.Show("变动日志表TB_TablesChangeLogs创建成功！");
                                    }
                                    else
                                    {
                                        MessageBox.Show("取消操作");
                                    }
                                }
                                else
                                {
                                    result = Convert.ToInt32(SqlHelper.getRowsMySQL(createTableTablesChangeLogs, mysqlconn));
                                    MessageBox.Show("变动日志表TB_TablesChangeLogs创建成功！");
                                }
                            }
                            else
                            {
                                mysqlconn.Open();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("请先查看表结构！");
                    btn_SQLTableStructure.Focus();
                }
            }
            else
            {
                MessageBox.Show("请先连接数据库！");
                btnConnect.Focus();
            }
        }
        #endregion

        #region 对库中所有表批量建触发器
        private void button2_Click(object sender, EventArgs e)
        {
            if (labConnectStatus.Text == "状态：已连接")
            {
                if (treeView1.Nodes.Count != 0)
                {
                    #region 使用MSSQL
                    if (radiobtnMSSQL.Checked == true)
                    {
                        try
                        {
                            //SqlHelper中把conn.close都去掉了
                            //处理单击树节点显示表结构后连接被关闭
                            /*if (mssqlconn.State == ConnectionState.Closed)
                            {
                                mssqlconn.Open();
                            }*/
                            if (mssqlconn.State == ConnectionState.Open)
                            {
                                //获取当前数据库下的表名
                                dtDatabaseTablesNameList = SqlHelper.getDataSetMSSQL(sqlGetDatabaseTablesNameListForMSSQL, mssqlconn).Tables[0];
                                //mssqlconn.Open();
                                //将表名存到list
                                listDatabaseTablesName = DataTableToList(dtDatabaseTablesNameList);

                                int result = 0;
                                foreach (var item in listDatabaseTablesName)
                                {
                                    string getTableFirstColumnNameSQL = "SELECT TOP 1 [ColumnName] = [Columns].name FROM sys.tables AS [Tables] INNER JOIN sys.columns AS [Columns] ON [Tables].object_id = [Columns].object_id WHERE [Tables].name = '" + item + "' ORDER BY [Columns].column_id;";
                                    string tableFirstColumnName = SqlHelper.getResultMSSQL(getTableFirstColumnNameSQL, mssqlconn).ToString();
                                    //MessageBox.Show(item.ToString());

                                    //MessageBox.Show(getTriggerInsertSQL(item));
                                    //MessageBox.Show(getTriggerUpdateSQL(item));
                                    //MessageBox.Show(getTriggerDeleteSQL(item));

                                    try
                                    {
                                        //string reEXISTS = getTriggerInsertSQLEXISTS(item);
                                        //string re = getTriggerInsertSQL(item);
                                        //MessageBox.Show(re);
                                        //Clipboard.SetText(re);
                                        result += getAffectRowsMSSQL(getTriggerInsertSQLEXISTS(item), mssqlconn);
                                        result += getAffectRowsMSSQL(getTriggerInsertSQL(item, tableFirstColumnName), mssqlconn);
                                        result += getAffectRowsMSSQL(getTriggerUpdateSQLEXISTS(item), mssqlconn);
                                        result += getAffectRowsMSSQL(getTriggerUpdateSQL(item, tableFirstColumnName), mssqlconn);
                                        result += getAffectRowsMSSQL(getTriggerDeleteSQLEXISTS(item), mssqlconn);
                                        result += getAffectRowsMSSQL(getTriggerDeleteSQL(item, tableFirstColumnName), mssqlconn);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                if (result <= 0)
                                {
                                    MessageBox.Show("OK");

                                }
                                else
                                {
                                    MessageBox.Show("error");

                                }

                                //Clipboard.SetText(triggerInsertSQL);//复制内容到剪切板



                            }
                            else
                            {
                                mssqlconn.Open();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                    #region 使用MYSQL（语句未完成，未添加取每个表第一个字段功能）
                    if (radiobtnMYSQL.Checked == true)
                    {
                        try
                        {
                            //SqlHelper中把conn.close都去掉了
                            //处理单击树节点显示表结构后连接被关闭
                            /*if (mysqlconn.State == ConnectionState.Closed)
                            {
                                mysqlconn.Open();
                            }*/
                            if (mysqlconn.State == ConnectionState.Open)
                            {
                                //获取当前数据库下的表名
                                dtDatabaseTablesNameList = SqlHelper.getDataSetMySQL(sqlGetDatabaseTablesNameListForMySQL, mysqlconn).Tables[0];
                                //mysqlconn.Open();
                                //将表名存到list
                                listDatabaseTablesName = DataTableToList(dtDatabaseTablesNameList);

                                int result = 0;
                                foreach (var item in listDatabaseTablesName)
                                {
                                    //MessageBox.Show(item.ToString());

                                    //MessageBox.Show(getTriggerInsertSQL(item));
                                    //MessageBox.Show(getTriggerUpdateSQL(item));
                                    //MessageBox.Show(getTriggerDeleteSQL(item));

                                    try
                                    {
                                        //string reEXISTS = getTriggerInsertSQLEXISTS(item);
                                        //string re = getTriggerInsertSQL(item);
                                        //MessageBox.Show(re);
                                        //Clipboard.SetText(re);
                                        result += getAffectRowsMySQL(getTriggerInsertSQLEXISTS(item), mysqlconn);
                                        result += getAffectRowsMySQL(getTriggerInsertSQL(item, ""), mysqlconn);
                                        result += getAffectRowsMySQL(getTriggerUpdateSQLEXISTS(item), mysqlconn);
                                        result += getAffectRowsMySQL(getTriggerUpdateSQL(item, ""), mysqlconn);
                                        result += getAffectRowsMySQL(getTriggerDeleteSQLEXISTS(item), mysqlconn);
                                        result += getAffectRowsMySQL(getTriggerDeleteSQL(item, ""), mysqlconn);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                if (result <= 0)
                                {
                                    MessageBox.Show("OK");

                                }
                                else
                                {
                                    MessageBox.Show("error");

                                }

                                //Clipboard.SetText(triggerInsertSQL);//复制内容到剪切板



                            }
                            else
                            {
                                mysqlconn.Open();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("请先查看表结构！");
                    btn_SQLTableStructure.Focus();
                }
            }
            else
            {
                MessageBox.Show("请先连接数据库！");
                btnConnect.Focus();
            }
        }
        #endregion




        /// <summary>
        /// 传入SQL，返回该命令所影响行数，其他类型语句（建表）、回滚，返回值为-1
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="SQLConn">SqlConnection连接</param>
        /// <returns></returns>
        public int getAffectRowsMSSQL(string Query, SqlConnection SQLConn)
        {
            try
            {
                SqlCommand comm = new SqlCommand(Query, SQLConn);

                int result = comm.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //SQLConn.Close();
            }
        }

        /// <summary>
        /// 传入SQL，返回该命令所影响行数，其他类型语句（建表）、回滚，返回值为-1
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="MySQLConn">MySqlConnection连接</param>
        /// <returns></returns>
        public int getAffectRowsMySQL(string Query, MySqlConnection MySQLConn)
        {
            try
            {
                MySqlCommand comm = new MySqlCommand(Query, MySQLConn);

                int result = comm.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //MySQLConn.Close();
            }
        }
        private string getTriggerInsertSQLEXISTS(string tableName)
        {
            string oldValue = "TB_Users";
            string triggerInsertSQL = "SELECT 1";
            if (this.sqltype == "MSSQL")
            {
                triggerInsertSQL = "IF \n" +
                "EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = object_id( N'[dbo].[tr_TB_Users_i]' ) AND OBJECTPROPERTY( id, N'IsTrigger' ) = 1 ) DROP TRIGGER tr_TB_Users_i  \n";
            }
            if (this.sqltype == "MySQL")
            {
                triggerInsertSQL = "DROP TRIGGER IF EXISTS `tr_TB_Users_i`;  \n";
            }

            return triggerInsertSQL.Replace(oldValue, tableName);
        }
        private string getTriggerInsertSQL(string tableName, string tableFirstColumnName)
        {
            string oldValue = "TB_Users";
            string triggerInsertSQL = "SELECT 1";
            if (this.sqltype == "MSSQL")
            {
                triggerInsertSQL = //"IF \n" +
                //"EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = object_id( N'[dbo].[tr_TB_Users_i]' ) AND OBJECTPROPERTY( id, N'IsTrigger' ) = 1 ) DROP TRIGGER tr_TB_Users_i  \n" +
                //"GO \n" +
                "CREATE TRIGGER tr_TB_Users_i ON [dbo].[TB_Users] AFTER INSERT AS \n" +
                "IF \n" +
                "@@ROWCOUNT = 0 \n" +
                "RETURN \n" +
                "DECLARE \n" +
                "@ip VARCHAR ( 32 ) = ( SELECT client_net_address FROM sys.dm_exec_connections WHERE session_id = @@SPID ); \n" +
                "DECLARE \n" +
                "@schemaName VARCHAR ( 255 ) = ( SELECT OBJECT_SCHEMA_NAME( parent_id ) FROM sys.triggers WHERE object_id = @@PROCID ); \n" +
                "DECLARE \n" +
                "@objectName VARCHAR ( 255 ) = ( SELECT OBJECT_NAME( parent_id ) tableName FROM sys.triggers WHERE object_id = @@PROCID ); \n" +
                "DECLARE \n" +
                "@columnName VARCHAR ( 255 ) = ( SELECT column_name FROM information_schema.columns WHERE table_name = 'TB_Users' AND ordinal_position = 1 ); \n" +
                "DECLARE \n" +
                "@newValue VARCHAR ( 255 ) = ( SELECT " + tableFirstColumnName + " FROM inserted ); \n" +
                "INSERT INTO [dbo].[TB_TablesChangeLogs] ( changeType, columnName, oldValue, newValue, databaseName, schemaName, objectName, hostName, iPAddress, programName, loginName ) SELECT \n" +
                "'insert', \n" +
                "@columnName, \n" +
                "'', \n" +
                "@newValue, \n" +
                "DB_NAME( ), \n" +
                "@schemaName, \n" +
                "@objectName, \n" +
                "HOST_NAME( ), \n" +
                "@ip, \n" +
                "PROGRAM_NAME ( ), \n" +
                "SuseR_SNAME( ) \n" +
                "GO ";
            }
            if (this.sqltype == "MySQL")
            {
                triggerInsertSQL = "SELECT 1";
            }

            return triggerInsertSQL.Replace(oldValue, tableName);
        }
        private string getTriggerUpdateSQLEXISTS(string tableName)
        {
            string oldValue = "TB_Users";
            string triggerUpdateSQL = "SELECT 1";
            if (this.sqltype == "MSSQL")
            {
                triggerUpdateSQL = "IF \n" +
                "EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = object_id( N'[dbo].[tr_TB_Users_u]' ) AND OBJECTPROPERTY( id, N'IsTrigger' ) = 1 ) DROP TRIGGER tr_TB_Users_u  \n";
            }
            if (this.sqltype == "MySQL")
            {
                triggerUpdateSQL = "DROP TRIGGER IF EXISTS `tr_TB_Users_u`;  \n";
            }

            return triggerUpdateSQL.Replace(oldValue, tableName);
        }
        private string getTriggerUpdateSQL(string tableName, string tableFirstColumnName)
        {
            string oldValue = "TB_Users";
            string triggerUpdateSQL = "SELECT 1";
            if (this.sqltype == "MSSQL")
            {
                triggerUpdateSQL = //"IF \n" +
                //"EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = object_id( N'[dbo].[tr_TB_Users_u]' ) AND OBJECTPROPERTY( id, N'IsTrigger' ) = 1 ) DROP TRIGGER tr_TB_Users_u  \n" +
                //"GO \n" +
                "CREATE TRIGGER tr_TB_Users_u ON [dbo].[TB_Users] AFTER UPDATE AS \n" +
                "IF \n" +
                "@@ROWCOUNT = 0 \n" +
                "RETURN \n" +
                "DECLARE \n" +
                "@ip VARCHAR ( 32 ) = ( SELECT client_net_address FROM sys.dm_exec_connections WHERE session_id = @@SPID ); \n" +
                "DECLARE \n" +
                "@schemaName VARCHAR ( 255 ) = ( SELECT OBJECT_SCHEMA_NAME( parent_id ) FROM sys.triggers WHERE object_id = @@PROCID ); \n" +
                "DECLARE \n" +
                "@objectName VARCHAR ( 255 ) = ( SELECT OBJECT_NAME( parent_id ) tableName FROM sys.triggers WHERE object_id = @@PROCID ); \n" +
                "DECLARE \n" +
                "@columnName VARCHAR ( 255 ) = ( SELECT column_name FROM information_schema.columns WHERE table_name = 'TB_Users' AND ordinal_position = 1 ); \n" +
                "DECLARE \n" +
                "@oldValue VARCHAR ( 255 ) = ( SELECT " + tableFirstColumnName + " FROM deleted ); \n" +
                "DECLARE \n" +
                "@newValue VARCHAR ( 255 ) = ( SELECT " + tableFirstColumnName + " FROM inserted ); \n" +
                "INSERT INTO [dbo].[TB_TablesChangeLogs] ( changeType, columnName, oldValue, newValue, databaseName, schemaName, objectName, hostName, iPAddress, programName, loginName ) SELECT \n" +
                "'update', \n" +
                "@columnName, \n" +
                "@oldValue, \n" +
                "@newValue, \n" +
                "DB_NAME( ), \n" +
                "@schemaName, \n" +
                "@objectName, \n" +
                "HOST_NAME( ), \n" +
                "@ip, \n" +
                "PROGRAM_NAME ( ), \n" +
                "SuseR_SNAME( ) \n" +
                "GO ";
            }
            if (this.sqltype == "MySQL")
            {
                triggerUpdateSQL = "SELECT 1";
            }

            return triggerUpdateSQL.Replace(oldValue, tableName);
        }
        private string getTriggerDeleteSQLEXISTS(string tableName)
        {
            string oldValue = "TB_Users";
            string triggerDeleteSQL = "SELECT 1";
            if (this.sqltype == "MSSQL")
            {
                triggerDeleteSQL = "IF \n" +
                "	EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = object_id( N'[dbo].[tr_TB_Users_d]' ) AND OBJECTPROPERTY( id, N'IsTrigger' ) = 1 ) DROP TRIGGER tr_TB_Users_d  \n";
            }
            if (this.sqltype == "MySQL")
            {
                triggerDeleteSQL = "DROP TRIGGER IF EXISTS `tr_TB_Users_d`;  \n";
            }

            return triggerDeleteSQL.Replace(oldValue, tableName);
        }
        private string getTriggerDeleteSQL(string tableName, string tableFirstColumnName)
        {
            string oldValue = "TB_Users";
            string triggerDeleteSQL = "SELECT 1";
            if (this.sqltype == "MSSQL")
            {
                triggerDeleteSQL = //"IF \n" +
                //"	EXISTS ( SELECT * FROM dbo.sysobjects WHERE id = object_id( N'[dbo].[tr_TB_Users_d]' ) AND OBJECTPROPERTY( id, N'IsTrigger' ) = 1 ) DROP TRIGGER tr_TB_Users_d  \n" +
                //"GO \n" +
                "	CREATE TRIGGER tr_TB_Users_d ON [dbo].[TB_Users] AFTER DELETE AS \n" +
                "IF \n" +
                "	@@ROWCOUNT = 0 \n" +
                "RETURN \n" +
                "DECLARE \n" +
                "	@ip VARCHAR ( 32 ) = ( SELECT client_net_address FROM sys.dm_exec_connections WHERE session_id = @@SPID ); \n" +
                "DECLARE \n" +
                "	@schemaName VARCHAR ( 255 ) = ( SELECT OBJECT_SCHEMA_NAME( parent_id ) FROM sys.triggers WHERE object_id = @@PROCID ); \n" +
                "DECLARE \n" +
                "	@objectName VARCHAR ( 255 ) = ( SELECT OBJECT_NAME( parent_id ) tableName FROM sys.triggers WHERE object_id = @@PROCID ); \n" +
                "DECLARE \n" +
                "	@columnName VARCHAR ( 255 ) = ( SELECT column_name FROM information_schema.columns WHERE table_name = 'TB_Users' AND ordinal_position = 1 ); \n" +
                "DECLARE \n" +
                "	@oldValue VARCHAR ( 255 ) = ( SELECT " + tableFirstColumnName + " FROM deleted ); \n" +
                "INSERT INTO [dbo].[TB_TablesChangeLogs] ( changeType, columnName, oldValue, newValue, databaseName, schemaName, objectName, hostName, iPAddress, programName, loginName ) SELECT \n" +
                "'delete', \n" +
                "@columnName, \n" +
                "@oldValue, \n" +
                "'', \n" +
                "DB_NAME( ), \n" +
                "@schemaName, \n" +
                "@objectName, \n" +
                "HOST_NAME( ), \n" +
                "@ip, \n" +
                "PROGRAM_NAME ( ), \n" +
                "SuseR_SNAME( ) \n" +
                "GO ";
            }
            if (this.sqltype == "MySQL")
            {
                triggerDeleteSQL = "SELECT 1";
            }

            return triggerDeleteSQL.Replace(oldValue, tableName);
        }

        #region richTextBox1按下ESC键关闭当前窗口
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                this.Close();
            }
        }
        #endregion

        #region 获取MSSQL/MySQL当前所选表的表结构消息
        /// <summary>
        /// 获取MSSQL/MySQL当前所选表的表结构消息
        /// </summary>
        /// <param name="sqlType">MSSQL/MySQL</param>
        /// <param name="DataBaseName">数据库名</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        private string GetsqlGetTableStructureForMSSQLORMySQL(string sqlType, string TableName)
        {
            string result = "SELECT 1;";

            if (string.IsNullOrEmpty(sqlType) == false && string.IsNullOrEmpty(TableName) == false)
            {
                //MSSQL
                if (sqlType == "MSSQL")
                {
                    #region 查表结构sql
                    /*
                     USE dbname;
                     SELECT (case when a.colorder=1 then d.name else null end) 表名,
                     a.colorder 字段序号,a.name 字段名,
                     (case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end) 标识,
                     (case when (SELECT count(*) FROM sysobjects
                     WHERE (name in (SELECT name FROM sysindexes
                     WHERE (id = a.id) AND (indid in
                     (SELECT indid FROM sysindexkeys
                     WHERE (id = a.id) AND (colid in
                     (SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name)))))))
                     AND (xtype = 'PK'))>0 then '√' else '' end) 主键,b.name 类型,a.length 占用字节数,
                     COLUMNPROPERTY(a.id,a.name,'PRECISION') as 长度,
                     isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as 小数位数,(case when a.isnullable=1 then '√'else '' end) 允许空,
                     isnull(e.text,'') 默认值,isnull(g.[value], ' ') AS [说明]
                     FROM  syscolumns a
                     left join systypes b on a.xtype=b.xusertype
                     inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties'
                     left join syscomments e on a.cdefault=e.id
                     left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id
                     left join sys.extended_properties f on d.id=f.class and f.minor_id=0
                     where b.name is not null
                     --WHERE d.name='info' --如果只查询指定表,加上此条件
                     order by a.id,a.colorder
                     */
                    #endregion

                    result = "USE " + databasename + "; " +
                        "SELECT (case when a.colorder=1 then d.name else null end) 表名, " +
                        "a.colorder 字段序号, a.name 字段名, " +
                        "(case when COLUMNPROPERTY(a.id, a.name, 'IsIdentity') = 1 then '√'else '' end) 标识, " +
                        "(case when(SELECT count(*) FROM sysobjects " +
                        "WHERE(name in (SELECT name FROM sysindexes " +
                        "WHERE(id = a.id) AND(indid in " +
                        "(SELECT indid FROM sysindexkeys " +
                        "WHERE(id = a.id) AND(colid in " +
                        "(SELECT colid FROM syscolumns WHERE(id = a.id) AND(name = a.name))))))) " +
                        "AND(xtype = 'PK'))> 0 then '√' else '' end) 主键,b.name 类型, a.length 占用字节数, " +
                        "COLUMNPROPERTY(a.id, a.name, 'PRECISION') as 长度, " +
                        "isnull(COLUMNPROPERTY(a.id, a.name, 'Scale'), 0) as 小数位数,(case when a.isnullable = 1 then '√'else '' end) 允许空, " +
                        "isnull(e.text, '') 默认值,isnull(g.[value], ' ') AS[说明] " +
                        "FROM syscolumns a " +
                        "left join systypes b on a.xtype = b.xusertype " +
                        "inner join sysobjects d on a.id = d.id and d.xtype = 'U' and d.name <> 'dtproperties' " +
                        "left join syscomments e on a.cdefault = e.id " +
                        "left join sys.extended_properties g on a.id = g.major_id AND a.colid = g.minor_id " +
                        "left join sys.extended_properties f on d.id = f.class and f.minor_id=0 " +
                        "WHERE d.name= '" + TableName + "' " +
                        "order by a.id, a.colorder";
                    return result;
                }
                //MySQL
                if (sqlType == "MySQL")
                {
                    result = "SELECT COLUMN_NAME, COLUMN_TYPE, COLUMN_COMMENT FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = '" + databasename + "' AND TABLE_NAME = '" + TableName + "';";
                    //SELECT COLUMN_NAME AS 字段名, COLUMN_TYPE AS 字段类型, COLUMN_COMMENT AS 字段注释 FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = '" + DataBaseName + "' AND TABLE_NAME = '" + TableName + "';
                    return result;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }
        #endregion

        #region dg绑定数据，获取表结构信息
        /// <summary>
        /// dg绑定数据，获取表结构信息
        /// </summary>
        /// <param name="sqlType">MSSQL/MySQL</param>
        /// <param name="TableName">表名</param>
        private void dgBindData(string sqlType, string TableName)
        {
            if (string.IsNullOrEmpty(sqlType) == false && string.IsNullOrEmpty(TableName) == false)
            {
                sqlGetTableStructureForMSSQL = GetsqlGetTableStructureForMSSQLORMySQL("MSSQL", TableName);
                sqlGetTableStructureForMySQL = GetsqlGetTableStructureForMSSQLORMySQL("MySQL", TableName);

                //MSSQL
                if (sqlType == "MSSQL")
                {
                    //dataGridView1.DataSource = SqlHelper.getDataSetMSSQL(sqlGetTableStructureForMSSQL, mssqlconn).Tables[0];
                }
                //MySQL
                if (sqlType == "MySQL")
                {
                    //dataGridView1.DataSource = SqlHelper.getDataSetMySQL(sqlGetTableStructureForMySQL, mysqlconn).Tables[0];

                    //dataGridView1.Columns[0].HeaderText = "列名";
                    //dataGridView1.Columns[1].HeaderText = "类型";
                    //dataGridView1.Columns[2].HeaderText = "注释";
                }
            }
        }
        #endregion

        #region 选定表节点后，调用dgBindData，获取所选表 表结构，并显示到dg中
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = this.treeView1.GetNodeAt(pi);
            //获取深度，0：数据库名；1：表/视图；2：表名/视图名
            //MessageBox.Show(node.Level.ToString());
            if (node.Level == 2)
            {
                if (pi.X < node.Bounds.Left || pi.X > node.Bounds.Right)
                {
                    //不触发事件

                    return;
                }
                else
                {
                    //触发事件
                    dgBindData(sqltype, treeView1.SelectedNode.Text);
                }
            }
        }
        #endregion

        #region 处理点击已选择节点后重新获取所选表 表结构，并显示到dg中
        private void treeView1_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView1.GetNodeAt(pi);
            //获取深度，0：数据库名；1：表/视图；2：表名/视图名
            //MessageBox.Show(node.Level.ToString());
            if (node.Level == 2)
            {
                if (pi.X < node.Bounds.Left || pi.X > node.Bounds.Right)
                {
                    //不触发事件

                    return;
                }
                else
                {
                    //触发事件
                    if (treeView1.SelectedNode.Text == node.Text)
                    {
                        dgBindData(sqltype, treeView1.SelectedNode.Text);
                    }
                }
            }

        }
        #endregion

    }
}