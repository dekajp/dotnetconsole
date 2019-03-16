using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CLIAppManager;
using System.IO;

namespace CLIAppManagerWinForm
{
    public partial class CLIOptionUserControl : UserControl
    {

        public delegate void InfoLoadEventHandler(CLIOptionUserControl obj, CLIAppManager.CLIParamBase CLIparam);
       
        public event InfoLoadEventHandler InfoLoad;

        private CLIAppManager.CLIParamBase CLIParamObj;


        public CLIOptionUserControl()
        {
            InitializeComponent();         
        }
        public CLIOptionUserControl(CLIAppManager.CLIParamBase param)
        {
            InitializeComponent();
            InitializeComponent( param);
            CLIParamObj = param;
        }


        public void InitializeComponent(CLIAppManager.CLIParamBase param){          
            this.gbCLIOption.Text = string.Format(@"{0} - Type:{1}", Convert.ToString(param.ParamName), Convert.ToString(param.ParameterType));
            tipCLIOption.SetToolTip(tbCLIOption, Convert.ToString(param.ParamDesc));
            tipCLIOption.SetToolTip(gbCLIOption, Convert.ToString(param.ParamDesc));

            ShowDisplayControl(param);

            InitializeDefaultValue(param);
        }

        public void ShowDisplayControl(CLIAppManager.CLIParamBase param)
        {
            if (param.ParameterType == CLIParamType.LIST ||
                param.ParameterType == CLIParamType.TRUE_FALSE)
            {
                this.tbCLIOption.Visible = false;
                this.cbCLIOption.Visible = true;
            }
            else if (param.ParameterType == CLIParamType.PASSWORD)
            {
                this.tbCLIOption.Visible = true;
                this.cbCLIOption.Visible = false;
                tbCLIOption.PasswordChar = '*';
            }
            else
            {
                this.tbCLIOption.Visible = true;
                this.cbCLIOption.Visible = false;
            }
        }


        public void InitializeDefaultValue(CLIAppManager.CLIParamBase param)
        {
            if (tbCLIOption.Visible)
            {
                this.tbCLIOption.Text = Convert.ToString(param.ParamValue).Trim().Length < 1 ? Convert.ToString(param.ParamDefaultValue) : Convert.ToString(param.ParamValue);
            }
            else if (cbCLIOption.Visible)
            {
                this.cbCLIOption.Items.Clear();
                cbCLIOption.DropDownStyle = ComboBoxStyle.DropDownList;
                if (param.ParameterType == CLIParamType.LIST)
                {
                    //http://stackoverflow.com/questions/600869/how-to-bind-a-list-to-a-combobox-winforms
                    foreach (string s in param.ParamList)
                    {
                        this.cbCLIOption.Items.Add(s);
                    }
                    //http://msdn.microsoft.com/en-us/library/system.windows.forms.combobox.aspx
                    int index=-1;
                    try
                    {
                        if (Convert.ToString(param.ParamValue).Trim().Length > 0)
                        {
                            index = this.cbCLIOption.FindString(Convert.ToString(param.ParamValue));
                        }
                        else
                        {
                            index = this.cbCLIOption.FindString(Convert.ToString(param.ParamDefaultValue));
                        }
                    }
                    catch 
                    {
                        throw;
                    }
                    
                    this.cbCLIOption.SelectedIndex = index;
                }
                else if (param.ParameterType == CLIParamType.TRUE_FALSE)
                {
                    this.cbCLIOption.Items.Add("TRUE");
                    this.cbCLIOption.Items.Add("FALSE");

                    int index = this.cbCLIOption.FindString(Convert.ToString(param.ParamDefaultValue).ToUpper());
                    this.cbCLIOption.SelectedIndex = index;
                }
                else
                {
                    cbCLIOption.DropDownStyle = ComboBoxStyle.DropDown;
                }

                
            }
        }

        public string Value
        {
            get
            {
                
                if (tbCLIOption.Visible)
                {
                    return this.tbCLIOption.Text.Trim();
                }
                else if (cbCLIOption.Visible)
                {
                    int selectedIndex = this.cbCLIOption.SelectedIndex;
                    Object selectedItem = this.cbCLIOption.SelectedItem;
                    return Convert.ToString(selectedItem);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string ParamNameValue
        {
            get
            {
                return CLIParamObj.ParamName;
            }
        }

        public string ParamCommandOption
        {
            get
            {
                return string.Format(@"/{0}=""{1}""", this.ParamNameValue, this.Value);
            }
        }

        public CLIAppManager.CLIParamBase Param { get { return this.CLIParamObj; } }

       

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            InfoLoad?.Invoke(this, this.CLIParamObj);
        }

        private void TbCLIOption_Leave(object sender, EventArgs e)
        {
            if (ValidateInput())
            {

            }
            else
            {
                TextBox tb= (TextBox)sender;
                tb.Focus();
                tb.SelectAll();
                System.Windows.Forms.MessageBox.Show(string.Format(@"{0}-Invalid value.",this.CLIParamObj.ParamName), "Error");
            }
        }

        private bool ValidateInput()
        {
            bool bResult = true;
            if (this.CLIParamObj.ParameterType == CLIParamType.TRUE_FALSE)
            {
                try
                {
                    Convert.ToBoolean(this.Value);
                }
                catch
                {
                    bResult = false;
                }
            }else if (this.CLIParamObj.ParameterType == CLIParamType.FILE)
            {

                try
                {
                    FileSystemInfo fsinfo = new FileInfo(this.Value);
                    bResult = fsinfo.Exists;
                }
                catch 
                {
                    bResult = false;
                }
            }
            else if (this.CLIParamObj.ParameterType == CLIParamType.FOLDER)
            {
                try
                {
                    FileSystemInfo dirInfo = new DirectoryInfo(this.Value);
                    bResult = dirInfo.Exists;
                }
                catch 
                {
                    bResult = false;
                }
            }
            else if (this.CLIParamObj.ParameterType == CLIParamType.LIST)
            {
                try
                {
                    if (this.CLIParamObj.ParamList.IndexOf(this.Value) >= 0)
                    {
                        bResult = true;
                    }
                    else
                    {
                        bResult = false;
                    }
                }
                catch 
                {
                    bResult = false;
                }
            }

            return bResult;
        }

        private void CbCLIOption_Leave(object sender, EventArgs e)
        {
            if (ValidateInput())
            {

            }
            else
            {
                ComboBox tb = (ComboBox)sender;
                tb.Focus();
                tb.SelectAll();
                System.Windows.Forms.MessageBox.Show(string.Format(@"{0}-Invalid value.", this.CLIParamObj.ParamName), string.Format("Error at Param:{0}",this.Name));
            }
        }

    }
}
