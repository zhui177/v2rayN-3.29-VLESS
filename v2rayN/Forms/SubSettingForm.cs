﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using v2rayN.Handler;
using v2rayN.Mode;

namespace v2rayN.Forms
{
    public partial class SubSettingForm : BaseForm
    {
        List<SubSettingControl> lstControls = new List<SubSettingControl>();

        public SubSettingForm()
        {
            InitializeComponent();
        }

        private void SubSettingForm_Load(object sender, EventArgs e)
        {
            int screenWidth = Screen.FromHandle(this.Handle).WorkingArea.Width;
            int screenHeight = Screen.FromHandle(this.Handle).WorkingArea.Height;

            // 设置窗口的尺寸不大于屏幕的尺寸
            if (this.Width > screenWidth)
            {
                this.Width = screenWidth;
            }
            if (this.Height > screenHeight)
            {
                this.Height = screenHeight;
            }

            if (appConfig.subItem == null)
            {
                appConfig.subItem = new List<SubItem>();
            }

            RefreshSubsView();
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshSubsView()
        {
            panCon.Controls.Clear();
            lstControls.Clear();

            for (int k = appConfig.subItem.Count - 1; k >= 0; k--)
            {
                SubItem item = appConfig.subItem[k];
                if (Utils.IsNullOrEmpty(item.remarks)
                    && Utils.IsNullOrEmpty(item.url))
                {
                    if (!Utils.IsNullOrEmpty(item.id))
                    {
                        AppConfigHandler.RemoveServerViaSubid(ref appConfig, item.id);
                    }
                    appConfig.subItem.RemoveAt(k);
                }
            }

            foreach (SubItem item in appConfig.subItem)
            {
                SubSettingControl control = new SubSettingControl();
                control.OnButtonClicked += Control_OnButtonClicked;
                control.subItem = item;
                control.Dock = DockStyle.Top;

                panCon.Controls.Add(control);
                panCon.Controls.SetChildIndex(control, 0);

                lstControls.Add(control);
            }
        }

        private void Control_OnButtonClicked(object sender, EventArgs e)
        {
            RefreshSubsView();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (appConfig.subItem.Count <= 0)
            {
                AddSub();
            }

            if (AppConfigHandler.SaveSubItem(ref appConfig) == 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                UI.ShowWarning(UIRes.I18N("OperationFailed"));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSub();

            RefreshSubsView();
        }


        private void AddSub()
        {
            SubItem subItem = new SubItem
            {
                id = string.Empty,
                remarks = "remarks",
                url = "url"
            };
            appConfig.subItem.Add(subItem);
        }
    }
}
