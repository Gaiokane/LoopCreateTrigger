namespace LoopCreateTrigger
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupbox_DataBase = new System.Windows.Forms.GroupBox();
            this.btn_SQLTableStructure = new System.Windows.Forms.Button();
            this.btnShowDatabases = new System.Windows.Forms.Button();
            this.chkboxPort = new System.Windows.Forms.CheckBox();
            this.txtboxPort = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.radiobtnMYSQL = new System.Windows.Forms.RadioButton();
            this.radiobtnMSSQL = new System.Windows.Forms.RadioButton();
            this.txtboxPassword = new System.Windows.Forms.TextBox();
            this.txtboxUsername = new System.Windows.Forms.TextBox();
            this.txtboxDatabase = new System.Windows.Forms.TextBox();
            this.txtboxHost = new System.Windows.Forms.TextBox();
            this.labPassword = new System.Windows.Forms.Label();
            this.labUsername = new System.Windows.Forms.Label();
            this.labDatabase = new System.Windows.Forms.Label();
            this.labConnectStatus = new System.Windows.Forms.Label();
            this.labHost = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupbox_DataBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Location = new System.Drawing.Point(12, 101);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(260, 337);
            this.treeView1.TabIndex = 1;
            // 
            // groupbox_DataBase
            // 
            this.groupbox_DataBase.Controls.Add(this.btn_SQLTableStructure);
            this.groupbox_DataBase.Controls.Add(this.btnShowDatabases);
            this.groupbox_DataBase.Controls.Add(this.chkboxPort);
            this.groupbox_DataBase.Controls.Add(this.txtboxPort);
            this.groupbox_DataBase.Controls.Add(this.btnDisconnect);
            this.groupbox_DataBase.Controls.Add(this.btnConnect);
            this.groupbox_DataBase.Controls.Add(this.radiobtnMYSQL);
            this.groupbox_DataBase.Controls.Add(this.radiobtnMSSQL);
            this.groupbox_DataBase.Controls.Add(this.txtboxPassword);
            this.groupbox_DataBase.Controls.Add(this.txtboxUsername);
            this.groupbox_DataBase.Controls.Add(this.txtboxDatabase);
            this.groupbox_DataBase.Controls.Add(this.txtboxHost);
            this.groupbox_DataBase.Controls.Add(this.labPassword);
            this.groupbox_DataBase.Controls.Add(this.labUsername);
            this.groupbox_DataBase.Controls.Add(this.labDatabase);
            this.groupbox_DataBase.Controls.Add(this.labConnectStatus);
            this.groupbox_DataBase.Controls.Add(this.labHost);
            this.groupbox_DataBase.Location = new System.Drawing.Point(12, 12);
            this.groupbox_DataBase.Name = "groupbox_DataBase";
            this.groupbox_DataBase.Size = new System.Drawing.Size(894, 83);
            this.groupbox_DataBase.TabIndex = 2;
            this.groupbox_DataBase.TabStop = false;
            this.groupbox_DataBase.Text = "数据库";
            // 
            // btn_SQLTableStructure
            // 
            this.btn_SQLTableStructure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SQLTableStructure.Location = new System.Drawing.Point(369, 17);
            this.btn_SQLTableStructure.Name = "btn_SQLTableStructure";
            this.btn_SQLTableStructure.Size = new System.Drawing.Size(75, 23);
            this.btn_SQLTableStructure.TabIndex = 14;
            this.btn_SQLTableStructure.Text = "查看表结构";
            this.btn_SQLTableStructure.UseVisualStyleBackColor = true;
            this.btn_SQLTableStructure.Click += new System.EventHandler(this.btn_SQLTableStructure_Click);
            // 
            // btnShowDatabases
            // 
            this.btnShowDatabases.Location = new System.Drawing.Point(518, 45);
            this.btnShowDatabases.Name = "btnShowDatabases";
            this.btnShowDatabases.Size = new System.Drawing.Size(31, 23);
            this.btnShowDatabases.TabIndex = 19;
            this.btnShowDatabases.Text = "...";
            this.btnShowDatabases.UseVisualStyleBackColor = true;
            this.btnShowDatabases.Click += new System.EventHandler(this.btnShowDatabases_Click);
            // 
            // chkboxPort
            // 
            this.chkboxPort.AutoSize = true;
            this.chkboxPort.Location = new System.Drawing.Point(183, 49);
            this.chkboxPort.Name = "chkboxPort";
            this.chkboxPort.Size = new System.Drawing.Size(60, 16);
            this.chkboxPort.TabIndex = 16;
            this.chkboxPort.Text = "Port：";
            this.chkboxPort.UseVisualStyleBackColor = true;
            // 
            // txtboxPort
            // 
            this.txtboxPort.Location = new System.Drawing.Point(249, 46);
            this.txtboxPort.Name = "txtboxPort";
            this.txtboxPort.Size = new System.Drawing.Size(62, 21);
            this.txtboxPort.TabIndex = 6;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(205, 17);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 14;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(124, 17);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 13;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // radiobtnMYSQL
            // 
            this.radiobtnMYSQL.AutoSize = true;
            this.radiobtnMYSQL.Location = new System.Drawing.Point(65, 20);
            this.radiobtnMYSQL.Name = "radiobtnMYSQL";
            this.radiobtnMYSQL.Size = new System.Drawing.Size(53, 16);
            this.radiobtnMYSQL.TabIndex = 2;
            this.radiobtnMYSQL.TabStop = true;
            this.radiobtnMYSQL.Text = "mysql";
            this.radiobtnMYSQL.UseVisualStyleBackColor = true;
            // 
            // radiobtnMSSQL
            // 
            this.radiobtnMSSQL.AutoSize = true;
            this.radiobtnMSSQL.Location = new System.Drawing.Point(6, 20);
            this.radiobtnMSSQL.Name = "radiobtnMSSQL";
            this.radiobtnMSSQL.Size = new System.Drawing.Size(53, 16);
            this.radiobtnMSSQL.TabIndex = 1;
            this.radiobtnMSSQL.TabStop = true;
            this.radiobtnMSSQL.Text = "mssql";
            this.radiobtnMSSQL.UseVisualStyleBackColor = true;
            this.radiobtnMSSQL.CheckedChanged += new System.EventHandler(this.radiobtnMSSQL_CheckedChanged);
            // 
            // txtboxPassword
            // 
            this.txtboxPassword.Location = new System.Drawing.Point(788, 46);
            this.txtboxPassword.Name = "txtboxPassword";
            this.txtboxPassword.Size = new System.Drawing.Size(100, 21);
            this.txtboxPassword.TabIndex = 12;
            // 
            // txtboxUsername
            // 
            this.txtboxUsername.Location = new System.Drawing.Point(626, 46);
            this.txtboxUsername.Name = "txtboxUsername";
            this.txtboxUsername.Size = new System.Drawing.Size(85, 21);
            this.txtboxUsername.TabIndex = 10;
            // 
            // txtboxDatabase
            // 
            this.txtboxDatabase.Location = new System.Drawing.Point(388, 46);
            this.txtboxDatabase.Name = "txtboxDatabase";
            this.txtboxDatabase.Size = new System.Drawing.Size(124, 21);
            this.txtboxDatabase.TabIndex = 8;
            // 
            // txtboxHost
            // 
            this.txtboxHost.Location = new System.Drawing.Point(54, 46);
            this.txtboxHost.Name = "txtboxHost";
            this.txtboxHost.Size = new System.Drawing.Size(123, 21);
            this.txtboxHost.TabIndex = 4;
            // 
            // labPassword
            // 
            this.labPassword.AutoSize = true;
            this.labPassword.Location = new System.Drawing.Point(717, 50);
            this.labPassword.Name = "labPassword";
            this.labPassword.Size = new System.Drawing.Size(65, 12);
            this.labPassword.TabIndex = 11;
            this.labPassword.Text = "Password：";
            // 
            // labUsername
            // 
            this.labUsername.AutoSize = true;
            this.labUsername.Location = new System.Drawing.Point(555, 50);
            this.labUsername.Name = "labUsername";
            this.labUsername.Size = new System.Drawing.Size(65, 12);
            this.labUsername.TabIndex = 9;
            this.labUsername.Text = "Username：";
            // 
            // labDatabase
            // 
            this.labDatabase.AutoSize = true;
            this.labDatabase.Location = new System.Drawing.Point(317, 50);
            this.labDatabase.Name = "labDatabase";
            this.labDatabase.Size = new System.Drawing.Size(65, 12);
            this.labDatabase.TabIndex = 7;
            this.labDatabase.Text = "Database：";
            // 
            // labConnectStatus
            // 
            this.labConnectStatus.AutoSize = true;
            this.labConnectStatus.Location = new System.Drawing.Point(286, 22);
            this.labConnectStatus.Name = "labConnectStatus";
            this.labConnectStatus.Size = new System.Drawing.Size(77, 12);
            this.labConnectStatus.TabIndex = 15;
            this.labConnectStatus.Text = "状态：已断开";
            // 
            // labHost
            // 
            this.labHost.AutoSize = true;
            this.labHost.Location = new System.Drawing.Point(7, 50);
            this.labHost.Name = "labHost";
            this.labHost.Size = new System.Drawing.Size(41, 12);
            this.labHost.TabIndex = 3;
            this.labHost.Text = "Host：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(278, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "创建变动日志表";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(278, 130);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "对库中所有表批量建触发器";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(442, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "暂时只有记录各表第一个字段";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupbox_DataBase);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupbox_DataBase.ResumeLayout(false);
            this.groupbox_DataBase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupbox_DataBase;
        private System.Windows.Forms.Button btnShowDatabases;
        private System.Windows.Forms.CheckBox chkboxPort;
        private System.Windows.Forms.TextBox txtboxPort;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RadioButton radiobtnMYSQL;
        private System.Windows.Forms.RadioButton radiobtnMSSQL;
        private System.Windows.Forms.TextBox txtboxPassword;
        private System.Windows.Forms.TextBox txtboxUsername;
        private System.Windows.Forms.TextBox txtboxDatabase;
        private System.Windows.Forms.TextBox txtboxHost;
        private System.Windows.Forms.Label labPassword;
        private System.Windows.Forms.Label labUsername;
        private System.Windows.Forms.Label labDatabase;
        private System.Windows.Forms.Label labConnectStatus;
        private System.Windows.Forms.Label labHost;
        private System.Windows.Forms.Button btn_SQLTableStructure;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
    }
}

