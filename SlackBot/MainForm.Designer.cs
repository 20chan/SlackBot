/*
 * 사용자: 10414_이영찬
 * 날짜: 2016-06-01
 * 시간: 오후 1:30
 */
namespace SlackBot
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TextBox tbChat;
		private System.Windows.Forms.ListBox lbBan;
		private System.Windows.Forms.RichTextBox tbConsole;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.ListBox lbAdmin;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lbAdmin = new System.Windows.Forms.ListBox();
			this.tbChat = new System.Windows.Forms.TextBox();
			this.lbBan = new System.Windows.Forms.ListBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tbConsole = new System.Windows.Forms.RichTextBox();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel2.Controls.Add(this.lbAdmin, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.tbChat, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.lbBan, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(643, 264);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// lbAdmin
			// 
			this.lbAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbAdmin.FormattingEnabled = true;
			this.lbAdmin.ItemHeight = 12;
			this.lbAdmin.Location = new System.Drawing.Point(546, 3);
			this.lbAdmin.Name = "lbAdmin";
			this.lbAdmin.Size = new System.Drawing.Size(94, 258);
			this.lbAdmin.TabIndex = 4;
			// 
			// tbChat
			// 
			this.tbChat.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbChat.Location = new System.Drawing.Point(3, 3);
			this.tbChat.Multiline = true;
			this.tbChat.Name = "tbChat";
			this.tbChat.Size = new System.Drawing.Size(437, 258);
			this.tbChat.TabIndex = 0;
			// 
			// lbBan
			// 
			this.lbBan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbBan.FormattingEnabled = true;
			this.lbBan.ItemHeight = 12;
			this.lbBan.Location = new System.Drawing.Point(446, 3);
			this.lbBan.Name = "lbBan";
			this.lbBan.Size = new System.Drawing.Size(94, 258);
			this.lbBan.TabIndex = 1;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tbConsole, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(649, 370);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// tbConsole
			// 
			this.tbConsole.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbConsole.Location = new System.Drawing.Point(3, 273);
			this.tbConsole.Name = "tbConsole";
			this.tbConsole.Size = new System.Drawing.Size(643, 94);
			this.tbConsole.TabIndex = 4;
			this.tbConsole.Text = "";
			this.tbConsole.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbConsoleKeyDown);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(649, 370);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "MainForm";
			this.Text = "SlackBot";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
