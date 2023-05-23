using Microsoft.VisualBasic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Windows.Forms;

namespace DotNetTask6
{
    public partial class MainForm : Form
    {
        private readonly AssembleyAdapter assembleyAdapter;
        private Type currentClassType = null;

        public MainForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            assembleyAdapter = new AssembleyAdapter("DLL/DotNetTask5.dll");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            List<string> classsNames = assembleyAdapter.GetClassNamesByInterfaceType("DotNetTask5.Entity.IFurniture");
            foreach (string className in classsNames)
            {
                classComboBox.Items.Add(className);
            }
        }

        private void ClassComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string className = classComboBox.SelectedItem as string;
            currentClassType = assembleyAdapter.GetClassType(className);
            methodComboBox.SelectedIndex = -1;
            methodComboBox.Items.Clear();
            paramsPanel.Controls.Clear();
            resultBox.Text = "";
            foreach (MethodInfo methodInfo in currentClassType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                methodComboBox.Items.Add(methodInfo.Name);
            }
        }

        private void MethodComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (methodComboBox.SelectedIndex == -1)
                return;
            string methodName = methodComboBox.SelectedItem as string;
            MethodInfo methodInfo = currentClassType.GetMethod(methodName);
            paramsPanel.Controls.Clear();
            resultBox.Text = "";
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                var textBox = new TextBox
                {
                    PlaceholderText = parameterInfo.Name,
                    Width = 400
                };
                paramsPanel.Controls.Add(textBox);
            }
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (methodComboBox.SelectedIndex == -1)
                return;
            string methodName = methodComboBox.SelectedItem as string;
            MethodInfo methodInfo = currentClassType.GetMethod(methodName);
            var paramList = new List<object>();
            int i = 0;
            try
            {
                foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
                {
                    string value = paramsPanel.Controls[i++].Text;
                    object param = Convert.ChangeType(value, parameterInfo.ParameterType);
                    paramList.Add(param);
                }
                object obj = Activator.CreateInstance(currentClassType);
                resultBox.Text = methodInfo.Invoke(obj, paramList.ToArray())?.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}