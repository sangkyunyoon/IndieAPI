using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aegis.Client;
using IndieAPI;



namespace TestClient
{
    public partial class FormServiceMain : Form
    {
        public FormServiceMain()
        {
            InitializeComponent();
        }


        public void OnInitView()
        {
            _tbTextData.Text = "";


            FormMain.SetMessage(Color.Black, "Ready");
        }


        ////////////////////////////////////////////////////////////////////////////////
        //   TextData
        private void OnClick_GetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Storage_GetTextData'...");
            FormMain.API.Storage_Text_GetData(OnRecv_GetTextData);
        }


        private void OnRecv_GetTextData(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            String textData = resPacket.GetStringFromUtf16();

            _tbTextData.Text = textData;

            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_SetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Storage_SetTextData'...");
            FormMain.API.Storage_Test_SetData(_tbTextData.Text, OnRecv_SetTextData);
        }


        private void OnRecv_SetTextData(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            if (result != ResultCode.Ok)
                FormMain.SetMessage(Color.Red, ResultCode.ToString(result));

            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_RefreshCloudSheet(object sender, EventArgs e)
        {
            FormMain.API.Storage_Sheet_Refresh(_tbFilename.Text, OnComplete_RefreshCloudSheet);
        }


        private void OnComplete_RefreshCloudSheet(Int32 result)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { OnComplete_RefreshCloudSheet(result); });
            else
            {
                _lvTables.Items.Clear();
                _lvData.Columns.Clear();
                _lvData.Items.Clear();

                foreach (var table in FormMain.API.CloudSheetTables.Items)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = table.Name;
                    lvi.SubItems.Add(table.RecordCount.ToString());

                    _lvTables.Items.Add(lvi);
                }
            }
        }


        private void OnSelected_Table(object sender, EventArgs e)
        {
            if (_lvTables.SelectedItems.Count == 0)
                return;

            ListViewItem lviTable = _lvTables.SelectedItems[0];
            var table = FormMain.API.CloudSheetTables.GetTable(lviTable.Text);


            _lvData.Columns.Clear();
            _lvData.Items.Clear();
            _lvData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);


            //  테이블의 컬럼정보
            foreach (var column in table.Columns)
            {
                ColumnHeader hdr = new ColumnHeader();
                hdr.Text = column.name;
                _lvData.Columns.Add(hdr);
            }


            //  데이터 리스트
            foreach (var record in table.Records)
            {
                ListViewItem lvi = new ListViewItem();
                Int32 idx = 0;


                foreach (var column in table.Columns)
                {
                    String text = record[column.name];


                    //  DateTime은 OADate 형식에서 변환한다.
                    if (column.type == IndieAPI.CloudSheet.FieldDataType.DateTime)
                    {
                        Double dt = Double.Parse(text);
                        text = DateTime.FromOADate(dt).ToString();
                    }


                    if (idx++ == 0)
                        lvi.Text = text;
                    else
                        lvi.SubItems.Add(text);
                }

                _lvData.Items.Add(lvi);
            }
        }
    }
}
