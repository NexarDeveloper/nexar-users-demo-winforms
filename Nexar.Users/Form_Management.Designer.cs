namespace Nexar.Users
{
    partial class Form_Management
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Management));
            this.listViewGroup = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMeneGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listViewUser = new System.Windows.Forms.ListView();
            this.columnHeaderUserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFirstName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLastName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEmail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderRole = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboWorkspaces = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMeneGroup.SuspendLayout();
            this.contextMenuUser.SuspendLayout();
            this.SuspendLayout();
            //
            // listViewGroup
            //
            this.listViewGroup.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
            this.listViewGroup.ContextMenuStrip = this.contextMeneGroup;
            this.listViewGroup.FullRowSelect = true;
            this.listViewGroup.HideSelection = false;
            this.listViewGroup.LabelEdit = true;
            this.listViewGroup.Location = new System.Drawing.Point(12, 40);
            this.listViewGroup.MultiSelect = false;
            this.listViewGroup.Name = "listViewGroup";
            this.listViewGroup.Size = new System.Drawing.Size(624, 140);
            this.listViewGroup.SmallImageList = this.imageList1;
            this.listViewGroup.TabIndex = 1;
            this.listViewGroup.UseCompatibleStateImageBehavior = false;
            this.listViewGroup.View = System.Windows.Forms.View.Details;
            this.listViewGroup.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewGroup_AfterLabelEdit);
            this.listViewGroup.SelectedIndexChanged += new System.EventHandler(this.listViewGroup_SelectedIndexChanged);
            //
            // columnHeaderName
            //
            this.columnHeaderName.Text = "Group Name";
            this.columnHeaderName.Width = 620;
            //
            // contextMeneGroup
            //
            this.contextMeneGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGroupToolStripMenuItem,
            this.deleteGroupToolStripMenuItem});
            this.contextMeneGroup.Name = "contextMeneGroup";
            this.contextMeneGroup.Size = new System.Drawing.Size(144, 48);
            //
            // addGroupToolStripMenuItem
            //
            this.addGroupToolStripMenuItem.Name = "addGroupToolStripMenuItem";
            this.addGroupToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.addGroupToolStripMenuItem.Text = "Add Group";
            this.addGroupToolStripMenuItem.Click += new System.EventHandler(this.addGroupToolStripMenuItem_Click);
            //
            // deleteGroupToolStripMenuItem
            //
            this.deleteGroupToolStripMenuItem.Name = "deleteGroupToolStripMenuItem";
            this.deleteGroupToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.deleteGroupToolStripMenuItem.Text = "Delete Group";
            this.deleteGroupToolStripMenuItem.Click += new System.EventHandler(this.deleteGroupToolStripMenuItem_Click);
            //
            // imageList1
            //
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "group.png");
            this.imageList1.Images.SetKeyName(1, "user.png");
            //
            // listViewUser
            //
            this.listViewUser.CheckBoxes = true;
            this.listViewUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUserName,
            this.columnHeaderFirstName,
            this.columnHeaderLastName,
            this.columnHeaderEmail,
            this.columnHeaderRole});
            this.listViewUser.ContextMenuStrip = this.contextMenuUser;
            this.listViewUser.FullRowSelect = true;
            this.listViewUser.HideSelection = false;
            this.listViewUser.Location = new System.Drawing.Point(12, 186);
            this.listViewUser.MultiSelect = false;
            this.listViewUser.Name = "listViewUser";
            this.listViewUser.Size = new System.Drawing.Size(624, 264);
            this.listViewUser.SmallImageList = this.imageList1;
            this.listViewUser.TabIndex = 2;
            this.listViewUser.UseCompatibleStateImageBehavior = false;
            this.listViewUser.View = System.Windows.Forms.View.Details;
            this.listViewUser.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewUser_ItemCheck);
            //
            // columnHeaderUserName
            //
            this.columnHeaderUserName.Text = "User Name";
            this.columnHeaderUserName.Width = 180;
            //
            // columnHeaderFirstName
            //
            this.columnHeaderFirstName.Text = "First Name";
            this.columnHeaderFirstName.Width = 75;
            //
            // columnHeaderLastName
            //
            this.columnHeaderLastName.Text = "Last Name";
            this.columnHeaderLastName.Width = 75;
            //
            // columnHeaderEmail
            //
            this.columnHeaderEmail.Text = "Email";
            this.columnHeaderEmail.Width = 180;
            //
            // columnHeaderRole
            //
            this.columnHeaderRole.Text = "Role";
            this.columnHeaderRole.Width = 110;
            //
            // contextMenuUser
            //
            this.contextMenuUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUserToolStripMenuItem,
            this.deleteUserToolStripMenuItem,
            this.updateUserToolStripMenuItem});
            this.contextMenuUser.Name = "contextMeneGroup";
            this.contextMenuUser.Size = new System.Drawing.Size(181, 92);
            //
            // addUserToolStripMenuItem
            //
            this.addUserToolStripMenuItem.Name = "addUserToolStripMenuItem";
            this.addUserToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addUserToolStripMenuItem.Text = "Add User";
            this.addUserToolStripMenuItem.Click += new System.EventHandler(this.addUserToolStripMenuItem_Click);
            //
            // updateUserToolStripMenuItem
            //
            this.updateUserToolStripMenuItem.Name = "updateUserToolStripMenuItem";
            this.updateUserToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.updateUserToolStripMenuItem.Text = "Update User";
            this.updateUserToolStripMenuItem.Click += new System.EventHandler(this.updateUserToolStripMenuItem_Click);
            //
            // deleteUserToolStripMenuItem
            //
            this.deleteUserToolStripMenuItem.Name = "deleteUserToolStripMenuItem";
            this.deleteUserToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteUserToolStripMenuItem.Text = "Delete User";
            this.deleteUserToolStripMenuItem.Click += new System.EventHandler(this.deleteUserToolStripMenuItem_Click);
            //
            // comboWorkspaces
            //
            this.comboWorkspaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWorkspaces.FormattingEnabled = true;
            this.comboWorkspaces.Location = new System.Drawing.Point(81, 10);
            this.comboWorkspaces.Name = "comboWorkspaces";
            this.comboWorkspaces.Size = new System.Drawing.Size(555, 21);
            this.comboWorkspaces.TabIndex = 0;
            this.comboWorkspaces.SelectionChangeCommitted += new System.EventHandler(this.comboWorkspaces_SelectionChangeCommitted);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Workspace";
            //
            // Form_Management
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 461);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboWorkspaces);
            this.Controls.Add(this.listViewUser);
            this.Controls.Add(this.listViewGroup);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_Management";
            this.Text = "Nexar.Users";
            this.Load += new System.EventHandler(this.Form_Management_Load);
            this.contextMeneGroup.ResumeLayout(false);
            this.contextMenuUser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewGroup;
        private System.Windows.Forms.ListView listViewUser;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMeneGroup;
        private System.Windows.Forms.ToolStripMenuItem addGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteGroupToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderUserName;
        private System.Windows.Forms.ContextMenuStrip contextMenuUser;
        private System.Windows.Forms.ToolStripMenuItem addUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteUserToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderFirstName;
        private System.Windows.Forms.ColumnHeader columnHeaderLastName;
        private System.Windows.Forms.ColumnHeader columnHeaderEmail;
        private System.Windows.Forms.ColumnHeader columnHeaderRole;
        private System.Windows.Forms.ComboBox comboWorkspaces;
        private System.Windows.Forms.Label label1;
    }
}

