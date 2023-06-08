namespace Tanji.Views.Pages;

partial class ConnectionView
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        viewModelSource = new System.Windows.Forms.BindingSource(components);
        lstVariables = new Controls.TanjiListView();
        lblStatus = new Controls.TanjiLabel();
        btnConnect = new Controls.TanjiButton();
        line2 = new System.Windows.Forms.Label();
        btnBrowse = new Controls.TanjiButton();
        lbxCustomClient = new Controls.TanjiLabelBox();
        btnExport = new Controls.TanjiButton();
        btnDestroy = new Controls.TanjiButton();
        line1 = new System.Windows.Forms.Label();
        lbxVariable = new Controls.TanjiLabelBox();
        lbxValue = new Controls.TanjiLabelBox();
        btnUpdate = new Controls.TanjiButton();
        btnReset = new Controls.TanjiButton();
        ((System.ComponentModel.ISupportInitialize)viewModelSource).BeginInit();
        SuspendLayout();
        // 
        // viewModelSource
        // 
        viewModelSource.DataSource = typeof(Core.ViewModels.ConnectionViewModel);
        // 
        // lstVariables
        // 
        lstVariables.Dock = System.Windows.Forms.DockStyle.Top;
        lstVariables.FullRowSelect = true;
        lstVariables.GridLines = true;
        lstVariables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
        lstVariables.Location = new System.Drawing.Point(0, 0);
        lstVariables.MultiSelect = false;
        lstVariables.Name = "lstVariables";
        lstVariables.ShowItemToolTips = true;
        lstVariables.Size = new System.Drawing.Size(536, 251);
        lstVariables.TabIndex = 0;
        lstVariables.UseCompatibleStateImageBehavior = false;
        lstVariables.View = System.Windows.Forms.View.Details;
        // 
        // lblStatus
        // 
        lblStatus.AnimationInterval = 0;
        lblStatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", viewModelSource, "Status", true));
        lblStatus.DisplayBoundary = true;
        lblStatus.Location = new System.Drawing.Point(3, 364);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new System.Drawing.Size(424, 20);
        lblStatus.TabIndex = 1;
        // 
        // btnConnect
        // 
        btnConnect.DataBindings.Add(new System.Windows.Forms.Binding("Command", viewModelSource, "ConnectCommand", true));
        btnConnect.Location = new System.Drawing.Point(433, 364);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new System.Drawing.Size(100, 20);
        btnConnect.TabIndex = 2;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = true;
        // 
        // line2
        // 
        line2.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
        line2.Location = new System.Drawing.Point(3, 360);
        line2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        line2.Name = "line2";
        line2.Size = new System.Drawing.Size(530, 1);
        line2.TabIndex = 110;
        // 
        // btnBrowse
        // 
        btnBrowse.DataBindings.Add(new System.Windows.Forms.Binding("Command", viewModelSource, "ConnectCommand", true));
        btnBrowse.Location = new System.Drawing.Point(433, 337);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new System.Drawing.Size(100, 20);
        btnBrowse.TabIndex = 111;
        btnBrowse.Text = "Browse";
        btnBrowse.UseVisualStyleBackColor = true;
        // 
        // lbxCustomClient
        // 
        lbxCustomClient.Location = new System.Drawing.Point(3, 337);
        lbxCustomClient.Name = "lbxCustomClient";
        lbxCustomClient.Size = new System.Drawing.Size(424, 21);
        lbxCustomClient.TabIndex = 112;
        lbxCustomClient.Text = "";
        lbxCustomClient.Title = "Custom Client Path";
        // 
        // btnExport
        // 
        btnExport.DataBindings.Add(new System.Windows.Forms.Binding("Command", viewModelSource, "ConnectCommand", true));
        btnExport.Location = new System.Drawing.Point(3, 311);
        btnExport.Name = "btnExport";
        btnExport.Size = new System.Drawing.Size(262, 20);
        btnExport.TabIndex = 114;
        btnExport.Text = "Export Certificate Authority";
        btnExport.UseVisualStyleBackColor = true;
        // 
        // btnDestroy
        // 
        btnDestroy.DataBindings.Add(new System.Windows.Forms.Binding("Command", viewModelSource, "ConnectCommand", true));
        btnDestroy.Location = new System.Drawing.Point(271, 311);
        btnDestroy.Name = "btnDestroy";
        btnDestroy.Size = new System.Drawing.Size(262, 20);
        btnDestroy.TabIndex = 115;
        btnDestroy.Text = "Destroy Certificates";
        btnDestroy.UseVisualStyleBackColor = true;
        // 
        // line1
        // 
        line1.BackColor = System.Drawing.Color.FromArgb(243, 63, 63);
        line1.Location = new System.Drawing.Point(3, 307);
        line1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        line1.Name = "line1";
        line1.Size = new System.Drawing.Size(530, 1);
        line1.TabIndex = 116;
        // 
        // lbxVariable
        // 
        lbxVariable.Location = new System.Drawing.Point(3, 257);
        lbxVariable.Name = "lbxVariable";
        lbxVariable.Size = new System.Drawing.Size(427, 21);
        lbxVariable.TabIndex = 117;
        lbxVariable.Text = "";
        lbxVariable.Title = "Variable";
        // 
        // lbxValue
        // 
        lbxValue.Location = new System.Drawing.Point(3, 283);
        lbxValue.Margin = new System.Windows.Forms.Padding(0);
        lbxValue.Name = "lbxValue";
        lbxValue.Size = new System.Drawing.Size(427, 21);
        lbxValue.TabIndex = 118;
        lbxValue.Text = "";
        lbxValue.TextPaddingWidth = 23;
        lbxValue.Title = "Value";
        // 
        // btnUpdate
        // 
        btnUpdate.DataBindings.Add(new System.Windows.Forms.Binding("Command", viewModelSource, "ConnectCommand", true));
        btnUpdate.Location = new System.Drawing.Point(433, 258);
        btnUpdate.Name = "btnUpdate";
        btnUpdate.Size = new System.Drawing.Size(100, 20);
        btnUpdate.TabIndex = 119;
        btnUpdate.Text = "Update";
        btnUpdate.UseVisualStyleBackColor = true;
        // 
        // btnReset
        // 
        btnReset.DataBindings.Add(new System.Windows.Forms.Binding("Command", viewModelSource, "ConnectCommand", true));
        btnReset.Location = new System.Drawing.Point(433, 284);
        btnReset.Name = "btnReset";
        btnReset.Size = new System.Drawing.Size(100, 20);
        btnReset.TabIndex = 120;
        btnReset.Text = "Reset";
        btnReset.UseVisualStyleBackColor = true;
        // 
        // ConnectionView
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(btnReset);
        Controls.Add(btnUpdate);
        Controls.Add(lbxValue);
        Controls.Add(lbxVariable);
        Controls.Add(line1);
        Controls.Add(btnDestroy);
        Controls.Add(btnExport);
        Controls.Add(lbxCustomClient);
        Controls.Add(btnBrowse);
        Controls.Add(line2);
        Controls.Add(btnConnect);
        Controls.Add(lblStatus);
        Controls.Add(lstVariables);
        Name = "ConnectionView";
        ((System.ComponentModel.ISupportInitialize)viewModelSource).EndInit();
        ResumeLayout(false);
    }

    #endregion
    private System.Windows.Forms.BindingSource viewModelSource;
    private Controls.TanjiListView lstVariables;
    private Controls.TanjiLabel lblStatus;
    private Controls.TanjiButton btnConnect;
    private System.Windows.Forms.Label line2;
    private Controls.TanjiButton btnBrowse;
    private Controls.TanjiLabelBox lbxCustomClient;
    private Controls.TanjiButton btnExport;
    private Controls.TanjiButton btnDestroy;
    private System.Windows.Forms.Label line1;
    private Controls.TanjiLabelBox lbxVariable;
    private Controls.TanjiLabelBox lbxValue;
    private Controls.TanjiButton btnUpdate;
    private Controls.TanjiButton btnReset;
}
