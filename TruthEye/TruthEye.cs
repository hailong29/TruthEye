using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tool
{
    /// <summary>
    /// 用于对控件进行实时检测, 了解控件真实信息, 从而知道页面上bug的来源。
    /// </summary>
    public class TruthEye
    {
        #region 私有字段

        private Form targetForm;  //要监控的窗体

        Timer refreshTimer = new Timer();  //用于将新建控件加入监控列表的时钟

        #endregion


        #region 构造方法
        #endregion


        #region 属性
        #endregion


        #region 公共方法
        /// <summary>
        /// 开启真实之眼，右键点击控件，可查看控件细节, 中键点击控件, 可查看控件的子控件
        /// 使用方法：在窗体的load事件中调用，推荐放在load事件的最后
        /// </summary>
        /// <param name="form">要监控的窗体</param>
        public void OpenTruthEye(Form form)
        {
            OpenTruthEye(form, true);
        }

        /// <summary>
        /// 开启真实之眼，右键点击控件，可查看控件细节, 中键点击控件, 可查看控件的子控件
        /// 使用方法：在窗体的load事件中调用，推荐放在load事件的最后
        /// </summary>
        /// <param name="form">要监控的窗体</param>
        /// <param name="isRefresh">是否对后续新建的控件进行监控，默认为true</param>
        public void OpenTruthEye(Form form, bool isRefresh)
        {
            SetAllControlEvent(form);
            //标记窗体
            form.MouseDown -= item_MouseDown;  //防重复
            form.MouseDown += item_MouseDown;
            this.targetForm = form;
            if (isRefresh)
                OpenRefresh();
        }

        /// <summary>
        /// 将控件的细节直接打印出来
        /// </summary>
        /// <param name="ctrl"></param>
        public void ShowDetail(Control ctrl)
        {
            Console.WriteLine("     " + ctrl.Name);
            Console.WriteLine("       文本：" + ctrl.Text);
            Console.WriteLine("       位置：" + ctrl.Location);
            Console.WriteLine("       尺寸：" + ctrl.Size);
            Console.WriteLine("       可见：" + ctrl.Visible);
            Console.WriteLine("       字体：" + ctrl.Font);
            Console.WriteLine("       标签：" + ctrl.Tag);
            //确保不为空(窗体)
            if (ctrl.Parent != null)
                Console.WriteLine("       父控件：" + ctrl.Parent.Name);
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 打开刷新功能(5s刷新)，能看到后面的自建控件
        /// </summary>
        private void OpenRefresh()
        {
            refreshTimer.Stop();
            refreshTimer.Dispose();
            refreshTimer = new Timer();
            refreshTimer.Interval = 5000;
            refreshTimer.Tick += Timer_Refresh;
            refreshTimer.Start();
        }

        /// <summary>
        /// 定时刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Refresh(object sender, EventArgs e)
        {
            SetAllControlEvent(this.targetForm);
        }

        /// <summary>
        /// 为全部控件添加事件
        /// </summary>
        /// <param name="CrlContainer"></param>
        private void SetAllControlEvent(Control CrlContainer)
        {
            foreach (Control item in CrlContainer.Controls)
            {
                if (!String.IsNullOrEmpty(item.Name.Trim()))
                {
                    item.MouseDown -= item_MouseDown;  //防重复
                    item.MouseDown += item_MouseDown;
                }
                if ((item as UserControl) == null && item.Controls.Count > 0)
                    SetAllControlEvent(item);
            }
        }

        /// <summary>
        /// 鼠标按下的事件, 用右键或中键可查看控件，当为中键时，展示子控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ShowCtrl((Control)sender, false);
            }
            if (e.Button == MouseButtons.Middle)
            {
                ShowCtrl((Control)sender, true);
            }
        }

        //判断是否打印子控件
        private void ShowCtrl(Control ctrl, bool isLookChild)
        {
            //不看子控件时
            if (!isLookChild)
            {
                Console.WriteLine("----------------开始汇报----------");
                ShowDetail(ctrl);
                Console.WriteLine("----------------汇报结束----------");
            }
            else
            {
                int i = 0;
                foreach (Control item in ctrl.Controls)
                {
                    i++;
                    Console.WriteLine("     ----------------子控件" + i + "开始汇报----------");
                    ShowDetail(item);
                    Console.WriteLine("     ----------------子控件" + i + "汇报结束----------");
                }
            }
        }
        #endregion
    }
}
