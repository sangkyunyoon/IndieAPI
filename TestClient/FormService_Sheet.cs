﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IndieAPI;
using TestClient.WinFormHelper;



namespace TestClient
{
    public partial class FormService_Sheet : Form
    {
        public FormService_Sheet()
        {
            InitializeComponent();
        }


        public void OnViewEntered()
        {
            FormMain.SetMessageReady();
        }


        private void OnClick_Back(object sender, EventArgs e)
        {
            UIViews.ChangeView<FormService_Profile>();
        }


        private void OnClick_RefreshSheet(object sender, EventArgs e)
        {
            FormMain.SetMessageBlue("Requesting 'Storage_Sheet_Refresh'...");
            NetworkAPI.Storage_Sheet_Refresh(_tbFilename.Text, (response) =>
            {
                _lvSheets.Items.Clear();
                _lvData.Columns.Clear();
                _lvData.Items.Clear();

                foreach (var sheet in NetworkAPI.Workbook.Sheets)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = sheet.Name;
                    lvi.SubItems.Add(sheet.RecordCount.ToString());

                    _lvSheets.Items.Add(lvi);
                }

                FormMain.SetMessageReady();
            });
        }


        private void OnSelectChanged_Sheet(object sender, EventArgs e)
        {
            if (_lvSheets.SelectedItems.Count == 0)
                return;

            ListViewItem lviSheet = _lvSheets.SelectedItems[0];
            var sheet = NetworkAPI.Workbook.GetSheet(lviSheet.Text);


            _lvData.Columns.Clear();
            _lvData.Items.Clear();
            _lvData.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);


            //  테이블의 컬럼정보
            foreach (var column in sheet.Fields)
            {
                ColumnHeader hdr = new ColumnHeader();
                hdr.Text = column.name;
                _lvData.Columns.Add(hdr);
            }


            //  데이터 리스트
            foreach (var record in sheet.Records)
            {
                ListViewItem lvi = new ListViewItem();
                Int32 idx = 0;


                foreach (var column in sheet.Fields)
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
